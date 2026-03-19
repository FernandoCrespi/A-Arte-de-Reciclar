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

    private float moveX;
    private bool direita = true;
    private bool noChao;
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveJogador();
    }

    private void LateUpdate()
    {
        viraJogador();
    }

    void moveJogador()
    {
        moveX = 0;

        if (Input.GetKey(teclaDireita))
            moveX = 1;

        if (Input.GetKey(teclaEsquerda))
            moveX = -1;

        noChao = Physics2D.Linecast(transform.position, terra.position, chao);

        if (Input.GetKeyDown(teclaPulo) && noChao)
        {
            pula();
        }

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

    void OnCollisionEnter2D(Collision2D outro)
    {
        if (outro.gameObject.CompareTag("PlataformaMovel"))
        {
            transform.parent = outro.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D outro)
    {
        if (outro.gameObject.CompareTag("PlataformaMovel"))
        {
            transform.parent = null;
        }
    }
}
