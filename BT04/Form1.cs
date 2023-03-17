using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BT04
{
	public partial class Form1 : Form
	{
		public static Form1 Instance;
		public Form1()
		{
			InitializeComponent();
			Init();
		}

		public void Init()
		{

			Instance = this;

			// Load class
			cbLopSH.DataSource = DBHelper.Instance.GetAllClass();

			// Load Students
			gvTable.DataSource  = DBHelper.Instance.GetAllRecord(new SqlCommand("SELECT * FROM SV"));

			// Load Sort
			cbSort.Items.Add("ID");
			cbSort.Items.Add("Name");
			cbSort.Items.Add("GPA");
			cbSort.SelectedIndex = 0;


		}
		public void btnSearch_Click(object sender, EventArgs e)
		{
			SqlCommand cmd = new SqlCommand();
			string className = cbLopSH.SelectedItem as string;
			if (className == "All") className = "";
			if (txtSearch.Text.Contains(','))
			{
				
				string[] nameAndID = txtSearch.Text.Split(',');
				// xoa bo khoang trang
				nameAndID[0] = nameAndID[0].Trim();
				nameAndID[1]=nameAndID[1].Trim();
				cmd.CommandText = $"SELECT * FROM SV\r\nWHERE Name LIKE '%{nameAndID[0]}%' and ID LIKE '%{nameAndID[1]}%' and class like '%{className}%';";
			}
			else
			{
				string name = txtSearch.Text;
				cmd.CommandText = $"SELECT * FROM SV\r\nWHERE Name LIKE '%{name}%' and class like '%{className}%';";
				
			}
			gvTable.DataSource = DBHelper.Instance.GetAllRecord(cmd);
		}

		private void btnDel_Click(object sender, EventArgs e)
		{
			DialogResult result ;
			result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận xóa", MessageBoxButtons.YesNo);

			if (result == DialogResult.Yes)
			{
				if (gvTable.SelectedRows.Count > 0)
				{
					List<string> list = new List<string>();
					foreach (DataGridViewRow row in gvTable.SelectedRows)
					{
						list.Add(row.Cells[0].Value.ToString());
					}
					DBHelper.Instance.RemoveSV(list);
				}
			}
			
			btnSearch_Click(sender, e);
		}

		private void cbLopSH_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbLopSH.SelectedIndex == 0)
			{
				gvTable.DataSource = DBHelper.Instance.GetAllRecord(new SqlCommand("SELECT * FROM SV"));
			}
			else
			{
				gvTable.DataSource = DBHelper.Instance.GetAllRecord(new SqlCommand($"SELECT * FROM SV WHERE  class ='{cbLopSH.SelectedItem.ToString()}';"));
			}
			txtSearch.Text = "";
		}

		private void btnSort_Click(object sender, EventArgs e)
		{
			string className = cbLopSH.SelectedItem.ToString();
			if (className == "All") className="";

			string option = cbSort.SelectedItem.ToString();
			SqlCommand cmd = new SqlCommand($"select * FROM SV where class like '%{className}%' order by {option}");
			gvTable.DataSource = DBHelper.Instance.GetAllRecord(cmd);
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			Form2 f2 = new Form2();
			f2.ShowDialog();
		}

		private void btnEdit_Click(object sender, EventArgs e)
		{
			if (gvTable.SelectedRows.Count == 1)
			{
				string mssv = gvTable.SelectedRows[0].Cells[0].Value.ToString();
				Form2 f2 = new Form2(mssv);
				f2.ShowDialog();

			}

		}
	}
}
