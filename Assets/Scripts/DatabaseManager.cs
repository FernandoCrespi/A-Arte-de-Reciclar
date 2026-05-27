using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SQLite4Unity3d;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    private SQLiteConnection db;

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

    private void InicializarBancoDeDados()
    {
        string caminho = Path.Combine(Application.persistentDataPath, "ranking.db");
        Debug.Log("[DB] Caminho: " + caminho);
        db = new SQLiteConnection(caminho,
            SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
        db.CreateTable<EntradaRanking>();
        Debug.Log("[DB] Tabela pronta.");
    }

    public bool SalvarTempo(string nome, float fase1, float fase2, float fase3)
    {
        nome = nome.ToUpper().Trim();
        if (nome.Length == 0 || nome.Length > 3)
        {
            Debug.LogWarning("[DB] Nome invalido: use 1 a 3 letras.");
            return false;
        }
        try
        {
            EntradaRanking entrada = new EntradaRanking();
            entrada.Nome = nome;
            entrada.Fase1 = fase1;
            entrada.Fase2 = fase2;
            entrada.Fase3 = fase3;
            entrada.Total = fase1 + fase2 + fase3;
            entrada.Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            db.Insert(entrada);
            Debug.Log("[DB] Salvo: " + nome);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("[DB] Erro ao salvar: " + e.Message);
            return false;
        }
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

    public EntradaRanking ObterMelhorTempo(string nome)
    {
        nome = nome.ToUpper().Trim();
        try
        {
            List<EntradaRanking> lista = db.Query<EntradaRanking>(
                "SELECT * FROM EntradaRanking WHERE Nome = ? ORDER BY Total ASC LIMIT 1", nome);
            if (lista.Count > 0)
                return lista[0];
            return null;
        }
        catch (Exception e)
        {
            Debug.LogError("[DB] Erro ao buscar melhor tempo: " + e.Message);
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
        catch (Exception e)
        {
            Debug.LogError("[DB] Erro ao limpar: " + e.Message);
        }
    }

    void OnDestroy()
    {
        if (db != null)
            db.Close();
    }
}