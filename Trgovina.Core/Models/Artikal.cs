namespace Trgovina.Core.Models;

public class Artikal    //klasa koja predstavlja artikal 
{
    public int Id { get; set; } //jedinstveni identifikator artikla
    public string Naziv { get; set; }   //naziv artikla
    public int KategorijaId { get; set; } //id kategorije kojoj artikal pripada
    public decimal Cena { get; set; }   //cena artikla
    public string JedinicaMere { get; set; }    //jedinica mere , kom,kg...

    public Artikal( //konstruktor za kreiranje artikla
        int id,
        string naziv,
        int kategorijaId,
        decimal cena,
        string jedinicaMere)
    {
        Id = id;
        Naziv = naziv;
        KategorijaId = kategorijaId;
        Cena = cena;
        JedinicaMere = jedinicaMere;
    }

    public Artikal() //prazan konstruktor potreban za xml ucitavanje
    {
    }

    public override string ToString()   //odredjuje kako ce se artikal prikazivati tekstualno
    {
        return $"{Naziv} - {Cena:0.00} din/{JedinicaMere}";
    }
}