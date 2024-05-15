using NServiceBus;

var builder = Host.CreateDefaultBuilder(args);

builder.UseNServiceBus(context => 
{
  var endpointConfiguration = new EndpointConfiguration("contributors-saga");
  endpointConfiguration.UseTransport<LearningTransport>();

  return endpointConfiguration;
});

var host = builder.Build();
host.Run();
