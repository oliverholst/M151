using System;

namespace Garage.Models
{
    /// <summary>
    /// Hilfklasse
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// Standard Datum
        /// </summary>
        public static DateTime StandardDatum = new DateTime(1900, 1, 1);

        /// <summary>
        /// Standard Datum USA AM, PM
        /// </summary>
        public static string StandardDatumUSA = "1900-01-01";
        /// <summary>
        /// Maximale Grösse eines Bildes in der Datenbank (upload)
        /// </summary>
        public const int MAX_PIC_SIZE = 1000000;
    }
}