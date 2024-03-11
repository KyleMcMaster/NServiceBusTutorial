using NServiceBus;

var builder = Host.CreateDefaultBuilder();

builder.UseConsoleLifetime();
builder.UseNServiceBus(context => 
{
  var endpointConfiguration = new EndpointConfiguration("contributors-worker");
  endpointConfiguration.UseTransport<LearningTransport>();

  return endpointConfiguration;
});

var host = builder.Build();
host.Run();
