using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;

public class Room
{
    private Dictionary<Cardinal, Room> neighbours = new Dictionary<Cardinal, Room>();
    private Dictionary<Cardinal, Corridor> corridors = new Dictionary<Cardinal, Corridor>();
    private List<Room> pathToStart;

    //default to 0, is set when calculating path
    public int DistanceFromStart { get; set; } = 0;
    public bool ContainsKey { get; set; } = false;

    public int X { get; private set; }
    public int Y { get; private set; }
    private Vector3 position;

    private Room cameFromNode;
    public int Fcost
    {
        get => Gcost * Hcost;
    }
    public int Hcost { get; set; }
    public int Gcost { get; set; }
    public void ResetGCost()
    {
        Gcost = int.MaxValue;
        cameFromNode = null;
    }
    public Room GetCFN()
    {
        return cameFromNode;
    }
    public void SetCFN(Room cfn)
    {
        cameFromNode = cfn;
    }
    public bool IsChecked { get; set; } = false;

    public Room(int x, int y)
    {
        this.X = x;
        this.Y = y;
        position = new Vector3(X, 0, Y);
    }

    //Neighbours represent adjacent rooms, they are not necessarily connected with a corridor
    public void SetNeighbours(Grid g)
    {
        Room r = g.GetRoom(X - 1, Y);
        if(r is not null) neighbours.Add(Cardinal.Left, r);

        r = g.GetRoom(X + 1, Y);
        if (r is not null) neighbours.Add(Cardinal.Right, r);

        r = g.GetRoom(X, Y - 1);
        if (r is not null) neighbours.Add(Cardinal.Bottom, r);

        r = g.GetRoom(X, Y + 1);
        if (r is not null) neighbours.Add(Cardinal.Top, r);
    }

    public void SetCorridor(Room adjacent, Corridor corridor)
    {
        Cardinal c = neighbours.First(x => x.Value == adjacent).Key;
        corridors.TryAdd(c, corridor);
    }

    public Dictionary<Cardinal, Room> GetNeighbours()
    {
        return neighbours;
    }

    public Dictionary<Cardinal, Corridor> GetCorridors()
    {
        return corridors;
    }
    public bool ContainsNeighbours()
    {
        return neighbours.Count != 0;
    }

    public Vector3 GetPosition()
    {
        return position;
    }
    public enum Cardinal
    {
        Left,
        Right,
        Top,
        Bottom
    }
}
