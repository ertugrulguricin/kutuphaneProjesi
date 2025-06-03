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

namespace kutuphaneProjesi
{
    public partial class anaSayfa : Form
    {
        public anaSayfa()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            formkitap kitap = new formkitap();
            kitap.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            formOgrenci ogrenci = new formOgrenci();
            ogrenci.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            formTur tur = new formTur();
            tur.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            formOdunckitap odunc = new formOdunckitap();
            odunc.ShowDialog();
        }
    }
}
