using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controle : MonoBehaviour
{
    public int velocidade = 10;
    public int forcaDoPulo = 1250;
    public Transform terra;
    public LayerMask chao;
    public KeyCode teclaDireita = KeyCode.D;
    public KeyCode teclaEsquerda = KeyCode.A;
    public KeyCode teclaPulo = KeyCode.Space;
    public KeyCode teclaAtaque = KeyCode.J;

    [HideInInspector] public bool btnDireita = false;
    [HideInInspector] public bool btnEsquerda = false;
    [HideInInspector] public bool btnPulo = false;
    [HideInInspector] public bool btnAtaque = false;

    private float moveX;
    private bool direita = true;
    private bool noChao;
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Carrega teclas salvas
        teclaDireita = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("teclaDireita", teclaDireita.ToString()));
        teclaEsquerda = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("teclaEsquerda", teclaEsquerda.ToString()));
        teclaPulo = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("teclaPulo", teclaPulo.ToString()));
        teclaAtaque = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("teclaAtaque", teclaAtaque.ToString()));
    }

    void Update()
    {
        moveJogador();
        Atacar();
    }

    private void LateUpdate()
    {
        viraJogador();
    }

    void moveJogador()
    {
        moveX = 0;
        if (Input.GetKey(teclaDireita) || btnDireita)
            moveX = 1;
        if (Input.GetKey(teclaEsquerda) || btnEsquerda)
            moveX = -1;

        noChao = Physics2D.Linecast(transform.position, terra.position, chao);

        if ((Input.GetKeyDown(teclaPulo) || btnPulo) && noChao)
            pula();

        btnPulo = false;
        rb.velocity = new Vector2(moveX * velocidade, rb.velocity.y);
        Physics2D.IgnoreLayerCollision(
            this.gameObject.layer,
            LayerMask.NameToLayer("chao"),
            (rb.velocity.y > 0.0f)
        );
        animator.SetBool("noChao", noChao);
        animator.SetBool("Correndo", moveX != 0);
    }

    void pula()
    {
        rb.AddForce(Vector2.up * forcaDoPulo);
    }

    void viraJogador()
    {
        if (moveX > 0)
            direita = true;
        else if (moveX < 0)
            direita = false;

        Vector2 escala = transform.localScale;
        if ((escala.x > 0 && !direita) || (escala.x < 0 && direita))
        {
            escala.x *= -1;
            transform.localScale = escala;
        }
    }

    void Atacar()
    {
        if (Input.GetKeyDown(teclaAtaque) || btnAtaque)
            animator.SetTrigger("Ataque");
        btnAtaque = false;
    }
}