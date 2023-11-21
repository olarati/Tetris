using UnityEngine;

public class ShapeMover : MonoBehaviour
{
    public GameStateChanger GameStateChanger;
    public GameField GameField;

    public float MoveDownDelay = 0.8f;

    private float _moveDownTimer = 0;
    private bool _isActive;

    private Shape _targetShape;

    public void MoveShape(Vector2Int deltaMove)
    {
        if (!CheckMovePossible(deltaMove))
        {
            return;
        }

        for (int i = 0; i < _targetShape.Parts.Length; i++) 
        {
            Vector2Int newPartCellId = _targetShape.Parts[i].CellId + deltaMove;
            Vector2 newPartPosition = GameField.GetCellPosition(newPartCellId);
            _targetShape.Parts[i].CellId = newPartCellId;
            _targetShape.Parts[i].SetPosition(newPartPosition);
        }

    }

    public void SetTargetChape(Shape targetShape)
    {
        _targetShape = targetShape;
    }

    public void SetActive(bool value)
    {
        _isActive = value;
    }

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

        SetShapePartCellsEmpty(true);
        HorizontalMove();
        VerticalMove();
        Rotate();

        bool reachBottom = CheckBottom();
        bool reachOtherShape = CheckOtherShape();
        SetShapePartCellsEmpty(false);

        if (reachBottom || reachOtherShape)
        {
            if (CheckShapeTopOver())
            {
                GameStateChanger.EndGame();
            }
            else
            {
                GameStateChanger.SpawnNextShape();
            }
        }
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
            _targetShape.Rotate();
            UpdateByWalls();
            UpdateByBottom();
            SetShapeInCells();
        }
    }

    private bool CheckMovePossible(Vector2Int deltaMove)
    {
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            Vector2Int newPartCellId = _targetShape.Parts[i].CellId + deltaMove;
            if (newPartCellId.x < 0 || newPartCellId.y < 0 
                || newPartCellId.x >= GameField.FieldSize.x || newPartCellId.y >= GameField.FieldSize.y)
            {
                return false;
            }
            else if (!GameField.GetCellEmpty(newPartCellId))
            {
                return false;
            }
        }
        return true;
    }

    private void SetShapeInCells()
    {
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            Vector2 shapePartPosition = _targetShape.Parts[i].transform.position;
            Vector2Int newPartCellId = GameField.GetNearestCellId(shapePartPosition);
            Vector2 newPartPosition = GameField.GetCellPosition(newPartCellId);
            _targetShape.Parts[i].CellId = newPartCellId;
            _targetShape.Parts[i].SetPosition(newPartPosition);
        }
    }

    private void UpdateByWalls()
    {
        UpdateByWall(true);
        UpdateByWall(false);
    }

    private void UpdateByWall(bool right)
    {
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            if (CheckWallOver(_targetShape.Parts[i], right))
            {
                for (int j = 0; j < _targetShape.Parts.Length; j++)
                {
                    _targetShape.Parts[j].transform.position += (right ? -1 : 1) * Vector3.right * GameField.CellSize.x;
                }
            }
        }
    }

    private bool CheckWallOver(ShapePart part, bool right)
    {
        float wallDistance = 0;
        if (right)
        {
            wallDistance = part.transform.position.x - (GameField.FirstCellPoint.position.x + (GameField.FieldSize.x - 1) * GameField.CellSize.x);
            wallDistance = GetRoundedWallDistance(wallDistance); 
            if (wallDistance !=  0 && wallDistance > 0)
            {
                return true;
            }
        }
        else
        {
            wallDistance = part.transform.position.x - GameField.FirstCellPoint.position.x;
            wallDistance = GetRoundedWallDistance(wallDistance);
            if (wallDistance != 0 && wallDistance < 0)
            {
                return true;
            }
        }
        return false;
    }

    private float GetRoundedWallDistance(float distance)
    {
        int roundValue = 100; // для округления до двух знаков после запятой
        distance = Mathf.Round(distance * roundValue);
        return distance;
    }

    private void UpdateByBottom()
    {
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            if (CheckBottomOver(_targetShape.Parts[i]))
            {
                for (int j = 0; j < _targetShape.Parts.Length; j++)
                {
                    _targetShape.Parts[j].transform.position += Vector3.up * GameField.CellSize.y;
                }
            }
        }
    }

    private bool CheckBottomOver(ShapePart part)
    {
        float wallDistance = part.transform.position.y - GameField.FirstCellPoint.position.y;
        wallDistance = GetRoundedWallDistance(wallDistance);
        if (wallDistance != 0 && wallDistance < 0)
        {
            return true;
        }
        return false;
    }

    private bool CheckBottom()
    {
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            if (_targetShape.Parts[i].CellId.y == 0)
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckOtherShape()
    {
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            if (!GameField.GetCellEmpty(_targetShape.Parts[i].CellId + Vector2Int.down))
            {
                return true;
            }
        }
        return false;
    }

    private void SetShapePartCellsEmpty(bool value)
    {
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            GameField.SetCellEmpty(_targetShape.Parts[i].CellId, value);
        }
    }



    private bool CheckShapeTopOver()
    {
        float topCellYPosition = GameField.FirstCellPoint.position.y + (GameField.FieldSize.y - GameField.InvisibleYFieldSize) * GameField.CellSize.y;
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            float wallDistance = _targetShape.Parts[i].transform.position.y - topCellYPosition;
            wallDistance = GetRoundedWallDistance(wallDistance);
            if (wallDistance != 0 && wallDistance > 0)
            {
                return true;
            }
        }
        return false;
    }
}
