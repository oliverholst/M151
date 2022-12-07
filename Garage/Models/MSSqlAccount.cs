using System;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using Microsoft.Data.SqlClient;

namespace Garage.Models
{
    public class MSSqlAccount :  MSSqlBase, IAccount
    {
        /// <summary>
        /// Erzeugt 1 Account in der Tabelle Account
        /// </summary>
        /// <param name="account"></param>
        public void Create(Account account)
        {
            string sql = "";
            sql = "INSERT INTO Account(Password,Email)"
                    + "VALUES(@Password, @Email)";
            try
            {
                MD5 md5 = MD5.Create();

                //Command Klasse erzeugen
                using (SqlCommand cmd = new SqlCommand(sql, dbHandle))
                {
                    byte[] hashPassword = Encoding.Default.GetBytes(account.Passwort);
                    byte[] hashStore = md5.ComputeHash(hashPassword);
                    var hashStoreString = Convert.ToBase64String(hashStore);

                    cmd.Parameters.AddWithValue("@Email", account.Benutzername);
                    cmd.Parameters.AddWithValue("@Password", hashStoreString);
                    //Daten abfüllen
                    cmd.Parameters["@Email"].Value = account.Benutzername;
                    cmd.Parameters["@Password"].Value = hashStoreString;  

                    //ExecuteNonQuery -wird benutzt zum Daten einzufügen und zu löschen
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("INSERT failed. " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
