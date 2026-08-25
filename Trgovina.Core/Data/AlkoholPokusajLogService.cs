namespace Trgovina.Core.Data;

public class AlkoholPokusajLogService
{
    private readonly string putanja;

    public AlkoholPokusajLogService(string putanja)
    {
        this.putanja = putanja;
        // cuvamo putanju do fajla u koji beležimo pokusaje
    }

    public void SacuvajPokusaj(string poruka)
    {
        string zapis =
            $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} - {poruka}";

        File.AppendAllText(
            putanja,
            zapis + Environment.NewLine);
        // dodajemo novi zapis na kraj fajla
    }
}