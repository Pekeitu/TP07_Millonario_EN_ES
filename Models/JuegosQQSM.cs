namespace TP7.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Dapper; 
static class JuegoQQSM{
    private static int PreguntaActual;
    public static char RespuestaCorrectaActual {get; private set;}
    public static int PosicionPozo {get; private set;}
    private static int PozoAcumuladoSeguro = 0;
    public static List<Pozo> ListaPozo {get; private set;}
    public static Jugador Player {get; private set;}
    private static List<int> ListaPregRes = new List<int>();
    private static int DificultadActual;
    private static bool PreguntaActualRespondida;

    private static string _connectionString = @"Server=DESKTOP-ON8F0P6\SQLEXPRESS;DataBase=JuegoQQSM;Trusted_Connection=True;";

    public static void IniciarJuego(string Nombre){
        DificultadActual = 0;
        Player = new Jugador();
        Player.Nombre = Nombre;
        ListaPozo = new List<Pozo>() {new Pozo(100, false), new Pozo(250, false), new Pozo(500, false), new Pozo(1000, true), new Pozo(2000, false), new Pozo(3500, false), new Pozo(5000, false), new Pozo(10000, true), new Pozo(25000, false), new Pozo(50000, false), new Pozo(100000, false), new Pozo(250000, true), new Pozo(500000, false), new Pozo(1000000, false), new Pozo(2500000, true)};
    }

    public static void ReiniciarPartida()
    {
        PreguntaActual = 0;
        RespuestaCorrectaActual = '\0';
        PosicionPozo = 0;
        PozoAcumuladoSeguro = 0;
        PreguntaActualRespondida = false;
        //Dificultad actual, player y lista pozo seran reseteados en iniciar juego
        return;
    }

    public static List<Jugador> ListarPodio()
    {
        using(SqlConnection db = new SqlConnection(_connectionString))
        {
            string sp = "ListarPodio";
            return db.Query<Jugador>(sp, commandType: CommandType.StoredProcedure).ToList();
        }
    }

    public static void InsertarJugador()
    {
        using(SqlConnection db = new SqlConnection(_connectionString))
        {
            string sp = "InsertarJugador";
            db.Execute(sp, new {Nombre = Player.Nombre, FechaHora = Player.FechaHora, PozoGanado = Player.PozoGanado, ComodinDobleChance = Player.ComodinDobleChance, Comodin50 = Player.Comodin50, ComodinSaltear = Player.ComodinSaltear}, commandType: CommandType.StoredProcedure);
        }
    }

    public static void GuardarJugador(int PosPozo)
    {
        PozoAcumuladoSeguro = PosPozo;
        Player.PozoGanado = ListaPozo[PosPozo].importe;
        InsertarJugador();
        return;
    }

    private static void obtenerIdPreguntasxDif(int dificultad)
    {
        /* listar todas las preguntas que tengan dificultad = x */
        /* Se iguala directo la lista ListaPregRes al resultado de la query */
        using(SqlConnection db = new SqlConnection(_connectionString)) {
            string sp = "obtenerIdPreguntasxDif";
            ListaPregRes = db.Query<int>(sp, new {dif = dificultad}, commandType: CommandType.StoredProcedure).ToList();
        }
        return;
    }

    private static Pregunta buscarPreguntaxId(int id)
    {
        using(SqlConnection db = new SqlConnection(_connectionString)){
            string sp = "obtenerProximaPregunta";
            return db.QueryFirstOrDefault<Pregunta>(sp, new {IdPregunta = id}, commandType: CommandType.StoredProcedure);
        }
    }

    public static Pregunta obtenerProximaPregunta(){
        if(ListaPregRes.Count == 0)
        {
            //Asumimos que ya que estamos aca, el juego no termino. PERO, se acabaron las preguntas de esta dificultad. Cargar siguiente dificultad.
            DificultadActual++; //Para poder testear el programa
            obtenerIdPreguntasxDif(DificultadActual);
        }

        if(ListaPregRes.Count == 0) return (Pregunta)null;
        if(PosicionPozo > 0 && !PreguntaActualRespondida) return buscarPreguntaxId(PreguntaActual);

        PreguntaActualRespondida = false;

        PosicionPozo++;

        Random rnd = new Random();
        int idPregunta = ListaPregRes[rnd.Next() % ListaPregRes.Count];
        PreguntaActual = idPregunta;
        ListaPregRes.Remove(idPregunta);

        /*Modificar SP*/
        return buscarPreguntaxId(idPregunta);
    }

    public static List<Respuesta> obtenerRespuesta(){
        using(SqlConnection db = new SqlConnection(_connectionString)){
         string sp = "obtenerRespuestaCorrecta";
         RespuestaCorrectaActual = db.QueryFirstOrDefault<char>(sp, new {PregActual = PreguntaActual}, commandType: CommandType.StoredProcedure);
         
         sp = "obtenerRespuestas";
         return db.Query<Respuesta>(sp, new {PregActual = PreguntaActual}, commandType: CommandType.StoredProcedure).ToList();
        }
    }

    public static bool comprobarRespuesta(char Opcion)
    {
        if(RespuestaCorrectaActual == Opcion) PreguntaActualRespondida = true;
        return RespuestaCorrectaActual == Opcion;
    }
    
    public static List<char> descartar50(){
        if(!Player.Comodin50) return (List<char>)null;
        Player.Comodin50 = false;

        List<Respuesta> respuestasActuales = obtenerRespuesta();
        int totalBorrados = 0;
        bool[] dpBorrados = new bool[respuestasActuales.Count];
        while(totalBorrados<2){
            Random rnd = new Random();            
            int rnd_idx = rnd.Next(0, respuestasActuales.Count);
            if(!respuestasActuales[rnd_idx].correcta && !dpBorrados[rnd_idx]){
                totalBorrados++;
                dpBorrados[rnd_idx] = true;
            }
        }

        //Conseguir Opciones descartadas
        List<char> ret = new List<char>();
        for(int i = 0; i < dpBorrados.Length; i++)
        {
            if(dpBorrados[i]) ret.Add((char)(((int)'A') + i)); 
        }
        return ret;
    }
    public static bool SaltearPregunta(){
        if(!Player.ComodinSaltear) return false;
        Player.ComodinSaltear = false;
        PreguntaActualRespondida = true;

        return true;
    }

    public static bool ComodinDobleChance() {
        if(!Player.ComodinDobleChance) return false;
        Player.ComodinDobleChance = false;

        return true;
    }
}