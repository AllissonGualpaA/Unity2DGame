using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Moneda : MonoBehaviour
{
    public Sprite[] mySprites;
    private int index = 0;
    private SpriteRenderer mySpriteRenderer;
    public GameManager myGameManager;
    public Text textScore; // Objeto Text para mostrar la puntuación en la interfaz de usuario

    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(WalkCoRoutine());
    }

    IEnumerator WalkCoRoutine()
    {
        yield return new WaitForSeconds(0.05f);
        mySpriteRenderer.sprite = mySprites[index];
        index = (index + 1) % mySprites.Length; // Simplifica la lógica del índice
        StartCoroutine(WalkCoRoutine());
    }

    // Detectar colisiones con el jugador
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Si el objeto con el que colisiona tiene la etiqueta "Player"
        {
            myGameManager.AddScore(); // Aumentar la puntuación del jugador al recoger la moneda (por ejemplo, 10 puntos)
            Destroy(this.gameObject); // Destruir la moneda
        }
    }
}

