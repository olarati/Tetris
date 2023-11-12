using UnityEngine;

public class Shape : MonoBehaviour
{
    public ShapePart[] Parts = new ShapePart[0];
    
    public virtual void Rotate()
    {
        Vector2 rotatePosition = Parts[0].transform.position;
        for (int i = 0; i < Parts.Length; i++)
        {
            // ������� ��� ����� ������� �����
            Parts[i].transform.RotateAround(rotatePosition, Vector3.forward, 90f);
            // �������, ����� ������ ������ ��� �����������
            Parts[i].transform.Rotate(Vector3.forward, -90f);
        }
    }
}
