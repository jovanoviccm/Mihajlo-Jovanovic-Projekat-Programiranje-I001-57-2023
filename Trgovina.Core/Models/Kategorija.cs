namespace Trgovina.Core.Models;

//klasa koja predstavlja kategoriju artikla
public class Kategorija
{
    public int Id { get; set; } //jedinstveni identifikator kategorije
    public string Naziv { get; set; }   //naziv kategorije

        
    public Kategorija(int id, string naziv)// konstruktor za normalno kreiranje kategorije
    {
        Id = id;
        Naziv = naziv;
    }
    public Kategorija() //prazan konstruktor potreban za xml ucitavanje
    {

    }

    public override string ToString() //odredjuje kako ce se kategorija prikazivati tekstualno
    {
        return $"{Id} - {Naziv}";
    }


}