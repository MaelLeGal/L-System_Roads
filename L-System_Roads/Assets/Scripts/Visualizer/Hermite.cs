using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Hermite : MonoBehaviour
{
    public List<Transform> transforms;
    public List<Vector3> tangentes;
    List<Vector3> points;
    
    [Range(0,1)]
    public float t;

    public Material mat;
    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        /*points = transforms.Select(x => x.position).ToList();
        List<Vector3> pointsCurve = new List<Vector3>();
        for(float u=t; u <= 1; u+=0.01f){
            pointsCurve.Add(computePointAtT(u,points, tangentes));
        }

        LineRenderer lineRenderer;
        lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = mat;
        lineRenderer.material.SetColor("_Color", color);

        int lengthOfLineRenderer = pointsCurve.Count;
        lineRenderer.positionCount = lengthOfLineRenderer;

        for (int i = 0; i < pointsCurve.Count; i++)
        {
            lineRenderer.SetPosition(i, pointsCurve[i]);
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = pointsCurve[i];
            sphere.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Vector3 computePointAtT(float t, List<Vector3> points, List<Vector3> tangentes){

        Vector3 point = new Vector3(0,0,0);
        point += points[0]*((2*Mathf.Pow(t,3)) - (3*Mathf.Pow(t,2)) + 1);
        point += points[1]*((-2*Mathf.Pow(t,3)) + (3*Mathf.Pow(t,2)));
        point += tangentes[0]*((Mathf.Pow(t,3)) - (2*Mathf.Pow(t,2)) + t);
        point += tangentes[1]*((Mathf.Pow(t,3)) - (Mathf.Pow(t,2)));
        return point;
    }


}
