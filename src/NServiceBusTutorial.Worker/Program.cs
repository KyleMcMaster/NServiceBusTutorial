using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBusTutorial.Infrastructure.Data;
using NServiceBusTutorial.Worker;
using System.Reflection;

var builder = Host.CreateDefaultBuilder();

builder.UseConsoleLifetime();

builder.ConfigureServices((hostContext, services) =>
{
  string? connectionString = hostContext.Configuration.GetConnectionString("SqliteConnection");
  Guard.Against.Null(connectionString);
  services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

  services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
  services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));

  var mediatRAssemblies =
  services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies([typeof(ContributorCreateCommandHandler).Assembly]));
  services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
});

builder.UseNServiceBus(context => 
{
  var endpointConfiguration = new EndpointConfiguration("contributors-worker");
  endpointConfiguration.UseTransport<LearningTransport>();

  return endpointConfiguration;
});

var host = builder.Build();
host.Run();
