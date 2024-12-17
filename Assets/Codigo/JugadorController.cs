using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JugadorController : MonoBehaviour
{
    [Header("VALORES CONFIGURABLES")]
    [SerializeField] private int Vida = 3;
    [SerializeField] private float velocidad;
    [SerializeField] private float fuerzaDeSalto;
    [SerializeField] private bool SaltoMejorado;
    [SerializeField] public float SaltoLargo = 1.5f;
    [SerializeField] public float SaltoCorto = 1f;
    [SerializeField] private Transform checkGround;
    [SerializeField] private float checkGroundRadio;
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private float addRayo;
    [SerializeField] private float AnguloMax;
    [SerializeField] private PhysicsMaterial2D SinF;
    [SerializeField] private PhysicsMaterial2D MaxF;
    [SerializeField] private float FuerzaToque;

    [Header("BARRA DE VIDA")]
    [SerializeField] private GameObject BarraDeVida;
    [SerializeField] private Sprite Vida3, Vida2, Vida1, Vida0;

    [Header("VALORES INFORMATIVOS")]
    [SerializeField] private bool tocaSuelo = false;
    [SerializeField] private bool EnPendiente;
    [SerializeField] private bool PuedoAndar;
    [SerializeField] private float AnguloPendiente;
    [SerializeField] private float h;

    [Header("EFECTOS DE SONIDO")]
    [SerializeField] private GameObject oSaltoJugador;
    [SerializeField] private GameObject oMuerteJugador;

    private Rigidbody2D rJugador;
    private Animator aJugador;
    private CapsuleCollider2D ccJugador;
    private SpriteRenderer sJugador;
    private Vector2 ccSize;
    private Camera Camara;

    private bool mirarDerecha = true;
    private bool saltando = false;
    private bool puedoSaltar = false;
    private bool EnPlataforma = false;
    private Vector2 NuevaVelocidad;
    private float AnguloLateral;
    
    private float AnguloAnterior;
    private Vector2 AnguloPer;
    private Vector3 PosIni;

    private bool tocando = false;
    private Color ColorOriginal;
    private bool Muerto = false;
    private float PosJugador, AltoCam, AltoJugador;

    private AudioSource sSaltoJugador;
    private AudioSource sMuerteJugador;

    // Start is called before the first frame update
    void Start()
    {
        PosIni = transform.position;
        rJugador = GetComponent<Rigidbody2D>();
        aJugador = GetComponent<Animator>();
        ccJugador = GetComponent<CapsuleCollider2D>();
        sJugador = GetComponent<SpriteRenderer>();
        ccSize = ccJugador.size;
        ColorOriginal = sJugador.color;
        Camara = Camera.main;
        AltoCam = Camara.orthographicSize * 2;
        AltoJugador = GetComponent<Renderer>().bounds.size.y;
        sSaltoJugador = oSaltoJugador.GetComponent<AudioSource>();
        sMuerteJugador = oMuerteJugador.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (JuegoController.GameOn)
        {
            recibePulsacioness();
            variablesDeAnimador();
        }
        if (Muerto)
        {
            PosJugador = Camara.transform.InverseTransformDirection(transform.position - Camara.transform.position).y;
            if (PosJugador < ((AltoCam / 2) * -1) - (AltoJugador / 2))
            {
                Invoke("LlamaRecarga", 1);
                Muerto = false;
            }
            
        }
    }

    private void LlamaRecarga()
    {
        JuegoController.jugadorMuerto = true;
    }


    void FixedUpdate()
    {
        if (JuegoController.GameOn)
        {
            chequeoTocaSuelo();
            chequeoPendiente();
            if (!tocando) movimientoJugador();
        }
    }

    private void movimientoJugador()
    {
        if (tocaSuelo && !EnPendiente && !saltando)
        {
            NuevaVelocidad.Set(velocidad * h, rJugador.velocity.y);
            rJugador.velocity = NuevaVelocidad;
        }
        else if (tocaSuelo && EnPendiente && !saltando && PuedoAndar)
        {
            NuevaVelocidad.Set(velocidad * AnguloPer.x * -h, velocidad * AnguloPer.y * -h);
            rJugador.velocity = NuevaVelocidad;
        }
        else if (!tocaSuelo)
        {
            NuevaVelocidad.Set(velocidad * h, rJugador.velocity.y);
            rJugador.velocity = NuevaVelocidad;
        }
    }

    private void recibePulsacioness()
    {
        if (Input.GetKey(KeyCode.R)) JuegoController.jugadorMuerto = true; //VOLVER A COLOCAR AL JUGADOR A LA POSICION INICIAL
        h = Input.GetAxisRaw("Horizontal");
        if ((h > 0 && !mirarDerecha) || (h < 0 && mirarDerecha)) giraJugador();
        if (Input.GetButtonDown("Jump") && puedoSaltar && tocaSuelo) salto();
        if (SaltoMejorado) saltoMejorado();

    }


    private void salto()
    {
        saltando = true;
        puedoSaltar = false;
        rJugador.velocity = new Vector2(rJugador.velocity.x, 0);
        rJugador.AddForce(new Vector2(0, fuerzaDeSalto), ForceMode2D.Impulse);
        sSaltoJugador.Play();
    }

    private void saltoMejorado()
    {
        if (rJugador.velocity.y < 0)
        {
            rJugador.velocity += Vector2.up * Physics2D.gravity.y * SaltoLargo * Time.deltaTime;
        }
        else if (rJugador.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rJugador.velocity += Vector2.up * Physics2D.gravity.y * SaltoCorto * Time.deltaTime;
        }
    }

    private void chequeoTocaSuelo()
    {
        tocaSuelo = Physics2D.OverlapCircle(checkGround.position, checkGroundRadio, capaSuelo);
        if (rJugador.velocity.y <= 0f)
        {
            saltando = false;
            if (tocando && tocaSuelo)
            {
                rJugador.velocity = Vector2.zero;
                tocando = false;
                sJugador.color = ColorOriginal;
            }
        }
        if (tocaSuelo && !saltando)
        {
            puedoSaltar = true;
            
        }
    }

    //DETECCION DE PLATAFORMAS MOVILES
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "plataformaMovil" && !Muerto)
        {
            rJugador.velocity = Vector3.zero;
            transform.parent = collision.transform;
            EnPlataforma = true;
        }

        if (collision.gameObject.tag == "HeridaEnemigo" && !Muerto)
        {
            Tocado(collision.transform.position.x);
        }

        if (collision.gameObject.tag == "espaldaEnemigo" && !tocando && !Muerto)
        {
            rJugador.velocity = Vector2.zero;
            rJugador.AddForce(new Vector2(0.0f, 10f), ForceMode2D.Impulse);
            collision.gameObject.SendMessage("Muere");
        }
    }

    private void Tocado(float PosX)
    {
        if (!tocando)
        {
            if (Vida > 1)
            {
                Color NuevoColor = new Color(255f / 255f, 100f / 255f, 100f / 255f);
                sJugador.color = NuevoColor;
                tocando = true;
                float lado = Mathf.Sign(PosX - transform.position.x);
                rJugador.velocity = Vector2.zero;
                rJugador.AddForce(new Vector2(FuerzaToque * -lado, FuerzaToque), ForceMode2D.Impulse);
                Vida--;
                barraVida(Vida);
            } else
            {
                MuerteJugador();
            }
        }
    }


    private void barraVida(int salud)
    {
        if (salud == 2) BarraDeVida.GetComponent<Image>().sprite = Vida2;
        if (salud == 1) BarraDeVida.GetComponent<Image>().sprite = Vida1;
    }


    private void MuerteJugador()
    {
        sMuerteJugador.Play();
        BarraDeVida.GetComponent<Image>().sprite = Vida0;
        aJugador.Play("JugadorMuerto");
        JuegoController.GameOn = false;
        rJugador.velocity = Vector2.zero;
        rJugador.AddForce(new Vector2(0.0f, fuerzaDeSalto), ForceMode2D.Impulse);
        ccJugador.enabled = false;
        Muerto = true;
    }


    //FIN DE DETECCION DE PLATAFORMAS MOVILES
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "plataformaMovil" && !Muerto)
        {
            transform.parent = null;
            EnPendiente = false;
            EnPlataforma = true;
            PuedoAndar = true;
            puedoSaltar = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pinchos" && !Muerto)
        {
            MuerteJugador();
        }

        if (collision.gameObject.tag == "CaidaVacio")
        {
            Invoke("LlamaRecarga", 1);
        }
    }

    private void chequeoPendiente()
    {
        if (!EnPlataforma)
        {
            Vector2 PosPies = transform.position - (Vector3)(new Vector2(0.0f, ccSize.y / 2));
            chequeoPendienteHoriz(PosPies);
            chequeoPendienteVerti(PosPies);
        }
    }

    private void chequeoPendienteHoriz(Vector2 PosPies)
    {
        RaycastHit2D HitDelante = Physics2D.Raycast(PosPies, Vector2.right, addRayo, capaSuelo);
        RaycastHit2D HitDetras = Physics2D.Raycast(PosPies, -Vector2.right, addRayo, capaSuelo);
        Debug.DrawRay(PosPies, Vector2.right * addRayo, Color.cyan);
        Debug.DrawRay(PosPies, -Vector2.right * addRayo, Color.red);
        if (HitDelante)
        {
            AnguloLateral = Vector2.Angle(HitDelante.normal, Vector2.up);
            if (AnguloLateral > 0) EnPendiente = true;

        }
        else if (HitDetras)
        {
            AnguloLateral = Vector2.Angle(HitDetras.normal, Vector2.up);
            if (AnguloLateral > 0) EnPendiente = true;
        }
        else
        {
            EnPendiente = false;
            AnguloLateral = 0.0f;
        }
    }

    private void chequeoPendienteVerti(Vector2 PosPies)
    {
        RaycastHit2D Hit = Physics2D.Raycast(PosPies, Vector2.down, addRayo, capaSuelo);
        if (Hit)
        {
            AnguloPendiente = Vector2.Angle(Hit.normal, Vector2.up);
            AnguloPer = Vector2.Perpendicular(Hit.normal).normalized;
            if (AnguloPendiente != AnguloAnterior && AnguloPendiente > 0) EnPendiente = true;
            AnguloAnterior = AnguloPendiente;
            Debug.DrawRay(Hit.point, AnguloPer, Color.blue);
            Debug.DrawRay(Hit.point, Hit.normal, Color.green);
        }

        if (AnguloPendiente > AnguloMax || AnguloLateral > AnguloMax)
        {
            PuedoAndar = false;
        }
        else
        {
            PuedoAndar = true;
        }
        if (EnPendiente && PuedoAndar && h == 0.0f)
        {
            rJugador.sharedMaterial = MaxF;
        }
        else
        {
            rJugador.sharedMaterial = SinF;
        }
    }

    private void variablesDeAnimador()
    {
        aJugador.SetFloat("velocidadX", Mathf.Abs(rJugador.velocity.x));
        aJugador.SetFloat("velocidadY", rJugador.velocity.y);
        aJugador.SetBool("tocaSuelo", tocaSuelo);
        aJugador.SetBool("saltando", saltando);
    }
        
    void giraJugador()
    {
            mirarDerecha = !mirarDerecha;
            Vector3 escalaDeGiro = transform.localScale;
            escalaDeGiro.x = escalaDeGiro.x * -1;
            transform.localScale = escalaDeGiro;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(checkGround.position, checkGroundRadio);
    }
}
