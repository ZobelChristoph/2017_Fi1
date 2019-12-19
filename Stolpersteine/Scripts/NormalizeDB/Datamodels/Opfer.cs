namespace NormalizeDB.Datamodels
{
  public class Opfer
  {
    public int ID { get; set; }

    public string Strasse { get; set; }

    public string Hausnummer { get; set; }

    public string Geburtsort { get; set; }

    public override string ToString() => $"{nameof(this.ID)}: {this.ID.ToString()} {TAB} {nameof(this.Strasse)}: {this.Strasse} {TAB}{TAB}{TAB} {nameof(this.Hausnummer)}: {this.Hausnummer} {TAB} {nameof(this.Geburtsort)}: {this.Geburtsort}";

    private const string TAB = "\t";
  }
}