# GrossToNetConversion

Clone and run application using `dotnet run`.

Postman collection json file is available `GrossToNetConversion.postman_collection.json`.

## Configuration

Change MSSQL connection string if running in production in `appsettings.json`.
Make an account on https://apilayer.com/marketplace/exchangerates_data-api and set your API key inside `appsettings.json`

### Local API addresses are:

* http://localhost:5000
* https://localhost:5001

### API endpoints

* [GET] all employees `/api/employees`
* [GET] employee by id `/api/employees/5` with optional parameter ?currency=EUR|USD
* [POST] Add employee `/api/employees/add`
* [DELETE] Delete employee `/api/employees/delete/5`
* [GET] Export to xlsx `/api/employees/export/excel`
* [GET] Export to CSV `/api/employees/export/csv`
* [GET] Export detailed employee data to PDF `/api/employees/export/pdf/5`