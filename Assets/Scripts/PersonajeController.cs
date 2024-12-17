using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersonajeController : MonoBehaviour
{
    public float playerJumpForce = 20f;
    public float playerSpeed = 10f;
    public Sprite[] mySprites;
    private int index = 0;

    private Rigidbody2D myrigidbody2D;
    private SpriteRenderer mySpriteRenderer;
    public GameObject Bullet;
    private bool isMoving = false;
    private bool isFacingRight = true;
    private bool hasJumped = false; // Variable para rastrear si el personaje ya ha saltado
    public GameManager myGameManager;

    void Start()
    {
        myrigidbody2D = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myGameManager = FindObjectOfType<GameManager>();
        StartCoroutine(WalkCoRoutine());
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        myrigidbody2D.velocity = new Vector2(horizontalInput * playerSpeed, Mathf.Clamp(myrigidbody2D.velocity.y, -Mathf.Abs(playerJumpForce), Mathf.Infinity));

        if (horizontalInput > 0)
        {
            mySpriteRenderer.flipX = false;
            isMoving = true;
            isFacingRight = true;
        }
        else if (horizontalInput < 0)
        {
            mySpriteRenderer.flipX = true;
            isMoving = true;
            isFacingRight = false;
        }
        else
        {
            isMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !hasJumped) // Solo salta si aún no ha saltado
        {
            myrigidbody2D.velocity = new Vector2(myrigidbody2D.velocity.x, playerJumpForce);
            hasJumped = true; // Marca que el personaje ha saltado
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector2 fireDirection = isFacingRight ? Vector2.right : Vector2.left;
        GameObject bulletObject = Instantiate(Bullet, transform.position, Quaternion.identity);
        Bullet bulletScript = bulletObject.GetComponent<Bullet>();

        if (!isFacingRight)
        {
            Vector3 originalScale = bulletObject.transform.localScale;
            bulletObject.transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }

        bulletScript.SetDirection(fireDirection);
    }

    IEnumerator WalkCoRoutine()
    {
        while (true)
        {
            if (isMoving)
            {
                mySpriteRenderer.sprite = mySprites[index];
                index++;
                if (index == mySprites.Length)
                {
                    index = 0;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathZone"))
        {
            // Zona de muerte normal, reposiciona al jugador en la parte superior de la pantalla
            PlayerDeath();
        }
        else if (collision.CompareTag("DeathZoneUp") || collision.gameObject.CompareTag("DeathZoneUp"))
        {
            // Zona de muerte especial, reposiciona al jugador en la parte superior de la pantalla
            RepositionPlayerUp();
        }

        // Restablece la capacidad de saltar cuando el jugador toca el suelo nuevamente
        if (collision.CompareTag("Ground"))
        {
            hasJumped = false;
        }
    }

    void RepositionPlayerUp()
    {
        float topPosition = Camera.main.transform.position.y + Camera.main.orthographicSize + 2f; // Ajustar este valor según sea necesario
        Vector3 newPosition = new Vector3(transform.position.x, topPosition, transform.position.z);
        transform.position = newPosition;
    }

    public void PlayerDeath()
    {
        Debug.Log("si llama este metodo");
        SceneManager.LoadScene("NivelTres");
    }
}