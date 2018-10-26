using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoAsteroide : MonoBehaviour
{

    //asteroide medio
    public GameObject asteroideMedio;
    //asteroide pequeno
    public GameObject asteroidePequeno;
    //ligar score ao player
    public GameObject player;
    //tamanho do asteroide
    public int asteroidetamanho;
    //pontuacao
    public int pontos;
    //limite superior da tela
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
    //gm do jogo
    public GameManager gm;


    void Start()
    {
        //criando rotacao e movimento do asteroide
        Vector2 impulso = new Vector2(Random.Range(-impulsoAst, impulsoAst), Random.Range(-impulsoAst, impulsoAst));
        float torque = Random.Range(-torqueAst, torqueAst);
        //adicioando forca e rotacao
        bodyAst.AddForce(impulso);
        bodyAst.AddTorque(torque);

        //Encontrar o player
        player = GameObject.FindWithTag("Player");

        gm = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
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
    void OnTriggerEnter2D(Collider2D other)
    {
        //checa se e um shot
        if (other.CompareTag("Bullet"))
        {
            //acao caso seja um shot
            if (asteroidetamanho == 3)
            {
                //criar os asteroides grandes
                GameObject asteroid1 = Instantiate(asteroideMedio, transform.position, transform.rotation);
                GameObject asteroid2 = Instantiate(asteroideMedio, transform.position, transform.rotation);
                gm.UpdateQtdAsteroides(1);
                //esconder o asteroide maior
            }
            if (asteroidetamanho == 2)
            {
                //criar asteroides medios
                GameObject asteroid1 = Instantiate(asteroideMedio, transform.position, transform.rotation);
                GameObject asteroid2 = Instantiate(asteroideMedio, transform.position, transform.rotation);
                gm.UpdateQtdAsteroides(1);
                //Invoke(, 1f);
                //esconder asteroide medio
                Destroy(this.gameObject);
            }
            if (asteroidetamanho == 1)
            {
                //remover os asteroides
                gm.UpdateQtdAsteroides(1);
            }
            //pontos para player
            player.SendMessage("ScorePoints", pontos);

            //remover o asteroide
        }
        
    }

}
