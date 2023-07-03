using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding
{
    private Grid grid;
    private Room startRoom;
    private Room trueEndRoom;
    private List<Room> openList;
    private List<Room> closedList;

    public Pathfinding(Grid grid)
    {
        this.grid = grid;
        this.startRoom = grid.GetStartRoom();
        this.trueEndRoom = grid.GetEndRoom();
    }

    public bool VerifyInitialGrid()
    {
        foreach (Room r in grid.GetRooms())
        {
            if (r is null) continue;
            if(r != grid.GetStartRoom() && !r.IsChecked)
            {
                List<Room> path = FindPathToStart(r);
                if (path is null) return false;
            }
        }

        return true;
    }

    private List<Room> FindPathToStart(Room endNode)
    {
        openList = new List<Room>() { startRoom };
        closedList = new List<Room>();

        var rooms = grid.GetRooms();
        for (int i = 0; i < rooms.GetLength(0); ++i)
        {
            for (int j = 0; j < rooms.GetLength(1); ++j)
            {
                rooms[i, j]?.ResetGCost();
            }
        }

        startRoom.Gcost = 0;
        startRoom.Hcost = CalculateDistanceCost(startRoom, endNode);

        while (openList.Count > 0)
        {
            Room cur = GetLowestCostFNode(openList);
            if (cur == endNode)
            {
                return CalculatePath(endNode);
            }
            openList.Remove(cur);
            closedList.Add(cur);
            foreach (Room r in cur.GetNeighbours().Values)
            {
                if (closedList.Contains(r)) continue;
                
                int tentativeCost = cur.Gcost + CalculateDistanceCost(cur, r);
                if (tentativeCost < r.Gcost)
                {
                    r.SetCFN(cur);
                    r.Gcost = tentativeCost;
                    r.Hcost = CalculateDistanceCost(r, endNode);

                    if (!openList.Contains(r)) openList.Add(r);
                }

            }
        }
        return null;
    }
    private List<Room> CalculatePath(Room endRoom)
    {
        List<Room> path = new List<Room>();
        path.Add(endRoom);
        var cur = endRoom;

        while (cur.GetCFN() is not null)
        {
            grid.AddCorridor(cur.GetCFN(), cur);
            cur.IsChecked = true;
            path.Add(cur.GetCFN());
            cur = cur.GetCFN();
        }

        //deaden
        if (endRoom.GetNeighbours().Count == 1 && endRoom != trueEndRoom)
        {
            endRoom.DistanceFromStart = path.Count;
        }
        
        path.Reverse();
        
        return path;
    }

    private int CalculateDistanceCost(Room a, Room b)
    {
        return Mathf.RoundToInt(Mathf.Sqrt((a.X - b.X) ^ 2 + (a.Y - b.Y) ^ 2));
    }

    private Room GetLowestCostFNode(List<Room> roomList)
    {
        int min = roomList.Min(x => x.Fcost);
        return roomList.First(x => x.Fcost == min);
    }
}
