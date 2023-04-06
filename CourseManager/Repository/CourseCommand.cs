using CourseManager.Models;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace CourseManager.Repository
{

    internal class CourseCommand
    {
        private string ConnectionString { get; }

        public CourseCommand(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IList<CourseModel> GetList()
        {
            List<CourseModel> courses = new List<CourseModel>();

            string stored_procedure = "Course_GetList";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                // Get Model List with Dapper
                courses = conn.Query<CourseModel>(stored_procedure).ToList();
            }

            return courses;
        }
    }
}
