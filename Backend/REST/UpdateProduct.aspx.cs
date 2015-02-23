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
    public class UpdateProductInput
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
    public class UpdateProductOutput
    {
        [DataMember]
        public string ErrorMessage { get; set; } // Null if OK
    }

    public partial class UpdateProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AddHeader("Access-Control-Allow-Origin", "*");

            UpdateProductOutput output = new UpdateProductOutput();
            UpdateProductInput input;

            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=DGDAAssignment.sqlite;Version=3;");

            try
            {
                string jsonInput = Request["JSON"];

                if (!String.IsNullOrEmpty(jsonInput))
                {
                    // Deserialize product JSON data and create input object
                    DataContractJsonSerializer inputSerializer = new DataContractJsonSerializer(typeof(UpdateProductInput));
                    byte[] inputBytes = Encoding.UTF8.GetBytes(jsonInput);
                    MemoryStream inputStream = new MemoryStream(inputBytes);
                    input = (UpdateProductInput)inputSerializer.ReadObject(inputStream);

                    output.ErrorMessage = null;

                    if (!String.IsNullOrEmpty(input.Name) && !String.IsNullOrEmpty(input.Description))
                    {
                        // Update product entry in db
                        m_dbConnection.Open();
                        string sql = "UPDATE Products SET " + 
                                     "Name='" +
                                     input.Name + 
                                     "', Description='" +
                                     input.Description + 
                                     "', Price=" +
                                     input.Price + 
                                     ", InStock=" +
                                     Convert.ToInt32(input.InStock) +  // True is 1, False is 0
                                     " WHERE ID='" + input.ID + "'";

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
            DataContractJsonSerializer output_serializer = new DataContractJsonSerializer(typeof(UpdateProductOutput));
            output_serializer.WriteObject(Response.OutputStream, output);
        }
    }
}