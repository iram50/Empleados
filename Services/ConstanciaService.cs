using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using CFE.Models;


namespace CFE.Services
{
    public class ConstanciaService
    {
        public byte[] GenerarConstancia(Empleadocurso empleadocurso)
        {

            QuestPDF.Settings.License = LicenseType.Community;

            var empleado = empleadocurso.IdEmpleadoNavigation;
            var grupo = empleadocurso.ClaveGrupoNavigation;
            var curso = grupo.IdCursoNavigation;
            var instructor = grupo.IdInstructorNavigation;
            var calificacion = grupo.Calificacion;
            var fechaActual = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
            var logoCFEBytes = System.IO.File.ReadAllBytes("wwwroot/images/logo-cfe-capacitaciones.png");
            var logoGobiernoBytes = System.IO.File.ReadAllBytes("wwwroot/images/logo-gobierno-mexico.png");

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
                        // Logos centrados arriba
                        col.Item().AlignCenter().Row(row =>
                        {
                            row.Spacing(30); // Espacio entre logos

                            row.AutoItem().Height(50).Image(logoGobiernoBytes);
                            row.AutoItem().Height(50).Image(logoCFEBytes);

                        });

                        col.Item().AlignCenter().Text("La Coordinación de Proyectos de Transmisión y Transformación\na través de la Residencia Regional Noroeste")
                            .FontSize(11).AlignCenter();

                        col.Item().AlignCenter().Text("Otorga el presente").FontSize(10);

                        // Título
                        col.Item().AlignCenter().Text("Reconocimiento")
                            .FontSize(26).Bold().FontColor(Colors.Green.Medium);

                        // Nombre
                        col.Item().AlignCenter().Text($"a {empleado.Nombre} {empleado.ApellidoPaterno} {empleado.ApellidoMaterno}")
                            .FontSize(16).Bold().FontColor(Colors.Grey.Darken3);

                        // Descripción
                        col.Item().AlignCenter().Text(text =>
                        {
                            text.Span("por haber acreditado el curso ").FontSize(11);
                            text.Span($"\"{curso?.NombreCurso}\"").Italic().FontSize(11);
                            text.Span($" impartido por {instructor?.NombreInstructor},").FontSize(11);
                            text.Span($" con una calificación de {calificacion}.").FontSize(11);
                        });

                        // Título del curso (resaltado)
                        col.Item().AlignCenter().Text($"{curso?.NombreCurso}")
                            .FontSize(18).Bold().FontColor(Colors.Green.Darken2);

                        // Fechas
                        col.Item().AlignCenter().Text(text =>
                        {
                            text.Span("Realizado del ").FontSize(11);
                            text.Span($"{grupo?.FechaInicial:dd 'de' MMMM} al {grupo?.FechaFinal:dd 'de' MMMM 'de' yyyy}")
                                .FontSize(11).FontColor(Colors.Purple.Medium);
                        });


                        col.Item().AlignCenter().Text("Modalidad presencial").FontSize(11).FontColor(Colors.Blue.Medium);

                        // Lugar y fecha
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

            return documento.GeneratePdf();
        }
    }
}