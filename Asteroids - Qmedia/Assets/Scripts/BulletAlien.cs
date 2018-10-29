using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAlien : MonoBehaviour
{
    //velocidade da bala
    public float moveSpeed;
    //rb da bala
    public Rigidbody2D rb;
    //posicao do player
    PlayerController target;
    //dando direcao a bala
    Vector2 moveDirection;

    void Start()
    {
        //criar GC em Start para otimizacao
        rb = GetComponent<Rigidbody2D>();


        //encontrar e ir ate o player
        target = GameObject.FindObjectOfType<PlayerController>();
        moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
