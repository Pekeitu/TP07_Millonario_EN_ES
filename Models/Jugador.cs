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
        FechaHora=DateTime.Parse("0000-00-00 00:00:00");
        PozoGanado=0;
        ComodinDobleChance=false;
        Comodin50=false;
        ComodinSaltear=false;
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