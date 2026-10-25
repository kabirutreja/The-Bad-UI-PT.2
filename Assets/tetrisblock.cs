using System.Collections.Generic;
using UnityEngine;

public class tetrisblock : MonoBehaviour
{
    public float fallTime = 0.8f;
    public float cellSize = 50f;

    [HideInInspector] public RectTransform board;
    [HideInInspector] public SpawnTetris spawner;

    public const int COLS = 13;
    public const int ROWS = 8;

    public static int linesCleared = 0;
    private static Transform[,] grid = new Transform[COLS, ROWS];

    private RectTransform rectTransform;
    private float lastFallTime;
    private bool isLocked = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        lastFallTime = Time.time;
    }

    void Update()
    {
        if (isLocked) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            TryMove(new Vector2(-cellSize, 0));
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            TryMove(new Vector2(cellSize, 0));
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            TryRotate();

        float delay = Input.GetKey(KeyCode.DownArrow) ? fallTime / 10f : fallTime;
        if (Time.time - lastFallTime > delay)
        {
            lastFallTime = Time.time;
            if (!TryMove(new Vector2(0, -cellSize)))
                LockBlock();
        }
    }

    bool TryMove(Vector2 offset)
    {
        if (!CanPlace(offset)) return false;
        rectTransform.anchoredPosition += offset;
        return true;
    }

    void TryRotate()
    {
        Quaternion original = rectTransform.localRotation;
        rectTransform.Rotate(0, 0, 90);

        if (!CanPlace(Vector2.zero))
            rectTransform.localRotation = original;
    }

    bool CanPlace(Vector2 offset)
    {
        foreach (Transform child in transform)
        {
            Vector2 p = (Vector2)board.InverseTransformPoint(child.position) + offset;

            if (!InsideBoard(p)) return false;

            Vector2Int cell = WorldToCell(p);
            if (grid[cell.x, cell.y] != null) return false;
        }
        return true;
    }

    bool InsideBoard(Vector2 p)
    {
        float half = cellSize / 2f;
        return p.x >= board.rect.xMin + half - 1f &&
               p.x <= board.rect.xMax - half + 1f &&
               p.y >= board.rect.yMin + half - 1f &&
               p.y <= board.rect.yMax - half + 1f;
    }

    public void AlignToTop()
    {
        RectTransform rt = GetComponent<RectTransform>();

        for (int i = 0; i < 20; i++)
        {
            bool allInside = true;
            foreach (Transform child in transform)
            {
                if (!InsideBoard(board.InverseTransformPoint(child.position)))
                {
                    allInside = false;
                    break;
                }
            }

            if (allInside) return;
            rt.anchoredPosition += new Vector2(0, -cellSize);
        }
    }

    public bool IsOverlapping()
    {
        return !CanPlace(Vector2.zero);
    }

    Vector2Int WorldToCell(Vector2 p)
    {
        float xMin = board.rect.xMin;
        float yMin = board.rect.yMin;

        int col = Mathf.RoundToInt((p.x - xMin - cellSize / 2f) / cellSize);
        int row = Mathf.RoundToInt((p.y - yMin - cellSize / 2f) / cellSize);
        return new Vector2Int(col, row);
    }

    void LockBlock()
    {
        isLocked = true;
        enabled = false;

        List<Transform> cells = new List<Transform>();
        foreach (Transform child in transform)
            cells.Add(child);

        foreach (Transform child in cells)
        {
            Vector2Int cell = WorldToCell(board.InverseTransformPoint(child.position));

            child.SetParent(board, true);

            if (cell.x >= 0 && cell.x < COLS && cell.y >= 0 && cell.y < ROWS)
                grid[cell.x, cell.y] = child;
        }

        Destroy(gameObject);

        ClearFullRows();
        spawner.NewTetris();
    }

    void ClearFullRows()
    {
        int cleared = 0;

        for (int row = 0; row < ROWS; row++)
        {
            if (IsRowFull(row))
            {
                ClearRow(row);
                cleared++;
                row--;   
            }
        }

        if (cleared > 0)
        {
            linesCleared += cleared;
            Debug.Log($"Cleared {cleared} line(s). Total: {linesCleared}");
        }
    }

    bool IsRowFull(int row)
    {
        for (int col = 0; col < COLS; col++)
        {
            if (grid[col, row] == null)
                return false;
        }
        return true;
    }

    void ClearRow(int row)
    {
        for (int col = 0; col < COLS; col++)
        {
            if (grid[col, row] != null)
            {
                Destroy(grid[col, row].gameObject);
                grid[col, row] = null;
            }
        }

        for (int r = row + 1; r < ROWS; r++)
        {
            for (int col = 0; col < COLS; col++)
            {
                Transform cell = grid[col, r];
                if (cell == null) continue;

                cell.localPosition += Vector3.down * cellSize;
                grid[col, r - 1] = cell;
                grid[col, r] = null;
            }
        }
    }

    public static void ResetGame()
    {
        for (int col = 0; col < COLS; col++)
        {
            for (int row = 0; row < ROWS; row++)
            {
                if (grid[col, row] != null)
                {
                    Destroy(grid[col, row].gameObject);
                    grid[col, row] = null;
                }
            }
        }

        linesCleared = 0;
       
    }
}