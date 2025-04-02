using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace labsgbd1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AfiseazaMagazin();

            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            btnSterge.Click += btnSterge_Click;
        }

        private void AfiseazaMagazin()
        {
            String connectionString = @"Server=DESKTOP-PB5S12C\SQLEXPRESS;Database=lab4;
            Integrated Security = True;TrustServerCertificate=True;";
            string query = "SELECT * FROM Magazin";


            using (SqlConnection con = new SqlConnection(connectionString))
                try
                {
                    {
                        con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                        DataTable tabel = new DataTable();
                        adapter.Fill(tabel);
                        dataGridView1.DataSource = tabel;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {

                int idMagazin = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);


                AfiseazaAngajati(idMagazin);
            }
        }
        private void AfiseazaAngajati(int idMagazin)
        {
            String connectionString = @"Server=DESKTOP-PB5S12C\SQLEXPRESS;Database=lab4;
    Integrated Security = True;TrustServerCertificate=True;";

            string query = "SELECT * FROM Angajat WHERE ID_magazin = @idMagazin";

            using (SqlConnection con = new SqlConnection(connectionString))
                try
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@idMagazin", idMagazin);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable tabel = new DataTable();
                    adapter.Fill(tabel);

                    dataGridView2.DataSource = tabel;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private void btnSterge_Click(object sender, EventArgs e)
        {

            if (dataGridView2.SelectedRows.Count > 0)
            {

                int idAngajat = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
                DialogResult result = MessageBox.Show("Sigur vrei să ștergi acest angajat?", "Confirmare", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {

                    StergeAngajat(idAngajat);
                }
            }
            else
            {
                MessageBox.Show("Te rog selectează un angajat din tabel.");
            }
        }

        private void StergeAngajat(int idAngajat)
        {
            String connectionString = @"Server=DESKTOP-PB5S12C\SQLEXPRESS;Database=lab4;
    Integrated Security=True;TrustServerCertificate=True;";

            string query = "DELETE FROM Angajat WHERE ID_angajat = @idAngajat";
            string query1 = "SELECT ID_magazin FROM Angajat WHERE ID_angajat = @idAngajat";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();


                    SqlCommand cmd1 = new SqlCommand(query1, con);
                    cmd1.Parameters.AddWithValue("@idAngajat", idAngajat);
                    object result = cmd1.ExecuteScalar();
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@idAngajat", idAngajat);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {

                        int idMagazin = Convert.ToInt32(result);
                        AfiseazaAngajati(idMagazin);
                        MessageBox.Show("Angajatul a fost șters cu succes!");


                    }
                    else
                    {
                        MessageBox.Show("Nu s-a găsit angajatul pentru ștergere.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    if (!string.IsNullOrWhiteSpace(textBox2.Text))
                    {
                        int idAngajat = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
                        DialogResult result = MessageBox.Show("Sigur vrei sa actualizezi datele angajatului?", "Confirmare", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {

                            ActualizareAngajat(idAngajat);
                        }
                    }
                    else { MessageBox.Show("Campul id este gol."); }
                }
                else
                {
                    MessageBox.Show("Campul nume este gol");
                }
            }
            else
            {
                MessageBox.Show("Te rog selecteaza un angajat din tabel.");
            }
        }

        private void ActualizareAngajat(int idAngajat)
        {
            String connectionString = @"Server=DESKTOP-PB5S12C\SQLEXPRESS;Database=lab4;
    Integrated Security=True;TrustServerCertificate=True;";

            string query = "UPDATE Angajat set Nume_angajat=@nume, ID_magazin=@id_magazin where ID_angajat = @idAngajat ";


            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();


                    string nume = textBox1.Text;

                    if (!char.IsUpper(nume[0]))
                        throw new Exception("Numele trebuie sa înceapa cu litera mare.");
                    int idMagazin = Convert.ToInt32(textBox2.Text);
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@nume", nume);
                    cmd.Parameters.AddWithValue("@id_magazin", idMagazin);
                    cmd.Parameters.AddWithValue("@idAngajat", idAngajat);


                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {


                        AfiseazaAngajati(idMagazin);
                        MessageBox.Show("Angajatul a fost actualizat cu succes!");


                    }
                    else
                    {
                        MessageBox.Show("Nu s-a găsit angajatul pentru ștergere.");
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    AdaugaAngajat();
                }
                else
                {
                    MessageBox.Show("Campul nume este gol");
                }
            }
            else
            {
                MessageBox.Show("Te rog selecteaza un magazin din tabel.");
            }
        }

        private void AdaugaAngajat()
        {
            String connectionString = @"Server=DESKTOP-PB5S12C\SQLEXPRESS;Database=lab4;
    Integrated Security=True;TrustServerCertificate=True;";
            string query = "INSERT INTO Angajat(Nume_angajat,ID_magazin) VALUES(@nume,@id_magazin)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string nume = textBox1.Text;

                    if (!char.IsUpper(nume[0]))
                        throw new Exception("Numele trebuie sa înceapa cu litera mare.");
                    int idMagazin = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                    Console.WriteLine(idMagazin);
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@nume", nume);
                    cmd.Parameters.AddWithValue("@id_magazin", idMagazin);
                    cmd.ExecuteNonQuery();


                    AfiseazaAngajati(idMagazin);
                    MessageBox.Show("Angajatul a fost adaugat cu succes!");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }

            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}