using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using CFE.Models;
using System.Globalization;


namespace CFE.Services
{

    public class ConstanciaService
    {
        public byte[] GenerarConstancia(Empleadocurso empleadoCurso)
{

            QuestPDF.Settings.License = LicenseType.Community;

            var empleado = empleadoCurso.IdEmpleadoNavigation;
            var grupo = empleadoCurso.ClaveGrupoNavigation;
            var curso = grupo?.IdCursoNavigation;
            var instructor = grupo?.IdInstructorNavigation;

            var calificacion = empleadoCurso.Calificacion;

            var fechaActual = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("es-MX"));

    var documento = Document.Create(container =>
    {
        container.Page(page =>
        {
            page.Margin(40);
            page.Size(PageSizes.A4);

            page.Content().Column(col =>
            {
                col.Spacing(20);

                col.Item().Text("CONSTANCIA DE ACREDITACIÓN").FontSize(20).Bold().AlignCenter();

                col.Item().Text($"Se hace constar que el(la) C. {empleado.Nombre} {empleado.ApellidoPaterno} {empleado.ApellidoMaterno}, con número de empleado {empleado.rpe}, ha acreditado satisfactoriamente el curso \"{curso?.NombreCurso}\" impartido por el instructor {instructor?.NombreInstructor}, con una calificación de {calificacion}.");

                col.Item().Text($"El curso se llevó a cabo del {grupo?.FechaInicial:dd/MM/yyyy} al {grupo?.FechaFinal:dd/MM/yyyy} en la sede {grupo?.Lugar}.");

col.Item().Text($"Se extiende la presente constancia el día {fechaActual}.");


                col.Item().PaddingTop(50).Row(row =>
                {
                    row.RelativeItem().AlignCenter().Text("______________________");
                    row.RelativeItem().AlignCenter().Text("______________________");
                });

                col.Item().Row(row =>
                {
                    row.RelativeItem().AlignCenter().Text("Instructor");
                    row.RelativeItem().AlignCenter().Text("Empleado");
                });
            });
        });
    });

    return documento.GeneratePdf();
        }
    }
}
