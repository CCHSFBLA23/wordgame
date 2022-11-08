using System.Diagnostics;
using UnityEngine;

public class TitleIntro : MonoBehaviour
{
    [Header("General Animation Values")]
    public GameObject titlePieces;
    public Vector2 titleTargetPos;
    private RectTransform titlePiecesTransform;
    public float speed;
    [Header("For the Player")]
    public Vector2 playerTargetInit;
    public Vector2 playerTargetFinal;
    private RectTransform playerTransform;
    private void Start()
    {
        titlePiecesTransform = titlePieces.GetComponent<RectTransform>();
        playerTransform = titlePieces.transform.GetChild(0).GetComponent<RectTransform>();
    }

    void Update()
    {
        if (titlePiecesTransform.anchoredPosition.x < titleTargetPos.x) // then move all parts of title
        {
            titlePiecesTransform.anchoredPosition = Vector2.MoveTowards(titlePiecesTransform.anchoredPosition, titleTargetPos, speed * Time.deltaTime);
        }
        else // then move the player
        {
            playerTargetInit.x = playerTransform.anchoredPosition.x;
            if (playerTransform.anchoredPosition.y > playerTargetInit.y) // move player down
            {
                playerTransform.anchoredPosition = Vector2.MoveTowards(playerTransform.anchoredPosition, playerTargetInit, speed * Time.deltaTime);
            }
            else // move player to the right and off screen
            {
                playerTargetFinal.y = playerTransform.anchoredPosition.y;
                playerTransform.anchoredPosition = Vector2.MoveTowards(playerTransform.anchoredPosition, playerTargetFinal, speed * Time.deltaTime);
            }
        }
    }
}
