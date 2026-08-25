using Trgovina.Core.Models;
using Trgovina.Core.Services;

namespace Trgovina.Tests;

public class KategorijaServiceTests // testovi za proveru rada KategorijaService klase
{
    [Fact] // proverava da li se kategorija uspesno dodaje
    public void Dodaj_DodajeKategoriju()
    {
        var service = new KategorijaService();
        // kreiramo servis za rad sa kategorijama

        var kategorija = new Kategorija( //kreiramo kategoriju
            1,                      // id kategorije
            "Prehrambeni proizvodi" // naziv kategorije
        ); 

        service.Dodaj(kategorija);//dodajemo kategoriju u servis
        

        Assert.Single(service.Kategorije); //proveravamo da lista sadrzi tacno jednu kategoriju

        Assert.Equal(
            "Prehrambeni proizvodi",
            service.Kategorije[0].Naziv
        );// provera da li je naziv kategorije tacno sacuvan
    }


    [Fact] // proverava da servis ne dozvoljava dve kategorije sa istim ID-em
    public void Dodaj_NedozvoljavaDupliId()
    {
        var service = new KategorijaService();//kreiramo servis za kategorije

        service.Dodaj(
            new Kategorija(1, "Prehrambeni proizvodi")
        );
        //dodajemo prvu kategoriju sa id 1

        Assert.Throws<ArgumentException>(() =>
            service.Dodaj(
                new Kategorija(1, "Pića")
            )
        ); //pokusavamo da dodamo jos jednu kategoriju sa istim id i ocekujemo gresku
    }


    [Fact] // proverava da naziv kategorije ne sme biti prazan
    public void Dodaj_NeDozvoljavaPrazanNaziv()
    {
        var service = new KategorijaService();
        // kreiramo servis za kategorije

        Assert.Throws<ArgumentException>(() =>
            service.Dodaj(
                new Kategorija(1, "")
            )
        );
    }


    [Fact] // proverava da se kategorija uspešno briše
    public void Obrisi_BriseKategoriju()
    {
        var service = new KategorijaService();
        // kreiramo servis za kategorije

        service.Dodaj(
            new Kategorija(1, "Prehrambeni proizvodi")
        );//kategorija koju cemo obrisati

        service.Obrisi(1);
        //brisemo kategoriju pomocu id

        Assert.Empty(service.Kategorije);
        //proveravamo da li je lista nakon brisanja prazna
    }
}