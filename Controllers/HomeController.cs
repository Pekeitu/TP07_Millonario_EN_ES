using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP7.Models;

namespace TP7.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult IniciarJuego(string nombre)
    {
        //Si el jugador no existe, lo creamos en la base de datos
        Jugador jug = JuegoQQSM.BuscarJugador(nombre);
        if(jug == null) JuegoQQSM.IniciarJuego(nombre);
        else ViewBag.Preexistente = true;
        ViewBag.NombreJugador = jug.Nombre;
        //ViewBag.Pregunta = JuegoQQSM.obtenerProximaPregunta();
        //ViewBag.Respuestas = JuegoQQSM.obtenerRespuesta();
        //ViewBag.ListaPozo = JuegoQQSM.ListarPozo();
        return View("Pregunta");
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
