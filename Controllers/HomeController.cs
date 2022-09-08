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
        bool preexistente = false;
        if(jug != null) preexistente = true;
        else { JuegoQQSM.IniciarJuego(nombre); jug = JuegoQQSM.BuscarJugador(nombre); }
        
        return Redirect(Url.Action("CargarPagina", "Home", new {preexistente, nombre}));
    }

    //Se pasa pregunta por parametro, para que no se pueda avanzar preguntas recargando la pagina
    public IActionResult CargarPagina(bool preexistente, string nombre)
    {
        ViewBag.Nombre = nombre;
        ViewBag.Preexistente = preexistente;
        ViewBag.Pregunta = JuegoQQSM.obtenerProximaPregunta();
        ViewBag.Respuestas = JuegoQQSM.obtenerRespuesta();
        ViewBag.ListaPozo = JuegoQQSM.ListarPozo();
        ViewBag.PosPozo = JuegoQQSM.devolverPosicionPozo()-1;
        return View("Pregunta");
    }

    [HttpPost]
    public JsonResult CargarSiguientePreguntaAjax() //returns true on game end.
    {
        //PosPozo devuelve la posicion de la pregunta que acaba de ser respondida
        //PozoSeguro devuelve si la ultima pregunta respondida era una segura
        //el resto es sobre la nueva pregunta
        return Json(new {PosPozo = JuegoQQSM.devolverPosicionPozo()-1, PozoSeguro = JuegoQQSM.ListarPozo()[JuegoQQSM.devolverPosicionPozo()-1].valorSeguro, Pregunta = JuegoQQSM.obtenerProximaPregunta(), Respuestas = JuegoQQSM.obtenerRespuesta()});
    }

    [HttpPost]
    public JsonResult GuardarJugadorAjax()
    {
        int q = JuegoQQSM.ActualizarJugadorSobreSeguro(JuegoQQSM.devolverPosicionPozo()-1);
        return Json(q);
    }

    [HttpPost]
    public void ReiniciarJugador()
    {
        string nom = JuegoQQSM.DevolverJugador().Nombre;
        Jugador newJug = new Jugador();
        newJug.Nombre = nom;
        JuegoQQSM.UpdateJugador();
        return;
    }

    [HttpPost]
    public JsonResult comprobarRespuesta(char Opcion){ //Llamado x ajax
        return Json(JuegoQQSM.comprobarRespuesta(Opcion));
    }
    public IActionResult PantallaFinDelJuego(){
        Jugador jug = JuegoQQSM.DevolverJugador();
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
