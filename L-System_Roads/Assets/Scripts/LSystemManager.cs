using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                

                
                
            }
        }



    }

}
