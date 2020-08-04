using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Application.Interfaces.Shared;
using Domain.Settings;
using MailKit.Net.Smtp;
using MimeKit;
using System.Linq;

namespace Shared.Services
{
    public class EmailService : IEmailService
    {
        private EmailConfig _config { get; set; }

        public EmailService(IOptions<EmailConfig> optionsConfig)
        {
            _config = optionsConfig.Value;
        }

        public async Task Send(string toAdresse, string toUsername, string messageHTML)
        {
            MimeMessage message = new MimeMessage();
            message.Prepare(EncodingConstraint.EightBit);

            message.From.Add(new MailboxAddress(_config.NameFrom, _config.EmailFrom));
            message.To.Add(new MailboxAddress(toUsername, toAdresse));
            message.Subject = _config.EmailSubject;

            BodyBuilder body = new BodyBuilder
            {
                HtmlBody = messageHTML
            };

            message.Body = body.ToMessageBody();

            foreach (var bodyPart in message.BodyParts.OfType<TextPart>())
                bodyPart.ContentTransferEncoding = ContentEncoding.Base64;

            using (SmtpClient client = new SmtpClient())
            {
                client.Connect(_config.SmtpServer, _config.SmtpPort);
                await client.AuthenticateAsync(_config.LoginSmtpServer, _config.PasswordSmtpServer);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
