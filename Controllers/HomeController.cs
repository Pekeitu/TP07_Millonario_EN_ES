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

    public IActionResult Player()
    {
        return View();
    }

    [HttpPost]
    public IActionResult IniciarJuego(string nombre)
    {
        //Si el jugador no existe, lo creamos en la base de datos
        Jugador jug = JuegoQQSM.BuscarJugador(nombre);
        if(jug != null) ViewBag.Preexistente = true;
        else { JuegoQQSM.IniciarJuego(nombre); jug = JuegoQQSM.BuscarJugador(nombre); }
        ViewBag.NombreJugador = jug.Nombre;
        
        ViewBag.Pregunta = JuegoQQSM.obtenerProximaPregunta();
        ViewBag.Respuestas = JuegoQQSM.obtenerRespuesta();
        ViewBag.ListaPozo = JuegoQQSM.ListarPozo();
        return View("Pregunta");
    }

    [HttpPost]
    public void ReiniciarJugador()
    {
        string nom = JuegoQQSM.DevolverJugador().Nombre;
        Jugador newJug = new Jugador();
        newJug.Nombre = nom;
        JuegoQQSM.UpdateJugador(newJug);
        return;
    }

    /*[HttpPost]
    public IActionResult PreguntaRespondida(char Opcion1, char Opcion2){

    }
        */
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
