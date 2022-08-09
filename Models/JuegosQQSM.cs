namespace TP7.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Dapper; 
static class JuegoQQSM{
    private static int PreguntaActual;
    private static char RespuestaCorrectaActual;
    private static int PosicionPozo;
    private static int PozoAcumuladoSeguro;
    private static int PozoAcumulado;
    private static bool Comodin5050=true, ComodinDobleChance=true, ComodinSaltearPregunta=true;
    private static List<Pozo> ListaPozo;
    private static Jugador Player;

    public static string IniciarJuego(string Nombre){
        return "";
    }
}