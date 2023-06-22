using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int gridHeight;
    [SerializeField] private int gridWidth;
    [SerializeField] private int sizeBetweenRooms;
    private static GridManager instance = null;
    private Grid grid;
    private Pathfinding pathfinding;

    public static GridManager Instance
    {
        get
        {
            if (!instance) instance = FindObjectOfType<GridManager>();
            return instance;
        }
    }


    public void CreateNewGrid()
    {
        System.Random newRandom = new System.Random(DateTime.Now.Millisecond);

        int startX = newRandom.Next(gridWidth);
        int startY = 0;
        int endX = newRandom.Next(gridWidth);
        int endY = gridHeight - 1;

        bool good = false;
        int safe = 0;
        do
        {
            grid = new Grid(gridWidth, gridHeight, startX, startY, endX, endY);
            pathfinding = new Pathfinding(grid);
            good = pathfinding.VerifyInitialGrid();
            safe++;
        } while (good == false && safe < 1000);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateNewGrid();
        }
    }

    void OnDrawGizmos()
    {
        if (grid is not null)
        {
            foreach (Room r in grid.GetRooms())
            {
                if (r is not null)
                {
                    if (r == grid.GetStartRoom())
                    {
                        Gizmos.color = Color.red;
                    }
                    else if (r == grid.GetEndRoom())
                    {
                        Gizmos.color = Color.black;
                    }
                    else Gizmos.color = Color.white;
                    Gizmos.DrawSphere(r.GetPosition() * sizeBetweenRooms, 1);
                }
            }
            
        }
    }
}
