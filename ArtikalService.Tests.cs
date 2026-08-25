using Trgovina.Core.Models;
using Trgovina.Core.Services;

namespace Trgovina.Tests;

public class ArtikalServiceTests // testovi za proveru rada ArtikalService klase
{
    [Fact] // proverava da li se artikal uspesno dodaje
    public void Dodaj_DodajeArtikal()
    {
        var kategorijaService = new KategorijaService(); // kreiramo servis za kategorije

        kategorijaService.Dodaj(
            new Kategorija(1, "Prehrambeni proizvodi")
        ); // dodajemo kategoriju kojoj ce artikal pripadati

        var artikalService = new ArtikalService(kategorijaService);
        // kreiramo servis za artikle i prosleđujemo mu servis kategorija

        var artikal = new Artikal(
            1,                  // id artikla
            "Mleko",            // naziv artikla
            1,                  // id kategorije
            150,                // cena artikla
            "l"                 // jedinica mere
        ); 

        artikalService.Dodaj(artikal);
        // dodajemo kreirani artikal preko ArtikalService-a

        Assert.Single(artikalService.Artikli);
        // provera da lista sadrži tačno jedan artikal

        Assert.Equal("Mleko", artikalService.Artikli[0].Naziv);
        // provera da je naziv dodatog artikla "Mleko"
    }


    [Fact] // provera da nije moguce dodati dva artikla sa istim id
    public void Dodaj_NeDozvoljavaDupliId()
    {
        var kategorijaService = new KategorijaService();
        // kreira se servis za kategorije

        kategorijaService.Dodaj(
            new Kategorija(1, "Prehrambeni proizvodi")
        ); // dodajemo kategoriju

        var artikalService = new ArtikalService(kategorijaService);
        // kreira se servis za artikle

        artikalService.Dodaj(
            new Artikal(
                1,              
                "Mleko",       
                1,             
                150,            
                "l"             
            )
        ); // dodajemo prvi artikal

        Assert.Throws<ArgumentException>(() =>
            artikalService.Dodaj(
                new Artikal(
                    1,              
                    "Hleb",       
                    1,            
                    80,            
                    "kom"           
                )
            )
        );
        // ocekuje se greska jer dva artikla ne mogu imati isti id
    }


    [Fact] // proverava da artikal ne moze pripadati nepostojecoj kategoriji
    public void Dodaj_NeDozvoljavaNepostojecuKategoriju()
    {
        var kategorijaService = new KategorijaService();
        // kreiramo servis za kategorije ali ne dodajemo nijednu kategoriju

        var artikalService = new ArtikalService(kategorijaService);
        // kreiramo servis za artikle

        Assert.Throws<ArgumentException>(() =>
            artikalService.Dodaj(
                new Artikal(
                    1,             
                    "Mleko",       
                    99,            
                    150,            
                    "l"             
                )
            )
        );
        // ocekuje se greska jer kategorija id 99 ne postoji
    }


    [Fact] // proverava da cena artikla ne može biti negativna
    public void Dodaj_NeDozvoljavaNegativnuCenu()
    {
        var kategorijaService = new KategorijaService();
        // kreiramo servis za kategorije

        kategorijaService.Dodaj(
            new Kategorija(1, "Prehrambeni proizvodi")
        ); // dodajemo kategoriju

        var artikalService = new ArtikalService(kategorijaService);
        // kreiramo servis za artikle

        Assert.Throws<ArgumentException>(() =>
            artikalService.Dodaj(
                new Artikal(
                    1,              // id artikla
                    "Mleko",        // naziv artikla
                    1,              // id kategorije
                    -150,           // negativna cena - nije dozvoljena
                    "l"             // jedinica mere
                )
            )
        );
        // ocekuje se greska jer cena ne moze biti negativna
    }


    [Fact] // proverava da se artikal uspesno
    public void Obrisi_BriseArtikal()
    {
        var kategorijaService = new KategorijaService();// kreiramo servis za kategorije

        kategorijaService.Dodaj(
            new Kategorija(1, "Prehrambeni proizvodi")
        ); // dodajemo kategoriju

        var artikalService = new ArtikalService(kategorijaService);// kreiramo servis za artikle

        artikalService.Dodaj(
            new Artikal(
                1,              
                "Mleko",       
                1,              
                150,            
                "l"             
            )
        ); // dodajemo artikal koji ćemo kasnije obrisati

        artikalService.Obrisi(1);// brisemo artikal pomocu id

        Assert.Empty(artikalService.Artikli);// provera da li je lista artikala sada prazna
    }
}