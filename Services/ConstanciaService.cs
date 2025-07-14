using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using CFE.Models;
using System.Text.RegularExpressions;
using System.IO; // Necesario para Path.Combine y File.ReadAllBytes
using Microsoft.EntityFrameworkCore; // Necesario para .FirstOrDefault()
using Microsoft.AspNetCore.Hosting; // Necesario para IWebHostEnvironment

namespace CFE.Services
{
    public class ConstanciaService
    {
        private readonly empresaContext _context; // Para acceder a la base de datos
        private readonly IWebHostEnvironment _webHostEnvironment; // Para obtener la ruta física de wwwroot

        // Constructor: Ahora inyectamos el contexto de la base de datos y el entorno web
        public ConstanciaService(empresaContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public (byte[] pdfBytes, string fileName) GenerarConstancia(Empleadocurso empleadocurso)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var empleado = empleadocurso.IdEmpleadoNavigation;
            var grupo = empleadocurso.ClaveGrupoNavigation;
            var curso = grupo.IdCursoNavigation;
            var instructor = grupo.IdInstructorNavigation;
            var calificacion = grupo.Calificacion;
            var fechaActual = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");

            // --- Lógica para cargar el logo de CFE dinámicamente desde la base de datos ---
            byte[] logoCFEBytes = null;
            string logoCFEName = "logo-cfe-capacitaciones.png"; // Nombre de archivo por defecto/fallback

            // Intenta obtener el nombre del logo de la configuración de la base de datos
            var configuracion = _context.Configuracion.FirstOrDefault();
            if (configuracion != null && !string.IsNullOrEmpty(configuracion.NombreLogo))
            {
                logoCFEName = configuracion.NombreLogo;
            }

            // Construye la ruta completa al archivo del logo en wwwroot/images
            string logoCFEPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", logoCFEName);

            // Verifica si el archivo del logo existe antes de intentar leerlo
            if (File.Exists(logoCFEPath))
            {
                logoCFEBytes = File.ReadAllBytes(logoCFEPath);
            }
            else
            {
                // Si el logo dinámico no se encuentra, usa el logo por defecto
                // Asegúrate de que "logo-cfe-capacitaciones.png" exista en wwwroot/images
                logoCFEBytes = File.ReadAllBytes(Path.Combine(_webHostEnvironment.WebRootPath, "images", "logo-cfe-capacitaciones.png"));
            }

            // El logo del gobierno sigue siendo estático y se carga de forma directa
            // Se usa Path.Combine para mayor robustez en la ruta
            var logoGobiernoBytes = System.IO.File.ReadAllBytes(Path.Combine(_webHostEnvironment.WebRootPath, "images", "logo-gobierno-mexico.png"));


            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);

                    page.Content().Column(col =>
                    {
                        col.Spacing(15);

                        // Logos centrados arriba
                        col.Item().AlignCenter().Row(row =>
                        {
                            row.Spacing(30);
                            row.AutoItem().Height(50).Image(logoGobiernoBytes);
                            row.AutoItem().Height(50).Image(logoCFEBytes); // Usa el logo dinámico
                        });

                        col.Item().AlignCenter().Text("La Coordinación de Proyectos de Transmisión y Transformación\na través de la Residencia Regional Noroeste")
                            .FontSize(11).AlignCenter();

                        // Lógica condicional para el tipo de constancia
                        if (calificacion >= 80)
                        {
                            // *** Contenido para ACREDITACIÓN (Calificación >= 80) ***
                            col.Item().AlignCenter().Text("Otorga el presente").FontSize(10);
                            col.Item().AlignCenter().Text("Reconocimiento")
                                .FontSize(26).Bold().FontColor(Colors.Green.Medium);

                            col.Item().AlignCenter().Text($"a {empleado.Nombre} {empleado.ApellidoPaterno} {empleado.ApellidoMaterno}")
                                .FontSize(16).Bold().FontColor(Colors.Grey.Darken3);

                            col.Item().AlignCenter().Text(text =>
                            {
                                text.Span("por haber acreditado el curso ").FontSize(11);
                                text.Span($"\"{curso?.NombreCurso}\"").Italic().FontSize(11);
                                text.Span($" impartido por {instructor?.NombreInstructor},").FontSize(11);
                                text.Span($" con una calificación de {calificacion}.").FontSize(11);
                            });

                            col.Item().AlignCenter().Text($"{curso?.NombreCurso}")
                                .FontSize(18).Bold().FontColor(Colors.Green.Darken2);

                            col.Item().AlignCenter().Text(text =>
                            {
                                text.Span("Realizado del ").FontSize(11);
                                text.Span($"{grupo?.FechaInicial:dd 'de' MMMM} al {grupo?.FechaFinal:dd 'de' MMMM 'de' yyyy}")
                                    .FontSize(11).FontColor(Colors.Purple.Medium);
                            });
                        }
                        else // Calificaciones menores a 80
                        {
                            // *** Contenido para PARTICIPACIÓN (Calificación < 80) ***
                            col.Item().AlignCenter().Text("Otorga el presente").FontSize(10);
                            col.Item().AlignCenter().Text("Constancia de Participación")
                                .FontSize(26).Bold().FontColor(Colors.Blue.Medium);

                            col.Item().AlignCenter().Text($"a {empleado.Nombre} {empleado.ApellidoPaterno} {empleado.ApellidoMaterno}")
                                .FontSize(16).Bold().FontColor(Colors.Grey.Darken3);

                            col.Item().AlignCenter().Text(text =>
                            {
                                text.Span("por haber participado en el curso ").FontSize(11);
                                text.Span($"\"{curso?.NombreCurso}\"").Italic().FontSize(11);
                                text.Span($" impartido por {instructor?.NombreInstructor}.").FontSize(11);
                            });

                            col.Item().AlignCenter().Text($"{curso?.NombreCurso}")
                                .FontSize(18).Bold().FontColor(Colors.Blue.Darken2);

                            col.Item().AlignCenter().Text(text =>
                            {
                                text.Span("Realizado del ").FontSize(11);
                                text.Span($"{grupo?.FechaInicial:dd 'de' MMMM} al {grupo?.FechaFinal:dd 'de' MMMM 'de' yyyy}")
                                    .FontSize(11).FontColor(Colors.Purple.Medium);
                            });
                        }

                        col.Item().AlignCenter().Text("Modalidad presencial").FontSize(11).FontColor(Colors.Blue.Medium);
                        col.Item().AlignCenter().Text($"{grupo?.Lugar}, a {fechaActual}").FontSize(11).Italic();

                        // Firmas
                        col.Item().PaddingTop(40).Row(row =>
                        {
                            row.RelativeItem().AlignCenter().Column(colFirma =>
                            {
                                colFirma.Item().Text("_");
                                colFirma.Item().Text($"{instructor?.NombreInstructor}").FontSize(10).AlignCenter();
                                colFirma.Item().Text("Instructor").FontSize(9).AlignCenter();
                            });

                            row.RelativeItem().AlignCenter().Column(colFirma =>
                            {
                                colFirma.Item().Text("_");
                                colFirma.Item().Text($"{empleado.Nombre} {empleado.ApellidoPaterno}").FontSize(10).AlignCenter();
                                colFirma.Item().Text("Empleado").FontSize(9).AlignCenter();
                            });
                        });
                    });
                });
            });

            byte[] pdfBytes = documento.GeneratePdf();

            // Lógica para construir el nombre del archivo (sin cambios aquí)
            string nombreCurso = curso?.NombreCurso ?? "CursoDesconocido";
            string nombreEmpleadoCompleto = $"{empleado?.Nombre ?? "EmpleadoDesconocido"}_{empleado?.ApellidoPaterno ?? ""}";

            string invalidChars = Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"[{0}]", invalidChars);
            nombreCurso = Regex.Replace(nombreCurso, invalidRegStr, "_");
            nombreEmpleadoCompleto = Regex.Replace(nombreEmpleadoCompleto, invalidRegStr, "_");

            nombreCurso = nombreCurso.Replace(" ", "_");
            nombreEmpleadoCompleto = nombreEmpleadoCompleto.Replace(" ", "_");

            nombreCurso = Regex.Replace(nombreCurso, @"+", "_");
            nombreEmpleadoCompleto = Regex.Replace(nombreEmpleadoCompleto, @"+", "_");

            nombreCurso = nombreCurso.Trim('_');
            nombreEmpleadoCompleto = nombreEmpleadoCompleto.Trim('_');

            string fileName = $"Constancia_{nombreCurso}_{nombreEmpleadoCompleto}.pdf";

            return (pdfBytes, fileName);
        }
    }
}