namespace Garage.Models
{
    /// <summary>
    /// Typ des Treibstoffs: Benzin; Diesel, etc.
    /// </summary>
    /// <remarks>
    /// Wird benötigt um im Formular eine DropDown-Liste anzuzeigen
    /// TreibstoffTyp ist in unserem fall ein string, weil in der SQL-Tabelle
    /// der Datentyp auch ein VARCHAR ist.
    /// Besser wäre wohl inder DB ein INT zu verwenden, leider wären das wieder
    /// Magic-Number , bsp. 0-Benzin, 1- Diesel, etc.
    /// In der Praxis werden jeweils Magic-Number in der Tabelle verwendet...
    /// Schwierig wird es auch, wenn der Datentyp ein float ist, weil einige Webbrowser
    /// den Wert als 1.2 andere als 1,2 darstellen. Das gibt dann Probleme beim
    /// Validieren bzw. beim Speichern/Updaten.
    /// </remarks>
    public class TreibstoffTyp
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}