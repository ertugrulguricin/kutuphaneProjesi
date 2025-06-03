using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace kutuphaneProjesi
{
    public partial class formOdunckitap : Form
    {
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-EI3O5LF;Initial Catalog=kutuphaneProjesi;Integrated Security=True;");

        public formOdunckitap()
        {
            InitializeComponent();
        }

        private void formOdunckitap_Load(object sender, EventArgs e)
        {
            kitapyukle();
            listele();
        }

        public void kitapyukle()
        {
            try
            {
                SqlCommand komut = new SqlCommand("SELECT * FROM kitaplar", baglanti);
                SqlDataAdapter adapter = new SqlDataAdapter(komut);
                DataTable tablo = new DataTable();
                adapter.Fill(tablo);
                comboBox1.DataSource = tablo;
                comboBox1.ValueMember = "kitap_id";
                comboBox1.DisplayMember = "kitap_adi";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listele()
        {
            try
            {
                SqlCommand komut = new SqlCommand("SELECT id ,ogrenci_no ,ad ,soyad ,kitap_adi ,verilis_tarihi ,teslim_tarihi ,aciklama " +
                    "FROM kitaplar ,ogrenciler ,odunc_kitaplar " +
                    "where ogr_no = ogrenci_no and kitaplar.kitap_id =odunc_kitaplar.kitap_id", baglanti);
                SqlDataAdapter adapter = new SqlDataAdapter(komut);
                DataTable tablo = new DataTable();
                adapter.Fill(tablo);
                dataGridView1.DataSource = tablo;

                dataGridView1.Columns["id"].HeaderText = "ID";
                dataGridView1.Columns["id"].Width = 30;
                dataGridView1.Columns["ogrenci_no"].HeaderText = "Öğrenci No";
                dataGridView1.Columns["ogrenci_no"].Width = 60;
                dataGridView1.Columns["ad"].HeaderText = "Ad";
                dataGridView1.Columns["ad"].Width = 70;
                dataGridView1.Columns["soyad"].HeaderText = "Soyad";
                dataGridView1.Columns["soyad"].Width = 70;
                dataGridView1.Columns["kitap_adi"].HeaderText = "Kitap Adı";
                dataGridView1.Columns["kitap_adi"].Width = 100;
                dataGridView1.Columns["verilis_tarihi"].HeaderText = "Veriliş Tarihi";
                dataGridView1.Columns["verilis_tarihi"].Width = 90;
                dataGridView1.Columns["teslim_tarihi"].HeaderText = "Teslim Tarihi";
                dataGridView1.Columns["teslim_tarihi"].Width = 90;
                dataGridView1.Columns["aciklama"].HeaderText = "Açıklama";
                dataGridView1.Columns["aciklama"].Width = 150;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Listeleme işlemi başarısız oldu. Hata: " + ex.Message, "Veritabanı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed) baglanti.Open();

                SqlCommand komut = new SqlCommand("INSERT INTO odunc_kitaplar(ogr_no,kitap_id,verilis_tarihi,aciklama) VALUES(@ogr_no,@kitap_id,@verilis_tarihi,@aciklama)", baglanti);

                komut.Parameters.AddWithValue("@ogr_no", int.Parse(textBox1.Text));
                komut.Parameters.AddWithValue("@kitap_id", int.Parse(comboBox1.SelectedValue.ToString()));
                komut.Parameters.AddWithValue("@verilis_tarihi", DateTime.Now.ToString("yyyy-MM-dd"));
                komut.Parameters.AddWithValue("@aciklama", richTextBox1.Text);
                komut.ExecuteNonQuery();
                temizle();
                kitapyukle();
                listele();

                MessageBox.Show("işlemi başarılı oldu.", "Mesaj", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("hata oluştu ", ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open) baglanti.Close();
            }
        }

        public void temizle()
        {
            textBox1.Clear();
            textBox2.Clear();
            richTextBox1.Clear();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                richTextBox1.Text = dataGridView1.CurrentRow.Cells["aciklama"].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed) baglanti.Open();

                SqlCommand komut = new SqlCommand("DELETE FROM odunc_kitaplar WHERE id=@id", baglanti);
                komut.Parameters.AddWithValue("@id", dataGridView1.CurrentRow.Cells["id"].Value.ToString());
                komut.ExecuteNonQuery();
                temizle();
                kitapyukle();
                listele();

                MessageBox.Show("Silme işlemi başarılı oldu.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bağlantı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open) baglanti.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed) baglanti.Open();

                SqlCommand komut = new SqlCommand("UPDATE odunc_kitaplar SET teslim_tarihi = @teslim_tarihi WHERE id = @id ", baglanti);

                komut.Parameters.AddWithValue("@id", int.Parse(dataGridView1.CurrentRow.Cells["id"].Value.ToString()));
                komut.Parameters.AddWithValue("@teslim_tarihi", DateTime.Now.ToString("yyyy-MM-dd"));
                komut.Parameters.AddWithValue("@aciklama", richTextBox1.Text);
                komut.ExecuteNonQuery();
                temizle();
                kitapyukle();
                listele();

                MessageBox.Show("Güncelleme işlemi başarılı oldu.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("hata oluştu ", ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open) baglanti.Close();
            }
        }

        public void ara(string aranacakelime)
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed) baglanti.Open();

                SqlCommand cmd = new SqlCommand("select * from odunc_kitaplar where id LIKE '" + aranacakelime + "%'", baglanti);
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ara(textBox2.Text);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textBox1.Text = row.Cells["ogrenci_no"].Value.ToString();
                richTextBox1.Text = row.Cells["aciklama"].Value.ToString();
                comboBox1.Text = row.Cells["kitap_adi"].Value.ToString();
            }
        }
    }
}