var builder = DistributedApplication.CreateBuilder(args);

var mysql = builder.AddMySql("mysql");

var mysqlDb = mysql.AddDatabase("mysqldb");

builder.AddProject<Projects.RealEstateAgencyApp_Api>("realestate-api")
                 .WithReference(mysqlDb, "mysqldb")
                 .WaitFor(mysqlDb);

builder.AddProject<Projects.RealEstateAgencyApp_Grp>("realestateagencyapp-grp");

builder.Build().Run();