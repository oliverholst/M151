using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Garage.Models
{
    /// <summary>
    /// CRUD auf der Tabelle: Feedback
    /// </summary>
    public class MSSqlFeedback : MSSqlBase
    {
        /// <summary>
        /// Liefert alle Feedbacks
        /// </summary>
        /// <param name="sID"></param>
        /// <returns></returns>
        public List<Feedback> Select(string sID = "")
        {
            // Eine leere Liste vom Typ Feedback
            List<Feedback> feedbacks = new List<Feedback>();

            // Ein SQL-String
            string sql = sID == "" ? "SELECT * FROM Feedback " : "SELECT * FROM Feedback WHERE PK_Feedback = " + sID;
            try
            {
                // Command Klasse erzeugen.
                using (SqlCommand cmd = new SqlCommand(sql, dbHandle))
                {
                    // Reader für Abfrage erzeugen
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Ein leeres Feedbackerzeugen
                            Feedback feedback = new Feedback();
                            // Das leere Feedback abfüllen
                            feedback.PK_Feedback = reader.GetInt32(0);
                            feedback.Name = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            feedback.Email = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            feedback.Mitteilung = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            feedback.Service = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);
                            feedback.Modellauswahl = reader.IsDBNull(5) ? -1 : reader.GetInt32(5);
                            feedback.FahrzeugArt = reader.IsDBNull(6) ? "" : reader.GetString(6);

                            // das befüllte Feedback der List<Feedback> hinzufügen
                            feedbacks.Add(feedback);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetFeedback failed: " + ex.Message);
                throw new Exception("GetFeedback failed: " + ex.Message, ex);
            }
            // Die Liste mit den feedbacks dem Aufrufer zurückliefern
            return feedbacks;
        }

        /// <summary>
        /// Fügt 1 Feedback in die Tabelle feedback
        /// </summary>
        /// <param name="feedback"></param>
        public void Insert(Feedback feedback)
        {
            string sql = "INSERT INTO Feedback(Name,Email,Mitteilung,Service,Modellauswahl,Fahrzeugart)"
    + "VALUES(@Name,@Email,@Mitteilung,@Service,@Modellauswahl,@FahrzeugArt) ";
            try
            {
                //Command Klasse erzeugen
                using (SqlCommand cmd = new SqlCommand(sql, dbHandle))
                {
                    cmd.Parameters.AddWithValue("@Name", feedback.Name);
                    cmd.Parameters.AddWithValue("@Email", feedback.Email);
                    cmd.Parameters.AddWithValue("@Mitteilung", feedback.Mitteilung);
                    cmd.Parameters.AddWithValue("@Service", feedback.Service);
                    cmd.Parameters.AddWithValue("@Modellauswahl", feedback.Modellauswahl);
                    cmd.Parameters.AddWithValue("@FahrzeugArt", feedback.FahrzeugArt);

                    //Daten abfüllen
                    cmd.Parameters["@Name"].Value = feedback.Name;
                    cmd.Parameters["@Email"].Value = feedback.Email;
                    cmd.Parameters["@Mitteilung"].Value = feedback.Mitteilung;
                    cmd.Parameters["@Service"].Value = feedback.Service;
                    cmd.Parameters["@Modellauswahl"].Value = feedback.Modellauswahl;
                    cmd.Parameters["@FahrzeugArt"].Value = feedback.FahrzeugArt;

                    //ExecuteNonQuery -used to insert and delete data
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