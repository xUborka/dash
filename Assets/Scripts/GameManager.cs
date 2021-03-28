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

    [Header("References")]
    private PlayerMovementInputHandler playerInputHandler;
    private CharacterController2D characterController;


    private void Start()
    {
        LevelGen = LevelGen.GetComponent<LevelGenerator>();
        playerInputHandler = Player.GetComponent<PlayerMovementInputHandler>();
        characterController = Player.GetComponent<CharacterController2D>();

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
                playerInputHandler.SetMovement(true);
            }
        }

        if (countdown_over)
        {
            score += Mathf.CeilToInt(Time.deltaTime);

            scoreText.GetComponent<TextMeshProUGUI>().SetText($"Score: {score}");
        }
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
