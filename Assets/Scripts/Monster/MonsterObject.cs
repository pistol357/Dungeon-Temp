using UnityEngine;

public class MonsterObject : MonoBehaviour
{
    [SerializeField] private Monster _monster;

    public void SetMonster(Monster monster)
    {
        _monster = monster;
    }
}
