using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Data.SQLite;
using System.Globalization;

namespace Backend.REST
{
    [DataContract]
    public class Product
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public double Price { get; set; }

        [DataMember]
        public bool InStock { get; set; }
    }

    [DataContract]
    public class ReadProductsOutput
    {
        [DataMember]
        public List<Product> Products { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; } // Null if OK
    }

    public partial class ReadProducts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AddHeader("Access-Control-Allow-Origin", "*");

            ReadProductsOutput output = new ReadProductsOutput();
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=DGDAAssignment.sqlite;Version=3;");

            try
            {
                // Get list of all products from db
                m_dbConnection.Open();
                string sql = "SELECT * FROM Products";
                
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (output.Products == null)
                    {
                        output.Products = new List<Product>();
                    }

                    Product p = new Product();
                    p.ID = (String)reader["ID"];
                    p.Name = (String)reader["Name"];
                    p.Description = (String)reader["Description"];
                    p.Price = (double)((Decimal)reader["Price"]);
                    p.InStock = Convert.ToBoolean((Decimal)reader["InStock"]);

                    output.Products.Add(p);
                }
            }
            catch (Exception ex)
            {
                output.ErrorMessage = ex.Message;
            }
            finally
            {
                m_dbConnection.Close();
            }

            DataContractJsonSerializer output_serializer = new DataContractJsonSerializer(typeof(ReadProductsOutput));
            output_serializer.WriteObject(Response.OutputStream, output);
        }
    }
}