using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BT04
{
	internal class DBHelper
	{
		private static DBHelper _Instance;
		private SqlConnection _connectionString;
		public static DBHelper Instance
		{
			get
			{
				if (_Instance == null)
				{
					string s = "Data Source=MSI\\SQLEXPRESS01;Initial Catalog=BT04;Integrated Security=True";
					_Instance = new DBHelper(s);
				}
				return _Instance;
			}
			private set { }
		}
        public DBHelper(string s)
        {
			_connectionString = new SqlConnection(s);
        }

		public DataTable GetAllRecord(SqlCommand cmd)
		{
			cmd.Connection = _connectionString;
			SqlDataAdapter adapter = new SqlDataAdapter();

			adapter.SelectCommand = cmd;

			DataTable table = new DataTable();
			adapter.Fill(table);

			return table;
		}
		public List<string> GetAllClass()
		{
			_connectionString.Open();
			List<string> list = new List<string>();
			list.Add("All");
			SqlCommand cmd = new SqlCommand("SELECT DISTINCT class FROM sv", _connectionString);
			var reader = cmd.ExecuteReader();
			if (reader.HasRows)
			{
				while(reader.Read())
				{
					list.Add(reader.GetString(0));
				}
			}
			_connectionString.Close();

			return list;

		}
		public void RemoveSV(List<string> id)
		{
			_connectionString.Open();

			foreach (var item in id)
			{
				var cmd = new SqlCommand($"DELETE FROM sv WHERE id = '{item}'", _connectionString);
				
				cmd.ExecuteNonQuery();
			}

			_connectionString.Close();
		}
		public void ExcueteUpdateDB(SqlCommand cmd)
		{
			_connectionString.Open();
			cmd.Connection = _connectionString;

			cmd.ExecuteNonQuery();

			_connectionString.Close();
		}
	}
	
}
