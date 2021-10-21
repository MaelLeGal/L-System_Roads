using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadHelper : MonoBehaviour
{
    /*
    The different segments of roads used
    */
    public GameObject roadStraight, roadCorner, road3way, road4way, roadEnd;

    /*
    A Dictionary with the position as Keys and the GameObject road segment as Values
    */
    Dictionary<Vector3Int, GameObject> roadDictonary = new Dictionary<Vector3Int, GameObject>();

    /*
    A HashSet with the position of each segment of road;
    */
    HashSet<Vector3Int> fixRoadCandidates = new HashSet<Vector3Int>();


    /*
    Instantiate the straight road at the position and direction passed as parameters
    Add the road segments to the dictionary and the hashset
    */
    public void PlaceStreetAtPosition(Vector3 startPos, Vector3Int direction, int length, float angle)
    {
        Quaternion rotation = Quaternion.identity;
        if(direction.x == 0){
            rotation = Quaternion.Euler(0,angle,0);
        }

        for(int i = 0; i < length; i++){
            Vector3Int position = Vector3Int.RoundToInt(startPos + direction * i);
            if(roadDictonary.ContainsKey(position)){
                continue;
            }
            GameObject road = Instantiate(roadStraight, position, rotation, transform);
            roadDictonary.Add(position, road);
            if(i==0 || i == length-1){
                fixRoadCandidates.Add(position);
            }
        }
    }

    /*
    Instantiate the different kind of road segment depending on the number of neighbors
    */
    public void FixRoad()
    {
        foreach(Vector3Int position in fixRoadCandidates)
        {
            List<Direction> neighborsDirections = PlacementHelper.FindNeighbors(position, roadDictonary.Keys);
            Quaternion rotation = Quaternion.identity;
            
            switch(neighborsDirections.Count){
                case 1:
                    Destroy(roadDictonary[position]);
                    if(neighborsDirections.Contains(Direction.Down)){
                        rotation = Quaternion.Euler(0,90,0);
                    }else if(neighborsDirections.Contains(Direction.Left)){
                        rotation = Quaternion.Euler(0,180,0);
                    }else if(neighborsDirections.Contains(Direction.Up)){
                        rotation = Quaternion.Euler(0,-90,0);
                    }
                    roadDictonary[position] = Instantiate(roadEnd, position, rotation, transform);
                    break;
                case 2:
                    if(neighborsDirections.Contains(Direction.Up) && neighborsDirections.Contains(Direction.Down)
                        || neighborsDirections.Contains(Direction.Right) && neighborsDirections.Contains(Direction.Left)){
                            continue;
                    }
                    Destroy(roadDictonary[position]);
                    if(neighborsDirections.Contains(Direction.Up) && neighborsDirections.Contains(Direction.Right)){
                        rotation = Quaternion.Euler(0,90,0);
                    }else if(neighborsDirections.Contains(Direction.Right) && neighborsDirections.Contains(Direction.Down)){
                        rotation = Quaternion.Euler(0,180,0);
                    }else if(neighborsDirections.Contains(Direction.Down) && neighborsDirections.Contains(Direction.Left)){
                        rotation = Quaternion.Euler(0,-90,0);
                    }
                    roadDictonary[position] = Instantiate(roadCorner, position, rotation, transform);
                    break;
                case 3:
                    Destroy(roadDictonary[position]);
                    if(neighborsDirections.Contains(Direction.Down) && neighborsDirections.Contains(Direction.Right) && neighborsDirections.Contains(Direction.Left)){
                        rotation = Quaternion.Euler(0,90,0);
                    }else if(neighborsDirections.Contains(Direction.Up) && neighborsDirections.Contains(Direction.Down) && neighborsDirections.Contains(Direction.Left)){
                        rotation = Quaternion.Euler(0,180,0);
                    }else if(neighborsDirections.Contains(Direction.Up) && neighborsDirections.Contains(Direction.Right) && neighborsDirections.Contains(Direction.Left)){
                        rotation = Quaternion.Euler(0,-90,0);
                    }
                    roadDictonary[position] = Instantiate(road3way, position, rotation, transform);
                    break;
                case 4:
                    Destroy(roadDictonary[position]);
                    roadDictonary[position] = Instantiate(road4way, position, rotation, transform);
                    break;
            }
        }
    }
}
