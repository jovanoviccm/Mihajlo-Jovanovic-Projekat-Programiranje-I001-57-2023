using Microsoft.Data.Sqlite;
using Trgovina.Core.Data;
using Trgovina.Core.Models;

namespace Trgovina.Tests;

public class DatabaseStorageServiceTests
{
    [Fact]
    public void KreirajBazu_KreiraBazu()
    {
        string putanja = Path.Combine(
            Path.GetTempPath(),
            $"trgovina-test-{Guid.NewGuid()}.db"
        );

        try
        {
            var service = new DatabaseStorageService(putanja);

            service.KreirajBazu();

            Assert.True(File.Exists(putanja));

            using (var connection = new SqliteConnection(
                $"Data Source={putanja}"))
            {
                connection.Open();

                using var command = connection.CreateCommand();

                command.CommandText = """
                    SELECT COUNT(*)
                    FROM sqlite_master
                    WHERE type = 'table'
                    AND name IN (
                        'Kategorije',
                        'Artikli',
                        'Racuni',
                        'StavkeRacuna'
                    );
                    """;

                long brojTabela =
                    (long)command.ExecuteScalar()!;

                Assert.Equal(4, brojTabela);
            }
            // ovde se connection zatvara pre brisanja fajla
        }
        finally
        {

        }
    }

    [Fact] //proverava cuvanje i ucitavanje kategorije iz sqlite baze
    public void SacuvajIUcitajKategoriju_RadiIspravno()
    {
        string putanja = Path.Combine(
            Path.GetTempPath(),
            $"kategorija-test-{Guid.NewGuid()}.db"
            );
            //kreiramo privremenu bazu za test

    try
    {
        var service = new DatabaseStorageService(putanja);
        // kreiramo servis

        service.KreirajBazu();
        // kreiramo potrebne tabele

        var kategorija = new Kategorija(
            1,
            "Hrana"
        );//kreiramo kategoriju koju cemo sasuvati

        service.SacuvajKategoriju(kategorija);//cuvamo kategoriju u bazu

        var ucitaneKategorije =
            service.UcitajKategorije();//ucitavamo kategorije iz baze

        Assert.Single(ucitaneKategorije);//ocekujemo jednu kategoriju

        Assert.Equal(1, ucitaneKategorije[0].Id);//proveravamo id

        Assert.Equal(
            "Hrana",
            ucitaneKategorije[0].Naziv);//proveravamo naziv
    }
    finally
    {
        //privremenu bazu ne brišemo jer SQLite može
        //još kratko držati fajl zaključanim
    }
    }
    [Fact] // proverava čuvanje i učitavanje artikla iz SQLite baze
    public void SacuvajIUcitajArtikal_RadiIspravno()
    {
        string putanja = Path.Combine(
            Path.GetTempPath(),
            $"artikal-test-{Guid.NewGuid()}.db"
            );

        try
        {
        var service = new DatabaseStorageService(putanja);

        service.KreirajBazu();

        // prvo cuvamo kategoriju kojoj ce artikal pripadati
        service.SacuvajKategoriju(
            new Kategorija(1, "Mlecni proizvodi"));

        var artikal = new Artikal(
            1,
            "Mleko",
            1,
            150,
            "l");



        service.SacuvajArtikal(artikal);

        var ucitaniArtikli =
            service.UcitajArtikle();

        Assert.Single(ucitaniArtikli);

        Assert.Equal(1, ucitaniArtikli[0].Id);
        Assert.Equal("Mleko", ucitaniArtikli[0].Naziv);
        Assert.Equal(1, ucitaniArtikli[0].KategorijaId);
        Assert.Equal(150, ucitaniArtikli[0].Cena);
        Assert.Equal("l", ucitaniArtikli[0].JedinicaMere);
    }
    finally
    {
        // privremena baza se ne brise zbog sqlite zaključavanja fajla
    }
    }
    [Fact] // proverava čuvanje i učitavanje računa sa stavkama
public void SacuvajIUcitajRacun_RadiIspravno()
{
    string putanja = Path.Combine(
        Path.GetTempPath(),
        $"racun-test-{Guid.NewGuid()}.db"
    );

    try
    {
        var service = new DatabaseStorageService(putanja);

        service.KreirajBazu();

        // Prvo čuvamo kategoriju
        service.SacuvajKategoriju(
            new Kategorija(1, "Hrana"));

        // Zatim čuvamo artikal
        var artikal = new Artikal(
            1,
            "Mleko",
            1,
            150,
            "l");

        service.SacuvajArtikal(artikal);

        // Kreiramo racun
        var racun = new Racun(1)
        {
            Datum = new DateTime(2026, 8, 24, 14, 30, 0)
        };

        // Dodajemo stavku na racun
        racun.DodajStavku(
            new StavkaRacuna(artikal, 3));

        // Čuvamo racun u bazu
        service.SacuvajRacun(racun);

        // Učitavamo racune iz baze
        var ucitaniRacuni =
            service.UcitajRacune();

        // Provera broja racuna
        Assert.Single(ucitaniRacuni);

        // Provera osnovnih podataka racuna
        Assert.Equal(
            1,
            ucitaniRacuni[0].Id);

        Assert.Equal(
            new DateTime(2026, 8, 24, 14, 30, 0),
            ucitaniRacuni[0].Datum);

        //Provera broja stavki
        Assert.Single(
            ucitaniRacuni[0].Stavke);

        //Provera artikla
        Assert.Equal(
            "Mleko",
            ucitaniRacuni[0].Stavke[0].Artikal.Naziv);

        //Provera kolicine
        Assert.Equal(
            3,
            ucitaniRacuni[0].Stavke[0].Kolicina);

        //Provera ukupne cene
        Assert.Equal(
            450,
            ucitaniRacuni[0].Ukupno);
    }
    finally
    {
        //privremenu bazu ne brišemo zbog sqlite zaključavanja fajla.
    }
}

}