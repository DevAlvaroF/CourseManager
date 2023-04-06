using CourseManager.Models;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace CourseManager.Repository
{
    internal class EnrollmentDetailReportCommand
    {
        public string ConnectionString { get; set; }

        public EnrollmentDetailReportCommand(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IList<EnrollmentDetailReportModel> GetList()
        {
            List<EnrollmentDetailReportModel> allReportList = new List<EnrollmentDetailReportModel>();

            string stored_procedure = "Enrollmentreport_GetList";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                // Use Dapper to get List from Stored Procedure
                allReportList = conn.Query<EnrollmentDetailReportModel>(stored_procedure).ToList();
            }

            return allReportList;
        }
    }
}
