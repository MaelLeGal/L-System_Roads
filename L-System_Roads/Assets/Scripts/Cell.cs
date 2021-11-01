using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private int id;
    private Vector3 coordMin;
    private Vector3 coordMax;
    public Vector3 meanPoints;
    public List<(Vector3, int)> points = new List<(Vector3,int)>();

    public Cell(int id_, Vector3 coordMin_, Vector3 coordMax_)
    {
        id = id_;
        coordMin = coordMin_;
        coordMax = coordMax_;
    }

    public bool PointInCell(Vector3 point)
    {
        return (point.x >= coordMin.x
            && point.x <= coordMax.x
            && point.y <= coordMin.y
            && point.y >= coordMax.y
            && point.z >= coordMax.z
            && point.z <= coordMin.z);
    }

}
