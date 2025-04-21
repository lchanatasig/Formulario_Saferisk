using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Formulario_Saferisk.Models;
using Formulario_Saferisk.Servicios;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Formulario_Saferisk.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly InsertarService _datosService;

    public HomeController(ILogger<HomeController> logger,InsertarService insertarService)
    {
        _logger = logger;
        _datosService = insertarService;
    }

    public async Task<IActionResult> Index()
    {
        // Llamamos al servicio para obtener la lista de brokers
        var brokers = await _datosService.ListarDatosAsync();

        // Creamos un SelectList donde el valor es "CodAuto" y se muestra "Descripcion"
        ViewBag.Brokers = new SelectList(brokers, "CodAuto", "Descripcion");

        return View();
    }

    [HttpPost("InsertarDatos")]
    public async Task<IActionResult> InsertarDatos([FromBody] Dato request)
    {
        if (request == null)
        {
            return BadRequest(new { message = "Solicitud inválida." });
        }

        try
        {
            ResultadoOperacion resultado = await _datosService.InsertarDatosAsync(
                request.NombresCompletos,
                request.Perfil,
                request.Broker,
                request.Correo,
                request.Ciudad,
                request.Cedula
            );

            if (!resultado.Success)
            {
                // Puedes devolver distintos códigos según el mensaje
                if (resultado.Message.Contains("correo"))
                    return Conflict(new { message = resultado.Message });

                if (resultado.Message.Contains("cédula"))
                    return Conflict(new { message = resultado.Message });

                return BadRequest(new { message = resultado.Message });
            }

            return Ok(new { message = resultado.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al insertar datos.", error = ex.Message });
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
