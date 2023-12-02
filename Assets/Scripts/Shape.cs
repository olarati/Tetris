using UnityEngine;

public class Shape : MonoBehaviour
{
    public ShapePart[] Parts = new ShapePart[0];
    public int ExtraSpawnYMove;

    public virtual void Rotate() { }

    public Vector2Int[] GetPartCellIds()
    {
        Vector2Int[] startCellIds = new Vector2Int[Parts.Length];
        for (int i = 0; i < Parts.Length; i++)
        {
            startCellIds[i] = Parts[i].CellId;
        }
        return startCellIds;
    }

}
