using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoAsteroide : MonoBehaviour {

    public float fimTelaSup;
    //limite esquerdo da tela
    public float fimTelaEsq;
    //limite direito da tela
    public float fimTelaDir;
    //limite inferior da tela
    public float fimTelaInf;
    //propulsao do asteroide
    public float impulsoAst;
    //propulsao de rotacao do asteroide
    public float torqueAst;
    //rigid body do asteroid
    public Rigidbody2D bodyAst;
	// Use this for initialization
	void Start () {
        //criando rotacao e movimento do asteroide
        Vector2 impulso = new Vector2(Random.Range(-impulsoAst, impulsoAst), Random.Range(-impulsoAst, impulsoAst));
        float torque = Random.Range(-torqueAst, torqueAst);
        //adicioando forca e rotacao
        bodyAst.AddForce(impulso);
        bodyAst.AddTorque(torque);
	}
	
	// Update is called once per frame
	void Update () {
        //manter o asteroide dentro do circulo
        Vector2 novaPos = transform.position;

        if (transform.position.y > fimTelaSup)
        {
            novaPos.y = fimTelaInf;
        }
        if (transform.position.y < fimTelaInf)
        {
            novaPos.y = fimTelaSup;
        }
        if (transform.position.x > fimTelaEsq)
        {
            novaPos.x = fimTelaDir;
        }
        if (transform.position.x < fimTelaDir)
        {
            novaPos.x = fimTelaEsq;
        }
        transform.position = novaPos;
    }
    private void OnTriggerEnter2D(Collider2D shot)
    {
        
    }
}
