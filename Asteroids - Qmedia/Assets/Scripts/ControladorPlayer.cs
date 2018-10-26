using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    //texto de pontuacao
    public Text scoreText;
    //texto de vidas
    public Text vidasText;
    //lista de highscore
    public Text HighScoreListText;
    //audio player
    public AudioSource perdeVida;
    //hyperspace ativado ou nao
    private bool hyperspace;
    //acessar o alien
    public AlienControlador alien;
    //game manager
    public GameManager gm;
    //player da input de nome
    public InputField highScoreInput;




    // Use this for initialization
    void Start()
    {
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

        //adicionar botao para atirar
        if (Input.GetButtonDown("Fire1"))
        {
            //instanciando o shot na posicao do player e dando direcao
            GameObject novoShot = Instantiate(shot, transform.position, transform.rotation);
            novoShot.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * shotForce);
            //shot desaparece e reutiliza
            if (transform.position.y > fimTelaSup)
            {
                shotEnable.SetActive(false);
                Debug.Log("saiu o tiro");
            }
            if (transform.position.y < fimTelaInf)
            {
                shotEnable.SetActive(false);
            }
            if (transform.position.x > fimTelaEsq)
            {
                shotEnable.SetActive(false);
            }
            if (transform.position.x < fimTelaDir)
            {
                shotEnable.SetActive(false);
            }


        }
        if (Input.GetButtonDown("Hyperspace") && !hyperspace)
        {
            hyperspace = true;
            //desligar colliders e desativar player
            Invoke("Hyperspace", 1f);
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
    void FixedUpdate()
    {
        //arrasto do player para direcao que estava
        body.AddRelativeForce(Vector2.up * InputPropulsao);
        // giro do player
        //body.AddTorque(-InputLateral);
    }
    void ScorePoints(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = "Score " + score;
    }
    void Hyperspace()
    {
        Vector2 newPosition = new Vector2(Random.Range(-8, +8), Random.Range(-5, 5));
        transform.position = newPosition;
        //ativar collider e objeto aqui



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
        if (other.CompareTag("Alienshot"))
        {
            PerderVida();
            alien.Desativado();
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

    }
    public void HighScoreInput()
    {
        string newInput = highScoreInput.text;
        newHighScorePanel.SetActive(false);
        gameOverpainel.SetActive(true);
        PlayerPrefs.SetString("highScoreName", newInput);
        PlayerPrefs.SetInt("highScore", score);
        HighScoreListText.text = "HIGH SCORE" + "\n" + PlayerPrefs.GetString("highScoreName")+ " " + PlayerPrefs.GetInt("highscore");
    }
    public void JogarNovamente()
    {
        SceneManager.LoadScene("QmediaAsteroids");
    }
    public void Menu()
    {
        SceneManager.LoadScene("Start Menu");
    }
}
