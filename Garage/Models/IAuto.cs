using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Garage.Models
{
    /// <summary>
    /// Fake SQL-Tabelle auto
    /// </summary>
    public interface IAuto
    {
        /// <summary>
        /// Verbindet zur DB
        /// </summary>
        public SqlConnection Connect();

        /// <summary>
        /// Schliesst DB
        /// </summary>
        public void Close();

        /// <summary>
        /// Liefert die Liste (PK_Account) der Autos
        /// </summary>
        /// <param name="sId"></param>
        /// <returns></returns>
        public List<Auto> GetAutos(string sId = "", int nPageStart = 0);

        /// <summary>
        /// Löscht ein Auto
        /// </summary>
        /// <param name="sId"></param>
        public void Delete(string sId);

        /// <summary>
        /// Erzeugt ein Auto
        /// </summary>
        /// <param name="auto"></param>
        public void Create(Auto auto);

        /// <summary>
        /// Bearbeitet ein Auto
        /// </summary>
        /// <param name="auto"></param>
        public void Update(Auto auto);
    }
}