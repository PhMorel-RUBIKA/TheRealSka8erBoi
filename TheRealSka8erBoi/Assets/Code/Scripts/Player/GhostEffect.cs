using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    public float ghostDelay;
    private float ghostDelaySeconds;
    public GameObject ghostRed, ghostBLue;

    private bool isRed;

    private void Start()
    {
        ghostDelaySeconds = ghostDelay;
        isRed = false;
    }

    private void Update()
    {
        if (!PlayerBehaviour.playerBehaviour.dash) return;
        var transform1 = PlayerBehaviour.playerBehaviour.transform;

        if (isRed)
        {
            isRed = false;
            GameObject currentGhost = Instantiate(ghostRed, transform1.position - transform1.forward, transform.rotation);
            Sprite currentSprite = transform1.gameObject.GetComponent<SpriteRenderer>().sprite;
            currentGhost.transform.localScale = this.transform.localScale;
            currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
            ghostDelaySeconds = ghostDelay;
            Destroy(currentGhost, 0.4f);
        }
        else
        {
            GameObject currentGhost = Instantiate(ghostBLue, transform1.position - transform1.forward, transform.rotation);
            isRed = true;
            Sprite currentSprite = transform1.gameObject.GetComponent<SpriteRenderer>().sprite;
            currentGhost.transform.localScale = this.transform.localScale;
            currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
            ghostDelaySeconds = ghostDelay;
            Destroy(currentGhost, 0.4f);
        }
    }
}

