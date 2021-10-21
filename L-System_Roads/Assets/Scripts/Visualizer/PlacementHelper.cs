using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlacementHelper
{
    /*
    Find the number of neigbors for a road segment at the position passed as parameter
    */
    public static List<Direction> FindNeighbors(Vector3Int position, ICollection<Vector3Int> collection)
    {
        List<Direction> neighborsDirections = new List<Direction>();

        if(collection.Contains(position + Vector3Int.right))
        {
            neighborsDirections.Add(Direction.Right);
        }

        if(collection.Contains(position - Vector3Int.right))
        {
            neighborsDirections.Add(Direction.Left);
        }

        if(collection.Contains(position + new Vector3Int(0,0,1)))
        {
            neighborsDirections.Add(Direction.Up);
        }

        if(collection.Contains(position - new Vector3Int(0,0,1)))
        {
            neighborsDirections.Add(Direction.Down);
        }

        return neighborsDirections;
    }
}
