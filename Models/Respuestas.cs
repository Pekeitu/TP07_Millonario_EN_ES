namespace TP7.Models;

public class Respuestas{
    public int idRespuesta{get; set;}
    public int idPregunta{get;}
    public char opcionRespuesta{get;}
    public string txtRespuesta{get;}
    public bool correcta{get;set;}
    
    public Respuestas(){
        idRespuesta=-1;
        idPregunta=-1;
        opcionRespuesta=' ';
        txtRespuesta="";
        correcta=false;
    }
    public Respuestas(int idR, int idP, char opResp, string txtResp, bool cor){
        idRespuesta=idR;
        idPregunta=idP;
        opcionRespuesta = opResp;
        txtRespuesta = txtResp;
        correcta = cor;
    }
}