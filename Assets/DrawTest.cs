using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawTest : MonoBehaviour
{
    public Tile highlightTile;
    public Tilemap highlightMap;
    private Vector3Int previous;

    void Start(){
        previous = new Vector3Int(10, -3, -1);
    }
 
    // do late so that the player has a chance to move in update if necessary
    private void LateUpdate()
    {
        // get current grid location
        Vector3Int currentCell = previous;
        // add one in a direction (you'll have to change this to match your directional control)
        currentCell.x += 1;
 
        // if the position has changed
        if(currentCell != previous)
        {
            // set the new tile
            highlightMap.SetTile(currentCell, highlightTile);
 
            // erase previous
            // highlightMap.SetTile(previous, null);
 
            // save the new position for next frame
            previous = currentCell;
        }
    }
}
