using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform Player;
    public LevelGenerator LevelGen;

    public GameObject game_over_screen;
    private bool gameHasEnded = false;
    private float restart_delay = 2.0f;
    private float death_player_platform_distance = 10.0f;

    private void Start()
    {
        LevelGen = GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>();
    }

    private void Update()
    {
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
