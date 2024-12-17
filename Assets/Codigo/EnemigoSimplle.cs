using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoSimplle : MonoBehaviour
{

    [SerializeField] private Transform[] PuntosMov;
    [SerializeField] private float Velocidad;
    [SerializeField] private GameObject padre;
    [SerializeField] private GameObject parte1, parte2;
    [SerializeField] private GameObject jugador;

    private BoxCollider2D BoxCol1, BoxCol2;
    private SpriteRenderer spr1, spr2;

    private float VelocidadIni;
    private int i = 0;

    private Vector3 EscalaIni, EscalaTemp;
    private float MiraDer = 1;

    // Start is called before the first frame update
    void Start()
    {
        EscalaIni = transform.localScale;
        BoxCol1 = parte1.GetComponent<BoxCollider2D>();
        BoxCol2 = parte1.GetComponent<BoxCollider2D>();
        spr1 = parte1.GetComponent<SpriteRenderer>();
        spr2 = parte2.GetComponent<SpriteRenderer>();
        VelocidadIni = Velocidad;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, PuntosMov[i].transform.position, Velocidad * Time.deltaTime);
        if (Vector2.Distance(transform.position, PuntosMov[i].transform.position) < 0.1f)
        {
            if (PuntosMov[i] != PuntosMov[PuntosMov.Length - 1]) i++;
            else i = 0;
            MiraDer = Mathf.Sign(PuntosMov[i].transform.position.x - transform.position.x);
            Gira(MiraDer);
        }
    }

    private void FixedUpdate()
    {
        float lado = Mathf.Sign(jugador.transform.position.x - transform.position.x);
        if (Mathf.Abs(transform.position.x -jugador.transform.position.x) < 30 && lado == MiraDer)
        {
            ataca();
        }
        else
        {
            defiende();
        }
    }

    private void ataca()
    {
        parte2.transform.rotation = Quaternion.Lerp(parte2.transform.rotation, Quaternion.Euler(0,0,-45), 10 * Time.deltaTime);
        Velocidad *= 1.1f;
    }

    private void defiende()
    {
        parte2.transform.rotation = Quaternion.Lerp(parte2.transform.rotation, Quaternion.Euler(0, 0, 0), 10 * Time.deltaTime);
        Velocidad = VelocidadIni;
    }



    private void Gira(float lado)
    {
        if (MiraDer == -1)
        {
            EscalaTemp = transform.localScale;
            EscalaTemp.x = EscalaTemp.x * -1;
        }
        else EscalaTemp = EscalaIni;
        transform.localScale = EscalaTemp;
    }

    public void Muere()
    {
        BoxCol1.enabled = false;
        BoxCol2.enabled = false;
        StartCoroutine("FadeOut");
    }

    IEnumerator FadeOut()
    {
        for (float f = 1f; f >= 0; f -= 0.2f)
        {
            Color c1 = spr1.material.color;
            c1.a = f;
            spr1.material.color = c1;
            Color c2 = spr2.material.color;
            c2.a = f;
            spr2.material.color = c1;
            yield return new WaitForSeconds(0.025f);
        }
        Destroy(padre);
    }
}
