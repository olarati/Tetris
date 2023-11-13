using UnityEngine;

public class ShapeTwoRotations : Shape
{
    bool _rotated = false;

    public override void Rotate()
    {
        float rotateMultiplier = _rotated ? -1 : 1;
        Vector2 rotatePosition = Parts[0].transform.position;
        for (int i = 0; i < Parts.Length; i++)
        {
            // поворот для смены позиции части
            Parts[i].transform.RotateAround(rotatePosition, Vector3.forward, 90f * rotateMultiplier);
            // поворот, чтобы спрайт всегда был вертикально
            Parts[i].transform.Rotate(Vector3.forward, -90f * rotateMultiplier);
        }
        _rotated = !_rotated;
    }
}
