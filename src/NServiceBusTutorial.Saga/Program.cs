using Npgsql;
using NpgsqlTypes;
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
    typeof(NotVerifyContributorCommand),
    "contributors-worker");

  var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
  var subscriptions = persistence.SubscriptionSettings();
  subscriptions.CacheFor(TimeSpan.FromMinutes(1));
  var dialect = persistence.SqlDialect<SqlDialect.PostgreSql>();
  dialect.JsonBParameterModifier(
      modifier: parameter =>
      {
          var npgsqlParameter = (NpgsqlParameter)parameter;
          npgsqlParameter.NpgsqlDbType = NpgsqlDbType.Jsonb;
      });
  string connectionString = context.Configuration.GetConnectionString("DefaultConnection")!;
  persistence.ConnectionBuilder(
      connectionBuilder: () =>
      {
          return new NpgsqlConnection(connectionString);
      });
  endpointConfiguration.EnableInstallers();

  var recoverability = endpointConfiguration.Recoverability();
  recoverability.Immediate(c => c.NumberOfRetries(0));
  recoverability.Delayed(c => c.NumberOfRetries(0));

  return endpointConfiguration;
});

var host = builder.Build();
host.Run();
