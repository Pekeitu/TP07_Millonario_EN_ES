namespace TP7.Models;

public class Jugador{
    public int IdJugador{get; set;}
    public string Nombre{get; set;}
    public DateTime FechaHora{get;set;}
    public int PozoGanado{get;set;}
    public bool ComodinDobleChance{get;set;}
    public bool Comodin50{get;set;}
    public bool ComodinSaltear{get;set;}

    public Jugador(){
        IdJugador= 0;
        Nombre="";
        FechaHora=DateTime.Now;
        PozoGanado=0;
        ComodinDobleChance=true;
        Comodin50=true;
        ComodinSaltear=true;
    }

    public Jugador(Jugador jug)
    {
        IdJugador = jug.IdJugador;
        Nombre = jug.Nombre;
        FechaHora = jug.FechaHora;
        PozoGanado = jug.PozoGanado;
        ComodinDobleChance = jug.ComodinDobleChance;
        Comodin50 = jug.Comodin50;
        ComodinSaltear = jug.ComodinSaltear;
    }

    public Jugador(int id, string nom, DateTime fec, int pozGan, bool comDC, bool Com50, bool ComSal){
        IdJugador= id;
        Nombre=nom;
        FechaHora=fec;
        PozoGanado=pozGan;
        ComodinDobleChance=comDC;
        Comodin50=Com50;
        ComodinSaltear=ComSal;
    }
}