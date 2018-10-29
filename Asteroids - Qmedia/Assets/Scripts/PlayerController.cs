using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Propulsao
    //propulsao lateral do player
    public float propulsaolateral;
    //propulsao frontal do player
    public float propulsao;
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
    #region Score
    //ativar painel de gameover
    public GameObject gameOverpainel;
    //painel de novo High Score
    public GameObject newHighScorePanel;
    //texto de pontuacao
    public Text scoreText;
    //texto de vidas
    public Text vidasText;
    //lista de highscore
    public Text HighScoreListText;
    //player da input de nome
    public InputField highScoreInput;
    #endregion
    #region Player
    //movimentacao do player
    private float InputPropulsao;
    //movimentacao do player
    private float InputLateral;
    //rigid body do player
    public Rigidbody2D body;
    //renderer do player
    public SpriteRenderer rendererPlayer;
    //score do player
    public int score;
    //vidas do player
    public int vidas;
    #endregion
    #region Audio
    //audio player
    public AudioSource perdeVida;
    #endregion
    #region Asteroide
    //se bater no asteroide
    public float Astforce;
    #endregion
    #region Tiro
    //lista do shot
    List<Rigidbody2D> bulletRigidBodyList;
    //velocidade do shot
    public float shotForce;
    //numero de balas na lista
    public int numeroBullets;
    //cria o shot
    public GameObject shot;
    #endregion
    #region GameManager
    //game manager
    public GameManager gm;
    #endregion
    void Start()
    {
        //cria as balas do player ao iniciar o jogo e as deixa desativadas
        bulletRigidBodyList = new List<Rigidbody2D>();
        for (int i = 0; i < numeroBullets; i++)
        {
            Rigidbody2D objbullet = ((Rigidbody2D)Instantiate(shot).GetComponent<Rigidbody2D>());
            objbullet.gameObject.SetActive(false);
            bulletRigidBodyList.Add(objbullet);
        }
        score = 0;
        //score e vidas iniciais
        scoreText.text = "Score " + score;
        vidasText.text = "Vidas " + vidas;
    }
    void Update()
    {
        //criar a movimentacao do player
        InputPropulsao = Input.GetAxis("Vertical");
        InputLateral = Input.GetAxis("Horizontal");

        //fazer a rotacao da nave
        transform.Rotate(Vector2.up * InputLateral * Time.deltaTime * propulsaolateral);
        //criando nova posicao caso o player passe o limite
        Vector2 novaPos = transform.position;
        //se o player sumir do limite superior e limitando a player a se manter na area da camera
        if (transform.position.y > fimTelaSup)
        {
            novaPos.y = fimTelaInf;
            body.velocity = Vector2.up;
            NovoImpulso();
        }
        if (transform.position.y < fimTelaInf)
        {
            novaPos.y = fimTelaSup;
            body.velocity = Vector2.down;
            NovoImpulso();
        }
        if (transform.position.x > fimTelaEsq)
        {
            novaPos.x = fimTelaDir;
            body.velocity = Vector2.right;
            NovoImpulso();
        }
        if (transform.position.x < fimTelaDir)
        {
            novaPos.x = fimTelaEsq;
            body.velocity = Vector2.left;
            NovoImpulso();
        }
        transform.position = novaPos;
        //adicionar botao para atirar
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }
    void Fire()
    {
        //metodo pooling usando a lista
        for (int i = 0; i < bulletRigidBodyList.Count; i++)
        {
            if (!bulletRigidBodyList[i].gameObject.activeInHierarchy)
            {
                ResetObj(bulletRigidBodyList[i]);
                bulletRigidBodyList[i].gameObject.SetActive(true);
                bulletRigidBodyList[i].AddRelativeForce(Vector2.up * shotForce);
                break;
            }
        }

    }
    void FixedUpdate()
    {
        //arrasto do player para direcao que estava
        body.AddRelativeForce(Vector2.up * InputPropulsao);
        // arrasto do giro do player
        body.AddTorque(-InputLateral);
    }
    void ScorePoints(int pointsToAdd)
    {
        //soma score
        score += pointsToAdd;
        scoreText.text = "Score " + score;
    }
    void PerderVida()
    {
        //soma vidas
        vidas--;
        vidasText.text = "Vidas " + vidas;
        if (vidas <= 0)
        {
            //fim de jogo
            GameOver();
        }
    }
    //perder vida apos colisao com asteroides ou tiros de aliens
    void OnCollisionEnter2D(Collision2D col)
    {
        perdeVida.Play();
        if (col.relativeVelocity.magnitude > Astforce)
        {
            PerderVida();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AlienShot"))
        {
            PerderVida();
        }
    }
    public void HighScoreInput()
    {
        //pegando o input do player para tabela de high score
        string newInput = highScoreInput.text;
        newHighScorePanel.SetActive(false);
        gameOverpainel.SetActive(true);
        PlayerPrefs.SetString("highScoreName", newInput);
        PlayerPrefs.SetInt("highScore", score);
        HighScoreListText.text = "HIGH SCORE" + "\n" + PlayerPrefs.GetString("highScoreName") + " " + PlayerPrefs.GetInt("highScore");
    }
    void GameOver()
    {
        //verificar o highscore
        if (gm.CheckforHighScores(score))
        {
            newHighScorePanel.SetActive(true);
        }
        else
        {
            //aparecer a tela para reiniciar
            gameOverpainel.SetActive(true);
            HighScoreListText.text = "HIGH SCORE" + "\n" + PlayerPrefs.GetString("highScoreName") + " " + PlayerPrefs.GetInt("highScore");
        }
        gameObject.SetActive(false);
        rendererPlayer.enabled = false;
    }
    private void ResetObj(Rigidbody2D ResetarObj)
    {
        //Reseta posicao e rotacao do objeto
        ResetarObj.transform.position = transform.position;
        ResetarObj.transform.rotation = transform.rotation;
    }
    void NovoImpulso()
    {
        //apos nova posicao e adicionado impulso ao player
        Vector2 impulso = new Vector2(propulsao, propulsao);
        body.AddForce(impulso);
    }
}
