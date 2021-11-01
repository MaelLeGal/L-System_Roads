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
        


    }

    // Update is called once per frame
    void Update()
    {
        
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
            lsystem.GeneratePrimaryNetwork();
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

        float radius = 1;
        float x, y;
        for(int i = 0; i < nbSquares; i++){
            x = -radius + i * step;

            for(int j = 0; j < nbSquares; j++){
                y = radius - j * step;

                // Opposite corners of the square
                Vector3 p1 = new Vector3(x, y, 0);
                Vector3 p2 = new Vector3(x + step, y - step, 0);

                Cell cell = new Cell(cells.Count, p1, p2);

                for(int k = 0; k < list_LSystem.Count; k++)
                {
                    foreach(Vector3 point in list_LSystem[k].LSystemPointsDictionary.SelectMany(d => d.Value).ToList())
                    {
                        if (cell.PointInCell(point))
                        {
                            cell.points.Add((point,k));
                        }
                    }
                }
                cell.meanPoints = cell.points.Select(tuple => tuple.Item1).Aggregate((res, val) => res + val) / cell.points.Count;

                List<Vector3> primaryPoints;
                List<Vector3> secondaryPoints;
                foreach ((Vector3,int) point in cell.points)
                {
                    primaryPoints = list_LSystem[point.Item2].LSystemPointsDictionary["PRIMARY"];
                    secondaryPoints = list_LSystem[point.Item2].LSystemPointsDictionary["SECONDARY"];
                    if (primaryPoints.Contains((point.Item1))){
                        int index = primaryPoints.FindIndex(p => p == point.Item1);
                        primaryPoints[index] = cell.meanPoints;
                        list_LSystem[point.Item2].LSystemPointsDictionary["PRIMARY"] = primaryPoints;
                    }
                    else if (secondaryPoints.Contains((point.Item1))){
                        int index = secondaryPoints.FindIndex(p => p == point.Item1);
                        secondaryPoints[index] = cell.meanPoints;
                        list_LSystem[point.Item2].LSystemPointsDictionary["SECONDARY"] = secondaryPoints;
                    }
                    else
                    {
                        Debug.LogError("Point not in primary nor secondary network");
                    }
                }
                cells.Add(cell);
            }
        }



    }

}
