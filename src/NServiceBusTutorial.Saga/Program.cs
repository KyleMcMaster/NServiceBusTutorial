using NServiceBus;
using NServiceBusTutorial.Core.ContributorAggregate.Commands;

var builder = Host.CreateDefaultBuilder(args);

builder.UseNServiceBus(context => 
{
  var endpointConfiguration = new EndpointConfiguration("contributors-saga");
  endpointConfiguration.UseSerialization<SystemJsonSerializer>();
  endpointConfiguration.EnableInstallers();

  var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
  transport.ConnectionString("host=localhost");
  transport.UseDirectRoutingTopology(QueueType.Quorum);

  transport.Routing().RouteToEndpoint(
    typeof(VerifyContributorCommand),
    "contributors-worker");

  transport.Routing().RouteToEndpoint(
    typeof(UnverifyContributorCommand),
    "contributors-worker");

  var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();

  return endpointConfiguration;
});

var host = builder.Build();
host.Run();
