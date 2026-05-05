using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAInimigoRonda : MonoBehaviour
{
    public GameObject inimigo;
    public GameObject[] pontos;
    public float velocidade = 5f;
    public float espera = 0f;
    public bool loop = true;
    public bool atacando = false;

    private new Transform transform;
    private int i = 0;
    private float proxTempo;
    private bool seMovendo;
    private Animator animator;
    private Saude saude;
    private DashInimigo dash;

    void Start()
    {
        transform = inimigo.transform;
        proxTempo = 0f;
        seMovendo = true;
        animator = GetComponent<Animator>();
        saude = gameObject.GetComponent<Saude>();
        dash = GetComponent<DashInimigo>();
    }

    void Update()
    {
        if (saude == null) return;

        bool dashando = dash != null && dash.estaDashando;

        if (!saude.morto && !dashando)
        {
            if (Time.time >= proxTempo)
            {
                if (!seMovendo)
                {
                    Vector2 escala = transform.localScale;
                    escala.x = escala.x * -1;
                    transform.localScale = escala;
                    seMovendo = true;
                }
            }

            if (!atacando)
            {
                movimenta();
            }
        }
    }

    void movimenta()
    {
        if ((pontos.Length != 0) && (seMovendo))
        {
            transform.position = Vector3.MoveTowards(transform.position, pontos[i].transform.position, velocidade * Time.deltaTime);
            animator.SetBool("Correndo", true);

            if (Vector3.Distance(pontos[i].transform.position, transform.position) <= 0.1f)
            {
                i++;
                proxTempo = Time.time + espera;
                seMovendo = false;
                animator.SetBool("Correndo", false);
            }

            if (i >= pontos.Length)
            {
                if (loop)
                    i = 0;
                else
                    seMovendo = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D outro)
    {
        if (outro.gameObject.tag == "Player")
        {
            ataca();
        }
    }

    public void ataca()
    {
        if (!atacando)
        {
            animator.SetTrigger("Ataque");
        }
    }
}