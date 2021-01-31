using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

public class Formation : MonoBehaviour
{

    public int gridSizeX = 10;
    public int gridSizeY = 2;

    public float gridOffsetX = 1f;
    public float gridOffsetY = 1f;

    public int divider = 4;

    public List<Vector3> gridList = new List<Vector3>();

    //move formation 
    public float maxMoveOffsetX = 5;

    float curPosx; //moving position 
    Vector3 startPosition;


    [Range(0,20)]
    public float speed = 3;
    public int direction;

    //SPREADING
    bool isSpreading;
    bool spreadStarted;

    float spreadAmmount = 1f;
    float currentSpread;
    float spreadSpeed = 0.5f;
    int spreadDirection = 1;

    //DIVING
    [Header("Diving")]
    public float minDiveTime = 3;
    public float maxDiveTime = 10;
    
    public bool canDive;
    public List<GameObject> divePathList = new List<GameObject>();





    [HideInInspector]public List<EnemyFormation> enemyList = new List<EnemyFormation>();

    [System.Serializable]
    public class EnemyFormation
    {
        public int index;
        public float xPos;
        public float zPos;
        public GameObject enemy;

        public Vector3 goal;
        public Vector3 start;

        public EnemyFormation (int _index, float _xPos, float _zPos, GameObject _enemy)
        {
            index = _index;
            xPos = _xPos;
            zPos = _zPos;
            enemy = _enemy;

            start = new Vector3(_xPos, 0, _zPos);
            goal = new Vector3(_xPos + (_xPos * 0.3f), 0, _zPos);
        }
    }




    public void Start()
    {
        startPosition = transform.position;
        curPosx = transform.position.x;
        CreateGrid();
    }



    public void Update()
    {
        if (!isSpreading && !spreadStarted)
        {

            curPosx += Time.deltaTime * speed * direction;
            if (curPosx >= maxMoveOffsetX)
            {
                direction *= -1;
                curPosx = maxMoveOffsetX;
            }
            else if (curPosx <= -maxMoveOffsetX)
            {
                direction *= -1;
                curPosx = -maxMoveOffsetX;
            }

            transform.position = new Vector3(curPosx, startPosition.y, startPosition.z);
        }

        if (isSpreading)
        {
            currentSpread += Time.deltaTime * spreadDirection * spreadSpeed;
            if(currentSpread >= spreadAmmount || currentSpread <= 0)
            {
                spreadDirection *= -1;
            }

            for (int i = 0; i < enemyList.Count; i++)
            {
                if(Vector3.Distance(enemyList[i].enemy.transform.position, enemyList[i].goal )>= 0.001f)
                {
                    enemyList[i].enemy.transform.position = Vector3.Lerp(transform.position + enemyList[i].start, 
                        transform.position + enemyList[i].goal, currentSpread);
                }
            }
        }
        if (canDive)
        {
            if (Input.GetMouseButtonDown(2))
            {
                SetDiving();
            }
        }
        //if (canDive)
        //{
        //    Invoke("SetDiving", Random.Range(minDiveTime, maxDiveTime));
        //    canDive = false;
        //}
    }

    public IEnumerator ActivateSpread()
    {
        if (spreadStarted)
        {
            yield break;
        }

        spreadStarted = true;
        while(transform.position.x != startPosition.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
            yield return null;
        }
        if (canDive)
        {
            SetDiving();
        }
        isSpreading = true;
        //canDive = true;
        Invoke("SetDiving", Random.Range(minDiveTime, maxDiveTime));
    }

    //   public void OnDrawGizmos()
    //   {
    //       gridList.Clear();
    //
    //       int num = 0;
    //
    //       for (int i = 0; i < gridSizeX; i++)
    //       {
    //           for (int j = 0; j < gridSizeY; j++)
    //           {
    //               float x = (gridOffsetX + gridOffsetX * 2*(num/divider )) * Mathf.Pow(-1, num%2+1);
    //               float y = gridOffsetY * ((num%divider)/2);
    //
    //               Vector3 vec = new Vector3(this.transform.position.x + x, 0, this.transform.position.z + y);
    //
    //               //visalise grid
    //               Handles.Label(vec, num.ToString());
    //               num++;
    //
    //
    //               gridList.Add(vec);
    //           }
    //       }
    //   }

    public void OnDrawGizmos()
    {
        int num = 0;
        CreateGrid();

        foreach(Vector3 pos in gridList)
        {
            Gizmos.DrawWireSphere(GetVector(num), 0.3f);
            num++;
        }
    }

    void CreateGrid()
    {
        gridList.Clear();

        int num = 0;

        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                float x = (gridOffsetX + gridOffsetX * 2 * (num / divider)) * Mathf.Pow(-1, num % 2 + 1);
                float z = gridOffsetY * ((num % divider) / 2);

                Vector3 vec = new Vector3(x, 0, z);

                //visalise grid
                //Handles.Label(vec, num.ToString());
                num++;


                gridList.Add(vec);
            }
        }
    }

    public Vector3 GetVector(int ID)
    {
        return transform.position + gridList[ID];
    }

    public void SetDiving()
    {
        if(enemyList.Count > 0)
        {
            int choosenPath = Random.Range(0, divePathList.Count);
            int choosenEnemy = Random.Range(0, enemyList.Count);

            GameObject newPath = Instantiate(divePathList[choosenPath],
                enemyList[choosenEnemy].start + transform.position, Quaternion.identity) as GameObject;

            enemyList[choosenEnemy].enemy.GetComponent<NewEnemyBeh>().DiveSetup(newPath.GetComponent<Path>());
            enemyList.RemoveAt(choosenEnemy);
            Invoke("SetDiving", Random.Range(minDiveTime,maxDiveTime));
        }else
        {
            CancelInvoke("SetDiving");
            return;
        }
    }


}
