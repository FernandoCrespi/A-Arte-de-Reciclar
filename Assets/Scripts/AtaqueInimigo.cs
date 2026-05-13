using UnityEngine;

public class AtaqueInimigo : MonoBehaviour
{
    public float cooldown = 2.5f;
    private float timer = 0f;
    private IAInimigoRonda iaRonda;

    void Start()
    {
        iaRonda = GetComponentInParent<IAInimigoRonda>();
        if (iaRonda == null)
            iaRonda = GetComponent<IAInimigoRonda>();
    }

    void Update()
    {
        if (timer > 0f)
            timer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (iaRonda != null)
            iaRonda.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (iaRonda != null)
            iaRonda.enabled = true;
    }
}