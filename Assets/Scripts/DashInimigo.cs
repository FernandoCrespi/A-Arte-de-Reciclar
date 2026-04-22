using System.Collections;
using UnityEngine;

public class DashInimigo : MonoBehaviour
{
    public float velocidadeDash = 20f;
    public float tempoDeDash = 0.3f;
    public float cooldown = 2f;

    [HideInInspector] public bool estaDashando;
    [HideInInspector] public bool podeDash = true;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void IniciaDash(Vector2 direcao)
    {
        if (!podeDash || estaDashando) return;

        // força só horizontal
        float dir = Mathf.Sign(direcao.x);
        StartCoroutine(Dash(dir));
    }

    IEnumerator Dash(float dir)
    {
        estaDashando = true;
        podeDash = false;

        float tempo = 0;

        while (tempo < tempoDeDash)
        {
            rb.velocity = new Vector2(dir * velocidadeDash, 0);
            tempo += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero;
        estaDashando = false;

        yield return new WaitForSeconds(cooldown);
        podeDash = true;
    }
}