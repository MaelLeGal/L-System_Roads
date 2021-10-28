using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private int id;
    private Vector3 coordMin;
    private Vector3 coordMax;
    public Vector3 meanPoints;
    public List<Vector3> points = new List<Vector3>();

    public Cell(int id_, Vector3 coordMin_, Vector3 coordMax_)
    {
        id = id_;
        coordMin = coordMin_;
        coordMax = coordMax_;
    }

}
