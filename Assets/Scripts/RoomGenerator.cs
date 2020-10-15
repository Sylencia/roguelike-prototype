using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ROOM_TYPES
{
    START = 0,
    FINISH,
    OBSTACLE
}
public class Room {
    public ROOM_TYPES roomType;
    public int roomIndex;
    public (int x, int y) coordinates;
    
    public Room(ROOM_TYPES type, int idx, (int, int) coord)
    {
        roomType = type;
        roomIndex = idx;
        coordinates = coord;
    }
}

public class RoomGenerator : MonoBehaviour
{
    public GameObject[] startRooms;
    public GameObject[] finishRooms;
    public GameObject[] obstacleRooms;
    public int numRooms;

    private List<Room> roomData = new List<Room>();
    public List<(int x, int y)> emptyRoomCoords = new List<(int, int)>();

    private void Start()
    {
        CreateRooms();
    }

    public void Reset()
    {
        roomData = new List<Room>();
        emptyRoomCoords = new List<(int, int)>();
        foreach(var tile in GameObject.FindGameObjectsWithTag("Rooms")) {
            Destroy(tile);
        }
    }
    public void JumpCallback(InputAction.CallbackContext context)
    {
        if (context.started) {
            Reset();
            CreateRooms();
        }
    }

    private void CreateRooms()
    {
        GetOpenAdjacentRooms((0, 0));
        
        // Add Start Rooms
        roomData.Add(new Room(ROOM_TYPES.START, Random.Range(0, startRooms.Length), (0, 0)));
        // Add Obstacle Rooms
        for(int i = 0; i < numRooms; ++i) {
            GenerateRoom();
        }
        // Add Finish Room
        (int, int) randomRoomLocation = GetRandomRoom();
        roomData.Add(new Room(ROOM_TYPES.FINISH, Random.Range(0, finishRooms.Length), randomRoomLocation));

        // Instantiate Rooms
        foreach (Room room in roomData) {
            if (room.roomType == ROOM_TYPES.START) {
                Instantiate(startRooms[room.roomIndex], new Vector3(room.coordinates.x, room.coordinates.y, 0), Quaternion.identity);
            } else if(room.roomType == ROOM_TYPES.FINISH) {
                Instantiate(finishRooms[room.roomIndex], new Vector3(room.coordinates.x, room.coordinates.y, 0), Quaternion.identity);
            } else {
                Instantiate(obstacleRooms[room.roomIndex], new Vector3(room.coordinates.x, room.coordinates.y, 0), Quaternion.identity);
            }
        }
    }

    private void GenerateRoom() 
    {
        (int x, int y) randomRoomLocation = GetRandomRoom();
        roomData.Add(new Room(ROOM_TYPES.OBSTACLE, Random.Range(0, obstacleRooms.Length), randomRoomLocation));
        GetOpenAdjacentRooms(randomRoomLocation);   
    }


    private void GetOpenAdjacentRooms((int x, int y) coordinates)
    {
        List<(int, int)> adjacentCoordinates = new List<(int, int)>();
        adjacentCoordinates.Add((coordinates.x, coordinates.y + 1));
        adjacentCoordinates.Add((coordinates.x, coordinates.y - 1));
        adjacentCoordinates.Add((coordinates.x + 1, coordinates.y));
        adjacentCoordinates.Add((coordinates.x - 1, coordinates.y));

        foreach((int x, int y) coord in adjacentCoordinates) {
            if(!GetIfRoomExists(coord) && !emptyRoomCoords.Exists(room => room.x == coord.x && room.y == coord.y)) {
                emptyRoomCoords.Add(coord);
            }
        }
    }

    private (int, int) GetRandomRoom()
    {
        int idx = Random.Range(0, emptyRoomCoords.Count);
        (int, int) roomCoords = emptyRoomCoords[idx];
        emptyRoomCoords.RemoveAt(idx);
        if (GetIfRoomExists(roomCoords)) {
            roomCoords = GetRandomRoom();
        }

        return roomCoords;
    }

    private bool GetIfRoomExists((int x, int y) coord)
    {
        return roomData.Exists(room => room.coordinates.x == coord.x && room.coordinates.y == coord.y);
    }
}
