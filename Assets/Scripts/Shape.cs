using UnityEngine;

public class Shape : MonoBehaviour
{
    public ShapePart[] Parts = new ShapePart[0];
    
    public virtual void Rotate() { }

}
