namespace SistemaTecnico.Services
{
    public interface IEmailService
    {
        Task EnviarAsync(
            string destinatario,
            string asunto,
            string cuerpo,
            byte[]? archivoAdjunto = null,
            bool esHtml = true            
        );

        Task EnviarAsync(
            IEnumerable<string> destinatarios,
            string asunto,
            string cuerpo,
            byte[]? archivoAdjunto,
            bool esHtml = true
        );
    }

}
