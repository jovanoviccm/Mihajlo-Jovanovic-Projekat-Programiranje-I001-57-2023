using Trgovina.Core.Models;

namespace Trgovina.Core.Services;

public class StatistikaArtikla
{
    public Artikal Artikal { get; set; }
    public decimal ProdataKolicina { get; set; }
    public decimal Procenat { get; set; }

    public StatistikaArtikla(
        Artikal artikal,
        decimal prodataKolicina,
        decimal procenat)
    {
        Artikal = artikal;
        ProdataKolicina = prodataKolicina;
        Procenat = procenat;
    }
}

public class StatistikaService
{
    private readonly IReadOnlyList<Racun> racuni;

    public StatistikaService(IReadOnlyList<Racun> racuni)
    {
        this.racuni = racuni;//cuvamo listu svih zatvorenih računa
    }

    public List<StatistikaArtikla> IzracunajZaDatum(
        DateTime datum,
        IReadOnlyList<Artikal> artikli)
    {
        //pronalazimo racune koji pripadaju zadatom datumu
        var racuniZaDatum = racuni
            .Where(r => r.Datum.Date == datum.Date)
            .ToList();

        // ukupna kolicina svih prodatih artikala
        decimal ukupnaKolicina = racuni
            .SelectMany(r => r.Stavke)
            .Sum(s => s.Kolicina);

        var rezultat = new List<StatistikaArtikla>();

        foreach (var artikal in artikli)
        {
            // pronalazimo sve prodaje ovog artikla na zadati datum
            decimal kolicina = racuniZaDatum
                .SelectMany(r => r.Stavke)
                .Where(s => s.Artikal.Id == artikal.Id)
                .Sum(s => s.Kolicina);

            // racunamo procenat od ukupne kolicine
            decimal procenat = ukupnaKolicina == 0
                ? 0
                : kolicina / ukupnaKolicina * 100;

            rezultat.Add(
                new StatistikaArtikla(
                    artikal,
                    kolicina,
                    procenat));
        }

        return rezultat;
    }
}