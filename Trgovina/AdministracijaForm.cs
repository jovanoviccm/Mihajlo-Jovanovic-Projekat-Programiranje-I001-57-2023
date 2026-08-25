using Trgovina.Core.Models;
using Trgovina.Core.Services;

namespace Trgovina;

public class AdministracijaForm : Form
{
    private readonly KategorijaService kategorijaService;
    private readonly ArtikalService artikalService;
    private readonly PodaciService podaciService;

    private ListBox lstKategorije;
    private TextBox txtNaziv;
    private NumericUpDown numId;
    private Button btnDodaj;
    private Button btnObrisi;

    // Lista u kojoj se prikazuju artikli
    private ListBox lstArtikli;

    // Polje za unos ID artikla
    private NumericUpDown numArtikalId;

    // Polje za unos naziva artikla
    private TextBox txtArtikalNaziv;

    // Polje za unos cene
    private NumericUpDown numCena;

    // Izbor jedinice mere
    private ComboBox cmbJedinicaMere;

    // Izbor kategorije artikla
    private ComboBox cmbKategorija;

    // Dugme za dodavanje artikla
    private Button btnDodajArtikal;

    // Dugme za brisanje artikla
    private Button btnObrisiArtikal;

    public AdministracijaForm(
        KategorijaService kategorijaService,
        ArtikalService artikalService,
        PodaciService podaciService)
    {
        // Čuvamo servise koje koristimo u ovoj formi
        this.kategorijaService = kategorijaService;
        this.artikalService = artikalService;
        this.podaciService = podaciService;

        // Podešavanje prozora
        Text = "Administracija i statistika";
        Width = 700;
        Height = 500;
        StartPosition = FormStartPosition.CenterScreen;

        // ================= KATEGORIJE =================

        // Naslov
        var lblNaslov = new Label();
        lblNaslov.Text = "Kategorije artikala";
        lblNaslov.Font = new Font(
            "Arial",
            20,
            FontStyle.Bold);
        lblNaslov.AutoSize = true;
        lblNaslov.Location = new Point(30, 25);

        // Labela za ID
        var lblId = new Label();
        lblId.Text = "ID kategorije:";
        lblId.AutoSize = true;
        lblId.Location = new Point(30, 90);

        // Polje za unos ID-a
        numId = new NumericUpDown();
        numId.Minimum = 1;
        numId.Maximum = 999999;
        numId.Location = new Point(150, 85);
        numId.Width = 150;

        // Labela za naziv
        var lblNaziv = new Label();
        lblNaziv.Text = "Naziv kategorije:";
        lblNaziv.AutoSize = true;
        lblNaziv.Location = new Point(30, 130);

        // Polje za unos naziva
        txtNaziv = new TextBox();
        txtNaziv.Location = new Point(150, 125);
        txtNaziv.Width = 150;

        // Dugme za dodavanje kategorije
        btnDodaj = new Button();
        btnDodaj.Text = "Dodaj";
        btnDodaj.Width = 100;
        btnDodaj.Location = new Point(320, 85);

        // Dugme za brisanje kategorije
        btnObrisi = new Button();
        btnObrisi.Text = "Obrisi";
        btnObrisi.Width = 100;
        btnObrisi.Location = new Point(320, 125);

        // Lista kategorija
        lstKategorije = new ListBox();
        lstKategorije.Location = new Point(30, 190);
        lstKategorije.Width = 390;
        lstKategorije.Height = 220;

        // ================= ARTIKLI =================

        // Naslov dela za artikle
        var lblArtikli = new Label();
        lblArtikli.Text = "Artikli";
        lblArtikli.Font = new Font(
            "Arial",
            16,
            FontStyle.Bold);
        lblArtikli.AutoSize = true;
        lblArtikli.Location = new Point(450, 25);

        // Labela za ID artikla
        var lblArtikalId = new Label();
        lblArtikalId.Text = "ID:";
        lblArtikalId.AutoSize = true;
        lblArtikalId.Location = new Point(450, 70);

        // Polje za unos ID-a artikla
        numArtikalId = new NumericUpDown();
        numArtikalId.Minimum = 1;
        numArtikalId.Maximum = 999999;
        numArtikalId.Location = new Point(520, 65);
        numArtikalId.Width = 120;

        // Labela za naziv artikla
        var lblArtikalNaziv = new Label();
        lblArtikalNaziv.Text = "Naziv:";
        lblArtikalNaziv.AutoSize = true;
        lblArtikalNaziv.Location = new Point(450, 105);

        // Polje za unos naziva artikla
        txtArtikalNaziv = new TextBox();
        txtArtikalNaziv.Location = new Point(520, 100);
        txtArtikalNaziv.Width = 120;

        // Labela za cenu
        var lblCena = new Label();
        lblCena.Text = "Cena:";
        lblCena.AutoSize = true;
        lblCena.Location = new Point(450, 140);

        // Polje za unos cene
        numCena = new NumericUpDown();
        numCena.Minimum = 0;
        numCena.Maximum = 1000000;
        numCena.DecimalPlaces = 2;
        numCena.Location = new Point(520, 135);
        numCena.Width = 120;

        // Labela za jedinicu mere
        var lblJedinica = new Label();
        lblJedinica.Text = "Jedinica:";
        lblJedinica.AutoSize = true;
        lblJedinica.Location = new Point(450, 175);

        // Izbor jedinice mere
        cmbJedinicaMere = new ComboBox();
        cmbJedinicaMere.DropDownStyle =
            ComboBoxStyle.DropDownList;

        cmbJedinicaMere.Items.AddRange(
            new object[]
            {
                "kom",
                "kg",
                "l"
            });

        cmbJedinicaMere.Location = new Point(520, 170);
        cmbJedinicaMere.Width = 120;
        cmbJedinicaMere.SelectedIndex = 0;

        // Labela za kategoriju
        var lblKategorija = new Label();
        lblKategorija.Text = "Kategorija:";
        lblKategorija.AutoSize = true;
        lblKategorija.Location = new Point(450, 210);

        // Izbor kategorije
        cmbKategorija = new ComboBox();
        cmbKategorija.DropDownStyle =
            ComboBoxStyle.DropDownList;

        cmbKategorija.Location = new Point(520, 205);
        cmbKategorija.Width = 120;

        // Dodavanje postojećih kategorija
        foreach (var kategorija in kategorijaService.Kategorije)
        {
            cmbKategorija.Items.Add(kategorija);
        }

        // Ako postoji kategorija, biramo prvu
        if (cmbKategorija.Items.Count > 0)
        {
            cmbKategorija.SelectedIndex = 0;
        }

        // Dugme za dodavanje artikla
        btnDodajArtikal = new Button();
        btnDodajArtikal.Text = "Dodaj artikal";
        btnDodajArtikal.Width = 120;
        btnDodajArtikal.Location = new Point(450, 250);

        // Dugme za brisanje artikla
        btnObrisiArtikal = new Button();
        btnObrisiArtikal.Text = "Obriši artikal";
        btnObrisiArtikal.Width = 120;
        btnObrisiArtikal.Location = new Point(450, 290);

        // Lista artikala
        lstArtikli = new ListBox();
        lstArtikli.Location = new Point(450, 330);
        lstArtikli.Width = 190;
        lstArtikli.Height = 120;

        // ================= DOGAĐAJI =================

        btnDodaj.Click += BtnDodaj_Click;
        btnObrisi.Click += BtnObrisi_Click;

        btnDodajArtikal.Click += BtnDodajArtikal_Click;
        btnObrisiArtikal.Click += BtnObrisiArtikal_Click;

        // ================= DODAVANJE KONTROLA =================

        Controls.Add(lblNaslov);
        Controls.Add(lblId);
        Controls.Add(numId);
        Controls.Add(lblNaziv);
        Controls.Add(txtNaziv);
        Controls.Add(btnDodaj);
        Controls.Add(btnObrisi);
        Controls.Add(lstKategorije);

        Controls.Add(lblArtikli);
        Controls.Add(lblArtikalId);
        Controls.Add(numArtikalId);
        Controls.Add(lblArtikalNaziv);
        Controls.Add(txtArtikalNaziv);
        Controls.Add(lblCena);
        Controls.Add(numCena);
        Controls.Add(lblJedinica);
        Controls.Add(cmbJedinicaMere);
        Controls.Add(lblKategorija);
        Controls.Add(cmbKategorija);
        Controls.Add(btnDodajArtikal);
        Controls.Add(btnObrisiArtikal);
        Controls.Add(lstArtikli);

        // Prikaz postojećih kategorija
        OsveziKategorije();

        // Prikaz postojećih artikala
        OsveziArtikle();
    }

    // ================= KATEGORIJE =================

    private void BtnDodaj_Click(
        object? sender,
        EventArgs e)
    {
        try
        {
            // Kreiranje nove kategorije
            var kategorija = new Kategorija(
                (int)numId.Value,
                txtNaziv.Text);

            // Dodavanje kategorije kroz servis
            kategorijaService.Dodaj(kategorija);

            // Čuvanje kategorije u SQLite bazi
            podaciService.SacuvajKategoriju(kategorija);

            // Osvežavanje liste kategorija
            OsveziKategorije();

            // Osvežavanje ComboBox-a
            OsveziKategorijeComboBox();

            // Čišćenje polja
            txtNaziv.Clear();
            numId.Value = 1;
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Greška",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void BtnObrisi_Click(
        object? sender,
        EventArgs e)
    {
        if (lstKategorije.SelectedItem
            is not Kategorija kategorija)
            return;

        try
        {
            //brisanje kategorije kroz servis
            kategorijaService.Obrisi(kategorija.Id);
            podaciService.ObrisiKategoriju(kategorija.Id);
          

            //osvezavanje prikaza
            OsveziKategorije();
            OsveziKategorijeComboBox();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Greška",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void OsveziKategorije()
    {
        //brisanje trenutnog prikaza
        lstKategorije.Items.Clear();

        //dodavanje svih kategorija
        foreach (var kategorija
            in kategorijaService.Kategorije)
        {
            lstKategorije.Items.Add(kategorija);
        }
    }

    private void OsveziKategorijeComboBox()
    {
        //brisanje starih kategorija
        cmbKategorija.Items.Clear();

        //ponovno ucitavanje kategorija
        foreach (var kategorija
            in kategorijaService.Kategorije)
        {
            cmbKategorija.Items.Add(kategorija);
        }

        //automatski izbor prve kategorije
        if (cmbKategorija.Items.Count > 0)
        {
            cmbKategorija.SelectedIndex = 0;
        }
    }

    // ================= ARTIKLI =================

    private void BtnDodajArtikal_Click(
        object? sender,
        EventArgs e)
    {
        try
        {
            //provera da li je kategorija izabrana
            if (cmbKategorija.SelectedItem
                is not Kategorija kategorija)
            {
                MessageBox.Show(
                    "Izaberite kategoriju.",
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            //kreiranje novog artikla
            var artikal = new Artikal(
                (int)numArtikalId.Value,
                txtArtikalNaziv.Text,
                kategorija.Id,
                numCena.Value,
                cmbJedinicaMere.Text);

            //dodavanje artikla kroz servis
            artikalService.Dodaj(artikal);

            //cuvanjee artikla u SQLite bazi
            podaciService.SacuvajArtikal(artikal);

            //osvezavanje liste artikala
            OsveziArtikle();

            //ciscenje polja
            txtArtikalNaziv.Clear();
            numArtikalId.Value = 1;
            numCena.Value = 0;
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Greška",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void BtnObrisiArtikal_Click(
        object? sender,
        EventArgs e)
    {
        //provera da li je artikal izabran
        if (lstArtikli.SelectedItem
            is not Artikal artikal)
            return;

        try
        {
            //brisanje artikla kroz servis
            artikalService.Obrisi(artikal.Id);

            //brise artikal i iz SQLite baze
            podaciService.ObrisiArtikal(artikal.Id);

            //osvezavanje liste
            OsveziArtikle();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Greška",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void OsveziArtikle()
    {
        //brisanje trenutnog prikaza
        lstArtikli.Items.Clear();

        //dodavanje svih artikala
        foreach (var artikal
            in artikalService.Artikli)
        {
            lstArtikli.Items.Add(artikal);
        }
    }
}