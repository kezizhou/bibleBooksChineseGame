using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleBooks {
	class DatabaseConnection {

		private static void connectToDatabase() {
			SqlConnection conn = new SqlConnection("Server=LAPTOP-M4399HDS;Database=dbBibleBooks;Integrated Security=true;");
			conn.Open();
		}

		private static void executeSQL(SqlConnection conn) {
			string qry = "SELECT * FROM TUsers";
			SqlDataAdapter da = new SqlDataAdapter(qry, conn);
			DataSet ds = new DataSet();
			da.Fill(ds);
		}
	}
}
