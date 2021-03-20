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
    private Transform parent;

    public List<Transform> platform_references;

    private void Awake()
    {
        PLAYER_DIST_TO_SPAWN = 15;
        _refLastLevel = start_level;
        platform_references.Add(_refLastLevel);
        parent = GameObject.Find("EndlessLevel").gameObject.transform;
    }

    private void SpawnLevel()
    {
        var prevPos = Vector3Int.FloorToInt(_refLastLevel.Find("EndPosition").position);
        _lastEnd = _refLastLevel.Find("EndPosition").position;
        var startPos = Vector3Int.FloorToInt(_refLastLevel.Find("StartPosition").position);
        _refLastLevel = Instantiate(next_levels[Random.Range(0, next_levels.Count)], prevPos, Quaternion.identity);
        _refLastLevel.transform.parent = parent;
        platform_references.Add(_refLastLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if (_lastEnd.x - player.position.x < PLAYER_DIST_TO_SPAWN)
        {
            SpawnLevel();
        }
        
        List<Transform> to_remove = new List<Transform>();
        foreach (var platform in platform_references)
        {
            Vector3 end_pos = platform.Find("EndPosition").position;
            if (player.position.x > end_pos.x + PLAYER_DIST_TO_SPAWN)
            {
                to_remove.Add(platform);
            }
        }
        foreach (var platform in to_remove){
            platform_references.Remove(platform);
            Destroy(platform.gameObject);
        }
    }
}
