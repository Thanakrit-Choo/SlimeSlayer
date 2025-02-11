using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBullet : MonoBehaviour
{
    public float speed = 20f;
    Rigidbody2D rb;
    SlimeBoss slimeBoss;
    public int bulletAttack = 25;
    private float lifeTime = 5.0f;
    void Start()
    {
        TryGetComponent<Rigidbody2D>(out rb);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.right * -speed;
        if(lifeTime < 0.0f)
        {
            Destroy(gameObject);
        }
        lifeTime -= Time.deltaTime;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if(collision.tag == "Player")
        {
            Destroy(gameObject);
            playerHealth.TakeDamage(bulletAttack);
            
        }

    }
}
