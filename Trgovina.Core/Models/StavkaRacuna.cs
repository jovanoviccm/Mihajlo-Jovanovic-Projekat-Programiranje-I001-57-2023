namespace Trgovina.Core.Models;

public class StavkaRacuna //predstavlja jedan artikal i kolicinu na racunu
{
    public Artikal Artikal{ get; set; } // artikal koji je kupac izabrao
    public decimal Kolicina{ get; set;} //kolicina izabranog artikla

    public decimal Ukupno => Artikal.Cena * Kolicina; // ukupna cena (artikal + kolicina)

    public StavkaRacuna(Artikal artikal, decimal kolicina) //konstruktor za kreiranje stavke racuna
    {
        Artikal = artikal;
        Kolicina = kolicina;
    }
    public StavkaRacuna()
    {
        
    }

    public override string ToString() //prikaz stavke
    {
        return $"{Artikal.Naziv} * {Kolicina} = {Ukupno:0.00} din";
    }
}
