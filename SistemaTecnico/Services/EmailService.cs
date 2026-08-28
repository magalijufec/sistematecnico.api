using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SistemaTecnico.Models;

namespace SistemaTecnico.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings; 

        public EmailService(IOptions<EmailSettings> options) 
        { 
            _settings = options.Value; 
        }

        public async Task EnviarAsync(string destinatario, string asunto, string cuerpo, List<byte[]>? archivosAdjuntos, bool esHtml = true) 
        { 
            await EnviarAsync(new[] { destinatario }, asunto, cuerpo, archivosAdjuntos, esHtml); 
        }

        public async Task EnviarAsync(IEnumerable<string> destinatarios, string asunto, string cuerpo, List<byte[]>? archivosAdjuntos, bool esHtml = true)
        {
            var message = new MimeMessage(); 

            // REMITENTE
            message.From.Add( new MailboxAddress( _settings.DisplayName, _settings.From ) ); 
            
            // DESTINATARIOS
            foreach (var destinatario in destinatarios) 
            { 
                if (!string.IsNullOrWhiteSpace(destinatario)) 
                { 
                    message.To.Add( MailboxAddress.Parse( destinatario ) ); 
                } 
            } 
            // ASUNTO
            message.Subject = asunto; 

            // CUERPO
            var bodyBuilder = new BodyBuilder(); 

            if (esHtml) { 
                bodyBuilder.HtmlBody = cuerpo; 
            } 
            else { 
                bodyBuilder.TextBody = cuerpo; 
            }

            if (archivosAdjuntos != null)
            {
                foreach (var archivo in archivosAdjuntos)
                {
                    bodyBuilder.Attachments.Add(
                    "Informe-tecnico.pdf",
                    archivo,
                    ContentType.Parse("application/pdf")
                    );
                }            
            }                

            message.Body = bodyBuilder.ToMessageBody(); 
            // SMTP
            using var smtp = new SmtpClient(); 
            await smtp.ConnectAsync( _settings.Host, _settings.Port, SecureSocketOptions.StartTls ); 
            await smtp.AuthenticateAsync( _settings.UserName, _settings.Password ); 
            await smtp.SendAsync( message ); 
            await smtp.DisconnectAsync( true ); 
        } 
    }
 }
