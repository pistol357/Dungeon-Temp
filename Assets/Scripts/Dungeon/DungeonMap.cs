using System.Collections.Generic;
using UnityEngine;

public class DungeonMap : MonoBehaviour
{
    [SerializeField] private ScenarioData _scenarioData;
    [SerializeField] private List<Room> _rooms;
    [SerializeField] private List<int>[] _connections;

    private List<int> _visited;
    private Dictionary<int, int> _distances;

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

    public int GetNextRoom(int current)
    {
        foreach(int connection in _connections[current])
        {
            if (_distances[connection] == _distances[current] - 1) return connection;
        }

        return current;
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

        foreach(RoomData data in _scenarioData.Rooms)
        {
            Room room = new Room(data);
            _rooms.Add(room);
        }

        foreach(Vector2Int c in _scenarioData.ConnectedRooms)
        {
            AddConnection(c.x, c.y);
        }

        RunBreadthFirst(0);
        PrintDistance();
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
