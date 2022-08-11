namespace TP7.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Dapper; 
static class JuegoQQSM{
    private static int PreguntaActual;
    private static char RespuestaCorrectaActual;
    private static int PosicionPozo;
    private static int PozoAcumuladoSeguro;
    private static int PozoAcumulado;
    private static bool Comodin5050=true, ComodinDobleChance=true, ComodinSaltearPregunta=true;
    private static List<Pozo> ListaPozo = new List<Pozo>();
    private static Jugador Player;

    private static string _connectionString = @"Server=A-phz2-cidi-048\SQL;DataBase=JuegoQQSM;Trusted_Connection=True;";
    public static void IniciarJuego(string Nombre){
        Player = new Jugador();
        Player.Nombre = Nombre;
        Player.FechaHora = DateTime.Now;
        using(SqlConnection db = new SqlConnection(_connectionString)){
            string sp = "InicializarPlayer";
            int num= db.Execute(sp, new {Player}, commandType: CommandType.StoredProcedure);
        }
    }
    public static Pregunta obtenerProximaPregunta(){
        PreguntaActual++;
        using(SqlConnection db = new SqlConnection(_connectionString)){
         string sp = "obtenerProximaPregunta";
         return db.QueryFirstOrDefault<Pregunta>(sp, new {PregActual = PreguntaActual}, commandType: CommandType.StoredProcedure);
        }
    }

    public static List<Respuesta> obtenerRespuesta(){
        using(SqlConnection db = new SqlConnection(_connectionString)){
         string fQuery = "SELECT opcionRespuesta FROM Respuestas WHERE idPregunta = @idpreg AND correcta = 1";
         RespuestaCorrectaActual = db.QueryFirstOrDefault<char>(fQuery, new {idpreg = PreguntaActual});

         string sp = "obtenerRespuestas";
         return db.Query<Respuesta>(sp, new {PregActual = PreguntaActual}, commandType: CommandType.StoredProcedure).ToList();
        }
    }
}