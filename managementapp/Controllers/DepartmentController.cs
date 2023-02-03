using managementapp.NewFolder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace managementapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllDepartment")]
        public ActionResult<List <Department>> GetAllDepartment()
        {
            var myList = new List<Department>();
            string query = @" select DepartmentId, DepartmentName from dbo.Department";
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
                myList.Add(new Department { DepartmentId= Convert.ToInt32(dataRow[0]),
                                            DepartmentName= Convert.ToString(dataRow[1])
                                            });
            }

            return myList;
        }

        [HttpPost]
        public ActionResult<List<KeyValuePair<int, string>>> Post(Department dep)
        {
            var myList = new List<KeyValuePair<int, string>>();
            string query = @" insert into dbo.Department values (@DepartmentName)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            foreach (DataRow dataRow in table.Rows)
            {
                myList.Add(new KeyValuePair<int, string>(Convert.ToInt32(dataRow[0]), dataRow[1].ToString()));
            }
            
            return new JsonResult("Added succesfully");
        }

        [HttpPut]
        public ActionResult<List<KeyValuePair<int, string>>> Put(Department dep)
        {
            var myList = new List<KeyValuePair<int, string>>();
            string query = @" update dbo.Department set DepartmentName= @DepartmentName where DepartmentId= @DepartmentId";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentId", dep.DepartmentId);
                    myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            foreach (DataRow dataRow in table.Rows)
            {
                myList.Add(new KeyValuePair<int, string>(Convert.ToInt32(dataRow[0]), dataRow[1].ToString()));
            }

            return new JsonResult("Update succesfully");
        }

        [HttpDelete("{id}")]
        public ActionResult<List<KeyValuePair<int, string>>> Delete(int id)
        {
            var myList = new List<KeyValuePair<int, string>>();
            string query = @" delete from dbo.Department where DepartmentId= @DepartmentId";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentId", id);
                   
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            foreach (DataRow dataRow in table.Rows)
            {
                myList.Add(new KeyValuePair<int, string>(Convert.ToInt32(dataRow[0]), dataRow[1].ToString()));
            }

            return new JsonResult("Delete succesfully");
        }


    }
}
