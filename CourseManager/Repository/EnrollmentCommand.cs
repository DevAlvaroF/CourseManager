using CourseManager.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CourseManager.Repository
{
    internal class EnrollmentCommand
    {
        private string ConnectionString { get; }

        public EnrollmentCommand(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void Upsert(EnrollmentModel enrollmentModel)
        {
            string stored_procedure = "Enrollments_Upsert";

            // Getting Computer name
            string userId = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();

            // Create Table for Data to send as the datatype created in the SPROC
            DataTable dt = new DataTable();
            dt.Columns.Add("EnrollmentId", typeof(int));
            dt.Columns.Add("StudentId", typeof(int));
            dt.Columns.Add("CourseId", typeof(int));
            dt.Rows.Add(enrollmentModel.EnrollmentId, enrollmentModel.StudentId, enrollmentModel.CourseId);

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Execute(stored_procedure, new { @EnrollmentType = dt.AsTableValuedParameter("EnrollmentType"), @UserId = userId }, commandType: CommandType.StoredProcedure);
            }
        }

        public IList<EnrollmentModel> GetList()
        {
            List<EnrollmentModel> enrolllments = new List<EnrollmentModel>();

            string stored_procedure = "Enrollments_GetList";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                // Get Model List with Dapper
                enrolllments = conn.Query<EnrollmentModel>(stored_procedure).ToList();
            }

            foreach (EnrollmentModel enrollment in enrolllments)
            {
                enrollment.IsComitted = true;
            }

            return enrolllments;
        }
    }
}
