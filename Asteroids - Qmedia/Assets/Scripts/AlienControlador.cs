using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienControlador : MonoBehaviour
{
    //criando shot para alien
    public GameObject bullet;
    //body do alien
    public Rigidbody2D body;
    //direcao do alien
    public Vector2 direcao;
    //velocidade do Alien
    public float velocidade;
    //tempo para atirar novamente
    public float shotDelay;
    //ultimo shot
    public float ultimoShot = 0f;
    //referenciar a posicao do player
    public Transform player;
    //pega o renderer do alien
    public SpriteRenderer spriteRenderer;
    //pega o collider do alien
    public Collider2D collider;
    //ativar e desativar alien
    public bool desativado;
    //pontos para score
    public int pontos;
    //tempo que o alien leva para nascer novamente
    public float tempoSpawn;
    //
    float levelStartTime;
    //
    public Transform startPosition;
    

    void Start()
    {
        //encontrando a posicao do player
        player = GameObject.FindWithTag("Player").transform;
        levelStartTime = Time.time;
        tempoSpawn = Random.Range(5f, 20f);
        Invoke("Ativado", tempoSpawn);
        Desativado();
    }

    void Update()
    {
        if (desativado)
        {
            if(Time.time > levelStartTime + tempoSpawn)
            {
                Ativado();
            }
            return;
        }
        if (Time.time > ultimoShot + shotDelay)
        {
            //criar metodo para atirar em direcao ao player
        }
    }
    void FixedUpdate()
    {
        if (desativado)
        {
            return;
        }
        //estou dizendo para o alien ir em direcao ao player
        direcao = (player.position - transform.position).normalized;
        body.MovePosition(body.position + direcao * velocidade * Time.deltaTime);
    }
    void Ativado()
    {
        //ativar o alien na posicao inicial e ativar collider e sprite novamente
        transform.position = startPosition.position;
        collider.enabled = true;
        spriteRenderer.enabled = true;
        desativado = false;

    }

    void Desativado()
    {
        //desativar colliders e render
        collider.enabled = false;
        spriteRenderer.enabled = false;
        desativado = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("bullet")){

            //pontos para player
            player.SendMessage("ScorePoints", pontos);
            //fazer o alien sumir
            Desativado();

            
        }
    }
}
