using CFE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TuProyecto.Models;

public class DocumentosController : Controller
{
    private readonly empresaContext _context;

    public DocumentosController(empresaContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Documento documento, IFormFile archivo)
    {
        if (archivo != null && archivo.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                await archivo.CopyToAsync(memoryStream);
                documento.Archivo = memoryStream.ToArray();
            }
        }

        if (ModelState.IsValid)
        {
            _context.Documentos.Add(documento);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Empleado", new { id = documento.EmpleadoId });
        }

        return View(documento);
    }

    public async Task<IActionResult> Download(int id)
    {
        var doc = await _context.Documentos.FindAsync(id);
        if (doc == null)
            return NotFound();

        return File(doc.Archivo, "application/pdf", doc.Nombre + ".pdf");
    }

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult DeleteDocumento(int id)
{
    var documento = _context.Documentos.FirstOrDefault(d => d.Id == id);
    if (documento == null)
    {
        return Json(new { success = false, message = "Documento no encontrado." });
    }

    _context.Documentos.Remove(documento);
    _context.SaveChanges();

    return Json(new { success = true });
}



}
