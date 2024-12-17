using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiedraPinchoss : MonoBehaviour
{
    [SerializeField] private Transform Destino;
    [SerializeField] private float velocidad;
    [SerializeField] private float TiempoQuieto;

    private Vector3 posIni, posFin;
    private bool EnMovi;
    private float Tiempo;

    // Start is called before the first frame update
    void Start()
    {
        Destino.parent = null;
        posIni = transform.position;
        posFin = Destino.position;

        EnMovi = true;
        Tiempo = 0.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (EnMovi)
        {
            transform.position = Vector3.MoveTowards(transform.position, Destino.position, velocidad * Time.deltaTime);
            if (transform.position == Destino.position)
            {
                Destino.position = (Destino.position == posFin) ? posIni : posFin;
                EnMovi = false;
            }
        } else
        {
            Tiempo += Time.deltaTime;
            if (Tiempo >= TiempoQuieto)
            {
                Tiempo = 0.0f;
                EnMovi = true;
            }
        }
        
        
    }
}