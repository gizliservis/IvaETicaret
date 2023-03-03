using System.Net.Mail;

namespace IvaETicaret.Email
{
    public static class SenderEmail
    {
        public static void Gonder(string konu, string icerik, string GondMail)
        {
            MailMessage ePosta = new MailMessage();
            ePosta.From = new MailAddress("cib_1742@hotmail.com");
            //
            ePosta.To.Add(GondMail);
            //ePosta.To.Add("eposta2@gmail.com");
            //ePosta.To.Add("eposta3@gmail.com");
            //
            //ePosta.Attachments.Add(new Attachment(@"C:\deneme-upload.jpg"));
            ePosta.Subject = konu;
            ePosta.Body = icerik;
            ePosta.IsBodyHtml= true;
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;
            smtp.Host = "smtp.office365.com";
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential("cib_1742@hotmail.com", "Cib_1453");

            // object userState = ePosta;
            bool kontrol = true;
            try
            {
                // smtp.SendAsync(ePosta, (object)ePosta);
               smtp.Send(ePosta);

            }
            catch (SmtpException ex)
            {
                kontrol = false;
            }
            
        }
    }
}
