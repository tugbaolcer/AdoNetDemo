using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetDemo
{
    public class ProductDal
    {

        SqlConnection _connection = new SqlConnection(@"server=(localdb)\mssqllocaldb; initial catalog=Etrade; integrated security=true");

        //DataTable kullanılması çok maliyetli bir nesne olduğundan genelde çok tercih edilmez.
        public DataTable GetAll2()
        {
            ConnectionControl();
            SqlCommand command = new SqlCommand("Select * from Products", _connection);

            SqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            reader.Close();
            _connection.Close();

            return dataTable;

        }

        //Listeleme işlemi
        public List<Product> GetAll()
        {

            ConnectionControl();

            SqlCommand command = new SqlCommand("Select * from Products", _connection);

            SqlDataReader reader = command.ExecuteReader();
            List<Product> products = new List<Product>();

            while (reader.Read())
            {
                Product product = new Product
                {
                    Id=Convert.ToInt32(reader["Id"]),
                    Name=reader["Name"].ToString(),
                    StockAmount=Convert.ToInt32(reader["StockAmount"]),
                    UnitPrice=Convert.ToInt32(reader["UnitPrice"])

                };
                products.Add(product);
            }


            reader.Close();
            _connection.Close();

            return products;

        }
        //Ekleme
        public void Add(Product product)
        {
            ConnectionControl();

            SqlCommand sqlCommand = new SqlCommand(
                "Insert Into Products values(@name, @unitPrice, @stockAmount)", _connection);
            //Sql injection saldırısına karşı bu şekilde yazılmalıdır.
            sqlCommand.Parameters.AddWithValue("@name", product.Name);
            sqlCommand.Parameters.AddWithValue("@unitPrice", product.UnitPrice);
            sqlCommand.Parameters.AddWithValue("@stockAmount", product.StockAmount);

            sqlCommand.ExecuteNonQuery();
            _connection.Close();

        }
        //Güncelleme
        public void Update(Product product)
        {
            ConnectionControl();

            SqlCommand sqlCommand = new SqlCommand(
                "Update Products set Name=@name, UnitPrice=@unitPrice, StockAmount=@stockAmount where Id=@id", _connection);
            //Sql injection saldırısına karşı bu şekilde yazılmalıdır.
            sqlCommand.Parameters.AddWithValue("@name", product.Name);
            sqlCommand.Parameters.AddWithValue("@unitPrice", product.UnitPrice);
            sqlCommand.Parameters.AddWithValue("@stockAmount", product.StockAmount);
            sqlCommand.Parameters.AddWithValue("@id", product.Id);

            sqlCommand.ExecuteNonQuery();
            _connection.Close();

        }


        public void Delete(int id)
        {
            ConnectionControl();

            SqlCommand sqlCommand = new SqlCommand(
                "Delete from Products where Id=@id", _connection);
           
            sqlCommand.Parameters.AddWithValue("@id", id);
            sqlCommand.ExecuteNonQuery();
            _connection.Close();

        }
        private void ConnectionControl()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }
    }

}
