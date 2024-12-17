using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivacionMensaje : MonoBehaviour
{
    [SerializeField] private GameObject mensaje;
    private SpriteRenderer Spr;

    // Start is called before the first frame update
    private void Start()
    {
        Spr = mensaje.GetComponent<SpriteRenderer>();
        Color c = Spr.material.color;
        c.a = 0f;
        Spr.material.color = c;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine("FadeIn");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine("FadeOut");
        }
    }

    IEnumerator FadeIn()
    {
        for (float f = 0.0f; f <= 1; f += 0.02f)
        {
            Color c = Spr.material.color;
            c.a = f;
            Spr.material.color = c;
            yield return (0.05f);
        }
    }

    IEnumerator FadeOut()
    {
        for (float f = 1f; f >= 0; f -= 0.02f)
        {
            Color c = Spr.material.color;
            c.a = f;
            Spr.material.color = c;
            yield return (0.05f);
        }
    }
}
