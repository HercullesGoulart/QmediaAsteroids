using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienControlador : MonoBehaviour
{
    #region Alien
    //body do alien
    public Rigidbody2D body;
    //direcao do alien
    public Vector2 direcao;
    //velocidade do Alien
    public float velocidade;
    //tempo que o alien leva para nascer novamente
    public float tempoSpawn;
    //pega o renderer do alien
    public SpriteRenderer spriteRendererAlien;
    //pega o collider do alien
    public Collider2D colliderAlien;
    //ativar e desativar alien
    public bool desativado;
    //posicao inicial do alien
    public Transform startPosition;
    #endregion
    #region Shot
    //cria o shot
    public GameObject bullet;
    //tempo para atirar novamente
    float fireRate;
    float nextFire;
    #endregion
    #region Player
    //referenciar a posicao do player
    public Transform player;
    #endregion
    #region Score
    //pontos para score
    public int pontos;
    #endregion
    #region Level
    //level atual 
    public int atualLevel = 0;
    #endregion
    void Start()
    {
        //Inicando o Alien no inicio do lvl 2
        fireRate = 3f;
        nextFire = Time.time;
        NewLevel();
    }
    void Update()
    {
        //checando o tempo para atirar
        CheckIfTimeToFire();
    }
    void FixedUpdate()
    {
        if (desativado)
        {
            return;
        }
        //estou dizendo para o alien ir em direcao ao player
        direcao = (player.position - transform.position).normalized;
        body.MovePosition(body.position + direcao * velocidade * Time.fixedDeltaTime);
    }
    public void NewLevel()
    {
        //configuracoes do novo level
        Desativado();
        atualLevel++;
        tempoSpawn = Random.Range(5f, 20f);
        Invoke("Ativado", tempoSpawn);
        velocidade = atualLevel;
        pontos = 500 * atualLevel;
    }
    public void CheckIfTimeToFire()
    {
        if (desativado)
        {
            if (Time.time > nextFire)
            {
                Ativado();
            }
            return;
        }
        if (Time.time > nextFire)
        {
            //aqui a bala e criada
            Instantiate(bullet, transform.position, Quaternion.identity);
            nextFire = Time.time + fireRate;
        }
    }
    public void Ativado()
    {
        //ativar o alien na posicao inicial 
        transform.position = startPosition.position;
        colliderAlien.enabled = true;
        spriteRendererAlien.enabled = true;
        desativado = false;
    }
    public void Desativado()
    {
        //desativar colliders e render
        colliderAlien.enabled = false;
        spriteRendererAlien.enabled = false;
        desativado = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Bullet")){

            //pontos para player
            player.SendMessage("ScorePoints", pontos);
            //fazer o alien sumir
            Desativado();  
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player")){
            //desativar alien
            Desativado();
        }
    }
    private void ResetObj(Rigidbody2D ResetarObj)
    {
        //Reseta os objetos
        ResetarObj.transform.position = transform.position;
        ResetarObj.transform.rotation = transform.rotation;
    }
}
