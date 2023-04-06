
//using MailKit.Net.Smtp;
//using MimeKit;

using System.Net;
using System.Net.Mail;

namespace CourseManager.Workers
{
    internal class EnrollmentDetailReportEmailSender
    {
        public void Send(string fileName, string toEmail, string fromEmail, string pwd)
        {

            //string toEmail = Console.ReadLine();
            //Console.WriteLine("Write Your Gmail System E-Mail");
            //string fromEmail = Console.ReadLine();
            //Console.WriteLine("Write your AppPassword generated (different from account password): ");
            //string pwd = Console.ReadLine();


            //SmtpClient client = new SmtpClient("smtp-mail.outlook.com");
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            // Assign new network credentials
            client.UseDefaultCredentials = false;
            NetworkCredential credentials = new NetworkCredential(fromEmail, pwd);
            client.Credentials = credentials;
            client.EnableSsl = true;


            // Construct Message

            MailMessage message = new MailMessage(fromEmail, toEmail);
            message.Subject = "Enrollment Details Report";
            message.IsBodyHtml = true;
            message.Body = "Hi<br><br>Please find attached the enrollment details report.<br>Let me know if you need anything else.<br><br>Best regards,<br>Alvaro";

            Attachment attachment = new Attachment(fileName);
            message.Attachments.Add(attachment);
            client.Send(message);

        }
    }
}
