var builder = DistributedApplication.CreateBuilder(args);

var mysql = builder.AddMySql("mysql").WithDataVolume().WithEnvironment("MYSQL_ROOT_HOST", "%");

var mysqlDb = mysql.AddDatabase("mysqldb");

var api = builder.AddProject<Projects.RealEstateAgencyApp_API>("realestate-api")
                 .WithReference(mysqlDb);

builder.Build().Run();