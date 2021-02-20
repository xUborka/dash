using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Transform Player;
    [SerializeField] private LevelGenerator LevelGen;

    [Header("Score")]
    [SerializeField] private GameObject scoreScreen;
    [SerializeField] private GameObject scoreText;

    [Header("Countdown")]
    [SerializeField] private GameObject countdown_screen;
    [SerializeField] private GameObject countdown_text;
    private float countdown_value = 3.9f;
    private bool countdown_over;

    [Header("GameOver")]
    [SerializeField] private GameObject game_over_screen;
    [SerializeField] private LayerMask danger_layer;
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
                Player.GetComponent<PlayerMovement>().SetMovement(true);
            }
        }

        // Game Over by spikes
        Transform gcheck = Player.Find("GroundCheck"); // HACK ?
        var colliders = Physics2D.OverlapCircleAll(gcheck.position, 0.2f, danger_layer);
        if (colliders.Length > 0){
            Player.GetComponent<PlayerMovement>().SetMovement(false);
            Player.GetComponent<PlayerMovement>().KillPlayer();
            EndGame(); //
        }

        // Game Over by falling
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
            game_over_screen.SetActive(true);
            scoreScreen.SetActive(false);
            Invoke("Restart", restart_delay);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
