using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Transform Player;
    public LevelGenerator LevelGen;

    // Countdown Related Stuff
    public GameObject countdown_screen;
    public GameObject countdown_text;
    public GameObject scoreText;
    public GameObject scoreScreen;
    public float countdown_value = 3.9f;
    private bool countdown_over;


    // Game Over Screen Related Stuff
    public GameObject game_over_screen;
    private bool gameHasEnded;
    private float restart_delay = 2.0f;
    private float death_player_platform_distance = 10.0f;
    private int score;

    private void Start()
    {
        LevelGen = GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>();
        countdown_screen.SetActive(true);
        scoreScreen.SetActive(false);
    }

    private void Update()
    {
        // Countdown
        if (!countdown_over)
        {
            countdown_value -= Time.deltaTime;
            countdown_text.GetComponent<TextMeshProUGUI>().SetText(Mathf.FloorToInt(countdown_value).ToString());

            if (countdown_value <= 0.1f)
            {
                countdown_over = true;
                countdown_screen.SetActive(false);
                scoreScreen.SetActive(true);
                Player.GetComponent<PlayerMovement>().EnableMovement();
            }
        }

        // Game Over
        var min_y = double.PositiveInfinity;
        foreach (Transform platform in LevelGen.platform_references)
        {
            if (platform.position.y < min_y)
            {
                min_y = platform.position.y;
            }
        }
        
        if (IsEndgame(min_y))
        {
            EndGame();
        }

        if(countdown_over && !IsEndgame(min_y))
        {
            score += Mathf.CeilToInt(Time.deltaTime);

            scoreText.GetComponent<TextMeshProUGUI>().SetText($"Score: {score}");
        }
    }

    // TODO: Not so nice :^)
    public bool IsEndgame(double minY) => LevelGen.platform_references.Count > 1 && Player.position.y + death_player_platform_distance < minY;

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            // print("GAME OVER");
            game_over_screen.SetActive(true);
            Invoke("Restart", restart_delay);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
