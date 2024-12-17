using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class JuegoController : MonoBehaviour
{
    static JuegoController current;
    
    [SerializeField] private GameObject FundidoDeNegro;
    [SerializeField] private TextMeshProUGUI ContadorMonedas;
    [SerializeField] private GameObject Camara;


    public static bool GameOn = false;
    private Image Fundido;
    public static bool jugadorMuerto;
    private AudioSource Musica;


    private int monedas;

    public static void SumarMonedas()
    {
        current.monedas++;
        if (current.monedas < 10) current.ContadorMonedas.text = "0" + current.monedas;
        else current.ContadorMonedas.text = current.monedas.ToString();
    }

    private void Awake()
    {
        /*if (current != null && current != this)
        {
            Destroy(gameObject);
            return;
        */
        current = this;
        //DontDestroyOnLoad(gameObject);
        FundidoDeNegro.SetActive(true);
    }

    private void Start()
    {
        Fundido = FundidoDeNegro.GetComponent<Image>();
        Musica = Camara.GetComponent<AudioSource>();
        Invoke("QuitaFundido", 0.5f);
    }

    private void Update()
    {
        if (jugadorMuerto)
        {
            Musica.Stop();
            StartCoroutine("PonerFC");
            jugadorMuerto = false;
        }
    }

    private void QuitaFundido()
    {
        StartCoroutine("QuitaFC");
    }

    IEnumerator QuitaFC()
    {
        for (float alpha = 1f; alpha >= 0; alpha -= Time.deltaTime * 2f)
        {
            Fundido.color = new Color(Fundido.color.r, Fundido.color.g, Fundido.color.b, alpha);
            yield return null;
        }
        GameOn = true;
        Musica.Play();
    }

    IEnumerator PonerFC()
    {
        // Verificar si el objeto Image existe
        if (Fundido != null)
        {
            for (float alpha = 0f; alpha <= 1; alpha += Time.deltaTime * 2f)
            {
                // Actualizar el color solo si el objeto Image sigue siendo válido
                Fundido.color = new Color(Fundido.color.r, Fundido.color.g, Fundido.color.b, alpha);
                yield return null;
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
