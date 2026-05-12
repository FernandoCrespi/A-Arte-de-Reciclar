using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueInimigo : MonoBehaviour
{
    public GameObject Ataque;
    public GameObject Inimigo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision != null && collision.tag == "Player") {
            Debug.Log("Entrou");
            Instantiate(Ataque, Inimigo.transform.position, Inimigo.transform.rotation);
        }
    }
}
