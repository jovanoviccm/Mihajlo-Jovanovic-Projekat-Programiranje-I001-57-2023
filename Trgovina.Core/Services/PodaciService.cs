using Trgovina.Core.Data;
using Trgovina.Core.Models;
using Microsoft.Data.Sqlite;


namespace Trgovina.Core.Services;

public class PodaciService
{
    private readonly DatabaseStorageService database;

    public PodaciService(DatabaseStorageService database)
    {
        this.database = database;
    }

    //kreira bazu ako jos ne postoji
    public void InicijalizujBazu()
    {
        database.KreirajBazu();
    }

    //cuva kategoriju u bazu
    public void SacuvajKategoriju(Kategorija kategorija)
    {
        database.SacuvajKategoriju(kategorija);
    }
    // brise kategoriju iz baze
    public void ObrisiKategoriju(int id)
    {
        database.ObrisiKategoriju(id);
    }

    //cuva artikal u bazu
    public void SacuvajArtikal(Artikal artikal)
    {
        database.SacuvajArtikal(artikal);
    }

    //cuva racun u bazu
    public void SacuvajRacun(Racun racun)
    {
        database.SacuvajRacun(racun);
    }

    //ucitava sve kategorije iz baze
    public List<Kategorija> UcitajKategorije()
    {
        return database.UcitajKategorije();
    }

    //ucitava sve artikle iz baze
    public List<Artikal> UcitajArtikle()
    {
        return database.UcitajArtikle();
    }

    //ucitava sve racune iz baze
    public List<Racun> UcitajRacune()
    {
        return database.UcitajRacune();
    }
    
    //brise artikal iz baze
    public void ObrisiArtikal(int id)
    {
        database.ObrisiArtikal(id);
    }
    
}