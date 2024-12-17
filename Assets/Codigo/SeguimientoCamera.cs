using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguimientoCamera : MonoBehaviour
{
    [SerializeField] private GameObject FondoLejosGo;
    [SerializeField] private GameObject FondoMedioGo;
    [SerializeField] private float VelocidadScroll;

    private Renderer FondoLejosR, FondoMedioR;
    private float IniCamX, DifCamX;

    public Vector2 MinCamaraPos, MaxCamaraPos;
    public GameObject SeguirCam;
    public float MovSuave;

    private Vector2 Velocidad;
    
    // Start is called before the first frame update
    void Start()
    {
        FondoLejosR = FondoLejosGo.GetComponent<Renderer>();
        FondoMedioR = FondoMedioGo.GetComponent<Renderer>();
        IniCamX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        DifCamX = IniCamX - transform.position.x;
        FondoLejosR.material.mainTextureOffset = new Vector2(DifCamX * VelocidadScroll * -1, 0.0f);
        FondoMedioR.material.mainTextureOffset = new Vector2(DifCamX * (VelocidadScroll * 1.5f) * -1, 0.0f);

        FondoLejosGo.transform.position = new Vector3(transform.position.x, FondoLejosGo.transform.position.y, FondoLejosGo.transform.position.z);
        FondoMedioGo.transform.position = new Vector3(transform.position.x, FondoMedioGo.transform.position.y, FondoMedioGo.transform.position.z);

        float PosX = Mathf.SmoothDamp(transform.position.x, SeguirCam.transform.position.x, ref Velocidad.x, MovSuave);
        float PosY = Mathf.SmoothDamp(transform.position.y, SeguirCam.transform.position.y, ref Velocidad.y, MovSuave);

        transform.position = new Vector3(
            Mathf.Clamp(PosX, MinCamaraPos.x, MaxCamaraPos.x),
            Mathf.Clamp(PosY, MinCamaraPos.y, MaxCamaraPos.y), 
            transform.position.z);
    }
}
