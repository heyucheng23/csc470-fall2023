using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    public int amountX;
    public int amountY;
    public GameObject oj;
    [HideInInspector]
    public GameObject[,] spotObjs;
    //public GameObject canvas;
    private float size;

    public float frequency;
    private float clock;
    private bool runOr;

    // Start is called before the first frame update

    void Start()
    {
       

        // Instantiate the 2D array that we will use to store the state of the
        // spotObjs.
        spotObjs = new GameObject[amountX,amountY];

        // Using nested for loops is a good way to create patterns like (0,0),
        // (0,1), (0,2), (0,3)... (2,0), (2,1), (2,2).... (5,0), (5,1)... etc.
        for (int x = 0; x < amountX; x++)
        {
            for (int y = 0; y < amountY; y++)
            {

                spotObjs[x, y] = Instantiate(Resources.Load("Spot")) as GameObject;
                spotObjs[x, y].name = "Spot" + x.ToString() + y.ToString();
                // Create a position based on x, y
                Vector3 pos = transform.position;
                float cellWidth = 1f;
                float spacing = 0.1f;
                pos.x = pos.x + x * (cellWidth + spacing);
                pos.z = pos.z + y * (cellWidth + spacing);
                spotObjs[x,y] = Instantiate(oj, pos, transform.rotation);

                // (x,y) is the index in the 2D array. Store a reference to the
                // SpotController of the instantiated object because that is the
                // object that contains the information we will be intereated in
                // (the 'alive' variable.
                //spotObjs[x, y] = cellObj.GetComponent<SpotController>();
                //spotObjs[x, y].x = x;
                //spotObjs[x, y].y = y;
                //spotObjs[x, y].alive = (Random.value < 0.2f);
            }
        }
    }
 

    // Update is called once per frame
    void Update()
    {
        if (runOr)
        {
            if (clock < frequency)
            {
                clock += Time.deltaTime;

            }
            else
            {
                clock = 0;
                RunGame();
            }
        }
    }

    public void PlayGame()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            runOr = true;
        }
    }

    public void PauseGame()
    {
        runOr = false;
    }
    public void ResetSpots()
    {
        runOr = false;
        for (int i = 0; i < amountX; i++)
        {
            for (int j = 0; j < amountX; j++)
            {
                spotObjs[i, j].GetComponent<SpotController>().state = 0;
            }
        }
    }




    
    private void RunGame()
    {
        for (int i = 0; i < amountX; i++)
        {
            for (int j = 0; j < amountY; j++)
            {
                spotObjs[i, j].GetComponent<SpotController>().count = LifeJudge(i, j);
            }
        }

        for (int i = 0; i < amountX; i++)
        {
            for (int j = 0; j < amountY; j++)
            {
                if (spotObjs[i, j].GetComponent<SpotController>().count == 3)
                {
                    if (spotObjs[i, j].GetComponent<SpotController>().state == 0)
                    {
                        spotObjs[i, j].GetComponent<SpotController>().state = 1;
                    }
                }

                else if (spotObjs[i, j].GetComponent<SpotController>().count == 2)
                {

                }
                else
                {
                    if (spotObjs[i, j].GetComponent<SpotController>().state >0)
                    {
                        spotObjs[i, j].GetComponent<SpotController>().state = 0;
                    }
                }
            }
        }
    }


    private int LifeJudge(int x, int y) // Check the number of neighbors
    {
        int count = 0;

        if (x > 0)
        {
            if (y > 0)
            {
                if (spotObjs[x - 1, y - 1].GetComponent<SpotController>().state > 0)
                {
                    count++;
                }
            }
            if(spotObjs[x - 1, y ].GetComponent<SpotController>().state > 0)
            {
                count++;
            }
            if (y < amountY - 1)
            {
                   if (spotObjs[x - 1, y +1].GetComponent<SpotController>().state > 0)
                {
                    count++;
                }
            }
        }
        if (x < amountX - 1)
        {
            if (y > 0)
            {
                if (spotObjs[x + 1, y - 1].GetComponent<SpotController>().state > 0)
                {
                    count++;
                }
            }
            if (spotObjs[x + 1, y ].GetComponent<SpotController>().state > 0)
            {
                count++;
            }
            if (y < amountY - 1)
            {
                if (spotObjs[x + 1, y + 1].GetComponent<SpotController>().state > 0)
                {
                    count++;
                }
            }

         }
        if (y > 0)
        {
            if (spotObjs[x , y -1].GetComponent<SpotController>().state > 0)
            {
                count++;
            }

        }
        if (y < amountY - 1)
        {
            if(spotObjs[x , y + 1].GetComponent<SpotController>().state > 0){
                count++;
            }
        }


        return count;
    }
}
         
