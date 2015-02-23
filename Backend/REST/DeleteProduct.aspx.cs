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

namespace Backend.REST
{
    [DataContract]
    public class DeleteProductInput
    {
        [DataMember]
        public string ID { get; set; }
    }

    public class DeleteProductOutput
    {
        [DataMember]
        public string ErrorMessage { get; set; } // Null if OK
    }

    public partial class DeleteProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AddHeader("Access-Control-Allow-Origin", "*");

            DeleteProductOutput output = new DeleteProductOutput();
            DeleteProductInput input;

            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=DGDAAssignment.sqlite;Version=3;");

            try
            {
                string jsonInput = Request["JSON"];

                if (!String.IsNullOrEmpty(jsonInput))
                {
                    // Deserialize product JSON data and create input object
                    DataContractJsonSerializer inputSerializer = new DataContractJsonSerializer(typeof(DeleteProductInput));
                    byte[] inputBytes = Encoding.UTF8.GetBytes(jsonInput);
                    MemoryStream inputStream = new MemoryStream(inputBytes);
                    input = (DeleteProductInput)inputSerializer.ReadObject(inputStream);

                    output.ErrorMessage = null;

                    if (!String.IsNullOrEmpty(input.ID))
                    {
                        // Delete product entry from db
                        m_dbConnection.Open();
                        string sql = "DELETE FROM Products where ID='" + input.ID + "'";

                        SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                        command.ExecuteNonQuery();
                    }
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

            // Serialize return values
            DataContractJsonSerializer output_serializer = new DataContractJsonSerializer(typeof(DeleteProductOutput));
            output_serializer.WriteObject(Response.OutputStream, output);
        }
    }
}