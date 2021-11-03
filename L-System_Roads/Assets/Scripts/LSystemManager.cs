using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LSystemManager : MonoBehaviour
{
    /*
    The number of LSystem that will be created
    */
    [Range(1,4)]
    public int nbLSystem;

    /*
    The size of the grid (width and height)
    */
    public int sizeGrid;

    /*
    More the number of squares is higher, more the simplification of nodes will be impactful
    */
    public int nbSquares;

    /*
    The list of all primary LSystems
    */
    public List<LSystemGenerator> list_LSystem;

    /*
    Visualizer for drawing LSystems
    */
    public SimpleVisualizer visualizer;

    /*
    The list of Cell 
    */
    private List<Cell> cells = new List<Cell>();


    // Start is called before the first frame update
    void Start()
    {
        PlaceLSystem();
        Debug.Log("End Placement L-System");
        StartGenerateLSystem();
        Debug.Log("End Generation");
        MergeLSystem();
        //Debug.Log("End Merge");

        foreach(var l in list_LSystem)
        {
            visualizer.VisualizeSequence(l);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaceLSystem(){
        
        float boundPos = 5 * this.transform.GetChild(0).transform.localScale.x; //The coordinate at the edges of the ground are 5 times the scale of it
        
        if(nbLSystem > list_LSystem.Count){
            nbLSystem = list_LSystem.Count;
        }

        for(int i = 0; i < nbLSystem; i++){

            Vector3 newPos;
            Vector3 newDirection;
            switch(i){
                case 0: // Left
                    newPos = new Vector3(-boundPos,0,Random.Range(0,boundPos));
                    newDirection = Vector3.left;
                    break;
                case 1: // Right
                    newPos = new Vector3(boundPos,0,Random.Range(0,boundPos));
                    newDirection = Vector3.right;
                    break;
                case 2: // Top
                    newPos = new Vector3(Random.Range(0,boundPos),0,boundPos);
                    newDirection = Vector3.forward;
                    break;
                case 3: // Bottom
                    newPos = new Vector3(Random.Range(0,boundPos),0,-boundPos);
                    newDirection = Vector3.back;
                    break;
                default: // Center
                    newPos = new Vector3(0,0,0);
                    newDirection = Vector3.forward;
                    break;
            }
            Debug.Log(newPos);
            Debug.Log(newDirection);
            list_LSystem[i].Position = newPos;
            list_LSystem[i].PrimaryDirection = newDirection;
        }
    }


    /*
    Start the visualization of all the LSystems
    */
    public void StartVisualizer()
    {
        //visualizer.DrawLine(); //TODO
    }

    /*
    Start the generation of all the primary LSystems
    */
    public void StartGenerateLSystem()
    {
        foreach(var lsystem in list_LSystem)
        {
            lsystem.GenerateNetwork();
        }
    }

    /*
    Merge points of the LSystems if they are close to each other
    */
    public void MergeLSystem()
    {
        // foreach(var lsystem in list_LSystem)
        // {
        //     //lsystem.LSystemPointsDictionary.Keys()
        // }
        float step = sizeGrid / nbSquares; // Size of square

        float radius = sizeGrid;
        float x, z;
        for(int i = 0; i < nbSquares*2; i++){
            x = -radius + i * step;

            for(int j = 0; j < nbSquares*2; j++){
                z = radius - j * step;

                // Opposite corners of the square
                Vector3 p1 = new Vector3(x, 0, z);
                Vector3 p2 = new Vector3(x + step, 0, z - step);

                // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                // cube.transform.position = (p1+p2)/2;

                Cell cell = new Cell(cells.Count, p1, p2);

                for(int k = 0; k < list_LSystem.Count; k++)
                {
                    foreach(Vector3 point in list_LSystem[k]._LSystemPointsList)
                    {
                        if (cell.PointInCell(point))
                        {
                            cell.points.Add((point,k));
                        }
                    }
                }
                //Debug.Log(cell.points.Count);
                if(cell.points.Count > 0){
                    cell.meanPoints = cell.points.Select(tuple => tuple.Item1).Aggregate((res, val) => res + val) / cell.points.Count;

                    foreach ((Vector3,int) point in cell.points)
                    {
                        if (list_LSystem[point.Item2]._LSystemPointsList.Contains((point.Item1))){
                            int index = list_LSystem[point.Item2]._LSystemPointsList.FindIndex(p => p == point.Item1);
                            list_LSystem[point.Item2]._LSystemPointsList[index] = cell.meanPoints;
                        }
                        else
                        {
                            Debug.LogError("Point not in primary nor secondary network");
                        }
                    }
                }
                cells.Add(cell);
            }
        }



    }

}
