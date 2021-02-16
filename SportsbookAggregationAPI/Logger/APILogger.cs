using System;
using System.IO;
using System.Net.Mail;

namespace SportsbookAggregationAPI.Logger
{
    public static class APILogger
    {
        public static void LogMessage(string errorMessage)
        {
            if (Startup.Configuration.ReadBooleanProperty("OutputToConsole"))
              Console.WriteLine(errorMessage);

            if (Startup.Configuration.ReadBooleanProperty("SendTexts"))
                SendText(errorMessage);
        }

        public static void LogError(Exception errorMessage)
        {
            string filePath = "ErrorLog.txt";

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("-----------------------------------------------------------------------------");
                writer.WriteLine("Date : " + DateTime.Now.ToString());
                writer.WriteLine();

                var exception = errorMessage;
                while (exception != null)
                {
                    writer.WriteLine(exception.GetType().FullName);
                    writer.WriteLine("Message : " + exception.Message);
                    writer.WriteLine("StackTrace : " + exception.StackTrace);

                    exception = exception.InnerException;
                }
            }
        }

        private static void SendText(string text)
        {
            if (Startup.Configuration.ReadBooleanProperty("SendTexts"))
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                System.Net.NetworkCredential basicAuthenticationInfo = new
                   System.Net.NetworkCredential("SportsAggregation@gmail.com", "MattNick69");
                client.Credentials = basicAuthenticationInfo;

                var message = new MailMessage();
                message.From = new MailAddress("SportsAggregation@gmail.com");

                message.To.Add(new MailAddress("4102927305@vtext.com")); //Nick
                message.To.Add(new MailAddress("3015025056@vtext.com")); //Myers

                message.Subject = "PROD ALERT";
                message.Body = text;

                try
                {
                    client.Send(message);
                }
                catch
                {
                    //Sent too many alerts :(
                }
            }
        }
    }
}
