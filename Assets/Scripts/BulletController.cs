using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public AudioClip Sound;
    private Rigidbody2D MyRigidBody2D;
    public float bulletSpeed;
    public GameManager myGameManager;

    private Vector2 direction; // Dirección en la que se lanzará la bala

    void Start()
    {
        MyRigidBody2D = GetComponent<Rigidbody2D>();
        Camera.main.GetComponent<AudioSource>().PlayOneShot(Sound);
    }

    void Update()
    {
        // Establecer la velocidad de la bala según la dirección
        MyRigidBody2D.velocity = direction * bulletSpeed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ItemBad"))
        {
            Debug.Log("Si entra");
            myGameManager.AddScore();
            myGameManager.EnemyDestroyed();
            Destroy(this.gameObject); // Destruye la bala
            Destroy(collision.gameObject);
        }
    }
    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
