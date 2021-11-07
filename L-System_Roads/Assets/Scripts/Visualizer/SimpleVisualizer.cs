using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;

[CreateAssetMenu(fileName = "L-System", menuName = "L-System_Roads/Visualizer", order = 2)]
public class SimpleVisualizer : ScriptableObject
{
    /*
    The line material
    */
    //public Material lineMaterial;

    /*
    The position of each node of our L-System
    */
    List<Vector3> positions = new List<Vector3>();

    /*
    Width of the primary roads
    */
    public float primaryWidth;

    /*
    Width of the secondary roads
    */
    public float secondaryWidth;

    /*
    Color of the primary roads
    */
    public Color primaryColor;

    /*
    Width of the secondary roads
    */
    public Color secondaryColor;

    /*
    Process the sequence to draw the graph/roads
    Switch between each rule of our grammar/EncodingLetters to read the sequence and act accordingly
    Call the DrawLine method to draw the roads
    */
    public void VisualizeSequence(LSystemGenerator lsystem)
    {
        Stack<AgentParameter> savePoints = new Stack<AgentParameter>();
        Vector3Int currentPosition = lsystem.Position;//Vector3.zero;
        Vector3Int direction = lsystem.PrimaryDirection;
        Vector3Int tempPosition = lsystem.Position;//Vector3.zero;
        int cpt = 1;

        // First node
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = currentPosition;

        foreach (char letter in lsystem.FullSentence)
        {
            EncodingLetters encoding = (EncodingLetters)letter;
            switch (encoding)
            {
                case EncodingLetters.unknown:
                    break;
                case EncodingLetters.save:
                    savePoints.Push(new AgentParameter { position = currentPosition, direction = direction, length = lsystem.lengthPrimary });
                    break;
                case EncodingLetters.load:
                    if (savePoints.Count > 0)
                    {
                        AgentParameter ap = savePoints.Pop();
                        currentPosition = ap.position;
                        direction = ap.direction;
                        lsystem.lengthPrimary = ap.length;
                    }
                    else
                    {
                        throw new System.Exception("No point saved in Stack");
                    }
                    break;
                case EncodingLetters.draw:
                    tempPosition = currentPosition;
                    currentPosition = (lsystem._LSystemPointsList[cpt]);
                    DrawLine(tempPosition, currentPosition, primaryColor, primaryWidth);
                    GameObject sphere1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere1.transform.position = currentPosition;
                    cpt++;
                    break;
                case EncodingLetters.drawSecondary:
                    tempPosition = currentPosition;
                    currentPosition = (lsystem._LSystemPointsList[cpt]);
                    DrawLine(tempPosition, currentPosition, secondaryColor, secondaryWidth);
                    GameObject sphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere2.transform.position = currentPosition;
                    cpt++;
                    break;
                default:
                    break;
            }
            //cpt++;
        }
    }

    /*
    Create a line renderer and a line game object between a start and an end
    */
    private void DrawLine(Vector3 start, Vector3 end, Color color, float width)
    {
        GameObject line = new GameObject("line");
        line.transform.position = start;
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material = GetDefaultMaterial();
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }


    MethodInfo getBuiltinExtraResourcesMethod;
    Material GetDefaultMaterial()
    {
        if (getBuiltinExtraResourcesMethod == null)
        {
            BindingFlags bfs = BindingFlags.NonPublic | BindingFlags.Static;
            getBuiltinExtraResourcesMethod = typeof(EditorGUIUtility).GetMethod("GetBuiltinExtraResource", bfs);
        }
        return (Material)getBuiltinExtraResourcesMethod.Invoke(null, new object[] { typeof(Material), "Default-Line.mat" });
    }



    /*public void VisualizeSequence(LSystemGenerator lsystem)
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

}
