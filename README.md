# GrossToNetConversion

Clone and run application using `dotnet run`.

Postman collection json file is available `GrossToNetConversion.postman_collection.json`.

## Configuration

Change MSSQL connection string if running in production in `appsettings.json`.

### Local API addresses are:

* http://localhost:5000
* https://localhost:5001

### API endpoints

* [GET] all employees `/api/employees`
* [GET] employee by id `/api/employees/5`
* [GET] Add employee `/api/employees/export/excel`
* [GET] Add employee `/api/employees/export/csv`
* [POST] Add employee `/api/employees/add`