using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomData
{
    public (int x, int y) coordinates;
    public GameObject roomObject;

    public RoomData((int, int) c, GameObject go)
    {
        coordinates = c;
        roomObject = go;
    }
}
public enum OPENING_DIRECTION
{
    LEFT = 1,
    BOTTOM,
    RIGHT,
    TOP
}

public class RoomGenerator3 : MonoBehaviour
{
    public GameObject[] rooms;
    public GameObject filledRoom;

    private Queue<(int x, int y)> roomCreationQueue = new Queue<(int, int)>();
    private List<RoomData> roomDataList = new List<RoomData>();

    [Range(1, 100)]
    public int roomBranchingChance = 50;
    [Range(2, 25)]
    public int minimumRooms = 10;

    private int roomsSpawned = 0;

    private void Start()
    {
        GenerateRooms();
        CreateRoomObjects();
    }

    private void GenerateRooms()
    {
        roomsSpawned = 0;
        roomCreationQueue = new Queue<(int, int)>();
        roomDataList = new List<RoomData>();

        roomCreationQueue.Enqueue((Random.Range(-2, 3), Random.Range(-2, 3)));
        while (roomCreationQueue.Count > 0) {
            CreateRoom(roomCreationQueue.Dequeue());
            roomsSpawned++;
        }

        if(roomsSpawned < minimumRooms) {
            GenerateRooms();
        }
    }

    private void CreateRoomObjects()
    {
        for (int cx = -2; cx <= 2; ++cx) {
            for (int cy = -2; cy <= 2; ++cy) {
                Vector2 convertedCoords = new Vector2(cx * 13, cy * 13);
                if (GetIfRoomExists((cx, cy))) {
                    RoomData roomData = roomDataList.Find(room => room.coordinates == (cx, cy));
                    GameObject instance = Instantiate(roomData.roomObject, convertedCoords, Quaternion.identity);
                } else {
                    GameObject instance = Instantiate(filledRoom, convertedCoords, Quaternion.identity);
                }
            }
        }
    }

    private void CreateRoom((int x, int y) roomCoords)
    {
        // Check which directions are open
        int idx = 0;
        (int, int) leftCoord = (roomCoords.x - 1, roomCoords.y);
        (int, int) bottomCoord = (roomCoords.x, roomCoords.y - 1);
        (int, int) rightCoord = (roomCoords.x + 1, roomCoords.y);
        (int, int) topCoord = (roomCoords.x, roomCoords.y + 1);
        // Check Left
        bool createLeft = !GetIfRoomExists(leftCoord) && Random.Range(0, 100) < roomBranchingChance;
        bool requiresLeft = GetIfRoomExists(leftCoord) && GetIfOpeningNeeded(leftCoord, OPENING_DIRECTION.LEFT);
        if (createLeft || requiresLeft) {
            idx += 1;
        }
        if (createLeft && !GetIfRoomQueued(leftCoord)) {
            roomCreationQueue.Enqueue(leftCoord);
        }
        
        // Check Bottom
        bool createBottom = !GetIfRoomExists(bottomCoord) && Random.Range(0, 100) < roomBranchingChance;
        bool requiresBottom = GetIfRoomExists(bottomCoord) && GetIfOpeningNeeded(bottomCoord, OPENING_DIRECTION.BOTTOM);
        if (createBottom || requiresBottom) {
            idx += 2;
        }
        if (createBottom && !GetIfRoomQueued(bottomCoord)) {
            roomCreationQueue.Enqueue(bottomCoord);
        }
        
        // Check Right
        bool createRight = !GetIfRoomExists(rightCoord) && Random.Range(0, 100) < roomBranchingChance;
        bool requiresRight = GetIfRoomExists(rightCoord) && GetIfOpeningNeeded(rightCoord, OPENING_DIRECTION.RIGHT);
        if (createRight || requiresRight) {
            idx += 4;
        }
        if (createRight && !GetIfRoomQueued(rightCoord)) {
            roomCreationQueue.Enqueue(rightCoord);
        }
        
        // Check Top
        bool createTop = !GetIfRoomExists(topCoord) && Random.Range(0, 100) < roomBranchingChance;
        bool requiresTop = GetIfRoomExists(topCoord) && GetIfOpeningNeeded(topCoord, OPENING_DIRECTION.TOP);
        if (createTop || requiresTop) {
            idx += 8;
        }
        if (createTop && !GetIfRoomQueued(topCoord)) {
            roomCreationQueue.Enqueue(topCoord);
        }

        if (idx == 0 && !requiresLeft && !requiresRight && !requiresBottom && !requiresTop) {
            CreateRoom(roomCoords);
        } else if (idx > 0) {
            roomDataList.Add(new RoomData(roomCoords, rooms[idx-1]));
        }
    }

    private bool GetIfRoomExists((int x, int y) coord)
    {
        if(coord.x < -2 || coord.x > 2 || coord.y < -2 || coord.y > 2) {
            return true;
        }

        return roomDataList.Exists(room => room.coordinates == coord);
    }

    private bool GetIfRoomQueued((int, int) coord)
    {
        return roomCreationQueue.Contains(coord);
    }

    private bool GetIfOpeningNeeded((int x, int y) coord, OPENING_DIRECTION direction)
    {
        if (coord.x < -2 || coord.x > 2 || coord.y < -2 || coord.y > 2) {
            return false;
        }

        RoomData roomData = roomDataList.Find(room => room.coordinates == coord);
        RoomInfo info = roomData.roomObject.GetComponent<RoomInfo>();

        if (direction == OPENING_DIRECTION.BOTTOM) {
            return info.topOpen;
        } else if (direction == OPENING_DIRECTION.TOP) {
            return info.bottomOpen;
        } else if (direction == OPENING_DIRECTION.LEFT) {
            return info.rightOpen;
        } else {
            return info.leftOpen;
        }
    }
}
