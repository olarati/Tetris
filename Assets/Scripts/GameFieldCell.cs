using UnityEngine;

public class GameFieldCell 
{
    private Vector2 _position;
    private bool _isEmpty;

    public GameFieldCell(Vector2 position)
    {
        _position = position;
        _isEmpty = true;
    }

    public Vector2 GetPosition()
    {
        return _position;
    }

    public void SetIsEmpty(bool value)
    {
        _isEmpty = value;
    }

    public bool GetIsEmpty()
    {
        return _isEmpty;
    }
}
