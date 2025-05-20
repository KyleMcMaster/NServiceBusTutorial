var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", "postgres",secret: false);
var password = builder.AddParameter("password", "postgres",secret: false);

var postgres = builder.AddPostgres("postgres", username, password, 5432);
var domainDb = postgres.AddDatabase("NServiceBusTutorial");
var sagaDb = postgres.AddDatabase("SagaDb");

builder.AddProject<Projects.NServiceBusTutorial_Web>("Web")
  .WithReference(postgres)
  .WaitFor(domainDb);

builder.AddProject<Projects.NServiceBusTutorial_Saga>("Saga")
  .WithReference(postgres)
  .WaitFor(domainDb)
  .WaitFor(sagaDb);

builder.AddProject<Projects.NServiceBusTutorial_Worker>("Worker")
  .WithReference(postgres)
  .WaitFor(domainDb);

builder.Build().Run();
