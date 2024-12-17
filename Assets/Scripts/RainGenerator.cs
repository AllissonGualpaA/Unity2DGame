using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainGenerator : MonoBehaviour
{
    public GameObject[] itemPrefab;

    public float minTime = 1f;
    public float maxTime = 2f;

    void Start()
    {
        float tiempoAleatorio = Random.Range(3, 11);
        StartCoroutine(SpawnCoroutine(tiempoAleatorio));
    }

    IEnumerator SpawnCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Instantiate(itemPrefab[Random.Range(0, itemPrefab.Length)], transform.position, Quaternion.identity);
        StartCoroutine(SpawnCoroutine(Random.Range(minTime, maxTime)));
    }
}

