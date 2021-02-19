using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private double PLAYER_DIST_TO_SPAWN;
    [SerializeField] private Transform start_level;
    [SerializeField] private List<Transform> next_levels;
    [SerializeField] private Transform player;
    private Vector3 _lastEnd;
    private Transform _refLastLevel;

    public List<Transform> platform_references;

    private void Awake()
    {
        PLAYER_DIST_TO_SPAWN = 15;
        _refLastLevel = start_level;
    }

    private void SpawnLevel()
    {
        var prevPos = Vector3Int.FloorToInt(_refLastLevel.Find("EndPosition").position);
        _lastEnd = _refLastLevel.Find("EndPosition").position;
        var startPos = Vector3Int.FloorToInt(_refLastLevel.Find("StartPosition").position);
        _refLastLevel = Instantiate(next_levels[Random.Range(0, next_levels.Count)], prevPos, Quaternion.identity);
        platform_references.Add(_refLastLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if (_lastEnd.x - player.position.x < PLAYER_DIST_TO_SPAWN)
        {
            SpawnLevel();
        }
    }
}
