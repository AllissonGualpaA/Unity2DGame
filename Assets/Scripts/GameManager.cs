using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int score; 
    public Text textScore;
    public int enemiesDestroyed = 0;
    public GameObject door;
    public float doorFallSpeed = 2f; // Velocidad de caída de la puerta
    private bool doorActivated = false;

    public void AddScore()
    {
        score ++;
        textScore.text = score.ToString();
    }

    public void EnemyDestroyed()
    {
        enemiesDestroyed++;
        CheckEnemyCount();
        Debug.Log("EnemigoControlador");
    }

    void CheckEnemyCount()
    {
        if (enemiesDestroyed >= 6 && !doorActivated)
        {
            doorActivated = true;
            StartCoroutine(FallDoor());
        }
    }

    IEnumerator FallDoor()
    {
        // Posición final en Y de la puerta (ajústala según tu escena)
        float finalYPosition = 12.7f;

        // Mueve la puerta hacia abajo hasta alcanzar su posición final
        while (door.transform.position.y > finalYPosition)
        {
            Vector3 newPosition = door.transform.position - Vector3.up * doorFallSpeed * Time.deltaTime;
            door.transform.position = newPosition;
            yield return null;
        }
    }
}
