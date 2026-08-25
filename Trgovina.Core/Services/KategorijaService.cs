using Trgovina.Core.Models;

namespace Trgovina.Core.Services;

public class KategorijaService
{
    private readonly List<Kategorija> kategorije = new(); //lista kategorija koje trenutno postoje

    public IReadOnlyList<Kategorija> Kategorije => kategorije;//omogucava drugim klasama da procitaju kategorije 
                                                                //ali ne menjaju listu

    public void Dodaj(Kategorija kategorija) //dodavanje kategorije
    {
        if (kategorije.Any(k => k.Id == kategorija.Id))
            throw new ArgumentException("Kategorija sa ovim ID-em već postoji.");

        if (string.IsNullOrWhiteSpace(kategorija.Naziv))
            throw new ArgumentException("Naziv kategorije ne sme biti prazan.");

        kategorije.Add(kategorija); //ako su podaci ispravni, ako kategorija sa istim
                                    // id ne postoji ili ako naziv nije prazan dodaje se kategorija
    }

    public void Obrisi(int id) //brisanje kategorije pomocu id
    {
        Kategorija? kategorija = kategorije.FirstOrDefault(k => k.Id == id); //pronalazenje kategorije

        if (kategorija == null)
            throw new ArgumentException("Kategorija ne postoji."); //provera da li kategorija ne postoji

        kategorije.Remove(kategorija); //brisanje pronadjene kategorije
    }

    public Kategorija? Pronadji(int id) //pronalazenje pomocu id
    {
        return kategorije.FirstOrDefault(k => k.Id == id);
    }
    //ucitava postojece kategorije iz baze
    public void UcitajKategorije(IEnumerable<Kategorija> ucitaneKategorije)
    {   
        //brisemo trenutne kategorije
        kategorije.Clear();

        //dodajemo kategorije koje smo ucitali iz baze
        kategorije.AddRange(ucitaneKategorije);
    }
}