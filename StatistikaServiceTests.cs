using Trgovina.Core.Models;
using Trgovina.Core.Services;

namespace Trgovina.Tests;

public class StatistikaServiceTests
{
    [Fact] //proverava da statistika pravilno racuna prodatu kolicinu
    public void IzracunajZaDatum_VracaTacnuKolicinu()
    {
        var mleko = new Artikal(
            1,
            "Mleko",
            1,
            150,
            "l");

        var sok = new Artikal(
            2,
            "Sok",
            2,
            180,
            "l");

        var racun1 = new Racun(1)
        {
            Datum = new DateTime(2026, 8, 24, 10, 0, 0)
        };

        racun1.DodajStavku(
            new StavkaRacuna(mleko, 3));

        racun1.DodajStavku(
            new StavkaRacuna(sok, 2));

        var racun2 = new Racun(2)
        {
            Datum = new DateTime(2026, 8, 24, 15, 0, 0)
        };

        racun2.DodajStavku(
            new StavkaRacuna(mleko, 2));

        var racuni = new List<Racun>
        {
            racun1,
            racun2
        };

        var artikli = new List<Artikal>
        {
            mleko,
            sok
        };

        var service = new StatistikaService(racuni);

        var rezultat = service.IzracunajZaDatum(
            new DateTime(2026, 8, 24),
            artikli);

        var statistikaMleka = rezultat
            .First(s => s.Artikal.Id == 1);

        var statistikaSoka = rezultat
            .First(s => s.Artikal.Id == 2);

        Assert.Equal(5, statistikaMleka.ProdataKolicina);
        // mleko: 3 + 2 = 5

        Assert.Equal(2, statistikaSoka.ProdataKolicina);
        // sok: 2
    }


    [Fact] // proverava da se procenat pravilno racuna
    public void IzracunajZaDatum_RacunaTacneProcentе()
    {
        var mleko = new Artikal(
            1,
            "Mleko",
            1,
            150,
            "l");

        var sok = new Artikal(
            2,
            "Sok",
            2,
            180,
            "l");

        var racun = new Racun(1)
        {
            Datum = new DateTime(2026, 8, 24, 12, 0, 0)
        };

        racun.DodajStavku(
            new StavkaRacuna(mleko, 6));

        racun.DodajStavku(
            new StavkaRacuna(sok, 4));

        var racuni = new List<Racun>
        {
            racun
        };

        var artikli = new List<Artikal>
        {
            mleko,
            sok
        };

        var service = new StatistikaService(racuni);

        var rezultat = service.IzracunajZaDatum(
            new DateTime(2026, 8, 24),
            artikli);

        var statistikaMleka = rezultat
            .First(s => s.Artikal.Id == 1);

        var statistikaSoka = rezultat
            .First(s => s.Artikal.Id == 2);

        Assert.Equal(60, statistikaMleka.Procenat);
        // 6 od ukupno 10 = 60%

        Assert.Equal(40, statistikaSoka.Procenat);
        // 4 od ukupno 10 = 40%
    }


    [Fact] // proverava da se racuni drugih datuma ne racunaju
    public void IzracunajZaDatum_IgnoriseDrugeDatume()
    {
        var mleko = new Artikal(
            1,
            "Mleko",
            1,
            150,
            "l");

        var racun = new Racun(1)
        {
            Datum = new DateTime(2026, 8, 23, 12, 0, 0)
        };

        racun.DodajStavku(
            new StavkaRacuna(mleko, 10));

        var racuni = new List<Racun>
        {
            racun
        };

        var artikli = new List<Artikal>
        {
            mleko
        };

        var service = new StatistikaService(racuni);

        var rezultat = service.IzracunajZaDatum(
            new DateTime(2026, 8, 24),
            artikli);

        Assert.Equal(
            0,
            rezultat[0].ProdataKolicina);
        // racun od 23.08. ne sme da utice na statistiku za 24.08.
    }
}