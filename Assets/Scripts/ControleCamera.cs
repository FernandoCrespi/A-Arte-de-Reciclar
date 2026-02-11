using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleCamera : MonoBehaviour {

	private GameObject jogador;
	public float xMax;
	public float xMin;
	public float yMax;
	public float yMin;


	// Use this for initialization
	void Start () {
		jogador = GameObject.FindGameObjectWithTag ("Player");		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		float x = Mathf.Clamp (jogador.transform.position.x, xMin, xMax);
		float y = Mathf.Clamp (jogador.transform.position.y, yMin, yMax);
		gameObject.transform.position = new Vector3 (x, y, gameObject.transform.position.z);


	}
}
