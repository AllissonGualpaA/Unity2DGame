using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCaida : MonoBehaviour
{
    [SerializeField] private float TiempoEspera;
    [SerializeField] private float TiempoReaparece;
    [SerializeField] private float Margen;
    [SerializeField] private GameObject Sprite1;
    [SerializeField] private GameObject Sprite2;

    private Rigidbody2D RBody;
    private Vector3 PosIni;
    private SpriteRenderer Spr1, Spr2;

    private bool Menea = false;
    private float MeneaDer = 0.02f;
    
    // Start is called before the first frame update
    void Start()
    {
        RBody = GetComponent<Rigidbody2D>();
        PosIni = transform.position;
        Spr1 = Sprite1.GetComponent<SpriteRenderer>();
        Spr2 = Sprite2.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Menea)
        {
            transform.position = new Vector3(transform.position.x + MeneaDer, transform.position.y, transform.position.z);
            if (transform.position.x >= PosIni.x + Margen || transform.position.x <= PosIni.x - Margen) MeneaDer *= -1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Invoke("Caer", TiempoEspera);
            Invoke("Reaparecer", TiempoReaparece);
            Menea = true;
        }
    }

    private void Caer()
    {
        RBody.isKinematic = false;
    }

    private void Reaparecer()
    {
        Menea = false;
        RBody.velocity = Vector3.zero;
        RBody.isKinematic = true;
        transform.position = PosIni;
        CambiaAlpha(Spr1, 0.0f);
        CambiaAlpha(Spr2, 0.0f);
        StartCoroutine("FadeIn");
    }

    IEnumerator FadeIn()
    {
        for (float f = 0.0f; f <= 1.0f; f+= 0.1f)
        {
            CambiaAlpha(Spr1, f);
            CambiaAlpha(Spr2, f);
            yield return new WaitForSeconds(0.025f);
        }
        CambiaAlpha(Spr1, 1f);
        CambiaAlpha(Spr2, 1f);
    }

    private void CambiaAlpha(SpriteRenderer Spr, float A)
    {
        Color C = Spr.material.color;
        C.a = A;
        Spr.material.color = C;
    }
}
