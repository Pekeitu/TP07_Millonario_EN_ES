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
        PozoGanado=-1;
        ComodinDobleChance=true;
        Comodin50=true;
        ComodinSaltear=true;
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