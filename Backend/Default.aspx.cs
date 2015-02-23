using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SQLite;

namespace Backend
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //SQLiteConnection.CreateFile("DGDAAssignment.sqlite");
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=DGDAAssignment.sqlite;Version=3;");
            m_dbConnection.Open();

            /*string sql = "CREATE TABLE Products (ID Text, Name TEXT, Description TEXT, Price NUMERIC, InStock NUMERIC)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery(); */

            /*string sql = "INSERT INTO Products (ID, Name, Description, Price, InStock) VALUES ('" + Guid.NewGuid() + "', 'Luksus Spaophold', 'Kombinationen af spa og koldbadehus giver en komplet spaoplevelse.', 3.399, 1)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery(); */

            /*string sql = "INSERT INTO Products (ID, Name, Description, Price, InStock) VALUES ('" + Guid.NewGuid() + "', 'Helikoptertur', 'Helikoptertur fra Aarhus, Odense eller Roskilde - når det passer dig eller gavemodtageren.', 2.850, 1)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            Response.Write(command.ExecuteNonQuery()); */

            /*string sql = "INSERT INTO Products (ID, Name, Description, Price, InStock) VALUES ('" + Guid.NewGuid() + "', 'Kør Lamborghini Gallardo', 'Lamborghinien i god fart, et sted i København.', 849, 0)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery(); */

            string sql = "SELECT * from Products";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Response.Write("ID: " + reader["ID"] + "Name: " + reader["Name"] + "\tDescription: " + reader["Description"]);
            
            m_dbConnection.Close();

        }
    }
}