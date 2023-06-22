using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corridor
{
    private Room roomA = null;

    private Room roomB = null;

    public Room Traverse(Room r)
    {
        return roomA == r ? roomB : roomA;
    }

    public Corridor(Room a, Room b)
    {
        roomA = a;
        roomB = b;

        roomA.SetCorridor(b, this);
        roomB.SetCorridor(a, this);

    }

    public Room GetRoomA()
    {
        return roomA;
    }
    public Room GetRoomB()
    {
        return roomB;
    }
}
