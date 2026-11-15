using UnityEngine;

public class Treasure
{
    private bool _isOpened;
    private int _value;

    public bool IsOpened => _isOpened;
    public int Value => _value;

    public Treasure(TreasureData data)
    {
        _value = data.Value;
    }

    public void Open()
    {
        _isOpened = true;
    }
}
