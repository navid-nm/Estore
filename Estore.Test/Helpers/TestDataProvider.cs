using System.Collections.Generic;
using System.Data.SqlClient;

namespace Estore.Test.Helpers
{
    /// <summary>
    /// Creates sample entities for tests.
    /// </summary>
    class TestDataProvider
    {
        public readonly string conn;

        public TestDataProvider()
        {
            conn = "Server=localhost;Database=estore;Integrated Security=True;"
                 + "Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;"
                 + "ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        /// <summary>
        /// Create a user row with an ID of 0 if it does not exist.
        /// </summary>
        public void CreateZeroUser()
        {
            string query = "SELECT * FROM dbo.Users WHERE Id = 0";
            string update = @"
                INSERT INTO dbo.Users (Id, Username, FirstName, Surname, [Password], Email, DateOfRegistration, 
                ShippingLocationId) 
                VALUES (0, 'TestUser', 'First', 'Second', 'null', 'testuser@test.com', GETDATE(), NULL) 
            ";
            CheckAndAction(query, update, "Users");
        }

        /// <summary>
        /// Create an item row with an ID of 0 if it does not exist.
        /// </summary>
        public void CreateZeroItem()
        {
            string query = "SELECT * FROM dbo.Items WHERE Id = 0";
            string update = @"
                INSERT INTO dbo.Items (Id, UserId, FindCode, Name, Quality, Description, Price, 
                Concluded, ShippingCost) 
                VALUES (0, 0, 'TestFindCode', 'TestName', 0, 'TestDescription', 10.00, 0, 1.00) 
            ";
            CheckAndAction(query, update, "Items");
        }

        /// <summary>
        /// Check whether data insertion is necessary and carry it out if it is.
        /// </summary>
        /// <param name="query">Query to check whether insertion is necessary</param>
        /// <param name="update">Update to be carried out</param>
        /// <param name="tabName">Name of table</param>
        private void CheckAndAction(string query, string update, string tabName)
        {
            using var sqlc = new SqlConnection(conn);
            sqlc.Open();
            var com = new SqlCommand(query, sqlc);
            SqlDataReader reader = com.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                string ins = "SET identity_insert dbo." + tabName + " ";
                string completeUpdate = ins + "ON;\n" + update + ";" + ins + "OFF";
                new SqlCommand(completeUpdate, sqlc).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Remove test entities.
        /// </summary>
        public void Down()
        {
            var com = string.Empty;
            foreach (var tab in new List<string> { "Items", "Users" })
            {
                com += "DELETE FROM dbo." + tab + " WHERE Id = 0;";
            }
            using var sqlc = new SqlConnection(conn);
            sqlc.Open();
            new SqlCommand(com, sqlc).ExecuteNonQuery();
        }
    }
}
