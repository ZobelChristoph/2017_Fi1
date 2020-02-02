namespace NormalizeDB.Datamodels
{
  public class Stolperstein
  {
    public int ID { get; set; }

    public string Strasse { get; set; }

    public string Hausnummer { get; set; }

    public string Ort { get; set; }

    public override string ToString() => $"{nameof(Stolperstein)} ({nameof(this.ID)}): {this.ID.ToString()} {TAB} {nameof(this.Strasse)}: {this.Strasse} {TAB}{TAB}{TAB} {nameof(this.Hausnummer)}: {this.Hausnummer} {TAB} {nameof(this.Ort)}: {this.Ort}";

    private const string TAB = "\t";
  }
}