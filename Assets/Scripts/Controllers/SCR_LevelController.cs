using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_LevelController : MonoBehaviour
{
    public enum RoomType {Empty, Four, Three, Two, One, Room, Start}

    public int RoomsToSpawn = 5;

    public static SCR_LevelController Instance;

    public GameObject SpawnRoom;
    public List<GameObject> FourExitRooms, ThreeExitRooms, LRoomList, LineRoomList, OneExitRooms;

    public int CurrentFloorLevel = 0;

    public int rooms = 0;

    [Header("Data stuff, dont touch")]
    [SerializeField] private List<GameObject> currentSpawnedRooms;
    [SerializeField] Vector2Int LayoutGridSize;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void CreateFloor()
    {
        InstantiateFloor(GenerateGrid());
        CurrentFloorLevel++;
    }

    public void DestroyCurrentLevel()
    {

    }

    public void PossiblePlayerSpawns()
    {

    }

    private void InstantiateFloor(RoomType[,] grid)
    {
        for (int x = 0; x < LayoutGridSize.x; x++)
        for (int y = 0; y < LayoutGridSize.y; y++)
            switch (grid[x, y])
            {
                    case RoomType.One:
                        bool shouldSpawn = true;
                        for (int i = -1; i <= 1; i++)
                        {
                            if (y + i < 0 || y + i >= LayoutGridSize.y)
                                continue;

                            if (grid[x, y + i] == RoomType.Start)
                                shouldSpawn = false;
                        }

                        for (int i = -1; i <= 1; i++)
                        {
                            if (x + i < 0 || x + i >= LayoutGridSize.x)
                                continue;

                            if (grid[x+i, y] == RoomType.Start)
                                shouldSpawn = false;
                        }

                        if (shouldSpawn)
                            currentSpawnedRooms.Add(Instantiate(OneExitRooms[Random.Range(0, OneExitRooms.Count)], new Vector3((x * 10) * 10f, 0, (y * 10) * 10f), Quaternion.identity));
                        break;
                    case RoomType.Two:

                        int line = 0;
                        for (int i = -1; i <= 1; i++)
                        {
                            if (x + i < 0 || x + i >= LayoutGridSize.x)
                                break;

                            if (grid[x+i, y] != RoomType.Empty)
                                line++;
                        }

                        if (line == 3)
                        {
                            currentSpawnedRooms.Add(Instantiate(LineRoomList[Random.Range(0, LineRoomList.Count)], new Vector3((x * 10) * 10f, 0, (y * 10) * 10f), Quaternion.identity));
                            break;
                        }

                        line = 0;

                        for (int i = -1; i <= 1; i++)
                        {
                            if (y + i < 0 || y+i >= LayoutGridSize.y)
                                break;

                            if (grid[x, y+i] != RoomType.Empty)
                                line++;
                        }
                        if (line == 3)
                        {
                            currentSpawnedRooms.Add(Instantiate(LineRoomList[Random.Range(0, LineRoomList.Count)], new Vector3((x * 10) * 10f, 0, (y * 10) * 10f), Quaternion.identity));
                            break;
                        }

                        currentSpawnedRooms.Add(Instantiate(LRoomList[Random.Range(0, LRoomList.Count)], new Vector3((x * 10) * 10f, 0, (y * 10) * 10f), Quaternion.identity));
                        break;
                    case RoomType.Three:
                        currentSpawnedRooms.Add(Instantiate(ThreeExitRooms[Random.Range(0, ThreeExitRooms.Count)], new Vector3((x * 10) * 10f, 0, (y * 10) * 10f), Quaternion.identity));
                        break;
                    case RoomType.Four:
                        currentSpawnedRooms.Add(Instantiate(FourExitRooms[Random.Range(0, FourExitRooms.Count)], new Vector3((x * 10) * 10f, 0, (y * 10) * 10f), Quaternion.identity));
                        break;
                    case RoomType.Start:
                        currentSpawnedRooms.Add(Instantiate(SpawnRoom, new Vector3((x * 10) * 10f, 0, (y * 10) * 10f), Quaternion.identity));
                        break;
                    case RoomType.Empty:
                    case RoomType.Room:
                    default:
                        continue;
            }
    }

    private RoomType[,] GenerateGrid()
    {
        RoomType[,] grid = new RoomType[LayoutGridSize.x, LayoutGridSize.y];

        List<Vector2Int> allowed_movements = new List<Vector2Int>
        {
            Vector2Int.left,
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down
        };

        System.Random rnd = new System.Random();
        Vector2Int curr_pos = new Vector2Int(rnd.Next(LayoutGridSize.x), rnd.Next(LayoutGridSize.y));

        grid[curr_pos.x, curr_pos.y] = RoomType.Start;
        rooms++;

        // iterate on the number of steps and move around
        for (int id_step = 0; id_step < RoomsToSpawn-1; id_step++)
        {
            while (true)
            {
                Vector2Int new_pos = curr_pos + allowed_movements[rnd.Next(allowed_movements.Count)];
                // check if the new position is in the grid
                if (new_pos.x >= 0 && new_pos.x < LayoutGridSize.x && new_pos.y >= 0 && new_pos.y < LayoutGridSize.y)
                {
                    //If the position is already a room, go next
                    if (grid[new_pos.x, new_pos.y] == RoomType.Room || grid[new_pos.x, new_pos.y] == RoomType.Start)
                    {
                        curr_pos = new_pos;
                        continue;
                    }

                    // this is a valid position, we set it in the grid
                    grid[new_pos.x, new_pos.y] = RoomType.Room;
                    rooms++;

                    // replace curr_pos with new_pos
                    curr_pos = new_pos;

                    // and finally break of the infinite loop
                    break;
                }
            }
        }

        Debug.Log(rooms);

        for (var x = 0; x < LayoutGridSize.x; x++)
        for (var y = 0; y < LayoutGridSize.y; y++)
        {
            if (grid[x, y] != RoomType.Room) continue;

            Debug.Log(grid[x, y]);

            int connectedRooms = 0;

            if (x + 1 < LayoutGridSize.x)
                if (grid[x + 1, y] != RoomType.Empty) connectedRooms++;
            if (x - 1 >= 0)
                if (grid[x - 1, y] != RoomType.Empty) connectedRooms++;
            if (y + 1 < LayoutGridSize.y)
                if (grid[x, y+1] != RoomType.Empty) connectedRooms++;
            if (y - 1 >= 0)
                if (grid[x, y-1] != RoomType.Empty) connectedRooms++;

            grid[x, y] = connectedRooms switch
            {
                1 => RoomType.One,
                2 => RoomType.Two,
                3 => RoomType.Three,
                4 => RoomType.Four,
                _ => RoomType.Empty
            };
        }

        return grid;
    }
}
