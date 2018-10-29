using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelimitarLinha : MonoBehaviour
{
    //limite superior da tela
    public float fimTelaSup;
    //limite esquerdo da tela
    public float fimTelaEsq;
    //limite direito da tela
    public float fimTelaDir;
    //limite inferior da tela
    public float fimTelaInf;
    //objeto shot
    public Rigidbody2D objeto;

    private void Update()
    {
        //se o objeto for para fora da area da camera, ele desativa e fica pronto para ser reusado
        if (transform.position.y > fimTelaSup)
        {
            gameObject.SetActive(false);
        }
        if (transform.position.y < fimTelaInf)
        {
            gameObject.SetActive(false);
        }
        if (transform.position.x > fimTelaEsq)
        {
            gameObject.SetActive(false);
        }
        if (transform.position.x < fimTelaDir)
        {
            gameObject.SetActive(false);
        }

    }
}
