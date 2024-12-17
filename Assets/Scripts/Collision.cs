using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si la etiqueta del objeto con el que colisionamos es "Fire" o "Coin"
        if (other.CompareTag("Fire") || other.CompareTag("Coin"))
        {
            // Destruir el objeto con el que colisionamos
            Destroy(other.gameObject);
        }
    }
}
