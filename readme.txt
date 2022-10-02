How to configure the DB

1-  update appsettings.json in api project

"UseInMemoryDatabase": false,

2- Set connection string to your local instance of SQL server in appsettings.json

3-If dotne is not insalled then execute this in command prompt

"dotnet tool install --global dotnet-ef"

4-cd {path to the root of the repository}

5- to create new migration script

dotnet-ef migrations add "firstmigration" --project Infrastructure --startup-project WebAPI --output-dir Persistence\Migrations

6- open package management console in VS

7- run
Update-Database
---------------------------------------------------------------------------

run the app in VS

Check the vedio at /project video for how to run and use the API project