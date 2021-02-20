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
    public float countdown_value = 3.9f;
    private bool countdown_over = false;


    // Game Over Screen Related Stuff
    public GameObject game_over_screen;
    private bool gameHasEnded = false;
    private float restart_delay = 2.0f;
    private float death_player_platform_distance = 10.0f;

    private void Start()
    {
        LevelGen = GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>();
        countdown_screen.SetActive(true);
    }

    private void Update()
    {
        // Countdown
        if (!countdown_over){
            countdown_value -= Time.deltaTime;
            countdown_text.GetComponent<TextMeshProUGUI>().SetText(Mathf.FloorToInt(countdown_value).ToString());

            if (countdown_value <= 0.1f){
                countdown_over = true;
                countdown_screen.SetActive(false);
                Player.GetComponent<PlayerMovement>().EnableMovement();
            }
        }


        // Game Over
        double min_y = double.PositiveInfinity;
        foreach (Transform platform in LevelGen.platform_references)
        {
            if (platform.position.y < min_y)
            {
                min_y = platform.position.y;
            }
        }
        // TODO: Not so nice :^)
        if (LevelGen.platform_references.Count > 1 && Player.position.y + death_player_platform_distance < min_y)
        {
            EndGame();
        }
    }

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

    void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
