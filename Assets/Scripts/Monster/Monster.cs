public class Monster
{
    private MonsterData _data;
    private int _hp;

    public Monster(MonsterData data)
    {
        _data = data;
        _hp = _data.MaxHp;
    }
}
