using API.Entities;

namespace API.Extensions
{
    public static class HtmlFormat
    {

        public static string Format(Employee employee)
        {   
            var template = $"<style>\r\n    table, td {{\r\n        border: 1px solid black;\r\n        padding: 2px;\r\n    }}\r\n</style>\r\n<body>\r\n    <h1>{employee.FirstName} {employee.LastName}</h1>\r\n    <h3>{employee.Address}</h3>\r\n    <h3>{employee.WorkPosition}</h3>\r\n    <table>\r\n        <tr>\r\n            <td>Gross Income</td>\r\n            <td>{employee.GrossIncome}</td>\r\n        </tr>\r\n        <tr>\r\n            <td>Tax</td>\r\n            <td>{employee.IncomeDetails.Tax}</td>\r\n        </tr>\r\n        <tr>\r\n            <td>Pension (PIO)</td>\r\n            <td>{employee.IncomeDetails.PIO}</td>\r\n        </tr>\r\n        <tr>\r\n            <td>Health care</td>\r\n            <td>{employee.IncomeDetails.HealthCare}</td>\r\n        </tr>\r\n        <tr>\r\n            <td>Unemployment</td>\r\n            <td>{employee.IncomeDetails.Unemployment}</td>\r\n        </tr>\r\n        <tr>\r\n            <td>Net Income</td>\r\n            <td>{employee.IncomeDetails.NetIncome}</td>\r\n        </tr>\r\n    </table>\r\n</body>";
            
            return template;
        }
    }
}
