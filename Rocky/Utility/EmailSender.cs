using Mailjet.Client;
using Mailjet.Client.Resources;
using System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Rocky.Utility
{
    public class EmailSender : IEmailSender
    {

        //    var emailMessage = new MimeMessage();

        //    emailMessage.From.Add(new MailboxAddress("Администрация сайта", "gmail.com"));
        //    emailMessage.To.Add(new MailboxAddress("", email));
        //    emailMessage.Subject = subject;
        //    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        //    {
        //        Text = message
        //    };

        //    using (var client = new SmtpClient())
        //    {
        //        await client.ConnectAsync("smtp.gmail.com", 465);
        //        await client.AuthenticateAsync("korotkevichkostya@gmail.com", "kostya20007");
        //        await client.SendAsync(emailMessage);

        //        await client.DisconnectAsync(true);
        //    }
        //}
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {
            MailjetClient client = new MailjetClient("3933ef5b3ecf2d82e14603ad0c511d0c", 
                "8616205a0f0d0a53ed5c38e91265bc9d");
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "test2000124@gmail.com"},
        {"Name", "Test"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          "korotkewich@mail.ru"
         }, {
          "Name",
          "Test"
         }
        }
       }
      }, {
       "Subject",
       "Greetings from Mailjet."
      }, {
       "TextPart",
       "My first Mailjet email"
      }, {
       "HTMLPart",
       "<h3>Dear passenger 1, welcome to <a href='https://www.mailjet.com/'>Mailjet</a>!</h3><br />May the delivery force be with you!"
      }, {
       "CustomID",
       "AppGettingStartedTest"
      }
     }
             });
            MailjetResponse response = await client.PostAsync(request);
        }
    }
}
