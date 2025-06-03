using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace kutuphaneProjesi

{
    public partial class formOgrenci : Form
    {
        public formOgrenci()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-EI3O5LF;Initial Catalog=kutuphaneProjesi;Integrated Security=True;");

        public void listele()
        {
            SqlCommand komut = new SqlCommand("select * from ogrenciler", baglanti);
            SqlDataAdapter adapter = new SqlDataAdapter(komut);
            DataTable tablo = new DataTable();
            adapter.Fill(tablo);
            dataGridView1.DataSource = tablo;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("insert into ogrenciler(ogrenci_no,ad,soyad,sinif,cinsiyet,telefon)values (@ogrenci_no,@ad,@soyad,@sinif,@cinsiyet,@telefon)", baglanti);
            
            komut.Parameters.AddWithValue("@ogrenci_no", textBox1.Text);
            komut.Parameters.AddWithValue("@ad", textBox2.Text);
            komut.Parameters.AddWithValue("@soyad", textBox3.Text);
            komut.Parameters.AddWithValue("@sinif", comboBox1.Text);
            komut.Parameters.AddWithValue("@cinsiyet", comboBox2.Text);
            komut.Parameters.AddWithValue("@telefon", textBox4.Text);

            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            listele();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                string ogrenciNo = dataGridView1.CurrentRow.Cells["ogrenci_no"].Value.ToString();

                SqlCommand komut = new SqlCommand("DELETE FROM ogrenciler WHERE ogrenci_no = @ogrenci_no", baglanti);
                komut.Parameters.AddWithValue("@ogrenci_no", ogrenciNo);

                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();

                listele();
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir satır seçin.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Update ogrenciler set ad=@ad, soyad=@soyad, sinif=@sinif, cinsiyet=@cinsiyet, telefon=@telefon where ogrenci_no=@ogrenci_no", baglanti);
            
            komut.Parameters.AddWithValue("@ogrenci_no", dataGridView1.CurrentRow.Cells["ogrenci_no"].Value.ToString());
            komut.Parameters.AddWithValue("@ad", textBox2.Text);
            komut.Parameters.AddWithValue("@soyad", textBox3.Text);
            komut.Parameters.AddWithValue("@sinif", comboBox1.Text);
            komut.Parameters.AddWithValue("@cinsiyet", comboBox2.Text);
            komut.Parameters.AddWithValue("@telefon", textBox4.Text);

            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            listele();
        }

        private void formOgrenci_Load(object sender, EventArgs e)
        {
            listele();
        }

        public void ara(string aranacakelime)
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed) baglanti.Open();

                SqlCommand cmd = new SqlCommand("select * from ogrenciler where ogrenci_no LIKE '" + aranacakelime + "%'", baglanti);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open) baglanti.Close();
            }
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            ara(textBox5.Text);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textBox1.Text = row.Cells["ogrenci_no"].Value.ToString();
                textBox2.Text = row.Cells["ad"].Value.ToString();
                textBox3.Text = row.Cells["soyad"].Value.ToString();
                comboBox1.Text = row.Cells["sinif"].Value.ToString();
                comboBox2.Text = row.Cells["cinsiyet"].Value.ToString();
                textBox4.Text = row.Cells["telefon"].Value.ToString();
            }
        }
    }
}
