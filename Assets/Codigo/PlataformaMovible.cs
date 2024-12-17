using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaMovible : MonoBehaviour
{
    [SerializeField] private Transform Destino;
    [SerializeField] private float velocidad;

    private Vector3 posIni, posFin;

    // Start is called before the first frame update
    void Start()
    {
        Destino.parent = null;
        posIni = transform.position;
        posFin = Destino.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Destino.position, velocidad * Time.deltaTime);
        if (transform.position == Destino.position)
        {
            Destino.position = (Destino.position == posFin) ? posIni : posFin; 
        }
    }
}
