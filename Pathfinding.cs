using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;


using Random = UnityEngine.Random;

public class Pathfinding : MonoBehaviour
{

    List<Node> path = new List<Node>();

    public LayerMask Destroyable;

    int D = 10;

    public Grid grid;

    

    [SerializeField] GameObject enemyGoal;
    [SerializeField] Transform secondTarget;
    [SerializeField] Transform thirdTarget;
    [SerializeField] Transform fourthTarget;

    //public Transform[] currentTargets;

    public float speed = 2;

    [Range(1, 3)]
    public int goalTarget;

    Vector3 nextNode, whereToGO;

    Vector3 up = Vector3.zero, right = new Vector3(0, 90, 0), down = new Vector3(0, 180, 0), left = new Vector3(0, 270, 0), currentDirection = Vector3.zero;

    Node lastVisitedNode;

    //public bool isSecond;

    private void Awake()
    {
        //grid = GetComponent<Grid>();
    }

    private void Start()
    {

        whereToGO = transform.position;
    }


    private void Update()
    {


        Movement();


    }

    void FindThePath()
    {
        Node startNode = grid.NodeRequest(this.gameObject.transform.position);/// current pos        
        Node goalNode = grid.NodeRequest(enemyGoal.transform.position);/// bomber  pos}




        List<Node> openList = new List<Node>();
        List<Node> closeList = new List<Node>();

        openList.Add(startNode);


        while (openList.Count > 0)
        {
            Node currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[1];
                }
            }
            openList.Remove(currentNode);
            closeList.Add(currentNode);

            //goal has been found

            if (currentNode == goalNode)
            {
                //get path before exit;

                PathTracer(startNode, goalNode);


                return;

            }
            // check all neighbours nodes 
            foreach (Node neighbour in grid.GetNeighbourNode(currentNode))
            {
                if (!neighbour.walkable || closeList.Contains(neighbour))// || neighbour == lastVisitedNode)
                {
                    continue;
                }

                int calcMoveCost = currentNode.gCost + GetDistance(currentNode, neighbour);

                if (calcMoveCost < neighbour.gCost || !openList.Contains(neighbour))
                {
                    neighbour.gCost = calcMoveCost;
                    neighbour.hCost = GetDistance(neighbour, goalNode);

                    neighbour.parentNode = currentNode;
                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }
    }

    void PathTracer(Node startNode, Node goalNode)
    {
        lastVisitedNode = startNode;
        path.Clear();

        //List<Node> path = new List<Node>();
        Node currentNode = goalNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        path.Reverse();
        grid.path = path;
    }

    int GetDistance(Node a, Node b)
    {
        int distX = Mathf.Abs(a.positionX - b.positionX);
        int distZ = Mathf.Abs(a.positionZ - b.positionZ);

        return D * (distX + distZ);
    }

    //public void RandomTarget()
    //{
    //    var cT = Random.Range(0, currentTargets.Length);
    //    
    //}


    void Movement()
    {

        transform.position = Vector3.MoveTowards(transform.position, whereToGO, speed * Time.deltaTime);


        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.forward, Color.green);
        if (Vector3.Distance(transform.position, whereToGO) < 0.00001f)
        {
            FindThePath();


            if (path.Count > 0)
            {
                //transform.rotation = Quaternion.LookRotation(nextPos);
                transform.rotation = Quaternion.LookRotation(currentDirection);
                //DESTINATION
                nextNode = grid.NextPathPoint(path[0]);
                whereToGO = nextNode;

                //ROTATION ONLY!!!
                SetDirection();
                transform.localEulerAngles = currentDirection;
            }
        }

    }

    private void SetDirection()
    {
        int dirX = (int)(nextNode.x - transform.position.x);
        int dirZ = (int)(nextNode.z - transform.position.z);

        //up
        if (dirX == 0 && dirZ == 0)
        {
            currentDirection = up;
        }
        //right
        if (dirX > 0 && dirZ == 0)
        {
            currentDirection = right;
        }

        else if (dirX < 0 && dirZ == 0)
        {
            currentDirection = left;
        }
        else if (dirX == 0 && dirZ < 0)
        {
            currentDirection = down;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
 
    }


    public bool Vaild()
    {
        Ray myRay = new Ray(transform.position + new Vector3(0, 0.8f, 0), transform.forward);
        RaycastHit myHit;



        if (Physics.Raycast(myRay, out myHit, 1f, Destroyable))
        {
            if (myHit.collider.tag == "Destroy")
            {
                print("gotit");
                return false;
            }


        }

        return true;
    }
}
