using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
    /*
    An instance of L-System
    */
    public LSystemGenerator LSystem;

    /*
    A Road helper instance that will contain the different road used and place them
    */
    public RoadHelper roadHelper;

    /*
    The length of lines
    */
    [SerializeField]
    private int length = 8;
    public int Length {get => length > 0 ? length : 1; set => length = value;}

    /*
    The angle between the lines at a node
    */
    [SerializeField]
    private float angle = 20;
    public float Angle {get => angle; set => angle = value;}

    /*
    The position of each node of our L-System
    */
    List<Vector3> positions = new List<Vector3>();

    private void Start()
    {
        string sequence = LSystem.GenerateSentence();
        VisualizeSequence(sequence);
    }

    /*
    Process the sequence to draw the graph/roads
    Switch between each rule of our grammar/EncodingLetters to read the sequence and act accordingly
    Call the PlaceStreetAtPosition method to draw the roads
    Call the FixRoad to change the road according to the number of neighbors
    */
    private void VisualizeSequence(string sequence)
    {
        Stack<AgentParameter> savePoints = new Stack<AgentParameter>();
        Vector3 currentPosition = Vector3.zero;
        Vector3 direction = Vector3.forward;
        Vector3 tempPosition = Vector3.zero;

        positions.Add(currentPosition);

        foreach(char letter in sequence){
            EncodingLetters encoding = (EncodingLetters)letter;
            switch(encoding){
                case EncodingLetters.unknown:
                    break;
                case EncodingLetters.save:
                    savePoints.Push(new AgentParameter{position = currentPosition, direction = direction, length=Length});
                    break;
                case EncodingLetters.load:
                    if(savePoints.Count > 0){
                        AgentParameter ap = savePoints.Pop();
                        currentPosition = ap.position;
                        direction = ap.direction;
                        Length = ap.length;
                    }else{
                        throw new System.Exception("No point saved in Stack");
                    }
                    break;
                case EncodingLetters.draw:
                    tempPosition = currentPosition;
                    currentPosition += (direction*length);
                    roadHelper.PlaceStreetAtPosition(tempPosition, Vector3Int.RoundToInt(direction), length, angle);
                    Length -= 2; // Make the line shorter through the iterations
                    positions.Add(currentPosition);
                    break;
                case EncodingLetters.turnRight:
                    direction = Quaternion.AngleAxis(angle, Vector3.up)*direction;
                    break;
                case EncodingLetters.turnLeft:
                    direction = Quaternion.AngleAxis(-angle, Vector3.up)*direction;
                    break;
                default:
                    break;
            }
        }
        roadHelper.FixRoad();
    }
}
