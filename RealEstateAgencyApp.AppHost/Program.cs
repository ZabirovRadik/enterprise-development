using Google.Protobuf.WellKnownTypes;

var builder = DistributedApplication.CreateBuilder(args);

var mysql = builder.AddMySql("mysql");
var mysqlDb = mysql.AddDatabase("mysqldb");

builder.AddProject<Projects.RealEstateAgencyApp_Api>("realestate-api")
    .WithReference(mysqlDb, "mysqldb")
    .WaitFor(mysqlDb);

var consumer = builder.AddProject<Projects.RealEstateAgencyApp_GrpcConsumer>("grpc-consumer")
    .WithReference(mysqlDb, "mysqldb")
    .WaitFor(mysqlDb);

builder.AddProject<Projects.RealEstateAgencyApp_GrpcProducer>("grpc-producer")
    .WaitFor(mysqlDb)
    .WaitFor(consumer);

builder.Build().Run();