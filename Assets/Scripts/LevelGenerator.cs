using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private double PLAYER_DIST_TO_SPAWN;
    [SerializeField] private Transform _level;
    [SerializeField] private Transform _level2;
    [SerializeField] private Transform _player;
    private Vector3 _lastEnd;
    private Transform _refLastLevel;

    private void Awake()
    {
        PLAYER_DIST_TO_SPAWN = 15;
        _refLastLevel = _level;
    }

    private void SpawnLevel()
    {
        var prevPos = Vector3Int.FloorToInt(_refLastLevel.Find("EndPosition").position);
        _lastEnd = _refLastLevel.Find("EndPosition").position;
        var startPos = Vector3Int.FloorToInt(_refLastLevel.Find("StartPosition").position);
        _refLastLevel = Instantiate(Random.Range(0.0f, 1.0f) < 0.5 ? _level : _level2, prevPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (_lastEnd.x - _player.position.x < PLAYER_DIST_TO_SPAWN)
        {
            SpawnLevel();
        }
    }
}
