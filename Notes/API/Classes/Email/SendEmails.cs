using System.Net.Mail;

/**
 * Enables emails to be sent to rochat@lsus.edu
 * with information about who is accessing the database via API, and when.
 */
namespace API.Classes.Email
{
    public class SendEmails
    {
        public SmtpClient client = new SmtpClient();

        public SendEmails()
        {
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(Settings.Settings.getEmailAccount(), Settings.Settings.getEmailPassword());
        }

        public void SendMessage(string subject, string body)
        {
            // From, To, Subject, Body
            MailMessage mm = new MailMessage("bot.kujou.karen@gmail.com", "rochat97@lsus.edu", subject, body);
            client.Send(mm);
        }
    }
}
