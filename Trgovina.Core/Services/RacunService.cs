using Trgovina.Core.Models;

namespace Trgovina.Core.Services;

public class RacunService // servis zaduzen za poslovnu logiku vezanu za racune
{
    private readonly List<Racun> racuni = new();
    // privatna lista svih kreiranih racuna

    public IReadOnlyList<Racun> Racuni => racuni;// omogućava drugim klasama da citaju listu racuna,
    // ali ne mogu direktno da je menjaju

    public Racun KreirajRacun()
    {
        int noviId = racuni.Count + 1;
        // određujemo id novog računa

        var racun = new Racun(noviId);
        // kreiramo novi račun

        racuni.Add(racun);
        // dodajemo račun u listu

        return racun;
        // vraćamo kreirani račun
    }

    public void DodajStavku(
        Racun racun,
        Artikal artikal,
        decimal kolicina)
    {
        if (kolicina <= 0)
            throw new ArgumentException(
                "Kolicina mora biti veca od nule.");

        var stavka = new StavkaRacuna(
            artikal,
            kolicina);
        //kreiramo novu stavku računa

        racun.DodajStavku(stavka);
        //dodajemo stavku na račun
    }

    public void ObrisiStavku(
        Racun racun,
        StavkaRacuna stavka)
    {
        racun.ObrisiStavku(stavka);
        //brisemo stavku sa racuna
    }

    public decimal IzracunajUkupno(Racun racun)
    {
        return racun.Ukupno;
        //vracamo ukupnu vrednost svih stavki na racunu
    }
}