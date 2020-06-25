using System;
using System.Net;
using System.Net.Mail;
using taskcore.Models;

namespace taskcore.Observer
{
    public class UserObserver : Observer
    {        public override void Create(object obj)
        {
            User user = (User)obj;
            string mailto = user.Email;
            string subject = "Task-Core Kayıt";
            string text = "<h1><b>Sayın " + user.Name + " " + user.Surname + "</b></h1> Task-Core Proje Planlama ve Yönetim sistemine hoşgeldiniz! ";
            string sender = "noreplytaskcore@gmail.com";
            MailMessage msg = new MailMessage(sender, mailto, subject, text);
            msg.IsBodyHtml = true;
            SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
            sc.UseDefaultCredentials = false;
            NetworkCredential cre = new NetworkCredential(sender, "Taskcore2811");
            sc.Credentials = cre;
            sc.EnableSsl = true;
            sc.Send(msg);
        }
        public override void Update(object obj)
        {
            User user = (User)obj;
            string mailto = user.Email;
            string subject = "Task-Core";
            string text = "<h1><b>Sayın " + user.Name + " " + user.Surname + "</b></h1> Profil bilgileriniz güncellenmiştir.";
            string sender = "noreplytaskcore@gmail.com";
            MailMessage msg = new MailMessage(sender, mailto, subject, text);
            msg.IsBodyHtml = true;
            SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
            sc.UseDefaultCredentials = false;
            NetworkCredential cre = new NetworkCredential(sender, "Taskcore2811");
            sc.Credentials = cre;
            sc.EnableSsl = true;
            sc.Send(msg);
        }
    }
}
