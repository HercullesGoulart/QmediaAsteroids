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
    //velocidade do shot
    public float velocidadeShot;
    //tempo para atirar novamente
    public float shotDelay;
    //ultimo shot
    public float ultimoShot = 0f;
    //tempo que o alien leva para nascer novamente
    public float tempoSpawn;
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
    //level atual 
    public int atualLevel = 0;
    //posicao inicial do alien
    public Transform startPosition;
    //lista do shot
    List<Rigidbody2D> bulletRigidBodyList;
    //numero de balas na lista
    public int numeroBullets;
    //cria o shot
    public GameObject shot;
    //velocidade do shot
    public float shotForce;


    void Start()
    {
        bulletRigidBodyList = new List<Rigidbody2D>();
        for (int i = 0; i < numeroBullets; i++)
        {
            Rigidbody2D objbullet = ((Rigidbody2D)Instantiate(shot).GetComponent<Rigidbody2D>());
            objbullet.gameObject.SetActive(false);
            bulletRigidBodyList.Add(objbullet);
        }
        //encontrando a posicao do player
        player = GameObject.FindWithTag("Player").transform;


        NewLevel();
    }

    void Update()
    {
        if (desativado)
        {
            if(Time.time > ultimoShot + tempoSpawn)
            {
                Ativado();
            }
            return;
        }
        if (Time.time > ultimoShot + shotDelay)
        {
            //criar metodo para atirar em direcao ao player
            body.AddForce(shot.transform.forward * shotForce);
            ultimoShot = Time.time;
            Fire();
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
        body.MovePosition(body.position + direcao * velocidade * Time.fixedDeltaTime);
    }

    public void NewLevel()
    {
        Desativado();
        atualLevel++;
        tempoSpawn = Random.Range(5f, 20f);
        Invoke("Ativado", tempoSpawn);
        velocidade = atualLevel;
        velocidadeShot = shotForce * atualLevel;
        pontos = 500 * atualLevel;
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
                bulletRigidBodyList[i].AddRelativeForce(Vector3.forward * shotForce);
                break;
            }
        }

    }

    public void Ativado()
    {
        //ativar o alien na posicao inicial 
        transform.position = startPosition.position;
        collider.enabled = true;
        spriteRenderer.enabled = true;
        desativado = false;

    }

    public void Desativado()
    {
        //desativar colliders e render
        collider.enabled = false;
        spriteRenderer.enabled = false;
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
        ResetarObj.transform.position = transform.position;
        ResetarObj.transform.rotation = transform.rotation;
    }
}
