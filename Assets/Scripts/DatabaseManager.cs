using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SQLite4Unity3d;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    private SQLiteConnection db;

    private float _fase1 = 0f;
    private float _fase2 = 0f;
    private float _fase3 = 0f;
    private string _nome = "";

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InicializarBancoDeDados();
    }

    private void InicializarBancoDeDados()
    {
        string caminho = Path.Combine(Application.persistentDataPath, "ranking.db");
        Debug.Log("[DB] Caminho: " + caminho);
        db = new SQLiteConnection(caminho,
            SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
        db.CreateTable<EntradaRanking>();
        Debug.Log("[DB] Tabela pronta.");
    }

    public void DefinirNome(string nome)
    {
        _nome = nome.ToUpper().Trim();
        Debug.Log("[DB] Nome definido: " + _nome);
    }

    public string ObterNome() => _nome;
    public float GetFase1() => _fase1;
    public float GetFase2() => _fase2;
    public float GetFase3() => _fase3;

    public void SalvarTempoFase(int fase, float tempo)
    {
        switch (fase)
        {
            case 1: _fase1 = tempo; break;
            case 2: _fase2 = tempo; break;
            case 3: _fase3 = tempo; break;
            default: Debug.LogWarning("[DB] Fase inválida: " + fase); return;
        }
        Debug.Log("[DB] Fase " + fase + " guardada: " + tempo.ToString("F2") + "s");
    }

    public bool SalvarRegistroFinal()
    {
        return SalvarRegistroFinal(_nome);
    }

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
            entrada.Nome  = nome;
            entrada.Fase1 = _fase1;
            entrada.Fase2 = _fase2;
            entrada.Fase3 = _fase3;
            entrada.Total = _fase1 + _fase2 + _fase3;
            entrada.Data  = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            db.Insert(entrada);
            Debug.Log("[DB] Registro salvo → " + nome +
                      " | F1:" + _fase1.ToString("F2") +
                      " F2:" + _fase2.ToString("F2") +
                      " F3:" + _fase3.ToString("F2") +
                      " Total:" + entrada.Total.ToString("F2"));
            _fase1 = _fase2 = _fase3 = 0f;
            _nome = "";
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("[DB] Erro ao salvar: " + e.Message);
            return false;
        }
    }

    public bool SalvarTempo(string nome, float fase1, float fase2, float fase3)
    {
        SalvarTempoFase(1, fase1);
        SalvarTempoFase(2, fase2);
        SalvarTempoFase(3, fase3);
        return SalvarRegistroFinal(nome);
    }

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

    public void LimparRanking()
    {
        try { db.DeleteAll<EntradaRanking>(); Debug.Log("[DB] Ranking limpo."); }
        catch (Exception e) { Debug.LogError("[DB] Erro ao limpar: " + e.Message); }
    }

    void OnDestroy() { db?.Close(); }
}