using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Transform Player;
    [SerializeField] private LevelGenerator LevelGen;
    [SerializeField] private Camera mainCamera;

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
        var colliders = Physics2D.OverlapCircleAll(gcheck.position, 0.3f, danger_layer);
        if (colliders.Length > 0){
            Player.GetComponent<PlayerMovement>().SetMovement(false);
            Player.GetComponent<PlayerMovement>().KillPlayer();
            GameOver(); //
        }

        // Game Over by falling
        if (IsOutOfBounds())
        {
            GameOver();
        }

        if (countdown_over && !IsOutOfBounds())
        {
            score += Mathf.CeilToInt(Time.deltaTime);

            scoreText.GetComponent<TextMeshProUGUI>().SetText($"Score: {score}");
        }
    }

    // TODO: Not so nice :^)
    public bool IsOutOfBounds()
    {
        var minY = double.PositiveInfinity;
        var maxY = double.NegativeInfinity;
        foreach (var platform in LevelGen.platform_references)
        {
            if (platform.position.y < minY)
            {
                minY = platform.position.y;
            }

            if (platform.position.y > maxY)
            {
                maxY = platform.position.y;
            }
        }

        return LevelGen.platform_references.Count > 1 && Player.position.y + death_player_platform_distance < minY 
            || LevelGen.platform_references.Count > 1 && Player.position.y - death_player_platform_distance > maxY;
    }

    public void GameOver()
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
        //hack
        Physics2D.gravity = new Vector3(0f, -9.81f, 0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
