using System;
using taskcore.Dao;
using taskcore.Models;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace taskcore.Manager
{
    public class MailManager
    {

        private static string code = null;

        public static async Task<bool> ResetPasswordCode()
        {
            string mailto = UserManager.GetCurrentUser().Email;
            string subject = "Parola Sıfırlama";
            string text = "<h1><b>Kodunuz:</b> " + getCode() + "</h1>";
            string sender = "noreplytaskcore@gmail.com";
            MailMessage msg = new MailMessage(sender, mailto, subject, text);
            msg.IsBodyHtml = true;
            SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
            sc.UseDefaultCredentials = false;
            NetworkCredential cre = new NetworkCredential(sender, "Taskcore2811");
            sc.Credentials = cre;
            sc.EnableSsl = true;
            await sc.SendMailAsync(msg);
            return true;

        }

        public static async Task<bool> WelcomeMessage(User user)
        {
            string mailto =user.Email;
            string subject = "Task-Core Kayıt";
            string text = "<h1><b>Sayın "+user.Name+" "+user.Surname+"</b></h1> Task-Core Proje Planlama ve Yönetim sistemine hoşgeldiniz! ";
            string sender = "noreplytaskcore@gmail.com";
            MailMessage msg = new MailMessage(sender, mailto, subject, text);
            msg.IsBodyHtml = true;
            SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
            sc.UseDefaultCredentials = false;
            NetworkCredential cre = new NetworkCredential(sender, "Taskcore2811");
            sc.Credentials = cre;
            sc.EnableSsl = true;
            await sc.SendMailAsync(msg);
            return true;
        }

        public static async Task<bool> ProjectRequestMessage(User user,Project project)
        {
            string mailto = user.Email;
            string subject = "Task-Core Proje Katılım İsteği";
            string text = "<h1><b>Sayın " + user.Name + " " + user.Surname + "</b></h1> "+UserManager.GetFullName()+" size "+project.Title+" projesine katılmanız için bir istek gönderdi!";
            string sender = "noreplytaskcore@gmail.com";
            MailMessage msg = new MailMessage(sender, mailto, subject, text);
            msg.IsBodyHtml = true;
            SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
            sc.UseDefaultCredentials = false;
            NetworkCredential cre = new NetworkCredential(sender, "Taskcore2811");
            sc.Credentials = cre;
            sc.EnableSsl = true;
            await sc.SendMailAsync(msg);
            return true;
        }


        public static void cleanCode(){
            code = null;
        }


        public static string getCode()
        {
            if (code == null)
            {
                Random random = new Random();
                code = "";
                for (int i = 0; i < 6; i++)
                {
                    char tmp = Convert.ToChar(random.Next(48, 58));
                    code += tmp;
                }
            }

            return code;
        }


    }
}