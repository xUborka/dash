using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawTest : MonoBehaviour
{
    public Tile HighlightTile;
    public Tilemap HighlightMap;
    private Vector3Int _previous;

    void Start()
    {
        _previous = new Vector3Int(10, -3, -1);
    }

    // do late so that the player has a chance to move in update if necessary
    private void LateUpdate()
    {
        // get current grid location
        Vector3Int currentCell = _previous;
        // add one in a direction (you'll have to change this to match your directional control)
        currentCell.x += 1;

        // if the position has changed
        if (currentCell != _previous)
        {
            // set the new tile
            HighlightMap.SetTile(currentCell, HighlightTile);

            // erase previous
            // highlightMap.SetTile(previous, null);

            // save the new position for next frame
            _previous = currentCell;
        }
    }
}
