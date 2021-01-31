using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public Color pathColor = Color.green;

    Transform[] objArray;

    int overload;

    [Range(1, 20)] public int lineDensity = 1;

    public List<Transform> pathObjList = new List<Transform>();

    public List<Vector3> bezierObjList = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        //straight path
        Gizmos.color = pathColor;
        //fill aray
        objArray = GetComponentsInChildren<Transform>();
        //clearobj
        pathObjList.Clear();

        foreach(Transform obj in objArray)
        {
            if(obj != this.transform)
            {
                pathObjList.Add(obj);
            }
        }
        //draw object
        for(int i=0; i < pathObjList.Count; i++)
        {
            Vector3 position = pathObjList[i].position;
            if (i > 0)
            {
                Vector3 previous = pathObjList[i - 1].position;
                Gizmos.DrawLine(previous, position);
                Gizmos.DrawWireSphere(position, 0.3f);
            }
        }




        if(pathObjList.Count % 2 == 0)
        {
            pathObjList.Add(pathObjList[pathObjList.Count - 1]);
            overload = 2;
        }else
        {
            pathObjList.Add(pathObjList[pathObjList.Count - 1]);
            pathObjList.Add(pathObjList[pathObjList.Count - 1]);
            overload = 3;
        }


        //curve crreation
        bezierObjList.Clear();

        Vector3 lineStart = pathObjList[0].position;

        for (int i = 0; i < pathObjList.Count-overload; i+=2)
        {
            for (int j = 0; j <= lineDensity; j++)
            {
                Vector3 lineEnd = GetPoint(pathObjList[i].position, pathObjList[i + 1].position, pathObjList[i + 2].position, j / (float)lineDensity);

                

                Gizmos.color = Color.red;
                Gizmos.DrawLine(lineStart, lineEnd);

                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(lineStart, 0.2f);



                lineStart = lineEnd;

                bezierObjList.Add(lineStart);
            }
        }
    }

    Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
    }

    internal static string GetDirectoryName(string path)
    {
        throw new NotImplementedException();
    }
}
