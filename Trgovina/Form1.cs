using Trgovina.Core.Services;
using Trgovina.Core.Data;

namespace Trgovina;

public partial class Form1 : Form
{

    private readonly KategorijaService kategorijaService;
    //dugme za administraciju i statistiku
    private Button btnAdministracija;

    //dugme za prodaju i naplatu
    private Button btnProdaja;
    //servis za artikle
    private readonly ArtikalService artikalService;

    //servis za racune
    private readonly RacunService racunService;

    //servis za prodaju
    private readonly ProdajaService prodajaService;

    //servis zaduzen za cuvanje i ucitavanje podataka iz baze
    private readonly PodaciService podaciService;

    //naslov aplikacije
    private Label lblNaslov;

    public Form1()
    {
        InitializeComponent();

        kategorijaService = new KategorijaService();
        //odredjujemo gde ce sqlite baza biti sacuvana
        string putanjaBaze = Path.Combine(
    AppDomain.CurrentDomain.BaseDirectory,
    "trgovina.db");

        //kreiramo servis za rad sa bazom
        var database = new DatabaseStorageService(
            putanjaBaze);

        //kreiramo servis za upravljanje podacima
        podaciService = new PodaciService(database);

        //kreiramo bazu i potrebne tabele
        podaciService.InicijalizujBazu();

        //ucitavanje kategorija
        var ucitaneKategorije = podaciService.UcitajKategorije();
        kategorijaService.UcitajKategorije(ucitaneKategorije);

        //kreiranje servisa za artikle
        artikalService = new ArtikalService(
            kategorijaService);

        //ucitavanje artikala
        var ucitaniArtikli = podaciService.UcitajArtikle();
        artikalService.UcitajArtikle(ucitaniArtikli);
       

        //kreiranje servisa za račune
        racunService = new RacunService();

        //kreiranje servisa za prodaju
        prodajaService = new ProdajaService(
            racunService,
            kategorijaService);
        // Podešavanje glavnog prozora
        Text = "Trgovina";
        Width = 600;
        Height = 400;
        StartPosition = FormStartPosition.CenterScreen;

        // Kreiranje naslova
        lblNaslov = new Label();
        lblNaslov.Text = "TRGOVINA";
        lblNaslov.Font = new Font(
            "Arial",
            24,
            FontStyle.Bold);
        lblNaslov.AutoSize = true;
        lblNaslov.Location = new Point(210, 50);

        //kreiranje dugmeta za administraciju
        btnAdministracija = new Button();
        btnAdministracija.Text = "Administracija i statistika";
        btnAdministracija.Width = 250;
        btnAdministracija.Height = 60;
        btnAdministracija.Location = new Point(175, 130);

        //kreiranje dugmeta za prodaju
        btnProdaja = new Button();
        btnProdaja.Text = "Prodaja / naplata";
        btnProdaja.Width = 250;
        btnProdaja.Height = 60;
        btnProdaja.Location = new Point(175, 210);

        //dodavanje kontrola na formu
        Controls.Add(lblNaslov);
        Controls.Add(btnAdministracija);
        Controls.Add(btnProdaja);

        btnAdministracija.Click += BtnAdministracija_Click;
        //povezivanje dugmeta Prodaja sa dogadjajem
        btnProdaja.Click += BtnProdaja_Click;
    }

        //dogadjaj koji se izvršava kada korisnik klikne
        //na dugme "Administracija i statistika"
        private void BtnAdministracija_Click(
        object? sender,
        EventArgs e)
        {
            //kreiranje forme za administraciju
            var forma = new AdministracijaForm(
                kategorijaService,
                artikalService,
                podaciService);

        //otvaranje forme
        forma.ShowDialog();
    }
    //otvara formu za prodaju i naplatu
    private void BtnProdaja_Click(
        object? sender,
        EventArgs e)
    {
    //kreiranje forme za prodaju
    var forma = new ProdajaForm(
        prodajaService,
        artikalService,
        podaciService);

    //otvaranje forme
    forma.ShowDialog();
}
}
