using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Endgame : MonoBehaviour
{
    public GameObject endgameScreenPrefab; // Assign a Canvas prefab with a TextMeshProUGUI child
    public Sprite victorySprite;
    public Sprite gameOverSprite;

    // Prevents duplicate screens, retaining endgame screen visual transparency
    private bool screenShown = false;

    void Update()
    {
        if (!screenShown && ((GameManager.Instance != null && GameManager.Instance.GameIsDone()) || Health.health <= 0))
        {
            Debug.Log("Game Over Condition Met");
            ShowEndgameScreen();
            screenShown = true;
        }
    }

    void ShowEndgameScreen()
    {
        if (endgameScreenPrefab != null)
        {
            GameObject screen = Instantiate(endgameScreenPrefab);

            Transform titleTransform = screen.transform.Find("TitleText");
            if (titleTransform != null)
            {
                TextMeshProUGUI title = titleTransform.GetComponent<TextMeshProUGUI>();
                if (title != null)
                {
                    title.text = (Health.health > 0) ? "VICTORY" : "GAME OVER";
                }
            }

            // Set the result sprite based on win/lose
            Transform resultSpriteTransform = screen.transform.Find("ResultSprite");
            if (resultSpriteTransform != null)
            {
                var image = resultSpriteTransform.GetComponent<UnityEngine.UI.Image>();
                if (image != null)
                {
                    image.sprite = (Health.health > 0) ? victorySprite : gameOverSprite;
                }
            }

            // Find the Play Again button and add listener
            Transform playAgainTransform = screen.transform.Find("PlayAgainButton");
            if (playAgainTransform != null)
            {
                var button = playAgainTransform.GetComponent<UnityEngine.UI.Button>();
                if (button != null)
                {
                    button.onClick.AddListener(RestartGame);
                }
            }
        }
    }

    public void RestartGame()
    {
        // Make sure to have the right scene name
        SceneManager.LoadScene("GameCopy1");
    }
}
