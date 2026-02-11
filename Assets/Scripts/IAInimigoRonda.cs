using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAInimigoRonda : MonoBehaviour {

	public GameObject inimigo; // referência ao inimigo a controlar

	public GameObject[] pontos; // vetor dos pontos de parada do inimigo

	//public Collider2D triggerAtaque;
	//public Collider2D espada;
	//public float inicioEspada = 1f;  // Tempo que leva entre pressionar o botão e realizar o ataque
	//public float duracaoEspada = 0.2f; // Duração do ataqur
	//private float tempoInicioEspada = 0f;    // Tempo de inicio do ataque atual
	// hora de "ligar" o collider da espada
	//private float tempoFimEspada = 0f;       // Tempo de final do ataque atual
	// hora de "desligar" o collider da espada

	public float velocidade = 5f; // velocidade do inimigo
	public float espera = 2f; // tempo de espera no ponto de parada

	public bool loop = true; // volta ao início após último ponto?
    public bool atacando = false;

	private Transform transform;
	int i = 0;		// indice do vetor pontos
	float proxTempo;  // tempo do próximo movimento
	bool seMovendo = true;
	
	Animator animator;
	Saude saude;

	// Inicialização
	void Start () {
		transform = inimigo.transform;
		proxTempo = 0f;
		seMovendo = true;
		animator = inimigo.GetComponent<Animator> ();
		//espada.enabled = false;
		saude = gameObject.GetComponent<Saude> ();
	}

	void Update () {
        if (!saude.morto)
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
            if (!atacando){
                movimenta();
            }
		}
	}

	void movimenta() {
		// Se há pontos para onde ir e estamos nos movendo
		if ((pontos.Length != 0) && (seMovendo)) {

			// move até o próximo ponto
			transform.position = Vector3.MoveTowards(transform.position, pontos[i].transform.position, velocidade * Time.deltaTime);

			//animação
			animator.SetBool("Andando",true);

			// Se o inimigo chegou no desrino, espera
            if(Vector3.Distance(pontos[i].transform.position, transform.position) <= 0.1) {
				i++;
				proxTempo = Time.time + espera;
				seMovendo = false;
				animator.SetBool("Andando",false);
			}

			// i volta para 0 se passou do limite ou paramos de nos mover se não há loop
			if(i >= pontos.Length) {
				if (loop)
					i = 0;
				else
					seMovendo = false;
			}
		}
	}

    void OnTriggerStay2D(Collider2D outro)
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
