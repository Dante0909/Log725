using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : NetworkBehaviour
{
    [SerializeField] private int gridHeight;
    [SerializeField] private int gridWidth;
    [SerializeField] private int sizeBetweenRooms;
    [SerializeField] private int numberOfKeys;
    [SerializeField] private GameObject[] roomPrefabs;
    [SerializeField] private GameObject corridorPrefab;
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

    public Grid GetGrid() => grid;
    public int SizeBetweenRooms => sizeBetweenRooms;
    public int GridHeight => gridHeight;
    public int GridWidth => gridWidth;
    public int NumberOfKeys => numberOfKeys;

    public void CreateNewGrid()
    {
        System.Random newRandom = new System.Random(DateTime.Now.Millisecond);
        
        int startX = newRandom.Next(gridWidth);
        int startY = 0;
        int endX = newRandom.Next(gridWidth);
        int endY = gridHeight - 1;

        bool good = false;
        int safe = 0;
        List<Room> deadEnds = new List<Room>();

        do
        {
            do
            {
                grid = new Grid(gridWidth, gridHeight, startX, startY, endX, endY);
                pathfinding = new Pathfinding(grid);
                good = pathfinding.VerifyInitialGrid();
                safe++;

            } while (good == false && safe < 1000);

            //separate do while to avoid doing this step every time

            foreach (Room room in grid.GetRooms())
            {
                if (room is not null && room.DistanceFromStart != 0)
                {
                    deadEnds.Add(room);
                }

            }

        } while (numberOfKeys >= deadEnds.Count);

        deadEnds = deadEnds.OrderBy(x => x.DistanceFromStart).ToList();
        int average = Mathf.FloorToInt((float)deadEnds.Count / (float)numberOfKeys);

        for (int i = 0; i < numberOfKeys; ++i)
        {
            int index = i * average;
            if (index >= deadEnds.Count) throw new Exception("Dead end index problem");
            deadEnds[i * average].ContainsKey = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (IsHost)
        {
            CreateNewGrid();
            SpawnPhysicalRooms();
            SpawnCorridors();
        }
    }

    void SpawnCorridors()
    {
        foreach (Corridor c in grid.GetCorridors())
        {
            if(c is null) continue;
            GameObject g = Instantiate(corridorPrefab, c.GetPosition() * sizeBetweenRooms, transform.rotation);
            if (c.GetRoomA().Y != c.GetRoomB().Y) g.transform.Rotate(Vector3.up, 90);
        }
    }

    void SpawnPhysicalRooms()
    {
        Vector3 rotation = Vector3.zero;
        foreach(Room r in grid.GetRooms())
        {
            if (r is null) continue;
            GameObject g = null;
            int rotate = 0;

            var c = r.GetCorridors();
            switch (c.Count)
            {
                case 1:
                    g = Instantiate(roomPrefabs[0], new Vector3(r.X * sizeBetweenRooms, 0, r.Y * sizeBetweenRooms), transform.rotation);

                    if (c.ContainsKey(Room.Cardinal.Bottom))
                        rotate = 90;
                    else if (c.ContainsKey(Room.Cardinal.Left))
                        rotate = 180;
                    else if (c.ContainsKey(Room.Cardinal.Top))
                        rotate = 270;
                    break;
                case 2:
                    if (c.ContainsKey(Room.Cardinal.Bottom))
                    {
                        if (c.ContainsKey(Room.Cardinal.Top))
                        {
                            g = Instantiate(roomPrefabs[2], new Vector3(r.X * sizeBetweenRooms, 0, r.Y * sizeBetweenRooms), transform.rotation);
                            rotate = 90;
                        }
                        else
                        {
                            g = Instantiate(roomPrefabs[1], new Vector3(r.X * sizeBetweenRooms, 0, r.Y * sizeBetweenRooms), transform.rotation);
                            rotate = c.ContainsKey(Room.Cardinal.Right) ? 0 : 90;
                        }

                    }
                    else if (c.ContainsKey(Room.Cardinal.Top))
                    {
                        g = Instantiate(roomPrefabs[1], new Vector3(r.X * sizeBetweenRooms, 0, r.Y * sizeBetweenRooms), transform.rotation);

                        rotate = c.ContainsKey(Room.Cardinal.Right) ? 270 : 180;
                    }
                    else
                    {
                        g = Instantiate(roomPrefabs[2], new Vector3(r.X * sizeBetweenRooms, 0, r.Y * sizeBetweenRooms), transform.rotation);
                    }
                    break;
                case 3:
                    g = Instantiate(roomPrefabs[3], new Vector3(r.X * sizeBetweenRooms, 0, r.Y * sizeBetweenRooms), transform.rotation);
                    if (!c.ContainsKey(Room.Cardinal.Top))
                        rotate = 90;
                    else if (!c.ContainsKey(Room.Cardinal.Right))
                        rotate = 180;
                    else if (!c.ContainsKey(Room.Cardinal.Bottom))
                        rotate = 270;

                    break;
                        default:
                            g = Instantiate(roomPrefabs[4], new Vector3(r.X * sizeBetweenRooms, 0, r.Y * sizeBetweenRooms), transform.rotation);
                    break;
            }

            if (g is not null)
            {
                g.transform.Rotate(Vector3.up, rotate);
                if (!r.ContainsKey && r.GetCorridors().Count == 1)
                { 
                    Transform key = RecursiveChildSearch(g.transform, "Key");
                    if (key is not null) Destroy(key.gameObject);
                }
            }
        }
    }

    //stupid function to delete the key object
    private Transform RecursiveChildSearch(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName) return child;

            else
            {
                Transform t2 = RecursiveChildSearch(child, childName);
                if (t2 != null) return t2;
            }
        }

        return null;
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
                    else if (r.ContainsKey)
                    {
                        Gizmos.color = Color.blue;
                    }
                    else
                    {
                        Gizmos.color = Color.white;
                    }

                    Gizmos.DrawSphere(r.GetPosition() * sizeBetweenRooms, 1);
                }
            }

            Gizmos.color = Color.white;
            foreach (Corridor corridor in grid.GetCorridors())
            {
                Gizmos.DrawLine(corridor.GetRoomA().GetPosition() * sizeBetweenRooms, corridor.GetRoomB().GetPosition() * sizeBetweenRooms);
            }
            
        }
    }
}
