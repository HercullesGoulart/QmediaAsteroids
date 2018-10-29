using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroideController : MonoBehaviour
{

    #region Asteroid
    //asteroide medio
    public GameObject asteroideMedio;
    //asteroide pequeno
    public GameObject asteroidePequeno;
    //tamanho do asteroide
    public int asteroidetamanho;
    //propulsao do asteroide
    public float impulsoAst;
    //propulsao de rotacao do asteroide
    public float torqueAst;
    //rigid body do asteroid
    public Rigidbody2D bodyAst;
    #endregion
    #region Tela
    //limite superior da tela
    public float fimTelaSup;
    //limite esquerdo da tela
    public float fimTelaEsq;
    //limite direito da tela
    public float fimTelaDir;
    //limite inferior da tela
    public float fimTelaInf;
    #endregion
    #region Player
    //ligar score ao player
    public GameObject player;
    #endregion
    #region Score
    //pontuacao
    public int pontos;
    #endregion
    #region GameManager
    //gm do jogo
    public GameManager gm;
    #endregion
    void Start()
    {
        //criando rotacao e movimento do asteroide
        Vector2 impulso = new Vector2(Random.Range(-impulsoAst, impulsoAst), Random.Range(-impulsoAst, impulsoAst));
        float torque = Random.Range(-torqueAst, torqueAst);
        //adicioando impulso e torque
        bodyAst.AddForce(impulso);
        bodyAst.AddTorque(torque);

        //encontra o game manager
        gm = GameObject.FindObjectOfType<GameManager>();

        //Encontrar o player
        player = GameObject.FindWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        //manter o asteroide dentro da visao da camera
        Vector2 novaPos = transform.position;

        if (transform.position.y > fimTelaSup)
        {
            novaPos.y = fimTelaInf;
            bodyAst.velocity = Vector2.up;
            NovoImpulso();
        }
        if (transform.position.y < fimTelaInf)
        {
            novaPos.y = fimTelaSup;
            bodyAst.velocity = Vector2.down;
            NovoImpulso();
        }
        if (transform.position.x > fimTelaEsq)
        {
            novaPos.x = fimTelaDir;
            bodyAst.velocity = Vector2.left;
            NovoImpulso();
        }
        if (transform.position.x < fimTelaDir)
        {
            novaPos.x = fimTelaEsq;
            bodyAst.velocity = Vector2.right;
            NovoImpulso();

        }
        transform.position = novaPos;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //checa se recebe um tiro
        if (other.CompareTag("Bullet"))
        {
            gameObject.SetActive(false);
            if (asteroidetamanho == 3)
            {
                //criar os asteroides medios caso os grandes sejam destruidos
                GameObject asteroid1 = Instantiate(asteroideMedio, transform.position, transform.rotation);
                GameObject asteroid2 = Instantiate(asteroideMedio, transform.position, transform.rotation);
                gm.UpdateQtdAsteroides(1);
            }
            if (asteroidetamanho == 2)
            {
                //criar asteroides pequenos se os medios forem destruidos 
                GameObject asteroid1 = Instantiate(asteroidePequeno, transform.position, transform.rotation);
                GameObject asteroid2 = Instantiate(asteroidePequeno, transform.position, transform.rotation);
                gm.UpdateQtdAsteroides(1);
            }
            if (asteroidetamanho == 1)
            {
                //se os pequenos forem atingidos 
                gm.UpdateQtdAsteroides(-1);
            }
            //pontos para player
            player.SendMessage("ScorePoints", pontos);
        }
    }
    void NovoImpulso()
    {
        //impulso ao asteroide apos ele ganhar nova posicao
        Vector2 impulso = new Vector2(impulsoAst, impulsoAst);
        float torque = torqueAst;
        bodyAst.angularVelocity = torqueAst;
        bodyAst.AddForce(impulso);
        bodyAst.AddTorque(torque);
    }

}
