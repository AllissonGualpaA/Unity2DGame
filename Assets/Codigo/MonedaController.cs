using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonedaController : MonoBehaviour
{
    private ParticleSystem Particulas;
    private SpriteRenderer spr;
    private bool Activa = true;
    private AudioSource sonido;

    private void Awake()
    {
        Particulas = GetComponent<ParticleSystem>();
        spr = GetComponent<SpriteRenderer>();
        sonido = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Activa)
        {
            JuegoController.SumarMonedas();
            spr.enabled = false;
            Particulas.Play();
            Activa = false;
            sonido.Play();
            //Destroy(gameObject);
        }
    }
}
