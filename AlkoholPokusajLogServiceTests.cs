using Trgovina.Core.Data;

namespace Trgovina.Tests;

public class AlkoholPokusajLogServiceTests
{
    [Fact] //proverava da se pokusaj kupovine upisuje u fajl
    public void SacuvajPokusaj_UpisujeZapisUFajl()
    {
        string putanja = Path.Combine(
            Path.GetTempPath(),
            $"alkohol-test-{Guid.NewGuid()}.txt"
        );
        //kreiramo privremenu putanju za test fajl

        var service = new AlkoholPokusajLogService(putanja);
        //kreiramo servis za beleženje pokusaja

        string poruka =
            "Pokusaj kupovine alkohola posle 22h. Artikal:Pivo";
        //poruka koju želimo da sacuvamo

        try
        {
            service.SacuvajPokusaj(poruka);
            //upisujemo pokusaj u fajl

            Assert.True(File.Exists(putanja));
            //proveravamo da li je fajl kreiran

            string sadrzaj = File.ReadAllText(putanja);
            //citamo sadrzaj fajla

            Assert.Contains(poruka, sadrzaj);
            //proveravamo da li fajl sadrži nasu poruku
        }
        finally
        {
            if (File.Exists(putanja))
            {
                File.Delete(putanja);
            }
            // brisemo privremeni fajl nakon testa
        }
    }
}