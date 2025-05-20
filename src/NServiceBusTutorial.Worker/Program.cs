using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using NServiceBusTutorial.Core.ContributorAggregate.Commands;
using NServiceBusTutorial.Core.Interfaces;
using NServiceBusTutorial.Infrastructure.Data;
using NServiceBusTutorial.Infrastructure.Notifications;
using NServiceBusTutorial.Worker.Contributors;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

string endpointName = "contributors-worker";

var builder = Host.CreateDefaultBuilder();

builder.UseConsoleLifetime();

builder.ConfigureServices((hostContext, services) =>
{
  string? connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
  Guard.Against.Null(connectionString);
  services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

  services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
  services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));

  var mediatRAssemblies =
  services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies([typeof(ContributorCreateCommandHandler).Assembly]));
  services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
  services.AddScoped<INotificationService, NoOpNotificationService>();

  services.AddSerilog((services, loggerConfiguration) => loggerConfiguration
      .ReadFrom.Configuration(hostContext.Configuration)
      .ReadFrom.Services(services)
      .Enrich.FromLogContext());

  services.AddOpenTelemetry()
    .ConfigureResource(resourceBuilder => resourceBuilder.AddService(endpointName))
    .WithTracing(builder =>
    {
        builder.AddSource("NServiceBus.*");
        builder.AddConsoleExporter();
    });
});

builder.UseNServiceBus(context => 
{
  var endpointConfiguration = new EndpointConfiguration(endpointName);
  endpointConfiguration.UseSerialization<SystemJsonSerializer>();
  endpointConfiguration.EnableInstallers();
  endpointConfiguration.SendFailedMessagesTo("error");

  var transport = endpointConfiguration.UseTransport<LearningTransport>();

  transport.Routing().RouteToEndpoint(
    typeof(StartContributorVerificationCommand),
    "contributors-saga");

  endpointConfiguration.EnableOpenTelemetry();

  var recoverability = endpointConfiguration.Recoverability();
  recoverability.Immediate(c => c.NumberOfRetries(0));
  recoverability.Delayed(c => c.NumberOfRetries(0));

  return endpointConfiguration;
});

var host = builder.Build();
host.Run();
