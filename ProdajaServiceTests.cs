using Trgovina.Core.Models;
using Trgovina.Core.Services;

namespace Trgovina.Tests;

public class ProdajaServiceTests //testovi za proveru rada ProdajaService klase
{
    [Fact] //proverava da se artikal moze dodati na racun
    public void DodajArtikal_DodajeObicanArtikal()
    {
        var kategorijaService = new KategorijaService();
        //kreiramo servis za kategorije

        kategorijaService.Dodaj(
            new Kategorija(1, "Prehrambeni proizvodi")
        ); //dodajemo kategoriju

        var racunService = new RacunService();//kreiramo servis za racune

        var ProdajaService = new ProdajaService(
            racunService,
            kategorijaService,
            () => new DateTime(2026,8,24,20,0,0)
        );//simuliramo vreme 20:00

        var racun = ProdajaService.ZapocniProdaju();//kreiramo novi racun
        var artikal = new Artikal(
                    1,
                    "mleko",
                    1,
                    150,
                    "1"
                    );//kreiramo artikal

        bool rezultat = ProdajaService.DodajArtikal
            (racun,
            artikal,
            2
        );//pokusavamo da dodamo 2 litra mleka

        Assert.True(rezultat);
        //ocekujemo da je prodaja dozvoljena

        Assert.Single(racun.Stavke);//racun treba da ima jednu stavku

        Assert.Equal(2, racun.Stavke[0].Kolicina);//proveravamo kol
    }

    [Fact] //provera da je kupovina alkohola posle 22h zabranjena
    public void DodajArtikal_AlkoholPosle22h_OdbijaProdaju()
    {
        var kategorijaService = new KategorijaService();

        kategorijaService.Dodaj(
            new Kategorija(1,"Alkoholna pica")
        );//dodajemo kategoriju alkoholnih pica

        var racunService = new RacunService();

        var prodajaService = new ProdajaService(
            racunService,
            kategorijaService,
        () => new DateTime(2026,8,24,22,30,0)
        );// simuliramo vreme 22:30

        var racun = prodajaService.ZapocniProdaju();

        var pivo = new Artikal(
            1,
            "Pivo",
            1,
            180,
            "kom"
        );//kreiramo alkoholni artikal

        bool rezultat = prodajaService.DodajArtikal(
            racun,
            pivo,
            1
        );//pokusavamo da dodamo alkohol posle 22h

        Assert.False(rezultat);//ocekujemo da prodaja bude odbijena

        Assert.Empty(racun.Stavke);//artikal ne sme biti dodat na racun

    }
    [Fact]//proverava da se event pokrece pri pokusaju kupovine alkohola posle 22h
    public void DodajArtikal_AlkoholPosle22h_PokreceEvent()
    {
        var kategorijaService = new KategorijaService();

        kategorijaService.Dodaj(
            new Kategorija(1,"Alkoholna pica")
        );

        var racunService = new RacunService();

        var prodajaService = new ProdajaService(
            racunService,
            kategorijaService,
            () => new DateTime(2026,8,24,23,0,0)
        );//simuliramo vreme 23:00

        var racun = prodajaService.ZapocniProdaju();

        var pivo = new Artikal (
            1,
            "Pivo",
            1,
            180,
            "kom"
        );

        bool eventPokrenut = false;//promenljiva kojom proveravamo da li event pokrenut

        prodajaService.PokusajKupovineAlkohola +=
        (sender,poruka) =>
        {
            eventPokrenut = true;
        };// pretplacujemo se na event

        prodajaService.DodajArtikal(
            racun,
            pivo,
            1
        );//pokusavamo kupovinu alkohola

        Assert.True(eventPokrenut);//proveravamo da li event pokrenut

    }

    [Fact]//proverava da je kupovina alkohola pre 22h dozvoljena
    public void DodajArtikal_AlkoholPosle22h_DozvoljavaProdaju()
    {
        var kategorijaService = new KategorijaService();

        kategorijaService.Dodaj(
            new Kategorija(1, "Alkoholna pica")
        );
        var racunService = new RacunService();
        
        var prodajaService = new ProdajaService(
            racunService,
            kategorijaService,
            () => new DateTime(2026,8,24,21,30,0)

        );
        //simuliramo vreme 21:30

        var racun = prodajaService.ZapocniProdaju();

        var pivo = new Artikal(
            1,
            "Pivo",
            1,
            180,
            "kom"
        );

        bool rezultat = prodajaService.DodajArtikal(
            racun,
            pivo,
            1
        );

        Assert.True(rezultat);
        //kupovina pre 22h treba da buude dozvoljena

        Assert.Single(racun.Stavke);
        //artikal treba da bude dodat na racun
    }
}