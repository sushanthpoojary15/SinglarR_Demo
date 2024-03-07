using Microsoft.AspNetCore.Mvc;
using SignalR_demo.Models;
using SignalR_demo.Singnal_r_services;
using System.Data.SqlClient;
using System.Diagnostics;

namespace SignalR_demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connection;

        private readonly ISignalRService _signalRService;
  
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration,ISignalRService signalRService)
        {
            _logger = logger;
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("EmployeeCrud");
            _signalRService= signalRService; 
          

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }
        public async Task<IActionResult> GetAllEmployees()
        {
            Response res = new Response();
            List<Emp> employees = new List<Emp>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connection))
                {

                    connection.Open();

                    string query = "SELECT * FROM Employees";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Emp emp = new Emp
                                {
                                    id = reader.GetInt32(reader.GetOrdinal("id")),
                                    name = reader.GetString(reader.GetOrdinal("name")),
                                    age = reader.GetInt32(reader.GetOrdinal("age")),
                                    phone_no = reader.GetInt64(reader.GetOrdinal("phone_no")),
                                    gender = reader.GetString(reader.GetOrdinal("gender")),

                                };

                                employees.Add(emp);

                            }
                        }
                    }

                }
                res.Message = "Success";
                res.Status = "200";
                res.Data = employees;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = "401";

            }

            return Json(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(Emp emp)
        {
            Response res = new Response();
            try
            {

                using (SqlConnection connection = new SqlConnection(_connection))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO Employees (name, age, phone_no, gender) VALUES (@Name, @Age, @PhoneNo, @Gender); SELECT SCOPE_IDENTITY();";

                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        // Assuming you have Emp object to get values from
                        insertCommand.Parameters.AddWithValue("@Name", emp.name);
                        insertCommand.Parameters.AddWithValue("@Age", emp.age);
                        insertCommand.Parameters.AddWithValue("@PhoneNo", emp.phone_no);

                        insertCommand.Parameters.AddWithValue("@Gender", emp.gender);

                        emp.id = Convert.ToInt32(insertCommand.ExecuteScalar());

                        if (emp.id > 0)
                        {
                            await _signalRService.SendEmpData(emp);
                         

                            res.Status = "Success";
                            res.Message = "Successfully inserted";
                        }
                        else
                        {
                            res.Status = "failed";
                            res.Message = "Failed to insert";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = "Bad";

            }
            return Json(res);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
