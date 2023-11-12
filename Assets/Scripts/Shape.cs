using UnityEngine;

public class Shape : MonoBehaviour
{
    public ShapePart[] Parts = new ShapePart[0];
    
    public virtual void Rotate()
    {
        Vector2 rotatePosition = Parts[0].transform.position;
        for (int i = 0; i < Parts.Length; i++)
        {
            // поворот для смены позиции части
            Parts[i].transform.RotateAround(rotatePosition, Vector3.forward, 90f);
            // поворот, чтобы спрайт всегда был вертикально
            Parts[i].transform.Rotate(Vector3.forward, -90f);
        }
    }
}
