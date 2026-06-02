using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SQLite4Unity3d;

/// <summary>
/// Singleton que persiste entre todas as cenas (DontDestroyOnLoad).
///
/// FLUXO:
/// 1. Ao fim da Fase 1 ? DatabaseManager.Instance.SalvarTempoFase(1, tempo)
/// 2. Ao fim da Fase 2 ? DatabaseManager.Instance.SalvarTempoFase(2, tempo)
///                       depois SalvarRegistroFinal(nome)
/// </summary>
public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    private SQLiteConnection db;

    // Tempos temporários guardados entre cenas
    private float _fase1 = 0f;
    private float _fase2 = 0f;

    // ?? AWAKE ????????????????????????????????????????????????
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InicializarBancoDeDados();
    }

    // ?? INICIALIZAR ??????????????????????????????????????????
    private void InicializarBancoDeDados()
    {
        string caminho = Path.Combine(Application.persistentDataPath, "ranking.db");
        Debug.Log("[DB] Caminho: " + caminho);
        db = new SQLiteConnection(caminho,
            SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
        db.CreateTable<EntradaRanking>();
        Debug.Log("[DB] Tabela pronta.");
    }

    // ?? SALVAR TEMPO DE UMA FASE ?????????????????????????????
    /// <summary>
    /// Chame ao fim de cada fase antes de trocar de cena.
    /// Ex: DatabaseManager.Instance.SalvarTempoFase(1, timer.GetElapsedTime());
    /// Na Fase 2, chame também SalvarRegistroFinal(nome) logo depois.
    /// </summary>
    public void SalvarTempoFase(int fase, float tempo)
    {
        switch (fase)
        {
            case 1: _fase1 = tempo; break;
            case 2: _fase2 = tempo; break;
            default:
                Debug.LogWarning("[DB] Fase inválida: " + fase);
                return;
        }
        Debug.Log("[DB] Fase " + fase + " guardada: " + tempo.ToString("F2") + "s");
    }

    // ?? SALVAR REGISTRO FINAL ????????????????????????????????
    /// <summary>
    /// Chame depois de SalvarTempoFase(2, tempo), passando o nome do jogador.
    /// O nome deve ter entre 1 e 3 letras maiúsculas (estilo ranking arcade).
    /// </summary>
    public bool SalvarRegistroFinal(string nome)
    {
        nome = nome.ToUpper().Trim();
        if (nome.Length == 0 || nome.Length > 3)
        {
            Debug.LogWarning("[DB] Nome inválido: use 1 a 3 letras.");
            return false;
        }
        try
        {
            EntradaRanking entrada = new EntradaRanking();
            entrada.Nome = nome;
            entrada.Fase1 = _fase1;
            entrada.Fase2 = _fase2;
            entrada.Total = _fase1 + _fase2;
            entrada.Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            db.Insert(entrada);

            Debug.Log("[DB] Registro salvo ? " + nome +
                      " | F1:" + _fase1.ToString("F2") +
                      " F2:" + _fase2.ToString("F2") +
                      " Total:" + entrada.Total.ToString("F2"));

            // Limpa os tempos para a próxima partida
            _fase1 = _fase2 = 0f;
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("[DB] Erro ao salvar: " + e.Message);
            return false;
        }
    }

    // ?? SALVAR DIRETO (atalho conveniente) ???????????????????
    /// <summary>
    /// Registra as duas fases e salva tudo de uma vez.
    /// Útil se você tiver os dois tempos disponíveis ao mesmo tempo.
    /// </summary>
    public bool SalvarTempo(string nome, float fase1, float fase2)
    {
        SalvarTempoFase(1, fase1);
        SalvarTempoFase(2, fase2);
        return SalvarRegistroFinal(nome);
    }

    // ?? RANKING ??????????????????????????????????????????????
    public List<EntradaRanking> ObterRanking(int limite)
    {
        try
        {
            return db.Query<EntradaRanking>(
                "SELECT * FROM EntradaRanking ORDER BY Total ASC LIMIT ?", limite);
        }
        catch (Exception e)
        {
            Debug.LogError("[DB] Erro ao buscar ranking: " + e.Message);
            return new List<EntradaRanking>();
        }
    }

    public EntradaRanking ObterMelhorTempo(string nome)
    {
        nome = nome.ToUpper().Trim();
        try
        {
            List<EntradaRanking> lista = db.Query<EntradaRanking>(
                "SELECT * FROM EntradaRanking WHERE Nome = ? ORDER BY Total ASC LIMIT 1", nome);
            return lista.Count > 0 ? lista[0] : null;
        }
        catch (Exception e)
        {
            Debug.LogError("[DB] Erro: " + e.Message);
            return null;
        }
    }

    public void LimparRanking()
    {
        try
        {
            db.DeleteAll<EntradaRanking>();
            Debug.Log("[DB] Ranking limpo.");
        }
        catch (Exception e) { Debug.LogError("[DB] Erro ao limpar: " + e.Message); }
    }

    void OnDestroy() { db?.Close(); }
}