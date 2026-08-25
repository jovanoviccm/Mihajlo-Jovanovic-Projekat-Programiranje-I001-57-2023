using Trgovina.Core.Models;
using Trgovina.Core.Data;

namespace Trgovina.Core.Services;

//delegat koji opisuje dogadjaj nedozvoljenog pokusaja kupovine alkohola
public delegate void PokusajKupovineAlkoholaEventHandler( 
    object sender,
    string poruka
);

public class ProdajaService
{
    private readonly RacunService racunService;
    private readonly KategorijaService kategorijaService;

    //funkcija koja vraca trenutno vreme i omogucava
    //nam da u testovima kontrolisemo vreme
    private readonly Func<DateTime> trenutnoVreme;
    public event PokusajKupovineAlkoholaEventHandler? PokusajKupovineAlkohola;

    public ProdajaService(
        RacunService racunService,
        KategorijaService kategorijaService,
        Func<DateTime>?trenutnoVreme = null)
    {
        this.racunService = racunService;
        this.kategorijaService = kategorijaService;

        this.trenutnoVreme = trenutnoVreme ?? (()=>DateTime.Now);
        //ako vreme nije prosledjeno koristi se stvarno trenutno vreme
    }

    public Racun ZapocniProdaju() //kreiranje novog racuna za pocetak prodaje
    {
        return racunService.KreirajRacun();
    }

    public bool DodajArtikal( //dodavanje artikla na racun
        Racun racun,
        Artikal artikal,
        decimal kolicina)
    {
        var kategorija = kategorijaService.Pronadji(//pronalazimo kategoriju kojoj artikal pripada
            artikal.KategorijaId);

        bool alkoholnoPice = 
            kategorija != null&&
            kategorija.Naziv.Equals(
                "Alkoholna pica",
                StringComparison.OrdinalIgnoreCase);

        // Uzimamo trenutno vreme
        DateTime vreme = trenutnoVreme();

        bool posle22h = vreme.Hour >= 22; //proveravamo da li je trenutno vreme posle 22h
        
        //ako je alkohol i vreme posle 22h odbijamo prodaju

        if(alkoholnoPice && posle22h)
        {
            string poruka = $"Pokusaj kupovine alkohola posle 22h."+
                            $"Artikal:{artikal.Naziv}";

            PokusajKupovineAlkohola?.Invoke(this,poruka);

        return false;  //prodaja se odbija               
        }

        racunService.DodajStavku( //ako prodaja nije zabranjena
            racun,                 //dodajemo artikal na racun
            artikal,
            kolicina
        );

        return true;

    }

    
    public void ObrisiStavku(//brisanje stavke sa racuna
        Racun racun,
        StavkaRacuna stavka
    )
    {
        racunService.ObrisiStavku(
            racun,stavka //brisemo stavku sa racuna
        );
    }

    public decimal TrenutnoUkupno (Racun racun)
    {
        return racunService.IzracunajUkupno(racun);
        //vracamo trenutno ukupno stanje racuna
    }
    public void PoveziLogServis(
        AlkoholPokusajLogService logService)
    {
        PokusajKupovineAlkohola +=
            (sender, poruka) =>
            {
            logService.SacuvajPokusaj(poruka);
            };
    }   
}