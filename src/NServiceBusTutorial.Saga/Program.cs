using NServiceBusTutorial.Core.ContributorAggregate.Commands;

var builder = Host.CreateDefaultBuilder(args);

builder.UseNServiceBus(context => 
{
  var endpointConfiguration = new EndpointConfiguration("contributors-saga");
  endpointConfiguration.UseSerialization<SystemJsonSerializer>();
  var transport = endpointConfiguration.UseTransport<LearningTransport>();

  transport.Routing().RouteToEndpoint(
    typeof(VerifyContributorCommand),
    "contributors-worker");

  transport.Routing().RouteToEndpoint(
    typeof(NotVerifyContributorCommand),
    "contributors-worker");

  var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();

  return endpointConfiguration;
});

var host = builder.Build();
host.Run();
