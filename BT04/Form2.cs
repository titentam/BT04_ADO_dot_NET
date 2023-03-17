using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BT04
{
	public partial class Form2 : Form
	{
		private string _Id;

		// add
		public Form2()
		{
			InitializeComponent();
			InitAdd();
		}

		// update
		public Form2(string Id)
		{
			_Id = Id;
			InitializeComponent();
			InitUpdate();
		}
		public void InitAdd()
		{
			InitCbLopSh();
			btnOk.Click += new System.EventHandler(this.btnOk_ClickAdd);
			txtMSSV.Enabled = true;
		}

		public void InitUpdate()
		{
			InitCbLopSh();
			btnOk.Click += new System.EventHandler(this.btnOk_ClickEdit);
			

			// load sv
			var table = DBHelper.Instance.GetAllRecord(new SqlCommand($"select * from sv where ID = '{_Id}'"));
			var sv = table.Rows[0];

			txtMSSV.Text = sv["ID"].ToString();
			txtName.Text = sv["Name"].ToString();
			cbLopSh.SelectedIndex = cbLopSh.Items.IndexOf(sv["Class"].ToString());
			dtNs.Text = sv["Dob"].ToString();
			if ((bool)(sv["Gender"]))
			{
				rbtnMale.Checked = true;
			}
			else
			{
				rbtnFemale.Checked = true;
			}
			txtDTB.Text = sv["GPA"].ToString();
			checkAnh.Checked = (bool)(sv["Picture"]);
			checkHocBa.Checked = (bool)(sv["School_report"]);
			checkCCCD.Checked = (bool)(sv["CCCD"]);
			
		}


		private void InitCbLopSh()
		{
			var listClass = DBHelper.Instance.GetAllClass();
			listClass.Remove("All");
			cbLopSh.DataSource = listClass;
		}


		private void btnOk_ClickAdd(object sender, EventArgs e)
		{	
			var ID = txtMSSV.Text;
			var Name = txtName.Text;	
			var Dob = Convert.ToDateTime(dtNs.Value.ToShortDateString());
			float GPA;
			float.TryParse(txtDTB.Text, out GPA);	

			// ktra Id co ton tai hay khong
			bool checkID = DBHelper.Instance.GetAllRecord(new SqlCommand($"select * from sv where ID = '{_Id}'")).Rows.Count==0;
			bool checkOK=true;

			if (checkID)
			{
				if (ID != ""&&ID.Length<=8 && Name != "" )
				{
					SqlCommand cmd = new SqlCommand("INSERT INTO SV (ID, Name, Class, Gender, Dob, GPA, Picture, School_report, CCCD) " +
						$"VALUES (@ID, @Name, @Class, @Gender, @Dob, @GPA, @Picture, @School_report, @CCCD)");
						cmd.Parameters.Add(new SqlParameter("@ID", ID));
						cmd.Parameters.Add(new SqlParameter("@Name", Name));
						cmd.Parameters.Add(new SqlParameter("@Class", cbLopSh.SelectedItem.ToString()));
						cmd.Parameters.Add(new SqlParameter("@Gender", rbtnMale.Checked?1:0));
						cmd.Parameters.Add(new SqlParameter("@Dob", Dob));
						cmd.Parameters.Add(new SqlParameter("@GPA", GPA));
						cmd.Parameters.Add(new SqlParameter("@Picture", checkAnh.Checked ? 1 : 0));
						cmd.Parameters.Add(new SqlParameter("@School_report", checkHocBa.Checked ? 1 : 0));
						cmd.Parameters.Add(new SqlParameter("@CCCD", checkCCCD.Checked ? 1 : 0));
					DBHelper.Instance.ExcueteUpdateDB(cmd);
			
				}
				else
				{
					MessageBox.Show("Thieu thong tin");
					checkOK = false;
				}
			}
			if (checkOK)
			{
				this.Dispose();
				Form1.Instance.btnSearch_Click(sender, e);
			}
		}

		private void btnOk_ClickEdit(object sender, EventArgs e)
		{
			var Name = txtName.Text;
			var Dob = Convert.ToDateTime(dtNs.Value.ToShortDateString());
			float GPA;
			float.TryParse(txtDTB.Text, out GPA);

			SqlCommand cmd = new SqlCommand("update SV set Name=@Name, Class=@Class, Gender=@Gender, Dob=@Dob, GPA=@GPA, Picture=@Picture, School_report=@School_report, CCCD=@CCCD" +
				$" where ID ='{_Id}'");
			cmd.Parameters.Add(new SqlParameter("@Name", Name));
			cmd.Parameters.Add(new SqlParameter("@Class", cbLopSh.SelectedItem.ToString()));
			cmd.Parameters.Add(new SqlParameter("@Gender", rbtnMale.Checked ? 1 : 0));
			cmd.Parameters.Add(new SqlParameter("@Dob", Dob));
			cmd.Parameters.Add(new SqlParameter("@GPA", GPA));
			cmd.Parameters.Add(new SqlParameter("@Picture", checkAnh.Checked ? 1 : 0));
			cmd.Parameters.Add(new SqlParameter("@School_report", checkHocBa.Checked ? 1 : 0));
			cmd.Parameters.Add(new SqlParameter("@CCCD", checkCCCD.Checked ? 1 : 0));
			DBHelper.Instance.ExcueteUpdateDB(cmd);


			this.Dispose();
			Form1.Instance.btnSearch_Click(sender, e);

		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Dispose();
		}
	}
}
