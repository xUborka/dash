using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private double PLAYER_DIST_TO_SPAWN;
    [SerializeField] private Transform level;
    [SerializeField] private Transform level2;
    [SerializeField] private Transform player;
    private Vector3 last_end;
    private Transform ref_last_level;

    private void Awake()
    {
        PLAYER_DIST_TO_SPAWN = 15;
        ref_last_level = level;
    }

    private void SpawnLevel()
    {
        Vector3Int prevPos = Vector3Int.FloorToInt(ref_last_level.Find("EndPosition").position);
        last_end = ref_last_level.Find("EndPosition").position;
        Vector3Int startPos = Vector3Int.FloorToInt(ref_last_level.Find("StartPosition").position);
        if (Random.Range(0.0f, 1.0f) < 0.5){
            ref_last_level = Instantiate(level, prevPos, Quaternion.identity);
        } else {
            ref_last_level = Instantiate(level2, prevPos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (last_end.x - player.position.x < PLAYER_DIST_TO_SPAWN)
        {
            SpawnLevel();
        }
    }
}
