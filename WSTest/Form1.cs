using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            Checklas.Service srv = new Checklas.Service();
            string a = srv.Login();
            List<Checklas.Request> req = new List<Checklas.Request>();
            //req.Add(new Checklas.Request { Table = "OITM", Field = "ItemCode", condition = "=", Value = "B1", op = "or" });
            //req.Add(new Checklas.Request { Table = "OITM", Field = "ItemCode", condition = "=", Value = "CL", op = "" });
            List<Checklas.Item> item = srv.GetItems(a, req.ToArray()).itemlist.ToList();
            var bindingList = new BindingList<Checklas.Item>(item);
            var source = new BindingSource(bindingList, null);
            this.dataGridView1.DataSource = source;
        }
    }
}
