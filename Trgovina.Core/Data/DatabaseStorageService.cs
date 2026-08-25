using Microsoft.Data.Sqlite;
using Trgovina.Core.Models;

namespace Trgovina.Core.Data;

public class DatabaseStorageService
{

    
    private readonly string connectionString;

    public DatabaseStorageService(string putanja)
    {
        connectionString = $"Data Source={putanja}";
        // određujemo gde će sqlite baza biti sačuvana
    }

    public void KreirajBazu()
    {
        

        using var connection = new SqliteConnection(connectionString);
        // kreiramo vezu sa bazom

        connection.Open();
        // otvaramo vezu

        using var command = connection.CreateCommand();
        // kreiramo sql komandu

        command.CommandText = """
            CREATE TABLE IF NOT EXISTS Kategorije
            (
                Id INTEGER PRIMARY KEY,
                Naziv TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Artikli
            (
                Id INTEGER PRIMARY KEY,
                Naziv TEXT NOT NULL,
                KategorijaId INTEGER NOT NULL,
                Cena REAL NOT NULL,
                JedinicaMere TEXT NOT NULL,
                FOREIGN KEY (KategorijaId)
                    REFERENCES Kategorije(Id)
            );

            CREATE TABLE IF NOT EXISTS Racuni
            (
                Id INTEGER PRIMARY KEY,
                Datum TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS StavkeRacuna
            (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                RacunId INTEGER NOT NULL,
                ArtikalId INTEGER NOT NULL,
                Kolicina REAL NOT NULL,
                FOREIGN KEY (RacunId)
                    REFERENCES Racuni(Id),
                FOREIGN KEY (ArtikalId)
                    REFERENCES Artikli(Id)
            );
            """;

        command.ExecuteNonQuery();
        // izvrsavamo sql komandu i kreiramo tabele
    }
        public void SacuvajKategoriju(Kategorija kategorija)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
            INSERT INTO Kategorije (Id, Naziv)
            VALUES ($id, $naziv);
            """;

        command.Parameters.AddWithValue("$id", kategorija.Id);
        command.Parameters.AddWithValue("$naziv", kategorija.Naziv);

        command.ExecuteNonQuery();
        // upisujemo kategoriju u bazu

    }
    public List<Kategorija> UcitajKategorije()
    {
    var kategorije = new List<Kategorija>();

    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    using var command = connection.CreateCommand();

    command.CommandText = """
        SELECT Id, Naziv
        FROM Kategorije
        ORDER BY Id;
        """;

    using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var kategorija = new Kategorija(
                reader.GetInt32(0),
                reader.GetString(1)
            );

            kategorije.Add(kategorija);
        }

        return kategorije;
    }
    public void SacuvajArtikal(Artikal artikal)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
            INSERT INTO Artikli
            (Id, Naziv, KategorijaId, Cena, JedinicaMere)
             VALUES
            ($id, $naziv, $kategorijaId, $cena, $jedinicaMere);
            """;

        command.Parameters.AddWithValue("$id", artikal.Id);
        command.Parameters.AddWithValue("$naziv", artikal.Naziv);
        command.Parameters.AddWithValue("$kategorijaId", artikal.KategorijaId);
        command.Parameters.AddWithValue("$cena", artikal.Cena);
        command.Parameters.AddWithValue("$jedinicaMere", artikal.JedinicaMere);

        command.ExecuteNonQuery();
        // cuvamo artikal u SQLite bazu
    }
    public List<Artikal> UcitajArtikle()
    {
        var artikli = new List<Artikal>();

        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
        SELECT Id, Naziv, KategorijaId, Cena, JedinicaMere
        FROM Artikli
        ORDER BY Id;
        """;

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var artikal = new Artikal(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetInt32(2),
                reader.GetDecimal(3),
                reader.GetString(4)
        );

        artikli.Add(artikal);
    }

        return artikli;
    }
    public void SacuvajRacun(Racun racun)
{
    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    using var transaction = connection.BeginTransaction();

    // cuvamo osnovne podatke o racunu
    using var racunCommand = connection.CreateCommand();

    racunCommand.Transaction = transaction;
    racunCommand.CommandText = """
        INSERT INTO Racuni (Id, Datum)
        VALUES ($id, $datum);
        """;

    racunCommand.Parameters.AddWithValue("$id", racun.Id);
    racunCommand.Parameters.AddWithValue(
        "$datum",
        racun.Datum.ToString("O"));

    racunCommand.ExecuteNonQuery();

    // cuvamo sve stavke racuna
    foreach (var stavka in racun.Stavke)
    {
        using var stavkaCommand = connection.CreateCommand();

        stavkaCommand.Transaction = transaction;
        stavkaCommand.CommandText = """
            INSERT INTO StavkeRacuna
            (RacunId, ArtikalId, Kolicina)
            VALUES
            ($racunId, $artikalId, $kolicina);
            """;

        stavkaCommand.Parameters.AddWithValue(
            "$racunId",
            racun.Id);

        stavkaCommand.Parameters.AddWithValue(
            "$artikalId",
            stavka.Artikal.Id);

        stavkaCommand.Parameters.AddWithValue(
            "$kolicina",
            stavka.Kolicina);

        stavkaCommand.ExecuteNonQuery();
    }

    transaction.Commit();
    // potvrdjujemo cuvanje racuna i svih njegovih stavki
}
    public List<Racun> UcitajRacune()
{
    var racuni = new List<Racun>();

    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    using var command = connection.CreateCommand();

    command.CommandText = """
        SELECT Id, Datum
        FROM Racuni
        ORDER BY Id;
        """;

    using var reader = command.ExecuteReader();

    while (reader.Read())
    {
        var racun = new Racun
        {
            Id = reader.GetInt32(0),
            Datum = DateTime.Parse(reader.GetString(1))
        };

        racuni.Add(racun);
    }

    // Nakon ucitavanja računa ucitavamo i njihove stavke
    foreach (var racun in racuni)
    {
        using var stavkeCommand = connection.CreateCommand();

        stavkeCommand.CommandText = """
            SELECT
                s.ArtikalId,
                s.Kolicina,
                a.Id,
                a.Naziv,
                a.KategorijaId,
                a.Cena,
                a.JedinicaMere
            FROM StavkeRacuna s
            INNER JOIN Artikli a
                ON s.ArtikalId = a.Id
            WHERE s.RacunId = $racunId;
            """;

        stavkeCommand.Parameters.AddWithValue(
            "$racunId",
            racun.Id);

        using var stavkeReader =
            stavkeCommand.ExecuteReader();

        while (stavkeReader.Read())
        {
            var artikal = new Artikal(
                stavkeReader.GetInt32(2),
                stavkeReader.GetString(3),
                stavkeReader.GetInt32(4),
                stavkeReader.GetDecimal(5),
                stavkeReader.GetString(6));

            var stavka = new StavkaRacuna(
                artikal,
                stavkeReader.GetDecimal(1));

            racun.DodajStavku(stavka);
        }
    }

    return racuni;
    }
    public void ObrisiKategoriju(int id)
    {
        using var connection = new SqliteConnection(connectionString);

        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
        DELETE FROM Kategorije
        WHERE Id = $id;
        """;


    }

    //brise artikal iz SQLite baze pomocu ID-a
    public void ObrisiArtikal(int id)
    {
        using var connection = new SqliteConnection(connectionString);

        // Otvaramo vezu sa bazom
        connection.Open();

        using var command = connection.CreateCommand();

        // SQL naredba za brisanje artikla
        command.CommandText = """
            DELETE FROM Artikli
            WHERE Id = $id;
            """;

        //prosledjujemo ujemo ID artikla
        command.Parameters.AddWithValue("$id", id);

        //izvrsavamo naredbu
        command.ExecuteNonQuery();
    }
}