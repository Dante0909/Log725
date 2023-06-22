using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid
{
    private int width;
    private int height;
    private int startX;
    private int startY;
    private int endX;
    private int endY;
    private Room startRoom;
    private Room endRoom;

    private Room[,] rooms;
    private List<Room> corridors;

    public Grid(int width, int height, int startX, int startY, int endX, int endY)
    {
        if (startX >= width || startX < 0) throw new Exception("Invalid startX position");
        if (endX >= width || endX < 0) throw new Exception("Invalid endX position");
        if (startX == endX && startY == endY) throw new Exception("Start position same as end position");

        this.startX = startX;
        this.startY = startY;
        this.endX = endX;
        this.endY = endY;
        this.width = width;
        this.height = height;

        CreateGridRoom();
        //add levers
        //corridors = CreateCorridors();
    }

    private void CreateGridRoom()
    {
        rooms = new Room[width, height];

        float emptyCounter;
        bool neighbourFlag;
        do
        {
            neighbourFlag = false;
            emptyCounter = 0;
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    Room r = null;
                    if (startX == i && startY == j)
                    {
                        startRoom = r = new Room(i, j);
                    }
                    else if (endX == i && endY == j)
                    {
                        endRoom = r = new Room(i, j);
                    }
                    else
                    {
                        if (Random.Range(0, 3) == 0)
                        {
                            emptyCounter++;
                        }
                        else
                        {
                            r = new Room(i, j);
                        }
                    }
                    rooms[i, j] = r;
                }
            }

            
            foreach (Room room in rooms)
            {
                room?.SetNeighbours(this);
                if (room?.ContainsNeighbours() == false)
                {
                    neighbourFlag = true;
                    break;
                }
            }

        } while (emptyCounter / (width * height) > 0.3f || neighbourFlag);
    }

    private List<Corridor> CreateCorridors()
    {
        List<Corridor> corrs = new List<Corridor>();



        return corrs;
    }

    public Room GetRoom(int x, int y)
    {
        if (x < width && x >= 0 && y < height && y >= 0)
        {
            return rooms[x, y];
        }

        return null;
    }

    public Room[,] GetRooms()
    {
        return rooms;
    }

    public Room GetStartRoom()
    {
        return startRoom;
    }

    public Room GetEndRoom()
    {
        return endRoom;
    }
}
