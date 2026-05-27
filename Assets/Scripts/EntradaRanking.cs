using SQLite4Unity3d;

[Table("EntradaRanking")]
public class EntradaRanking
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(3), NotNull]
    public string Nome { get; set; }

    public float Fase1 { get; set; }
    public float Fase2 { get; set; }
    public float Fase3 { get; set; }
    public float Total { get; set; }
    public string Data { get; set; }
}
