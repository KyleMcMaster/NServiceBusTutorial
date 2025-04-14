var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", "postgres",secret: false);
var password = builder.AddParameter("password", "postgres",secret: false);

var postgres = builder.AddPostgres("postgres", username, password, 5432);
var db = postgres.AddDatabase("NServiceBusTutorial");

builder.AddProject<Projects.NServiceBusTutorial_Web>("Web")
  .WithReference(postgres)
  .WaitFor(db);

builder.AddProject<Projects.NServiceBusTutorial_Saga>("Saga")
  .WithReference(postgres)
  .WaitFor(db);

builder.AddProject<Projects.NServiceBusTutorial_Worker>("Worker")
  .WithReference(postgres)
  .WaitFor(db);

builder.Build().Run();
