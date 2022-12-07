using Garage.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Garage.Test
{
    public class AutoFake : IAuto
    {
        // Liste von Autos
        private List<Auto> autos = new List<Auto>();
        // Standard Konstruktor
        public AutoFake()
        {
            Auto auto = new Auto
            {
                Marke = "Opel",
                Modell = "Meriva",
                Farbe = "Blau",
                Leistung = 100,
                Preis = 12000,
                Jahrgang = new DateTime(2000, 1, 1),
                Treibstoff = "Benzin"
            };
            autos.Add(auto);
        }
        public void Close()
        {
        }
        public void Connect()
        {
        }
        // Liefert eine Liste von Autos
        public List<Auto> GetAutos(string sId)
        {
            if (sId == "")
            {
                // Alle Autos
                return autos;
            }
            else
            {
                int.TryParse(sId, out int nID);
                // Lambda Syntax
                var auto = autos.First(item => item.PK_Auto == nID);
                if (auto == null)
                {
                    // kein Auto gefunden, leer List<Auto> liefern
                    return new List<Auto>();
                }
                else
                {
                    // 1 Auto gefunden. Zurückliefern.
                    return new List<Auto> { auto };
                }
            }
        }
        // noch nicht programmiert
        public void Delete(string sId)
        {
            throw new NotImplementedException();
        }
        // fügt 1 Auto in die Liste ein
        public void Create(Auto auto)
        {
            autos.Add(auto);
        }
        // noch nicht programmiert
        public void Update(Auto auto)
        {
            throw new NotImplementedException();
        }

        SqlConnection IAuto.Connect()
        {
            throw new NotImplementedException();
        }

        public List<Auto> GetAutos(string sId = "", int nPageStart = 0)
        {
            throw new NotImplementedException();
        }
    }
}
