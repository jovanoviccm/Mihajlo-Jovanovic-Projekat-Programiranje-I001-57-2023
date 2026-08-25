using Trgovina.Core.Models;
using Trgovina.Core.Services;

namespace Trgovina;

public class ProdajaForm : Form
{
    //servis za cuvanje podataka u SQLite bazu
    private readonly PodaciService podaciService;

    //servis koji upravlja prodajom
    private readonly ProdajaService prodajaService;

    //servis koji sadrži artikle
    private readonly ArtikalService artikalService;

    //trenutni račun
    private readonly Racun racun;

    //lista dostupnih artikala
    private ListBox lstArtikli;

    //lista stavki trenutnog računa
    private ListBox lstRacun;

    //polje za unos količine
    private NumericUpDown numKolicina;

    //dugme za dodavanje artikla na račun
    private Button btnDodaj;
    // dugme za naplatu računa
    private Button btnNaplati;

    //dugme za brisanje stavke sa računa
    private Button btnObrisi;

    //prikaz ukupne cene
    private Label lblUkupno;

    public ProdajaForm(
        ProdajaService prodajaService,
        ArtikalService artikalService,
        PodaciService podaciService)
    {
        this.prodajaService = prodajaService;
        this.artikalService = artikalService;
        this.podaciService = podaciService;

        //kreiranje novog racuna
        racun = prodajaService.ZapocniProdaju();

        //podesavanje prozora
        Text = "Prodaja / naplata";
        Width = 800;
        Height = 500;
        StartPosition = FormStartPosition.CenterScreen;

        //naslov
        var lblNaslov = new Label();
        lblNaslov.Text = "PRODAJA";
        lblNaslov.Font = new Font(
            "Arial",
            20,
            FontStyle.Bold);
        lblNaslov.AutoSize = true;
        lblNaslov.Location = new Point(30, 25);

        //lista artikala
        lstArtikli = new ListBox();
        lstArtikli.Location = new Point(30, 80);
        lstArtikli.Width = 300;
        lstArtikli.Height = 300;

        //dodavanje svih artikala u listu
        foreach (var artikal in artikalService.Artikli)
        {
            lstArtikli.Items.Add(artikal);
        }

        //labela za kolicinu
        var lblKolicina = new Label();
        lblKolicina.Text = "Količina:";
        lblKolicina.AutoSize = true;
        lblKolicina.Location = new Point(30, 400);

        //polje za unos kolicine
        numKolicina = new NumericUpDown();
        numKolicina.Minimum = 1;
        numKolicina.Maximum = 1000;
        numKolicina.Value = 1;
        numKolicina.Location = new Point(100, 395);
        numKolicina.Width = 100;

        //dugme za dodavanje
        btnDodaj = new Button();
        btnDodaj.Text = "Dodaj na račun";
        btnDodaj.Width = 130;
        btnDodaj.Location = new Point(220, 395);

        //lista stavki racuna
        lstRacun = new ListBox();
        lstRacun.Location = new Point(400, 80);
        lstRacun.Width = 330;
        lstRacun.Height = 300;

        //dugme za brisanje stavke
        btnObrisi = new Button();
        btnObrisi.Text = "Obriši stavku";
        btnObrisi.Width = 130;
        btnObrisi.Location = new Point(400, 395);

        //prikaz ukupne cene
        lblUkupno = new Label();
        lblUkupno.Text = "Ukupno: 0.00 din";
        lblUkupno.Font = new Font(
            "Arial",
            14,
            FontStyle.Bold);
        lblUkupno.AutoSize = true;
        lblUkupno.Location = new Point(560, 398);

        //povezivanje dugmadi sa metodama
        btnDodaj.Click += BtnDodaj_Click;
        btnObrisi.Click += BtnObrisi_Click;

        // dugme za naplatu
        btnNaplati = new Button();
        btnNaplati.Text = "Naplati račun";
        btnNaplati.Width = 130;
        btnNaplati.Location = new Point(400, 430);

        // povezivanje dugmeta sa metodom
        btnNaplati.Click += BtnNaplati_Click;

// dodavanje dugmeta na formu
Controls.Add(btnNaplati);

        //dodavanje kontrola na formu
        Controls.Add(lblNaslov);
        Controls.Add(lstArtikli);
        Controls.Add(lblKolicina);
        Controls.Add(numKolicina);
        Controls.Add(btnDodaj);
        Controls.Add(lstRacun);
        Controls.Add(btnObrisi);
        Controls.Add(lblUkupno);
    }

    //dodavanje artikla na racun
    private void BtnDodaj_Click(
        object? sender,
        EventArgs e)
    {
        //provera da li je artikal izabran
        if (lstArtikli.SelectedItem is not Artikal artikal)
        {
            MessageBox.Show(
                "Izaberite artikal.",
                "Greška",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            return;
        }

        try
        {
            //dodavanje artikla na račun
            bool uspesno = prodajaService.DodajArtikal(
                racun,
                artikal,
                numKolicina.Value);

            //ako prodaja nije dozvoljena
            if (!uspesno)
            {
                return;
            }

            //osvezavanje prikaza racuna
            OsveziRacun();
        }
        catch (ArgumentException ex)
        {
            //prikaz greske
            MessageBox.Show(
                ex.Message,
                "Greška",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    //brisanje stavke sa računa
    private void BtnObrisi_Click(
        object? sender,
        EventArgs e)
    {
        //provera da li je stavka izabrana
        if (lstRacun.SelectedItem is not StavkaRacuna stavka)
        {
            return;
        }

        //brisanje stavke
        prodajaService.ObrisiStavku(
            racun,
            stavka);

        //osvezavanje prikaza
        OsveziRacun();
    }

    //osvezavanje prikaza racuna
    private void OsveziRacun()
    {
        //brisanje starog prikaza
        lstRacun.Items.Clear();

        //dodavanje svih stavki racuna
        foreach (var stavka in racun.Stavke)
        {
            lstRacun.Items.Add(stavka);
        }

        //prikaz ukupne cene
        lblUkupno.Text =
            $"Ukupno: {prodajaService.TrenutnoUkupno(racun):0.00} din";
    }

    // zavrsava prodaju i cuva racun u SQLite bazi
    private void BtnNaplati_Click(
        object? sender,
        EventArgs e)
    {
        // racun mora imati bar jednu stavku
        if (racun.Stavke.Count == 0)
        {
            MessageBox.Show(
                "Račun nema nijednu stavku.",
                "Greška",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            return;
        }

        //cuvamo racun i njegove stavke u bazu
        podaciService.SacuvajRacun(racun);

        MessageBox.Show(
            $"Račun je uspešno naplaćen.\nUkupno: {racun.Ukupno:0.00} din",
            "Naplata",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        // zatvaramo formu prodaje
        Close();
    }
}