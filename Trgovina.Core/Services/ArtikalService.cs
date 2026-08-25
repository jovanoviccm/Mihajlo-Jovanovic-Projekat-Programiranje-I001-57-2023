using Trgovina.Core.Models;

namespace Trgovina.Core.Services;

public class ArtikalService //servis koji sadrzi poslovnu logiku za artikle
{
    private readonly List<Artikal> artikli = new(); //lista svih artikla
    private readonly KategorijaService kategorijaService;//servis za proveru postojecih kategorija

    public IReadOnlyList<Artikal> Artikli => artikli;//lista dostupnih artikla

    public ArtikalService(KategorijaService kategorijaService)//konstruktor prima servis kako bi mogao 
    {                                                          //da proverava da li kategorija postoji
        this.kategorijaService = kategorijaService;
    }

    public void Dodaj(Artikal artikal)//dodavanje artikla
    {
        if (artikli.Any(a=> a.Id == artikal.Id))
            throw new ArgumentException("Artikal sa ovim ID-em vec postoji.");

        if (string.IsNullOrWhiteSpace(artikal.Naziv))
            throw new ArgumentException("Naziv artikla ne sme biti prazan.");

        if (artikal.Cena <0)
            throw new ArgumentException("Cena ne moze biti negativna.");

        if (string.IsNullOrWhiteSpace(artikal.JedinicaMere))
            throw new ArgumentException("Jedinica mere ne sme biti prazna. ");
        
        if (kategorijaService.Pronadji(artikal.KategorijaId) == null)
         throw new ArgumentException("Kategorija ne postoji.");

         artikli.Add(artikal); //ako je sve dobro dodaje se artikal
        
    }

    public void Obrisi (int id) //brisanje artikla
    {
        Artikal? artikal = artikli.FirstOrDefault(a => a.Id == id);//pronalazenje artikla

        if (artikal == null)
            throw new ArgumentException("Artikal ne postoji.");//ako ne postoji prijavljuje se greska

        artikli.Remove(artikal); //brisanje artikla
    }

    public Artikal? Pronadji(int id)//pronalazenje artikla pomocu id
    {
        return artikli.FirstOrDefault(a => a.Id == id);
    }
    // Učitava postojeće artikle iz baze
    public void UcitajArtikle(IEnumerable<Artikal> ucitaniArtikli)  
    {
        //brisemo trenutne artikle
        artikli.Clear();

        //dodajemo artikle koje smo ucitali iz baze
        artikli.AddRange(ucitaniArtikli);
}
}