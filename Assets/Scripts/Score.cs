using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score; // Puntuación actual del jugador
    public Text textScore; // Objeto Text para mostrar la puntuación en la interfaz de usuario

    private static ScoreManager instance; // Instancia única de ScoreManager

    void Start()
    {
        score = 0; // Inicializar la puntuación al comenzar el juego
        instance = this; // Asignar esta instancia como la instancia única
        UpdateScoreText(); // Actualizar el texto de la puntuación al inicio del juego
    }

    public void AddScore(int points)
    {
        score += points; // Añadir puntos a la puntuación actual
        UpdateScoreText(); // Actualizar el texto de la puntuación después de añadir puntos
    }

    void UpdateScoreText()
    {
        if (textScore != null)
        {
            textScore.text = "Score: " + score.ToString(); // Actualizar el texto de la puntuación
        }
    }

    public static ScoreManager GetInstance()
    {
        return instance; // Obtener la instancia única de ScoreManager
    }
}

