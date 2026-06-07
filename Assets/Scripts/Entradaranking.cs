using SQLite4Unity3d;

/// <summary>
/// Linha da tabela "EntradaRanking" no banco SQLite.
/// Contém nome do jogador, tempo de cada fase e tempo total.
/// </summary>
public class EntradaRanking
{
    [PrimaryKey, AutoIncrement]
    public int    Id    { get; set; }

    public string Nome  { get; set; }   // 1–3 letras maiúsculas
    public float  Fase1 { get; set; }   // tempo em segundos
    public float  Fase2 { get; set; }   // tempo em segundos
    public float  Total { get; set; }   // Fase1 + Fase2
    public string Data  { get; set; }   // "yyyy-MM-dd HH:mm:ss"
}