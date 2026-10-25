
using UnityEngine;
using UnityEngine.UI;

public class TetrisGrid : MonoBehaviour
{
    public static TetrisGrid Instance;

    public RectTransform board;

    public int columns = 13;
    public int rows = 8;

    public Vector2 cellSize = new Vector2(50f, 50f);

    private Transform[,] grid;

    void Awake()
    {
        Instance = this;
        grid = new Transform[columns, rows];
    }

    Vector2Int GetCell(RectTransform square)
    {
        Vector3 localPosition =
            board.InverseTransformPoint(square.position);

        int x = Mathf.RoundToInt(
            (localPosition.x - board.rect.xMin) / cellSize.x - 0.5f);

        int y = Mathf.RoundToInt(
            (localPosition.y - board.rect.yMin) / cellSize.y - 0.5f);

        return new Vector2Int(x, y);
    }

    public bool IsValidPiece(RectTransform piece)
    {
        Image[] squares = piece.GetComponentsInChildren<Image>();

        foreach (Image square in squares)
        {
            if (square.transform == piece)
                continue;

            Vector2Int cell = GetCell(square.rectTransform);

            if (cell.x < 0 || cell.x >= columns ||
                cell.y < 0 || cell.y >= rows)
            {
                return false;
            }

            if (grid[cell.x, cell.y] != null)
            {
                return false;
            }
        }

        return true;
    }

    public void LockPiece(RectTransform piece)
    {
        Image[] squares = piece.GetComponentsInChildren<Image>();

        foreach (Image square in squares)
        {
            Vector2Int cell = GetCell(square.rectTransform);

            if (cell.x >= 0 && cell.x < columns &&
                cell.y >= 0 && cell.y < rows)
            {
                grid[cell.x, cell.y] = square.transform;
            }
        }
    }
}
