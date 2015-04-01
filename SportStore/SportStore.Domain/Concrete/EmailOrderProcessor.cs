using System.Text;

using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices.WindowsRuntime;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;

namespace SportStore.Domain.Concrete
{
    public class EmailSetting
    {
        public string MailToAddress = "orders@example.com";
        public string MailFromAddress = "sportsstore@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUserName";
        public string Password = "MySmtpPassword";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = @"c:\sport_store_emails";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSetting emailSettings;

        public EmailOrderProcessor(EmailSetting settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body =
                    new StringBuilder().AppendLine("A new order has been submitted")
                        .AppendLine("---")
                        .AppendLine("Items:");
                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price*line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c}", line.Quantity, line.Product.Name, subtotal);

                }

                body.AppendFormat("Total order value: {0:c}", cart.ComputeTotalValue())
                    .Append("---")
                    .Append("Ship to:")
                    .Append(shippingInfo.Name)
                    .Append(shippingInfo.Line1)
                    .Append(shippingInfo.Line2 ?? "")
                    .Append(shippingInfo.Line3 ?? "")
                    .Append(shippingInfo.City)
                    .Append(shippingInfo.State ?? "")
                    .Append(shippingInfo.Country)
                    .Append(shippingInfo.Zip)
                    .Append("---")
                    .AppendFormat("Gift wrap: {0}",
                        shippingInfo.GiftWrap ? "Yes" : "No");
                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAddress,
                    emailSettings.MailToAddress,
                    "New order submitted!",
                    body.ToString());
                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                smtpClient.Send(mailMessage);
            }
        }
    }

}