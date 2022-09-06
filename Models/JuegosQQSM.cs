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
    //private static bool Comodin5050=true, ComodinDobleChance=true, ComodinSaltearPregunta=true;
    private static List<Pozo> ListaPozo = new List<Pozo>() {new Pozo(100, false), new Pozo(250, false), new Pozo(500, false), new Pozo(1000, true), new Pozo(2000, false), new Pozo(3500, false), new Pozo(5000, false), new Pozo(10000, true), new Pozo(25000, false), new Pozo(50000, false), new Pozo(100000, false), new Pozo(250000, true), new Pozo(500000, false), new Pozo(1000000, false), new Pozo(2500000, true)};
    private static Jugador Player;
    private static List<int> ListaPregRes = new List<int>();
    private static int DificultadActual;

    private static string _connectionString = @"Server=DESKTOP-78D5FAT\SQLEXPRESS;DataBase=JuegoQQSM;Trusted_Connection=True;";

    public static void IniciarJuego(string Nombre){
        DificultadActual = 0;
        Player = new Jugador();
        Player.Nombre = Nombre;

        using(SqlConnection db = new SqlConnection(_connectionString)){
            string sp = "insertarJugador";
            int num= db.Execute(sp, new {Nombre = Player.Nombre, FechaHora = Player.FechaHora, PozoGanado = Player.PozoGanado, ComodinDobleChance = Player.ComodinDobleChance, Comodin50 = Player.Comodin50, ComodinSaltear = Player.ComodinSaltear}, commandType: CommandType.StoredProcedure);
            //int num = db.Execute(sp, new {Player}, commandType: CommandType.StoredProcedure);
        }
    }

    public static Jugador UpdateJugador(Jugador jug){
        Player = jug;
        //Seguir
        using(SqlConnection db = new SqlConnection(_connectionString)){
            string sp = "UpdateJugador";
            return db.QueryFirstOrDefault<Jugador>(sp, new {Nombre = jug.Nombre}, commandType: CommandType.StoredProcedure);
        }
    }

    public static Jugador BuscarJugador(string nom) {
        using(SqlConnection db = new SqlConnection(_connectionString)) {
            string sp = "buscarJugador";
            return db.QueryFirstOrDefault<Jugador>(sp, new {Nombre = nom}, commandType: CommandType.StoredProcedure);
        }
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

    public static Pregunta obtenerProximaPregunta(){
        if(ListaPregRes.Count == 0)
        {
            //Asumimos que ya que estamos aca, el juego no termino. PERO, se acabaron las preguntas de esta dificultad. Cargar siguiente dificultad.
            DificultadActual = 1; //Para poder testear el programa
            obtenerIdPreguntasxDif(DificultadActual);
        }
        Random rnd = new Random();
        int idPregunta = ListaPregRes[rnd.Next() % ListaPregRes.Count];
        PreguntaActual = idPregunta;
        ListaPregRes.Remove(idPregunta);

        /*Modificar SP*/
        using(SqlConnection db = new SqlConnection(_connectionString)){
            string sp = "obtenerProximaPregunta";
            return db.QueryFirstOrDefault<Pregunta>(sp, new {IdPregunta = idPregunta}, commandType: CommandType.StoredProcedure);
        }
    }

    public static List<Respuesta> obtenerRespuesta(){
        using(SqlConnection db = new SqlConnection(_connectionString)){
         string sp = "obtenerRespuestaCorrecta";
         RespuestaCorrectaActual = db.QueryFirstOrDefault<char>(sp, new {PregActual = PreguntaActual}, commandType: CommandType.StoredProcedure);
         
         sp = "obtenerRespuestas";
         return db.Query<Respuesta>(sp, new {PregActual = PreguntaActual}, commandType: CommandType.StoredProcedure).ToList();
        }
    }

    public static char obtenerRespuesaCorrecta()
    {
        return RespuestaCorrectaActual;
    }

    public static List<Pozo> ListarPozo(){
        return ListaPozo;
    }

    public static int  devolverPosicionPozo(){
        return PosicionPozo;
    }
    
    public static List<char> descartar50(){
        if(!Player.Comodin50) return (List<char>)null;
        Player.Comodin50 = false;
        using(SqlConnection db = new SqlConnection(_connectionString)){
            string sp = "descartar50";
            int num = db.Execute(sp, new {idJug = Player.IdJugador}, commandType: CommandType.StoredProcedure);
        }

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
            if(dpBorrados[i]) ret.Add((char)((int)'A' + i)); 
        }
        return ret;
    }
    public static void SaltearPregunta(){
        if(!Player.ComodinSaltear) return;
        /*FALTA EL SP*/
        Player.ComodinSaltear = false;
        using(SqlConnection db = new SqlConnection(_connectionString)){
            string sp = "SaltearPregunta";
            int num = db.Execute(sp, new {idJug = Player.IdJugador}, commandType: CommandType.StoredProcedure);
        }
        obtenerProximaPregunta();
        return;
    }

    public static Jugador DevolverJugador(){
        return Player;
    }
}