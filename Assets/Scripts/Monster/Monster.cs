using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterData _data;
    
    private int _hp;
    private Room _currentRoom;
    private State _state;

    public Room CurrentRoom => _currentRoom;

    public void Move()
    {

    }

    public void Activate()
    {

    }

    public void SRAttack()
    {

    }

    public void LRAttack()
    {

    }

    public void Defense()
    {

    }

    public void Summon(ActionType room)
    {

    }

    public void Create(ActionType resource)
    {

    }

    public void SetData(MonsterData data)
    {
        _data = data;
    }
}
