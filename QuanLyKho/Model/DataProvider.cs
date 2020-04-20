using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace QuanLyKho.Model
{
    public class DataProvider
    {
        private static DataProvider instance;

        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return instance; }
            private set { instance = value; }
        }
        // cách gọi DataProvider.Instance. ....
        private DataProvider() { }

        private string connectionSTR = "data source=desktop-tdmbjtk\\sqlexpress;initial catalog=QUANLYKHO;integrated security=true";
        //private string connectionSTR = "data source=desktop-tdmbjtk\\sqlexpress;initial catalog=QUANLYKHO;integrated security=true";
        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();
            DataSet dts = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }




                SqlDataAdapter adapter = new SqlDataAdapter(command);
                

                   adapter.Fill(dts);
                data = dts.Tables[0];


                connection.Close();
            }
            return data;
        }
      
        public int ExecuteNonQuery(string query, object[] parameter = null) // trả về số dòng thành công (INSERT)
        {
            int data = 0;

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }

                }

                data = command.ExecuteNonQuery();

                connection.Close();
            }
            return data;
        }

        public object ExecuteScalar(string query, object[] parameter = null)  // trả về số lượng COUNT(*)
        {
            object data = 0;

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }

                }

                data = command.ExecuteScalar();

                connection.Close();
            }
            return data;
        }
    }
}
