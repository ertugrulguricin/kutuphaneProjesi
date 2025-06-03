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
using System.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace kutuphaneProjesi
{
    public partial class formTur : Form
    {
        public formTur()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-EI3O5LF;Initial Catalog=kutuphaneProjesi;Integrated Security=True;");

        private void listele()
        {
            SqlCommand komut = new SqlCommand("select * from kitap_turleri", baglanti);
            SqlDataAdapter adapter = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void gridListe_CellClick(object sender, DataErrorsChangedEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells["tur_adi"].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("insert into kitap_turleri(tur_adi)values (@tur_adi)", baglanti);

            komut.Parameters.AddWithValue("@tur_adi", textBox1.Text);

            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            listele();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand komut = new SqlCommand("delete from kitap_turleri where tur_id=@tur_id", baglanti);

                komut.Parameters.AddWithValue("tur_id", dataGridView1.CurrentRow.Cells["tur_id"].Value.ToString());

                baglanti.Open();
                komut.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Silme işlemi başarısız oldu",ex.Message);
            }
            finally
            {
               if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
                listele();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("update kitap_turleri set tur_adi=@tur_adi where tur_id=@tur_id", baglanti);

            komut.Parameters.AddWithValue("@tur_adi", textBox1.Text);
            komut.Parameters.AddWithValue("@tur_id", dataGridView1.CurrentRow.Cells["tur_id"].Value.ToString());
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            listele();
        }

        private void formTur_Load(object sender, EventArgs e)
        {
            listele();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textBox1.Text = row.Cells["tur_adi"].Value.ToString();
            }
        }
    }
}
