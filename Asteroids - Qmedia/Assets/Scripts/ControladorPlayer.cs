using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControladorPlayer : MonoBehaviour
{
    //cria o shot
    public GameObject shot;
    //ativar e destivar shot
    public GameObject shotEnable;
    //ativar painel de gameover
    public GameObject gameOverpainel;
    //painel de novo High Score
    public GameObject newHighScorePanel;
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
    //se bater no asteroide
    public float Astforce;
    //score do player
    public int score;
    //vidas do player
    public int vidas;
    //numero de balas na lista
    public int numeroBullets;
    //texto de pontuacao
    public Text scoreText;
    //texto de vidas
    public Text vidasText;
    //lista de highscore
    public Text HighScoreListText;
    //audio player
    public AudioSource perdeVida;
    //acessar o alien
    public AlienControlador alien;
    //game manager
    public GameManager gm;
    //player da input de nome
    public InputField highScoreInput;
    //lista do shot
    List<Rigidbody2D> bulletRigidBodyList;




    // Use this for initialization
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        //criar a movimentacao do player
        InputPropulsao = Input.GetAxis("Vertical");
        InputLateral = Input.GetAxis("Horizontal");

        //fazer a rotacao da nave
        transform.Rotate(Vector2.up * InputLateral * Time.deltaTime * propulsaolateral);
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
        // giro do player
        body.AddTorque(-InputLateral);
    }
    void ScorePoints(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = "Score " + score;
    }
    void PerderVida()
    {
        vidas--;
        vidasText.text = "Vidas " + vidas;

        if (vidas <= 0)
        {
            //fim de jogo
            GameOver();
        }
    }
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
            HighScoreListText.text = "HIGH SCORE" + "\n" + PlayerPrefs.GetString("highScoreName") + " " + PlayerPrefs.GetInt("highscore");
        }
        gameObject.SetActive(false);

    }
    public void HighScoreInput()
    {
        string newInput = highScoreInput.text;
        newHighScorePanel.SetActive(false);
        gameOverpainel.SetActive(true);
        PlayerPrefs.SetString("highScoreName", newInput);
        PlayerPrefs.SetInt("highScore", score);
        HighScoreListText.text = "HIGH SCORE" + "\n" + PlayerPrefs.GetString("highScoreName") + " " + PlayerPrefs.GetInt("highscore");
    }
    private void ResetObj(Rigidbody2D ResetarObj)
    {
        ResetarObj.transform.position = transform.position;
        ResetarObj.transform.rotation = transform.rotation;
    }
}
