using managementapp.Models;
using managementapp.NewFolder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Security.AccessControl;

namespace managementapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public ActionResult<List<Employee>> GetAllEmployee()
        {
            var myList = new List<Employee>();
            string query = @" select EmployeeId, EmployeeName, Department, 
                            convert(varchar(10), DateOfJoining,120) as DateOfJoining, PhotofileName 
                               from dbo.Employee";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            foreach (DataRow dataRow in table.Rows)
            {
                myList.Add(new Employee { EmployeeId =  Convert.ToInt32(dataRow[0]),
                                EmployeeName= Convert.ToString(dataRow[1]), 
                                Department = Convert.ToString(dataRow[2]),
                                DateOfJoining = Convert.ToString(dataRow[3]),
                                PhotoFileName = Convert.ToString(dataRow[4])
                });
            }

            return myList;
        }

        [HttpPost]
        public ActionResult<List<Employee>> Post(Employee emp)
        {
            var myList = new List<Employee>();
            string query = @" insert into dbo.Employee 
                            (EmployeeName,Department,DateOfJoining,PhotoFileName)
                            values (@EmployeeName,@Department,@DateOfJoining,@PhotoFileName)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    myCommand.Parameters.AddWithValue("@Department", emp.Department);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            foreach (DataRow dataRow in table.Rows)
            {
                myList.Add(new Employee
                {
                    EmployeeId = Convert.ToInt32(dataRow[0]),
                    EmployeeName = Convert.ToString(dataRow[1]),
                    Department = Convert.ToString(dataRow[2]),
                    DateOfJoining = Convert.ToString(dataRow[3]),
                    PhotoFileName = Convert.ToString(dataRow[4])
                });
            }

            return new JsonResult("Added succesfully");
        }

        [HttpPut]
        public ActionResult<List<Employee>> Put( Employee emp )
        {
            var myList = new List<Employee>();
            string query = @" update dbo.Employee set 
                            EmployeeName= @EmployeeName,
                            Department =@Department,
                            DateOfJoining =@DateOfJoining,
                            PhotoFileName =@PhotoFileName
                            where EmployeeId= @EmployeeId";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
                    myCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    myCommand.Parameters.AddWithValue("@Department", emp.Department);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            foreach (DataRow dataRow in table.Rows)
            {
                myList.Add(new Employee
                {
                    EmployeeId = Convert.ToInt32(dataRow[0]),
                    EmployeeName = Convert.ToString(dataRow[1]),
                    Department = Convert.ToString(dataRow[2]),
                    DateOfJoining = Convert.ToString(dataRow[3]),
                    PhotoFileName = Convert.ToString(dataRow[4])
                });
            }

            return new JsonResult("Update succesfully");
        }

        [HttpDelete("{id}")]
        public ActionResult<List<Employee>> Delete(int id)
        {
            var myList = new List<Employee>();
            string query = @" delete from dbo.Employee where EmployeeId= @EmployeeId";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            foreach (DataRow dataRow in table.Rows)
            {
                myList.Add(new Employee
                {
                    EmployeeId = Convert.ToInt32(dataRow[0]),
                    EmployeeName = Convert.ToString(dataRow[1]),
                    Department = Convert.ToString(dataRow[2]),
                    DateOfJoining = Convert.ToString(dataRow[3]),
                    PhotoFileName = Convert.ToString(dataRow[4])
                });
            }

            return new JsonResult("Delete succesfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalpath = _env.ContentRootPath+ "/Photos" + filename;

                using (var stream=new FileStream(physicalpath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(filename);
            }
            catch(Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }


    }
}