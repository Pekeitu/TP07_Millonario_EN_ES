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
        JuegoQQSM.IniciarJuego(nombre);
        return Redirect(Url.Action("CargarPagina", "Home", new {nombre}));
    }

    //Se pasa pregunta por parametro, para que no se pueda avanzar preguntas recargando la pagina
    public IActionResult CargarPagina(string nombre)
    {
        ViewBag.Nombre = nombre;
        ViewBag.Pregunta = JuegoQQSM.obtenerProximaPregunta();
        ViewBag.Respuestas = JuegoQQSM.obtenerRespuesta();
        ViewBag.ListaPozo = JuegoQQSM.ListaPozo;
        ViewBag.PosPozo = JuegoQQSM.PosicionPozo-1;
        return View("Pregunta");
    }

    [HttpPost]
    public JsonResult CargarSiguientePreguntaAjax() //returns true on game end.
    {
        //PosPozo devuelve la posicion de la pregunta que acaba de ser respondida
        //PozoSeguro devuelve si la ultima pregunta respondida era una segura
        //el resto es sobre la nueva pregunta
        return Json(new {PosPozo = JuegoQQSM.PosicionPozo-1, PozoSeguro = JuegoQQSM.ListaPozo[JuegoQQSM.PosicionPozo-1].valorSeguro, Pregunta = JuegoQQSM.obtenerProximaPregunta(), Respuestas = JuegoQQSM.obtenerRespuesta()});
    }

    [HttpPost]
    public JsonResult GuardarJugadorAjax()
    {
        //Le restamos dos devido a que: Siempre que uno esta con una pregunta, posicion pozo apunta a la siguiente. Si a eso se le suma que antes de guardar sobre el pozo seguro, cargamos la siguiente pregunta, posicion pozo es +2 de lo que realmente queremos
        JuegoQQSM.GuardarJugador(JuegoQQSM.PosicionPozo-2);
        return Json(null);
    }

    [HttpPost]
    public JsonResult comprobarRespuesta(char Opcion){ //Llamado x ajax
        return Json(new {Correcta = JuegoQQSM.comprobarRespuesta(Opcion), PozoSeguro = JuegoQQSM.ListaPozo[JuegoQQSM.PosicionPozo-1].valorSeguro});
    }
    public IActionResult PantallaFinDelJuego(){
        Jugador jug = JuegoQQSM.Player;
        ViewBag.Pozo = jug.PozoGanado;
        ViewBag.Nombre = jug.Nombre;
        return View();
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
