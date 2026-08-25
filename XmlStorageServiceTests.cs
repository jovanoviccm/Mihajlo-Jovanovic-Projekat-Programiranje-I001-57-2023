using Microsoft.VisualBasic;
using Trgovina.Core.Data;
using Trgovina.Core.Models;

namespace Trgovina.Tests;

public class XmlStorageServiceTests //testovi za proveru rada xmlstorageservice klase
{
    [Fact] //provera da li se kategorije cuvaju i ucitavaju iz xml-a
    public void SacuvajIUcitajKategorije_RadiIspravno()
    {
        var service = new XmlStorageService(); //kreiranje xml servisa

        string putanja = Path.Combine( //kreiranje privremene putanje za test xml fajl
            Path.GetTempPath(),
            "kategorije-test.xml"
        );

        var kategorije = new List<Kategorija> //kreiranje kategorija koje cemo sacuvati
        {
            new Kategorija(1, "Hrana"),
            new Kategorija(2, "Pice"),
            new Kategorija(3, "Dezerti")
        };

        service.SacuvajKategorije(kategorije, putanja);//cuvanje kategorija u xml fajl

        var ucitaneKategorije = 
            service.UcitajKategorije(putanja); //ucitavanje kategorija iz xml fajla

        Assert.Equal(3, ucitaneKategorije.Count); //proverava da su ucitane tri kategorije

        Assert.Equal(1, ucitaneKategorije[0].Id); //provera prve kategorije
        Assert.Equal("Hrana", ucitaneKategorije[0].Naziv);

        Assert.Equal(2, ucitaneKategorije[1].Id); //provera druge kategorije
        Assert.Equal("Pice", ucitaneKategorije[1].Naziv);

        Assert.Equal(3, ucitaneKategorije[2].Id); //provera trece kategorije
        Assert.Equal("Dezerti", ucitaneKategorije[2].Naziv);

        File.Delete(putanja);//brisanje privremenog fajla nakon testa


    }

    [Fact] //provera da li se artikli uspesno cuvaju i ucitavaju iz xml-a
    public void SacuvajIUcitajArtikle_RadiIspravno()
    {
        var service = new XmlStorageService();//kreiranje xml servisa

        string putanja = Path.Combine(Path.GetTempPath(), //kreiranje privremene putanje za test xml fajl
        "artikli-test.xml");

        var artikli = new List<Artikal>
        {
            new Artikal(1,"Pizza",1,1100,"kom"),
            new Artikal(2, "Limunada",2,350,"kom"),
            new Artikal(3,"Cheesecake",3,550,"kom")
        };

        service.SacuvajArtikle(artikli, putanja);//cuvanje artikala u xml fajlu
        var ucitaniArtikli = 
            service.UcitajArtikle(putanja); // ucitavanje artikala iz xml fajla
        
        Assert.Equal (3, ucitaniArtikli.Count);//provera da su ucitana tri artikla

        Assert.Equal(1, ucitaniArtikli[0].Id); //provera prvog artikla
        Assert.Equal("Pizza", ucitaniArtikli[0].Naziv);
        Assert.Equal(1100, ucitaniArtikli[0].Cena);

        Assert.Equal(2, ucitaniArtikli[1].Id); //provera drugog artikla
        Assert.Equal("Limunada", ucitaniArtikli[1].Naziv);
        Assert.Equal(350, ucitaniArtikli[1].Cena);

        Assert.Equal(3, ucitaniArtikli[2].Id); //provera treceg artikla
        Assert.Equal("Cheesecake", ucitaniArtikli[2].Naziv);
        Assert.Equal(550, ucitaniArtikli[2].Cena);

        File.Delete(putanja); //brisanje privremenog fajla nakon testa

    }

    [Fact]
    public void SacuvajIUcitajRacun_RadiIspravno()
    {
        var service = 
            new XmlStorageService();
        string putanja = Path.Combine(Path.GetTempPath(), $"racun-test-{Guid.NewGuid()}.xml");

        var racun = 
            new Racun(1);
        var Pizza = new Artikal (
            1,
            "Pizza",
            1,
            1100,
            "kom"
        );
        var Limunada = new Artikal (
            2,
            "Limunada",
            1,
            350,
            "kom"
        );
        racun.DodajStavku(new StavkaRacuna(Pizza, 2));     // 2200
        racun.DodajStavku(new StavkaRacuna(Limunada, 3));   // 1050

        try
        {
            service.SacuvajRacun(racun, putanja);
            var ucitanRacun = 
                service.UcitajRacun(putanja);

        Assert.Equal(1, ucitanRacun.Id);
        Assert.Equal(2, ucitanRacun.Stavke.Count);
        
        // Provera izračunate ukupne vrednosti (2200 + 1050 = 3250)
        Assert.Equal(3250, ucitanRacun.Ukupno);
        Assert.Equal("Pizza", ucitanRacun.Stavke[0].Artikal.Naziv);
        Assert.Equal(2, ucitanRacun.Stavke[0].Kolicina);
        Assert.Equal(2200, ucitanRacun.Stavke[0].Ukupno);
        }
        finally
        {
            if (File.Exists(putanja))
            {
            File.Delete(putanja);
            }
        }
    }
}