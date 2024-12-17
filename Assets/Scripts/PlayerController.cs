using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 5f; // Velocidad de movimiento del personaje
    public float fuerzaSalto = 10f; // Fuerza aplicada al saltar
    public Sprite[] mySprites; // Sprites del personaje caminando

    private Rigidbody2D rb; // Rigidbody2D del personaje
    private SpriteRenderer spriteRenderer; // Componente SpriteRenderer del personaje
    private int index = 0; // Índice actual del sprite
    private bool isMoving = false; // Flag para controlar si el personaje está en movimiento
    private bool enSuelo = false; // Flag para controlar si el personaje está en el suelo
    //public Text scoreText;
    public Image progressBar;
    public AudioSource musicSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        progressBar.transform.localScale = new Vector3(0, 1f, 1f);
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(ChangeSpriteCoroutine());
        musicSource.Play();
    }

    void Update()
    {
        // Obtener la entrada del teclado
        float movimientoHorizontal = Input.GetAxis("Horizontal");

        // Mover el personaje
        Vector2 movimiento = new Vector2(movimientoHorizontal * velocidad, rb.velocity.y);
        rb.velocity = movimiento;

        // Girar el sprite según la dirección del movimiento
        if (movimientoHorizontal > 0)
        {
            // Movimiento hacia la derecha
            spriteRenderer.flipX = false;
            isMoving = true;
        }
        else if (movimientoHorizontal < 0)
        {
            // Movimiento hacia la izquierda
            spriteRenderer.flipX = true;
            isMoving = true;
        }
        else
        {
            // No se está moviendo
            isMoving = false;
        }
        // Saltar si el personaje está en el suelo
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            Saltar();
        }
    }

    void Saltar()
    {
        rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        enSuelo = false; // El personaje ya no está en el suelo después de saltar
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si el personaje ha tocado el suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            enSuelo = true;
        }
    }
    public static class ScoreManager
    {
        public static int score = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar si la etiqueta del objeto con el que colisionamos es "Fire" o "Coin"
        if (collision.CompareTag("Fire") || collision.CompareTag("Spikes"))
        {
            ScoreManager.score = 0;
            SceneManager.LoadScene("FinalLevel");
        }
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            ScoreManager.score += 10;
            UpdateScoreDisplay();
            if (ScoreManager.score == 10){
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    IEnumerator ChangeSpriteCoroutine()
    {
        while (true)
        {
            // Cambiar el sprite si el personaje está en movimiento
            if (isMoving)
            {
                // Cambia el sprite actual
                spriteRenderer.sprite = mySprites[index];
                index = (index + 1) % mySprites.Length;
            }
            else
            {
                // Si no se está moviendo, muestra el primer sprite
                index = 0;
                spriteRenderer.sprite = mySprites[index];
            }

            // Espera un tiempo antes de cambiar al siguiente sprite
            yield return new WaitForSeconds(0.05f); // Ajusta el tiempo según lo necesites
        }
    }
    public void UpdateScoreDisplay()
    {
        //scoreText.text = "Puntuación: " + ScoreManager.score.ToString();
        float progress = (float)ScoreManager.score / 100f; // Calcula el progreso como un valor entre 0 y 1
        progressBar.transform.localScale = new Vector3(progress, 1f, 1f);
    }
}
