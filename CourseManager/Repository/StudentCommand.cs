using CourseManager.Models;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace CourseManager.Repository
{
    internal class StudentCommand
    {
        private string ConnectionString { get; }

        public StudentCommand(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IList<StudentModel> GetList()
        {
            List<StudentModel> students = new List<StudentModel>();

            string stored_procedure = "Student_GetList";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                // Get Model List with Dapper
                students = conn.Query<StudentModel>(stored_procedure).ToList();
            }

            return students;
        }
    }
}
