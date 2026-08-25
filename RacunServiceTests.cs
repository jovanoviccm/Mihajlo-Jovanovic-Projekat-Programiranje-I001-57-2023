using Trgovina.Core.Models;
using Trgovina.Core.Services;

namespace Trgovina.Tests;

public class RacunServiceTests // testovi za proveru rada RacunService klase
{
    [Fact] // proverava da se novi račun uspesno kreira
    public void KreirajRacun_KreiraNoviRacun()
    {
        var service = new RacunService();
        //kreiramo servis za rad sa racunima

        var racun = service.KreirajRacun();
        //kreiramo novi racun

        Assert.Single(service.Racuni);
        //proveravamo da je racun dodat u listu racuna
        Assert.Equal(1, racun.Id);
        // proveravamo da prvi racun ima id 1

        Assert.Empty(racun.Stavke);
        // proveravamo da novi racun na pocetku nema stavke
    }


    [Fact] // proverava da se stavka uspesno dodaje na racun
    public void DodajStavku_DodajArtikalNaRacun()
    {
        var service = new RacunService();
        

        var racun = service.KreirajRacun();
        
        var artikal = new Artikal(
            1,                  // id artikla
            "Mleko",            // naziv artikla
            1,                  // id kategorije
            150,                // cena artikla
            "l"                 // jedinica mere
        ); // kreiramo artikal

        service.DodajStavku(racun, artikal, 2);
        // dodajemo 2 litra mleka na racun

        Assert.Single(racun.Stavke);//provera da racun ima jednu stavku
        

        Assert.Equal("Mleko", racun.Stavke[0].Artikal.Naziv);// proveravamo naziv artikla na racun

        Assert.Equal(2, racun.Stavke[0].Kolicina);//roveravamo unetu kolicinu
    }


    [Fact] // proverava da se ukupna cena racuna pravilno izracunava
    public void IzracunajUkupno_VracaTacnuCenu()
    {
        var service = new RacunService();
        

        var racun = service.KreirajRacun();
        

        var mleko = new Artikal(
            1,                  
            "Mleko",            
            1,                  
            150,                
            "l"                 
        ); 

        service.DodajStavku(racun, mleko, 2);
        

        Assert.Equal(300, service.IzracunajUkupno(racun));//roveravamo da je ukupna cena 150 × 2 = 300 dinara
    }


    [Fact] // proverava da kolicina mora biti veca od nule
    public void DodajStavku_NeDozvoljavaNultuKolicinu()
    {
        var service = new RacunService();
       

        var racun = service.KreirajRacun();
        

        var artikal = new Artikal(
            1,                  
            "Mleko",            
            1,                  
            150,                
            "l"                 
        ); 

        Assert.Throws<ArgumentException>(() =>
            service.DodajStavku(racun, artikal, 0)
        );
        // očekujemo grešku jer količina 0 nije dozvoljena
    }


    [Fact] // proverava da se stavka uspešno brise sa racuna
    public void ObrisiStavku_BriseStavkuSaRacuna()
    {
        var service = new RacunService();
       

        var racun = service.KreirajRacun();
       

        var artikal = new Artikal(
            1,                
            "Mleko",           
            1,                  
            150,                
            "l"                
        );

        service.DodajStavku(racun, artikal, 2);
        // dodajemo stavku na račun

        var stavka = racun.Stavke[0];
        // uzimamo prvu stavku sa računa

        service.ObrisiStavku(racun, stavka);
        // brišemo stavku sa računa

        Assert.Empty(racun.Stavke);
        // proveravamo da je racun nakon brisanja bez stavki
    }
}