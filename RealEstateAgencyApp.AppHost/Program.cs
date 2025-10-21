var builder = DistributedApplication.CreateBuilder(args);

var mysql = builder.AddMySql("mysql");

var mysqlDb = mysql.AddDatabase("mysqldb");

var api = builder.AddProject<Projects.RealEstateAgencyApp_API>("realestate-api")
                 .WithReference(mysqlDb, "mysqldb")
                 .WaitFor(mysqlDb);

builder.Build().Run();