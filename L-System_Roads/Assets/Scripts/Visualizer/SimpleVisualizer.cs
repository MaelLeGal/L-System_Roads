using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleVisualizer : MonoBehaviour
{
    /*
    An instance of L-System
    */
    public LSystemGenerator LSystem;

    /*
    A prefab to use at each end of line
    */
    public GameObject prefab;

    /*
    The line material
    */
    public Material lineMaterial;

    /*
    The position of each node of our L-System
    */
    List<Vector3> positions = new List<Vector3>();

    private void Start()
    {
        //VisualizeSequence(sequence);
    }

    /*
    Process the sequence to draw the graph/roads
    Switch between each rule of our grammar/EncodingLetters to read the sequence and act accordingly
    Call the DrawLine method to draw the roads
    */
    public void VisualizeSequence(LSystemGenerator lsystem)
    {
        Stack<AgentParameter> savePoints = new Stack<AgentParameter>();
        Vector3 currentPosition = lsystem.Position;//Vector3.zero;
        Vector3 direction = lsystem.PrimaryDirection;
        Vector3 tempPosition = lsystem.Position;//Vector3.zero;

        positions.Add(currentPosition);
        int tanValue = 5;
        foreach(char letter in lsystem.FullSentence){
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
                    DrawLine(tempPosition, currentPosition, Color.red, primaryWidth, tanValue);
                    Length -= 2; // Make the line shorter through the iterations
                    positions.Add(currentPosition);
                    break;
                case EncodingLetters.drawSecondary:
                    tempPosition = currentPosition;
                    currentPosition += (direction*length);
                    DrawLine(tempPosition, currentPosition, Color.red, secondaryWidth, tanValue);
                    Length -= 2; // Make the line shorter through the iterations
                    positions.Add(currentPosition);
                    break;
                case EncodingLetters.turnRight:
                    direction = Quaternion.AngleAxis(anglePrimary, Vector3.up)*direction;
                    tanValue = -Length*2;
                    break;
                case EncodingLetters.turnLeft:
                    direction = Quaternion.AngleAxis(-anglePrimary, Vector3.up)*direction;
                    tanValue = Length*2;
                    break;
                case EncodingLetters.turnRightSecondary:
                    direction = Quaternion.AngleAxis(angleSecondary, Vector3.up)*direction;
                    tanValue = -Length*2;
                    break;
                case EncodingLetters.turnLeftSecondary:
                    direction = Quaternion.AngleAxis(-angleSecondary, Vector3.up)*direction;
                    tanValue = Length*2;
                    break;
                default:
                    break;
            }
        }

        //foreach(Vector3 position in positions){
        //    Instantiate(prefab, position, Quaternion.identity);
        //}
    }*/

    /*
    Create a line renderer and a line game object between a start and an end
    */
    /*private void DrawLine(Vector3 start, Vector3 end, Color color, float width, int tanValue){

        List<Vector3> pointsCurve = new List<Vector3>();
        //int tanValue = 5;
        //if(index%2 == 0){
        //    tanValue = -tanValue;
        //}
        List<Vector3> tangentes = new List<Vector3>{new Vector3(tanValue,0,0), new Vector3(-tanValue,0,0)};
        for(float t=0; t <= 1; t+=0.01f){
            pointsCurve.Add(Hermite.computePointAtT(t,new List<Vector3>{start,end}, tangentes));
        }

        GameObject line = new GameObject("line");
        line.transform.position = start;
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        //lineRenderer.SetPosition(0,start);
        //lineRenderer.SetPosition(1,end);

        int lengthOfLineRenderer = pointsCurve.Count;
        lineRenderer.positionCount = lengthOfLineRenderer;

        for (int i = 0; i < pointsCurve.Count; i++)
        {
            lineRenderer.SetPosition(i, pointsCurve[i]);
        }
    }*/
}
