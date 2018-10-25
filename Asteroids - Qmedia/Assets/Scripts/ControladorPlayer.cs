using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorPlayer : MonoBehaviour
{
    //cria o shot
    public GameObject shot;
    //rigid body do player
    public Rigidbody2D body;
    //propulsao lateral do player
    public float propulsaolateral;
    //propulsao frontal do player
    public float propulsao;
    //movimentacao do player
    private float InputPropulsao;
    //movimentacao do player
    private float InputLateral;
    //limite superior da tela
    public float fimTelaSup;
    //limite esquerdo da tela
    public float fimTelaEsq;
    //limite direito da tela
    public float fimTelaDir;
    //limite inferior da tela
    public float fimTelaInf;
    //velocidade do shot
    public float shotForce;
    //ativar e destivar shot
    public GameObject shotEnable;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //criar a movimentacao do player
        InputPropulsao = Input.GetAxis("Vertical");
        InputLateral = Input.GetAxis("Horizontal");

        //adicionar botao para atirar
        if (Input.GetButtonDown("Fire1"))
        {
            //instanciando o shot na posicao do player e dando direcao
            GameObject novoShot = Instantiate(shot, transform.position, transform.rotation);
            novoShot.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * shotForce);
            //shot desaparece e reutiliza
            
        }

        //fazer a rotacao da nave
        transform.Rotate(Vector3.forward * InputLateral * Time.deltaTime * propulsaolateral);

        //criando nova posicao caso o player passe o limite
        Vector2 novaPos = transform.position;
        //se o player sumir do limite superior
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
    private void FixedUpdate()
    {
        //arrasto do player para direcao que estava
        body.AddRelativeForce(Vector2.up * InputPropulsao);
        // giro do player
        body.AddTorque(-InputLateral);
    }
}
