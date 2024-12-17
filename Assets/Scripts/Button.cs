using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void RestartGame(){
        SceneManager.LoadScene("Nivel1");
        PlayerController.ScoreManager.score = 0;
    }
}
