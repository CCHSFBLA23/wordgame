using UnityEngine;
using UnityEngine.UI;

public class TitleIntro : MonoBehaviour
{
    [Header("General Animation Values")]
    public GameObject titlePieces;
    public Vector2 titleTargetPos;
    private RectTransform titlePiecesTransform;
    public float speed;

    [Header("For the Player")]
    public GameObject player;
    public Vector2 playerTargetInit;
    public Vector2 playerTargetCenter;
    public Vector2 playerTargetFinal;
    private RectTransform playerTransform;

    [Header("Buttons")]
    public GameObject playButton;
    public GameObject settingsButton;
    public GameObject quitButton;

    [Header("Border")]
    public GameObject specialBlock;
    public Color gridColor;
    public Color borderColor;

    private void Start()
    {
        // reset visibility
        playButton.SetActive(false);
        quitButton.SetActive(false);
        settingsButton.SetActive(false);
        player.SetActive(true);

        // reset colors
        //specialBlock.GetComponent<Image>().color = gridColor;

        // get positions.
        titlePiecesTransform = titlePieces.GetComponent<RectTransform>();
        playerTransform = titlePieces.transform.GetChild(0).GetComponent<RectTransform>();

        // Play Music
        AudioManager.Play("MenuMusic");
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
            else if (playerTransform.anchoredPosition.x < playerTargetCenter.x) // move the player to the center right
            {
                playerTargetCenter.y = playerTransform.anchoredPosition.y;
                playerTransform.anchoredPosition = Vector2.MoveTowards(playerTransform.anchoredPosition, playerTargetCenter, speed * Time.deltaTime);
            }
            else if (playerTransform.anchoredPosition.y > playerTargetFinal.y) // move player into the play button position
            {
                playerTargetFinal.x = playerTransform.anchoredPosition.x;
                playerTransform.anchoredPosition = Vector2.MoveTowards(playerTransform.anchoredPosition, playerTargetFinal, speed * Time.deltaTime);
            }
            else
            {
                player.SetActive(false);
                playButton.SetActive(true);
                quitButton.SetActive(true);
                settingsButton.SetActive(true);
                //specialBlock.GetComponent<Image>().color = borderColor;
            }
        }
    }
}
