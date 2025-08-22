using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public enum BoardResult
    {
        OVER,
        RUNNING,
        WIN
    }
    public GameObject cellPrefab;  // A prefab for cells (empty GameObject)
    public Tilemap tilemap { get; private set; }
    private Cell[,] state;
    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void init(int width, int height, int mineNumber)
    {
        state = new Cell[width, height];

        GenerateCells();
        GenerateMines(mineNumber);
        GenerateNumbers();
        Draw();
    }
    private bool IsValid(int x, int y)
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);
        return x >= 0 && x < width && y >= 0 && y < height;
    }
    public void setCell(int x, int y, Cell cell)
    {
        state[x, y] = cell;
    }

    public BoardResult CheckWinCondition()
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];

                if (cell.getType() != Cell.Type.Mine && cell.isRevealed() == 0)
                    return BoardResult.RUNNING;
            }
        }

        return BoardResult.WIN;
    }


    public BoardResult Reveal(int x, int y)
    {
        Cell cell = state[x, y];
        if(cell.getType() == Cell.Type.Mine)
        {
            Explode(x, y);
            return BoardResult.OVER;
        }
        if(cell.getType() == Cell.Type.Empty)
        {
            Flood(x, y);
            return CheckWinCondition();
        }
        state[x, y].setRevealed(0.5f);
        return CheckWinCondition();
    }

    

    private void Explode(int x, int y)
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);
        state[x, y].setRevealed(1.0f);
        (state[x, y] as MineCell).isExploded = true;
        showAllBomb();
    }
    private IEnumerator endRevealing()
    {
        yield return new WaitForSeconds(0.3f);
        int width = state.GetLength(0);
        int height = state.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];
                if (cell.isRevealed() == 0.5f)
                {
                    cell.setRevealed(1.0f);
                    state[x, y] = cell;
                }
            }
        }
        Draw();
    }
    public void showAllBomb()
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];
                if (cell.getType() == Cell.Type.Mine)
                {
                    cell.setRevealed(1.0f);
                    (cell as MineCell).isExploded = true;
                    state[x, y] = cell;                }
            }
        }
    }
    private void Flood(int x, int y)
    {
        if (!IsValid(x, y)) return;
        if (state[x, y].flagged)
            return;
        Cell cell = state[x, y];
        if (state[x, y].isRevealed() != 0) return;
        if (state[x, y].getType() == Cell.Type.Mine) return;
        state[x, y].setRevealed(0.5f);
        if (state[x, y].getType() == Cell.Type.Empty)
        {
            Flood(x - 1, y);
            Flood(x + 1, y);
            Flood(x, y - 1);
            Flood(x, y + 1);
            Flood(x + 1, y + 1);
            Flood(x + 1, y - 1);
            Flood(x - 1, y + 1);
            Flood(x - 1, y - 1);
        }
    }
    public Cell GetCell(int x, int y)
    {
        return state[x, y];
    }
    private void GenerateCells()
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject cellObj = Instantiate(cellPrefab); // Create a new GameObject
                Cell cell = cellObj.AddComponent<EmptyCell>();
                cell.position = new Vector3Int(x, y, 0);
                state[x, y] = cell;
            }
        }
    }
    private void GenerateMines(int mineCount)
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);
        for (int i = 0; i < mineCount; i++)
        {
            while (true)
            {
                int x = Random.Range(0, width);
                int y = Random.Range(0, height);
                if(state[x, y].getType() != Cell.Type.Mine) {
                    GameObject cellObj = Instantiate(cellPrefab); // Create a new GameObject
                    Cell cell = cellObj.AddComponent<MineCell>();
                    cell.position = new Vector3Int(x, y, 0);
                    state[x, y] = cell;
                    break;
                }
            }
        }
    }
    private void GenerateNumbers()
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (state[x, y].getType() == Cell.Type.Mine)
                    continue;
                int number = CountMines(x, y);
                if (number > 0)
                {
                    GameObject cellObj = Instantiate(cellPrefab); // Create a new GameObject
                    Cell cell = cellObj.AddComponent<NumberCell>();
                    (cell as NumberCell).number = number;
                    cell.position = new Vector3Int(x, y, 0);
                    state[x, y] = cell;
                }            }
        }
    }
    private int CountMines(int x, int y)
    {
        int mineCount = 0;
        int width = state.GetLength(0);
        int height = state.GetLength(1);
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < width && ny >= 0 && ny < height && state[nx, ny].getType() == Cell.Type.Mine)
                    mineCount++;
            }
        }
        return mineCount;
    }

    public void Draw()
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];
                tilemap.SetTile(cell.position, cell.GetTile());
            }
        }
        StartCoroutine(endRevealing());
    }
    public void showUnRevealedCell()
    {
        do
        {
            int x = Random.Range(0, state.GetLength(0));
            int y = Random.Range(0, state.GetLength(1));
            Cell predict = state[x, y];
            if (predict.getType() != Cell.Type.Mine && predict.isRevealed() == 0)
            {
                Flood(x, y);
                return;
            }    
        } while (true);
    }
}