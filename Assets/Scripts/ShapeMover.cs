using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ShapeMover : MonoBehaviour
{
    public GameField GameField;
    public Shape TargetShape;

    public float MoveDownDelay = 0.8f;

    private float _moveDownTimer = 0;

    public void MoveShape(Vector2Int deltaMove)
    {
        if (!CheckMovePossible(deltaMove))
        {
            return;
        }

        for (int i = 0; i < TargetShape.Parts.Length; i++)
        {
            Vector2Int newPartCellId = TargetShape.Parts[i].CellId + deltaMove;
            Vector2 newPartPosition = GameField.GetCellPosition(newPartCellId);
            TargetShape.Parts[i].CellId = newPartCellId;
            TargetShape.Parts[i].SetPosition(newPartPosition);
        }
    }

    private void Update()
    {
        HorizontalMove();
        VerticalMove();
        Rotate();
    }

    private void HorizontalMove()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            MoveShape(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            MoveShape(Vector2Int.right);
        }
    }

    private void VerticalMove()
    {
        _moveDownTimer += Time.deltaTime;
        if(_moveDownTimer >= MoveDownDelay || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            _moveDownTimer = 0;
            MoveShape(Vector2Int.down);
        }
    }

    private void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            Vector2 rotatePoint = TargetShape.Parts[0].transform.position;
            for (int i = 0; i < TargetShape.Parts.Length; i++)
            {
                // ������� ��� ����� ������� �����
                TargetShape.Parts[i].transform.RotateAround(rotatePoint, Vector3.forward, 90f);
                // �������, ����� ������ ������ ��� �����������
                TargetShape.Parts[i].transform.Rotate(Vector3.forward, -90f);
            }
            UpdateByWalls();
            SetShapeInCells();
        }
    }

    private bool CheckMovePossible(Vector2Int deltaMove)
    {
        for (int i = 0; i < TargetShape.Parts.Length; i++)
        {
            Vector2Int newPartCellId = TargetShape.Parts[i].CellId + deltaMove;
            if(newPartCellId.x < 0 || newPartCellId.y < 0 
                || newPartCellId.x >= GameField.FieldSize.x || newPartCellId.y >= GameField.FieldSize.y)
            {
                return false;
            }
        }
        return true;
    }

    private void SetShapeInCells()
    {
        for (int i = 0; i < TargetShape.Parts.Length; i++)
        {
            Vector2 shapePartPosition = TargetShape.Parts[i].transform.position;
            Vector2Int newPartCellId = GameField.GetNearestCellId(shapePartPosition);
            Vector2 newPartPosition = GameField.GetCellPosition(newPartCellId);
            TargetShape.Parts[i].CellId = newPartCellId;
            TargetShape.Parts[i].SetPosition(newPartPosition);
        }
    }

    private void UpdateByWalls()
    {
        UpdateByWall(true);
        UpdateByWall(false);
    }

    private void UpdateByWall(bool right)
    {
        if (CheckWall(right))
        {
            for (int i = 0; i < TargetShape.Parts.Length; i++)
            {
                TargetShape.Parts[i].transform.position += (right ? -1 : 1) * Vector3.right * GameField.CellSize.x;
            }
        }
    }

    private bool CheckWall(bool right)
    {
        for (int i = 0; i < TargetShape.Parts.Length; i++)
        {
            float wallDistance = 0;
            if (right)
            {
                wallDistance = TargetShape.Parts[i].transform.position.x - (GameField.FirstCellPoint.position.x + (GameField.FieldSize.x - 1) * GameField.CellSize.x);
                if (!Mathf.Approximately(wallDistance, 0) && wallDistance > 0)
                {
                    return true;
                }
            }
            else
            {
                wallDistance = TargetShape.Parts[i].transform.position.x - GameField.FirstCellPoint.position.x;
                if (!Mathf.Approximately(wallDistance, 0) && wallDistance < 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
