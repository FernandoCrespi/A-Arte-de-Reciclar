using UnityEngine;

public class TriggerFrontalDash : MonoBehaviour
{
    private DashInimigo dashInimigo;

    void Start()
    {
        dashInimigo = GetComponentInParent<DashInimigo>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && dashInimigo != null && dashInimigo.podeDash)
        {
            Debug.Log("🎯 TRIGGER DETECTADO!");
            Vector2 direcao = (other.transform.position - transform.parent.position).normalized;
            dashInimigo.IniciaDash(direcao);
        }
    }
}