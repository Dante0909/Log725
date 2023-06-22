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

    public Corridor()
    {

    }
}
