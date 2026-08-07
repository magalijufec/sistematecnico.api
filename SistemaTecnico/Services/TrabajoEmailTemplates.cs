using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace SistemaTecnico.Services
{

    public static class TrabajoEmailTemplates
    {
        // ============================================================
        // 1. NUEVO TRABAJO ASIGNADO AL TÉCNICO
        // Estado: Pendiente
        // Destinatario: Técnico
        // ============================================================

        public static string NuevoTrabajoAsignado(
            string nombreTecnico,
            int trabajoId,
            string cliente,
            string tarea)
        {
            return Layout(
                "Nuevo trabajo asignado",
                $"""
            <p>Hola <strong>{nombreTecnico}</strong>,</p>

            <p>
                Se te ha asignado un nuevo trabajo técnico.
            </p>

            <div class="info">
                <p>
                    <strong>Trabajo:</strong>
                    #{trabajoId}
                </p>

                <p>
                    <strong>Cliente:</strong>
                    {cliente}
                </p>

                <p>
                    <strong>Tarea:</strong>
                    {tarea}
                </p>

                <p>
                    <strong>Estado:</strong>
                    Pendiente
                </p>
            </div>

            <p>
                Ingresá al Sistema Técnico para consultar
                los detalles del trabajo.
            </p>
            """
            );
        }


        // ============================================================
        // 2. TRABAJO INICIADO
        // Estado: En proceso
        // Destinatarios: Sistemas / Farmacia
        // ============================================================

        public static string TrabajoIniciado(
            string nombreTecnico,
            int trabajoId,
            string cliente,
            string tarea)
        {
            return Layout(
                "Trabajo iniciado",
                $"""
            <p>
                El técnico
                <strong>{nombreTecnico}</strong>
                ha iniciado un trabajo.
            </p>

            <div class="info">
                <p>
                    <strong>Trabajo:</strong>
                    #{trabajoId}
                </p>

                <p>
                    <strong>Cliente:</strong>
                    {cliente}
                </p>

                <p>
                    <strong>Tarea:</strong>
                    {tarea}
                </p>

                <p>
                    <strong>Estado:</strong>
                    En proceso
                </p>
            </div>

            <p>
                El trabajo se encuentra actualmente
                en proceso de realización.
            </p>
            """
            );
        }


        // ============================================================
        // 3. TRABAJO FINALIZADO POR EL TÉCNICO
        // Estado: Trabajado finalizado
        // Destinatario: Sistemas
        // ============================================================

        public static string TrabajoPendienteAprobacion(
            int trabajoId,
            string cliente,
            string tecnico,
            string tarea)
        {
            return Layout(
                "Trabajo pendiente de aprobación",
                $"""
            <p>
                Se ha completado un trabajo técnico
                y está pendiente de aprobación.
            </p>

            <div class="info">
                <p>
                    <strong>Trabajo:</strong>
                    #{trabajoId}
                </p>

                <p>
                    <strong>Cliente:</strong>
                    {cliente}
                </p>

                <p>
                    <strong>Técnico:</strong>
                    {tecnico}
                </p>

                <p>
                    <strong>Tarea:</strong>
                    {tarea}
                </p>

                <p>
                    <strong>Estado:</strong>
                    Trabajado finalizado
                </p>
            </div>
            """
            );
        }


        // ============================================================
        // 4. TRABAJO APROBADO
        // Estado: Aprobado
        // Destinatario: Técnico
        // ============================================================

        public static string TrabajoAprobado(
            string nombreTecnico,
            int trabajoId,
            string cliente,
            string tarea)
        {
            return Layout(
                "Trabajo aprobado",
                $"""
            <p>
                Hola <strong>{nombreTecnico}</strong>,
            </p>

            <p>
                El trabajo realizado ha sido revisado
                y aprobado por el sector de Sistemas.
            </p>

            <div class="info">
                <p>
                    <strong>Trabajo:</strong>
                    #{trabajoId}
                </p>

                <p>
                    <strong>Cliente:</strong>
                    {cliente}
                </p>

                <p>
                    <strong>Tarea:</strong>
                    {tarea}
                </p>

                <p>
                    <strong>Estado:</strong>
                    Aprobado
                </p>
            </div>

            <p>
                Ahora debés cargar la factura correspondiente
                al trabajo desde el Sistema Técnico.
            </p>
            """
            );
        }


        // ============================================================
        // 5. FACTURA CARGADA
        // Estado: Pendiente pago
        // Destinatario: Pagos
        // ============================================================

        public static string FacturaPendientePago(
            int trabajoId,
            string cliente,
            string tecnico,
            string tarea)
        {
            return Layout(
                "Factura pendiente de pago",
                $"""
            <p>
                Se ha cargado una factura correspondiente
                a un trabajo técnico.
            </p>

            <div class="info">
                <p>
                    <strong>Trabajo:</strong>
                    #{trabajoId}
                </p>

                <p>
                    <strong>Cliente:</strong>
                    {cliente}
                </p>

                <p>
                    <strong>Técnico:</strong>
                    {tecnico}
                </p>

                <p>
                    <strong>Tarea:</strong>
                    {tarea}
                </p>

                <p>
                    <strong>Estado:</strong>
                    Pendiente de pago
                </p>
            </div>

            <p>
                Por favor, ingresá al Sistema Técnico
                para consultar la factura y gestionar
                el pago correspondiente.
            </p>
            """
            );
        }


        // ============================================================
        // 6. PAGO REALIZADO
        // Estado: Finalizado
        // Destinatarios: Técnico / Farmacia
        // ============================================================

        public static string TrabajoFinalizado(
            int trabajoId,
            string cliente,
            string tecnico,
            string tarea)
        {
            return Layout(
                "Trabajo finalizado",
                $"""
            <p>
                El trabajo técnico ha sido finalizado
                correctamente.
            </p>

            <div class="info">
                <p>
                    <strong>Trabajo:</strong>
                    #{trabajoId}
                </p>

                <p>
                    <strong>Cliente:</strong>
                    {cliente}
                </p>

                <p>
                    <strong>Técnico:</strong>
                    {tecnico}
                </p>

                <p>
                    <strong>Tarea:</strong>
                    {tarea}
                </p>

                <p>
                    <strong>Estado:</strong>
                    Finalizado
                </p>
            </div>

            <p>
                El pago correspondiente ha sido registrado
                y el circuito del trabajo ha finalizado.
            </p>
            """
            );
        }


        // ============================================================
        // LAYOUT GENERAL
        // ============================================================

        private static string Layout(
    string titulo,
    string contenido)
        {
            return $$"""
<!DOCTYPE html>
<html>

<head>

    <meta charset="UTF-8">

    <style>

        body {
            font-family: Arial, Helvetica, sans-serif;
            background-color: #f4f6f8;
            margin: 0;
            padding: 0;
        }

        .container {
            max-width: 600px;
            margin: 30px auto;
            background-color: #ffffff;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }

        .header {
            background-color: #1976d2;
            color: #ffffff;
            padding: 20px;
            text-align: center;
        }

        .header h1 {
            margin: 0;
            font-size: 22px;
        }

        .content {
            padding: 30px;
            color: #333333;
            line-height: 1.6;
        }

        .content h2 {
            margin-top: 0;
            color: #1976d2;
        }

        .info {
            background-color: #f5f5f5;
            border-left: 4px solid #1976d2;
            padding: 15px;
            margin: 20px 0;
        }

        .info p {
            margin: 5px 0;
        }

        .footer {
            background-color: #eeeeee;
            padding: 15px;
            text-align: center;
            font-size: 12px;
            color: #666666;
        }

    </style>

</head>

<body>

    <div class="container">

        <div class="header">

            <h1>
                Sistema Técnico
            </h1>

        </div>

        <div class="content">

            <h2>
                {{titulo}}
            </h2>

            {{contenido}}

        </div>

        <div class="footer">

            Este correo fue generado automáticamente
            por el Sistema Técnico.

            <br>

            Por favor, no respondas a este correo.

        </div>

    </div>

</body>

</html>
""";
        }
    }

  }
