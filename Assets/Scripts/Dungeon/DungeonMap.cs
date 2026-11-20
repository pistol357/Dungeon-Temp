using System.Collections.Generic;
using UnityEngine;

public class DungeonMap : MonoBehaviour
{
    [SerializeField] private DungeonData _dungeonData;
    [SerializeField] private ScenarioData _scenarioData;
    [SerializeField] private List<Room> _rooms;
    [SerializeField] private List<int>[] _connections;

    private static DungeonMap _instance;
    private List<int> _visited;
    private Dictionary<int, int> _distances;
    private List<PlayCard> _craftCardDeck = new();
    private List<PlayCard> _craftCardOpen = new();

    public static DungeonMap Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<DungeonMap>();
            }

            return _instance;
        }
    }
    public List<Room> Rooms => _rooms;

    private void Awake() => SetSingleton();
    private void Start() => Init();

    private void AddConnection(int a, int b)
    {
        _connections[a].Add(b);
        _connections[b].Add(a);
    }

    private void RunBreadthFirst(int start)
    {
        Queue<int> queue = new Queue<int>();
        _visited.Add(start);
        _distances.Add(start, 0);
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            int current = queue.Dequeue();
            EnqueueUnvisitedNeighbors(current, queue);
        }
    }

    private void EnqueueUnvisitedNeighbors(int vertex, Queue<int> queue)
    {
        foreach (int neighbor in _connections[vertex])
        {
            if (!_visited.Contains(neighbor))
            {
                _visited.Add(neighbor);
                _distances.Add(neighbor, _distances[vertex] + 1);
                queue.Enqueue(neighbor);
            }
        }
    }

    public int GetNextRoomNumber(int current)
    {
        foreach(int connection in _connections[current])
        {
            if (_distances[connection] == _distances[current] - 1) return connection;
        }

        return current;
    }

    public Room GetRoomByNumber(int number)
    {
        foreach(Room room in _rooms)
        {
            if(room.RoomNumber == number)
            {
                return room;
            }
        }

        return null;
    }

    public RoomType GetRoomTypeByNumber(int number)
    {
        Room room = GetRoomByNumber(number);

        if (room == null) return RoomType.None;

        return room.RoomType;
    }

    public List<int> GetRoomNumbersOfType(RoomType type)
    {
        List<int> rooms = new();

        foreach(Room room in _rooms)
        {
            if(room.RoomType == type)
            {
                rooms.Add(room.RoomNumber);
            }
        }

        if (rooms.Count == 0) return null;

        return rooms;
    }

    public List<int> GetConnectedRoomNumberList(int number)
    {
        if (GetRoomByNumber(number) == null)
        {
            Debug.Log($"인덱스 범위 초과");
            return null;
        }

        return _connections[number];
    }

    public Dictionary<ResourceType, int> GetRoomResources(int number)
    {
        return GetRoomByNumber(number)?.Resources;
    }

    public int GetCountOfResourceType(ResourceType resourceType)
    {
        int count = 0;

        foreach(Room room in _rooms)
        {
            if (room.Resources.ContainsKey(resourceType)) count += room.Resources[resourceType];
        }

        return count;
    }

    public bool IsTreasureRoomPlundered()
    {
        foreach(Room room in _rooms)
        {
            if (room.RoomType == RoomType.Treasure && room.Treasure.IsOpened) return true;
        }

        return false;
    }

    private void SetSingleton()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Init()
    {
        _rooms = new();
        _connections = new List<int>[_scenarioData.Rooms.Count];
        for (int i = 0; i < _connections.Length; i++)
        {
            _connections[i] = new();
        }
        _visited = new();
        _distances = new();

        for(int i = 0; i < _scenarioData.Rooms.Count; i++)
        {
            RoomData data = _scenarioData.Rooms[i];
            int treasureIndex = _scenarioData.RoomToTreasure[i].y;
            TreasureData treasureData = _scenarioData.Treasures[treasureIndex];
            Room room = new Room(data, i, treasureData);
            _rooms.Add(room);
        }

        foreach(Vector2Int c in _scenarioData.ConnectedRooms)
        {
            AddConnection(c.x, c.y);
        }

        RunBreadthFirst(0);
        PrintDistance();

        foreach(PlayCardData cardData in _dungeonData.CraftCards)
        {
            _craftCardDeck.Add(new PlayCard(cardData));
        }
    }

    private void PrintDistance()
    {
        for(int i = 0; i < _distances.Count; i++)
        {
            int distance = _distances[i];
            Debug.Log($"{i}번 방의 거리 : {distance}");
        }
    }
}
