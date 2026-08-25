namespace Trgovina.Core.Models;

public class Racun //klasa koja predstavlja racun
{
    public int Id{ get; set; } //id racuna
    public DateTime Datum{ get; set; } //datum i vreme racuna
    public List<StavkaRacuna>Stavke{ get; set;} //lista stavki koje se nalaze na racunu
    public decimal Ukupno => Stavke.Sum(s => s.Ukupno); //ukupna cena na racunu
    public Racun(int id) //konstruktor za kreiranje novog racuna
    {
        Id = id;
        Datum = DateTime.Now;
        Stavke = new List<StavkaRacuna>();
    }

    public Racun () //prazan konstruktor potreban za xml ucitavanje
    {
        Stavke = new List<StavkaRacuna>();
    }

    public void DodajStavku(StavkaRacuna stavka) //dodavanje stavke na racun
    {
        Stavke.Add(stavka);
    }
    
    public void ObrisiStavku(StavkaRacuna stavka) //brisanje stavke sa racuna
    {
        Stavke.Remove(stavka);
    }
}