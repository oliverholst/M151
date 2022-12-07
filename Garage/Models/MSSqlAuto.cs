using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Garage.Models
{
    /// <summary>
    /// CRUD auf der Tabelle: Auto
    /// </summary>
    public class MSSqlAuto : MSSqlBase, IAuto
    {
        /// <summary>
        /// Liefert eine Liste von Autos
        /// - Bleibt sId leer werden alle Autos zurückgeliefert
        /// - Wenn eine gültige sId übergeben wird befindet sich 1 Auto
        ///   in der Liste
        /// - Wenn eine ungültige sId übergeben wird, wird eine leere 
        ///   Liste zurückgeliefert
        /// - Wenn nStart eine positive Zahl ist, dann werden max. 10 Autos geliefert    
        /// </summary>
        /// <param name="sId">Primary Key</param>
        /// <param name="nStart">Startindedx für das Blättern</param>
        /// <returns>Alle Autos, ein Auto kein Auto</returns>
        public List<Auto> GetAutos(string sId = "",int nStart = 0)
        {
            // Eine leere Liste vom Typ Auto
            List<Auto> autos = new List<Auto>();
            // Ein SQL-String
            string sql = "";

            if(nStart == 0)
            {
                //ohne Blättern (Pagging)
                sql = sId == "" ? "SELECT * FROM Auto " : "SELECT * FROM Auto WHERE PK_Auto = " + sId;
            }
            else
            {
                //mit Blättern (Paging)
                sql = "SELECT * FROM Auto LIMIT " + nStart.ToString() + ", 10";
            }

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
                            // Ein noch leeres Auto erzeugen
                            Auto auto = new Auto();
                            // das leere Auto abfüllen
                            auto.PK_Auto = reader.GetInt32(0);
                            auto.Marke = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            auto.Modell = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            auto.Farbe = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            auto.Leistung = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);
                            auto.Preis = reader.IsDBNull(5) ? -1 : reader.GetInt32(5);
                            // Jahrgang zuerst als Standarddatum setzen
                            auto.Jahrgang = Helper.StandardDatum;

                            try
                            {
                                // Test ob das Datum null ist
                                if ( reader.IsDBNull(6))
                                {
                                    Debug.WriteLine("Auto-Jahrgang ist null: PK = " + auto.PK_Auto);
                                }
                                else
                                {
                                    // Datum könnte ungültig sein, z.B 0000-00-00
                                    DateTime jahrgang;
                                    if (DateTime.TryParse(reader[6].ToString(), out jahrgang))
                                    {
                                        // the good case
                                        auto.Jahrgang = jahrgang;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //the bad case
                                Debug.WriteLine("Datum ungueltig: PK: " + auto.PK_Auto + ", " + ex.Message);
                            }
                            auto.Treibstoff = reader.IsDBNull(7) ? "" : reader.GetString(7);
                            if (!reader.IsDBNull(8))
                            { 
                                const int CHUNK_SIZE = 2 * 1024;
                                byte[] buffer = new byte[CHUNK_SIZE];
                                long bytesRead; 
                                long fieldOffset = 0;
                                using (var stream = new MemoryStream()) 
                                {
                                    while ((bytesRead = reader.GetBytes(reader.GetOrdinal("pic"),
                                        fieldOffset, buffer, 0, buffer.Length)) == buffer.Length)
                                    { 
                                        stream.Write(buffer, 0, (int)bytesRead);
                                        fieldOffset += bytesRead; 
                                    } auto.Pic = stream.ToArray(); 
                                } 
                            }


                            // das befüllte Auto der List<Auto> hinzufügen
                            autos.Add(auto);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine("GetAutos failed: " + exc.Message);
                throw new Exception("GetAutos failed: " + exc.Message, exc);
            }

            // Die Liste mit den Autos dem Aufrufer zurückliefern
            return autos;
        }

        /// <summary>
        /// Löscht 1 Auto aus der Datenbank Tabelle auto
        /// </summary>
        /// <param name="sId">Primary Key</param>
        public bool Delete(string sId)
        {
            string sql = "DELETE FROM Auto WHERE PK_Auto = " + sId;

            try
            {
                // CommandKlasse erzeugen.
                using (SqlCommand cmd = new SqlCommand(sql, dbHandle))
                {
                    // Reader für Abfrage erzeugen
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int zahl = reader.RecordsAffected;
                        if (zahl > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DELETE failed. " + ex.Message);
                throw new Exception(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Erzeugt 1 Auto in der Tabelle auto
        /// </summary>
        /// <param name="auto"></param>
        public void Create(Auto auto)
        {
            string sql = "";
            if (auto.Pic == null)
            {
                 sql = "INSERT INTO Auto(Marke,Modell,Farbe,Leistung,Preis,Jahrgang,Treibstoff)"
                    + "VALUES(@Marke,@Modell,@Farbe,@Leistung,@Preis,@Jahrgang,@Treibstoff) ";
            }
            else 
            {
                sql = "INSERT INTO Auto(Marke,Modell,Farbe,Leistung,Preis,Jahrgang,Treibstoff,Pic)"
                     + "VALUES(@Marke,@Modell,@Farbe,@Leistung,@Preis,@Jahrgang,@Treibstoff,@Pic) ";
            }
            try
            {
                //Command Klasse erzeugen
                using (SqlCommand cmd = new SqlCommand(sql, dbHandle))
                {
                    cmd.Parameters.AddWithValue("@Marke", auto.Marke);
                    cmd.Parameters.AddWithValue("@Modell", auto.Modell);
                    cmd.Parameters.AddWithValue("@Farbe", auto.Farbe);
                    cmd.Parameters.AddWithValue("@Leistung", auto.Leistung);
                    cmd.Parameters.AddWithValue("@Preis", auto.Preis);
                    cmd.Parameters.AddWithValue("@Jahrgang", GetValidJahrgang(auto));
                    cmd.Parameters.AddWithValue("@Treibstoff", auto.Treibstoff);

                    if( auto.Pic != null)
                    {
                        cmd.Parameters.AddWithValue("@Pic", auto.Pic);
                    }
                    //Daten abfüllen
                     cmd.Parameters["@Marke"].Value = auto.Marke;
                     cmd.Parameters["@Modell"].Value = auto.Modell;
                     cmd.Parameters["@Farbe"].Value = auto.Farbe;
                     cmd.Parameters["@Leistung"].Value = auto.Leistung;
                     cmd.Parameters["@Preis"].Value = auto.Preis;
                     cmd.Parameters["@Jahrgang"].Value = GetValidJahrgang(auto);
                     cmd.Parameters["@Treibstoff"].Value = auto.Treibstoff;

                     if (auto.Pic != null)
                     {
                         cmd.Parameters["@Pic"].Value = auto.Pic;
                     }
                    
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

        /// <summary>
        /// Konvertiert das Datum (Jahrgang) von CH nach USA
        /// </summary>
        /// <param name="auto"></param>
        /// <returns>Datumsstring im USA-Format</returns>
        private string GetValidJahrgang(Auto auto)
        {
            string sValidDatum = Helper.StandardDatumUSA;
            if (auto.Jahrgang != null)
            {
                sValidDatum = auto.Jahrgang.Year + "-" + auto.Jahrgang.Month + "-" + auto.Jahrgang.Day;
            }
            return sValidDatum;
        }

        /// <summary>
        /// Ändert ein bestehendes Auto in der Tabelle
        /// </summary>
        /// <param name="auto"></param>
        public void Update(Auto auto)
        {
            string sql = "";

            if (auto.Pic == null)
            {
                //ohne Bild
                 sql = "UPDATE Auto SET Marke = @Marke, Modell = @Modell," +
                    "Farbe = @Farbe, Leistung = @Leistung, Preis = @Preis, " +
                    "Jahrgang = @Jahrgang, Treibstoff= @Treibstoff " +
                    "WHERE PK_Auto = " + auto.PK_Auto + " ; ";
            }
            else
            {
                //mit Bild
                sql = "UPDATE Auto SET Marke = @Marke, Modell = @Modell," +
                   "Farbe = @Farbe, Leistung = @Leistung, Preis = @Preis, " +
                   "Jahrgang = @Jahrgang, Treibstoff= @Treibstoff, Pic = @Pic " +
                   "WHERE PK_Auto = " + auto.PK_Auto + " ; ";
            }

            try
            {
                //Command Klasse erzeugen
                using (SqlCommand cmd = new SqlCommand(sql, dbHandle))
                {
                    cmd.Parameters.AddWithValue("@Marke", auto.Marke);
                    cmd.Parameters.AddWithValue("@Modell", auto.Modell);
                    cmd.Parameters.AddWithValue("@Farbe", auto.Farbe);
                    cmd.Parameters.AddWithValue("@Leistung", auto.Leistung);
                    cmd.Parameters.AddWithValue("@Preis", auto.Preis);
                    cmd.Parameters.AddWithValue("@Jahrgang", GetValidJahrgang(auto));
                    cmd.Parameters.AddWithValue("@Treibstoff", auto.Treibstoff);

                    if (auto.Pic != null)
                    {
                        cmd.Parameters.AddWithValue("@Pic",auto.Pic);
                    }
                     
                    //Daten abfüllen
                    cmd.Parameters["@Marke"].Value = auto.Marke;
                    cmd.Parameters["@Modell"].Value = auto.Modell;
                    cmd.Parameters["@Farbe"].Value = auto.Farbe;
                    cmd.Parameters["@Leistung"].Value = auto.Leistung;
                    cmd.Parameters["@Preis"].Value = auto.Preis;
                    cmd.Parameters["@Jahrgang"].Value = GetValidJahrgang(auto);
                    cmd.Parameters["@Treibstoff"].Value = auto.Treibstoff;

                    if (auto.Pic != null)
                    {
                        cmd.Parameters["@Pic"].Value = auto.Pic;
                    }

                    //ExecuteNonQuery -used to insert and delete data
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("UPDATE failed. " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        void IAuto.Delete(string sId)
        {
            throw new NotImplementedException();
        }
    }
}