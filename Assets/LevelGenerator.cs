using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private const float PLAYER_DIST_TO_SPAWN = 30f;
    [SerializeField] private Transform level;
    [SerializeField] private Transform player;
    private Vector3 last_end;
    private int x_offset;
    private Transform ref_last_level;

    private void Awake()
    {
        ref_last_level = level;
        x_offset = Mathf.Abs(Vector3Int.FloorToInt(ref_last_level.Find("StartPosition").position).x);
    }

    private void SpawnLevel()
    {
        Vector3Int prevPos = Vector3Int.FloorToInt(ref_last_level.Find("EndPosition").position);
        last_end = ref_last_level.Find("EndPosition").position;
        Vector3Int startPos = Vector3Int.FloorToInt(ref_last_level.Find("StartPosition").position);
        prevPos.x += 2 + x_offset;
        prevPos.y = 0;
        ref_last_level = Instantiate(level, prevPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, last_end) < PLAYER_DIST_TO_SPAWN)
        {
            SpawnLevel();
        }
    }
}
