using CourseManager.Models;
using CourseManager.Repository;
using CourseManager.Workers;
using System;
using System.Collections.Generic;
using System.Windows;

namespace CourseManager.Views
{
    /// <summary>
    /// Interaction logic for EmailView.xaml
    /// </summary>
    public partial class EmailView : Window
    {
        private static string ReportFilename { get; } = "EnrollmentDetailsReport.xlsx";
        public EmailView()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            EnrollmentDetailReportCommand enrollmentDetailReportCmd = new EnrollmentDetailReportCommand("Data Source=localhost;Initial Catalog=CourseReport;Integrated Security=True");

            // Get Results
            IList<EnrollmentDetailReportModel> modelDetailList = enrollmentDetailReportCmd.GetList();

            // Create SpreadSheet
            Console.WriteLine("Generating Report...\n");
            EnrollmentDetailReportSpreadSheetCreator spreadSheetCreator = new EnrollmentDetailReportSpreadSheetCreator();

            spreadSheetCreator.Create(ReportFilename, modelDetailList);
            try
            {
                string toEmail = toTextBox.Text;
                string fromEmail = fromTextBox.Text;
                string pwd = pwdTextBox.Password.ToString();

                // Send Email
                EnrollmentDetailReportEmailSender emailSender = new EnrollmentDetailReportEmailSender();
                emailSender.Send(ReportFilename, toEmail, fromEmail, pwd);

                if (MessageBox.Show("Success Sending Email!", "Sucess", MessageBoxButton.OK) == MessageBoxResult.OK)
                {
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                string msg = $"Couldn't send message\n{ex.Message}";
                MessageBox.Show(msg);
            }

        }
    }
}
