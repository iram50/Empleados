using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using CFE.Models;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace CFE.Services
{
    public class ConstanciaService
    {
        private readonly empresaContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ConstanciaService(empresaContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public (byte[] pdfBytes, string fileName) GenerarConstancia(Grupo grupo)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var empleado = grupo.IdEmpleadoNavigation;
            var curso = grupo.IdCursoNavigation;
            var instructor = grupo.IdInstructorNavigation;
            var calificacion = grupo.Calificacion;
            var fechaActual = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");

            // --- Lógica para cargar el logo de CFE dinámicamente desde la base de datos ---
            byte[] logoCFEBytes = Array.Empty<byte>();
            string logoCFEName = "logo-cfe-capacitaciones.png"; // Nombre de archivo por defecto/fallback

            var configuracion = _context.Configuracion.FirstOrDefault();
            if (configuracion != null && !string.IsNullOrEmpty(configuracion.NombreLogo))
            {
                logoCFEName = configuracion.NombreLogo;
                Console.WriteLine($"[ConstanciaService] Nombre de logo desde DB: {logoCFEName}"); // Mensaje de depuración
            }
            else
            {
                Console.WriteLine($"[ConstanciaService] No se encontró nombre de logo en DB o está vacío. Usando por defecto: {logoCFEName}"); // Mensaje de depuración
            }

            string logoCFEPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", logoCFEName);
            Console.WriteLine($"[ConstanciaService] Ruta completa del logo CFE: {logoCFEPath}"); // Mensaje de depuración

            if (File.Exists(logoCFEPath))
            {
                logoCFEBytes = File.ReadAllBytes(logoCFEPath);
                Console.WriteLine("[ConstanciaService] Logo CFE encontrado y cargado."); // Mensaje de depuración
            }
            else
            {
                Console.WriteLine("[ConstanciaService] Logo CFE NO encontrado en la ruta especificada. Intentando cargar logo por defecto."); // Mensaje de depuración
                string defaultLogoPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "logo-cfe-capacitaciones.png");
                if (File.Exists(defaultLogoPath))
                {
                    logoCFEBytes = File.ReadAllBytes(defaultLogoPath);
                    Console.WriteLine("[ConstanciaService] Logo por defecto encontrado y cargado."); // Mensaje de depuración
                }
                else
                {
                    Console.WriteLine("[ConstanciaService] Error: Logo por defecto (logo-cfe-capacitaciones.png) NO encontrado en wwwroot/images."); // Mensaje de depuración
                }
            }

            // El logo del gobierno sigue siendo estático y se carga de forma directa
            byte[] logoGobiernoBytes = Array.Empty<byte>();
            string gobiernoLogoPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "logo-gobierno-mexico.png");
            Console.WriteLine($"[ConstanciaService] Ruta completa del logo Gobierno: {gobiernoLogoPath}"); // Mensaje de depuración
            if (File.Exists(gobiernoLogoPath))
            {
                logoGobiernoBytes = System.IO.File.ReadAllBytes(gobiernoLogoPath);
                Console.WriteLine("[ConstanciaService] Logo Gobierno encontrado y cargado."); // Mensaje de depuración
            }
            else
            {
                Console.WriteLine("[ConstanciaService] Error: Logo de gobierno (logo-gobierno-mexico.png) NO encontrado en wwwroot/images."); // Mensaje de depuración
            }


            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);

                    page.Header().Column(headerCol =>
                    {
                        headerCol.Spacing(10);

                        headerCol.Item().AlignCenter().Row(row =>
                        {
                            row.Spacing(30);
                            row.AutoItem().Height(50).Image(logoGobiernoBytes);
                            row.AutoItem().Height(50).Image(logoCFEBytes);
                        });

                        headerCol.Item().AlignCenter().Text("La Coordinación de Proyectos de Transmisión y Transformación\na través de la Residencia Regional Noroeste")
                            .FontSize(11).AlignCenter();

                        headerCol.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    });

                    page.Content().Column(col =>
                    {
                        col.Spacing(15);

                        if (calificacion >= 80)
                        {
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
                        else
                        {
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

                        // --- Firmas (reintroducidas con líneas y nombres en negro) ---
                        col.Item().PaddingTop(60).Row(rowFirma =>
                        {
                            rowFirma.RelativeItem().Column(colFirma =>
                            {
                                colFirma.Item().AlignCenter().Text("________________________").FontSize(10).FontColor(Colors.Black);
                                colFirma.Item().AlignCenter().Text($"{instructor?.NombreInstructor ?? "Nombre Instructor"}").FontSize(10).Bold().FontColor(Colors.Black);
                                colFirma.Item().AlignCenter().Text("INSTRUCTOR").FontSize(9).FontColor(Colors.Grey.Darken1);
                            });

                            rowFirma.RelativeItem().Column(colFirma =>
                            {
                                colFirma.Item().AlignCenter().Text("________________________").FontSize(10).FontColor(Colors.Black);
                                colFirma.Item().AlignCenter().Text($"{empleado.Nombre} {empleado.ApellidoPaterno}").FontSize(10).Bold().FontColor(Colors.Black);
                                colFirma.Item().AlignCenter().Text("EMPLEADO").FontSize(9).FontColor(Colors.Grey.Darken1);
                            });
                        });

                    });

                    page.Footer().Column(footerCol =>
                    {
                        footerCol.Spacing(5);
                        footerCol.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                        footerCol.Item().AlignCenter().Text(text =>
                        {
                            text.Span("Página ").FontSize(9).FontColor(Colors.Grey.Medium);
                            text.Span(text.CurrentPageNumber().ToString()).FontSize(9).FontColor(Colors.Grey.Medium);
                            text.Span(" de ").FontSize(9).FontColor(Colors.Grey.Medium);
                            text.Span(text.TotalPages().ToString()).FontSize(9).FontColor(Colors.Grey.Medium);
                        });
                    });
                });
            });

            byte[] pdfBytes = documento.GeneratePdf();

            string nombreCurso = curso?.NombreCurso ?? "CursoDesconocido";
            string nombreEmpleadoCompleto = $"{empleado?.Nombre ?? "EmpleadoDesconocido"}_{empleado?.ApellidoPaterno ?? ""}";

            string invalidChars = Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"[{0}]", invalidChars);
            nombreCurso = Regex.Replace(nombreCurso, invalidRegStr, "_");
            nombreEmpleadoCompleto = Regex.Replace(nombreEmpleadoCompleto, invalidRegStr, "_");

            nombreCurso = Regex.Replace(nombreCurso, @"\+", "_");
            nombreEmpleadoCompleto = Regex.Replace(nombreEmpleadoCompleto, @"\+", "_");

            nombreCurso = nombreCurso.Trim('_');
            nombreEmpleadoCompleto = nombreEmpleadoCompleto.Trim('_');

            string fileName = $"Constancia_{nombreCurso}_{nombreEmpleadoCompleto}.pdf";

            return (pdfBytes, fileName);
        }
    }
}