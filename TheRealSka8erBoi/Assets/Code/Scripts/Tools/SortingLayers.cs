using UnityEngine;

public class SortingLayers : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerRender;
    [SerializeField] private float offset;

    private void FixedUpdate()
    {
        playerRender.sortingOrder = -(int)(transform.position.y + offset);
    }
}
