using System.Xml.Serialization;
using Trgovina.Core.Models;

namespace Trgovina.Core.Data;

public class XmlStorageService //servis zaduzen za cuvanje i ucitavanje podataka iz xml fajlova
{
    public void SacuvajKategorije( //cuva listu kategorija u xml
        IReadOnlyList<Kategorija> kategorije,
        string putanja)
    {

        var serializer = //kreiranje xml serijalizatora za listu kategorija
            new XmlSerializer(typeof(List<Kategorija>));

        using var stream = //otvaranje fajla za upis
            new FileStream(putanja, FileMode.Create);

        serializer.Serialize(stream, kategorije.ToList()); //pretvaranje c# objekta u xml
    }

    public List<Kategorija> UcitajKategorije(string putanja) //Ucitava kategorije iz XML fajla
    {
        var serializer = 
            new XmlSerializer(typeof(List<Kategorija>)); //kreiranje xml serijalizatora

        using var stream = 
            new FileStream(putanja, FileMode.Open);//kreiranje postojeceg xml fajla

        return (List<Kategorija>)serializer.Deserialize(stream)!;//pretvaranje XML podataka nazad u c# objekte

    }

    public void SacuvajArtikle( //cuva listu artikala u xml fajlu
        IReadOnlyList<Artikal> artikli, string putanja)
    {
        var serializer = //kreirajnje xml serijalizatora za listu artikala
            new XmlSerializer(typeof(List<Artikal>));

        using var stream = //otvaranje fajla za upis
            new FileStream(putanja, FileMode.Create);

        serializer.Serialize(stream, artikli.ToList()); //pretvaranje c# objekata u xml
    }

    public List<Artikal> UcitajArtikle(string putanja) //Ucitava artikle iz xml fajla
    {
        var serializer =    //kreiranje xml serijalizatora
            new XmlSerializer(typeof(List<Artikal>));

        using var stream =  //otvaranje postojeceg xml fajla
            new FileStream(putanja, FileMode.Open);

        return (List<Artikal>)serializer.Deserialize(stream)!;
    }

public void SacuvajRacun(Racun racun, string putanja) //cuva racune u xml fajlu
    {
        var serializer = //kreiranje xml serijalizatora 
            new XmlSerializer(typeof(Racun));

        using var writer = 
            new StreamWriter(putanja);

        serializer.Serialize(writer, racun);
    }

    public Racun UcitajRacun(string putanja) //ucitava racun iz xml fajla
    {
        if (!File.Exists(putanja))
            throw new FileNotFoundException("Zatrazeni XML fajl sa racunom ne postoji.", putanja);

        var serializer = //kreiranje serijalizatora
            new XmlSerializer(typeof(Racun));

        using var reader = 
            new StreamReader(putanja);
        
        return (Racun)serializer.Deserialize(reader)!;
    }

    public void SacuvajRacune(List<Racun> racuni, string putanja)//cuvanje vise racuna iz xml
    {
        var serializer = //kreiranje serijalizatora
            new XmlSerializer(typeof(List<Racun>));

        using var writer = new StreamWriter(putanja);
        serializer.Serialize(writer, racuni);        
    }

    public List<Racun> UcitajRacune(string putanja) //ucitavanje vise racuna iz xml
    {
        if (!File.Exists(putanja)) 
            return new List<Racun>();

        var serializer = 
            new XmlSerializer(typeof(List<Racun>));

        using var reader = new StreamReader(putanja);
        return (List<Racun>)serializer.Deserialize(reader)!;
    }
}

