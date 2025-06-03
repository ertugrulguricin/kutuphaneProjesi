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
    public partial class formkitap : Form
    {

        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-EI3O5LF;Initial Catalog=kutuphaneProjesi;Integrated Security=True;");

        public formkitap()
        {
            InitializeComponent();
        }

        public void listele()
        {
            SqlCommand komut = new SqlCommand("select * from kitaplar", baglanti);
            SqlDataAdapter adapter = new SqlDataAdapter(komut);
            DataTable tablo = new DataTable();
            adapter.Fill(tablo);
            dataGridView1.DataSource = tablo;
        }

        public void kitapyükle()
        {
            try
            {
                SqlCommand komut = new SqlCommand("select * from kitap_turleri", baglanti);
                SqlDataAdapter adapter = new SqlDataAdapter(komut);
                DataTable tablo = new DataTable();
                adapter.Fill(tablo);
                comboBox1.DataSource = tablo;
                comboBox1.DisplayMember = "tur_adi";
                comboBox1.ValueMember = "tur_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedValue == null)
                {
                    MessageBox.Show("Tür seçilmedi. Lütfen bir tür seçin.");
                    return;
                }

                SqlCommand komut = new SqlCommand("INSERT INTO kitaplar (tur_id, kitap_adi, yazar, yayinevi, sayfa_sayisi) VALUES (@tur_id, @kitap_adi, @yazar, @yayin_evi, @sayfa_sayisi)", baglanti);
                komut.Parameters.AddWithValue("@tur_id", int.Parse(comboBox1.SelectedValue.ToString()));
                komut.Parameters.AddWithValue("@kitap_adi", textBox1.Text);
                komut.Parameters.AddWithValue("@yazar", textBox2.Text);
                komut.Parameters.AddWithValue("@yayin_evi", textBox3.Text);
                komut.Parameters.AddWithValue("@sayfa_sayisi", int.Parse(textBox4.Text));

                baglanti.Open();
                komut.ExecuteNonQuery();
                listele();
                MessageBox.Show("Kitap başarıyla eklendi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow != null)
                {
                    string kitapId = dataGridView1.CurrentRow.Cells["kitap_id"].Value.ToString();

                    SqlCommand komut = new SqlCommand("DELETE FROM kitaplar WHERE kitap_id = @kitap_id", baglanti);
                    komut.Parameters.AddWithValue("@kitap_id", kitapId);

                    baglanti.Open();
                    komut.ExecuteNonQuery();
                    listele();
                    MessageBox.Show("Kitap başarıyla silindi.");
                }
                else
                {
                    MessageBox.Show("Lütfen silmek için bir satır seçin.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                }

                SqlCommand komut = new SqlCommand("UPDATE kitaplar SET tur_id = @tur_id, kitap_adi = @kitap_adi, yazar = @yazar, yayinevi = @yayinevi, sayfa_sayisi = @sayfa_sayisi WHERE kitap_id = @kitap_id", baglanti);

                komut.Parameters.AddWithValue("@kitap_id", int.Parse(dataGridView1.CurrentRow.Cells["kitap_id"].Value.ToString()));
                komut.Parameters.AddWithValue("@tur_id", int.Parse(comboBox1.SelectedValue.ToString()));
                komut.Parameters.AddWithValue("@kitap_adi", textBox1.Text);
                komut.Parameters.AddWithValue("@yazar", textBox2.Text);
                komut.Parameters.AddWithValue("@yayinevi", textBox3.Text);
                komut.Parameters.AddWithValue("@sayfa_sayisi", int.Parse(textBox4.Text));

                komut.ExecuteNonQuery();
                MessageBox.Show("Güncelleme başarılı.");
                listele();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        public void ara(string aranacakelime)
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed) baglanti.Open();

                SqlCommand cmd = new SqlCommand("select * from kitaplar where kitap_id LIKE '" + aranacakelime + "%'", baglanti);
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

        private void formkitap_Load(object sender, EventArgs e)
        {
            listele();
            kitapyükle();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textBox1.Text = row.Cells["kitap_adi"].Value.ToString();
                textBox2.Text = row.Cells["yazar"].Value.ToString();
                textBox3.Text = row.Cells["yayinevi"].Value.ToString();
                comboBox1.Text = row.Cells["tur_id"].Value.ToString();
                textBox4.Text = row.Cells["sayfa_sayisi"].Value.ToString(); 
            }
        }
    }
}
