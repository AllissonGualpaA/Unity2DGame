using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    public Sprite[] mySprites;
    private int index = 0;
    private SpriteRenderer mySpriteRenderer;
    //public GameManager myGameManager;

    // Start is called before the first frame update
    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(WalkCoRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WalkCoRoutine()
    {
        yield return new WaitForSeconds(0.05f);
        mySpriteRenderer.sprite = mySprites[index];
        index++;
        if (index == mySprites.Length)
        {
            index = 0;
        }
        StartCoroutine(WalkCoRoutine());
    }

}

