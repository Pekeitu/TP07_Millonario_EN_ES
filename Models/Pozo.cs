namespace TP7.Models;

public class Pozo{
    public int importe{get; set;}
    public bool valorSeguro{get; set;}

    public Pozo(int imp, bool vSeg){
        importe = imp;
        valorSeguro = vSeg;
    }

}