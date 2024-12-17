using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltoMejorado : MonoBehaviour
{
    public float SaltoLargo = 1.5f;
    public float SaltoCorto = 1f;

    Rigidbody2D rB;
    
    // Start is called before the first frame update
    void Start()
    {
        rB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rB.velocity.y < 0)
        {
            rB.velocity += Vector2.up * Physics2D.gravity.y * SaltoLargo * Time.deltaTime;
        } else if (rB.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rB.velocity += Vector2.up * Physics2D.gravity.y * SaltoCorto * Time.deltaTime;
        }
    }
}
