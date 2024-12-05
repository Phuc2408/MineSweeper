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
    public Tilemap tilemap { get; private set; }

    public Tile tileUnknown;
    public Tile tileUnknownHaftOpacity;
    public Tile tileEmpty;
    public Tile tileMine;
    public Tile tileExploded;
    public Tile tileFlag;
    public Tile tileNum1;
    public Tile tileNum2;
    public Tile tileNum3;
    public Tile tileNum4;
    public Tile tileNum5;
    public Tile tileNum6;
    public Tile tileNum7;
    public Tile tileNum8;
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
    public Cell getCell(int x, int y)
    {
        if (IsValid(x, y))
        {
            return state[x, y];
        }
        else
        {
            return new Cell();
        }
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

                if (cell.type != Cell.Type.Mine && cell.revealed == 0)
                    return BoardResult.RUNNING;
            }
        }

        return BoardResult.WIN;
    }


    public BoardResult Reveal(int x, int y)
    {
        Cell cell = state[x, y];
        if(cell.type == Cell.Type.Mine)
        {
            Explode(x, y);
            return BoardResult.OVER;
        }
        if(cell.type == Cell.Type.Empty)
        {
            Flood(x, y);
            return CheckWinCondition();
        }
        state[x,y].revealed = 0.5f;
        return CheckWinCondition();
    }

    

    private void Explode(int x, int y)
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);
        Cell cell = state[x, y];
        cell.revealed = 1;
        cell.exploded = true;
        state[x, y] = cell;
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
                if (cell.revealed == 0.5f)
                {
                    cell.revealed = 1;
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
                if (cell.type == Cell.Type.Mine)
                {
                    cell.revealed = 1;
                    cell.exploded = true;
                    state[x, y] = cell;                }
            }
        }
    }
    private void Flood(int x, int y)
    {
        if (!IsValid(x, y)) return;
        Cell cell = state[x, y];
        if (cell.revealed != 0) return;
        if (cell.type == Cell.Type.Mine || cell.type == Cell.Type.Invalid) return;
        state[x, y].revealed = 0.5f;
        if (cell.type == Cell.Type.Empty)
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
    private void GenerateCells()
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = new Cell();
                cell.position = new Vector3Int(x, y, 0);
                cell.type = Cell.Type.Empty;
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
                if(state[x, y].type != Cell.Type.Mine) {
                    state[x, y].type = Cell.Type.Mine;
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
                Cell cell = state[x, y];
                if (cell.type == Cell.Type.Mine)
                {
                    continue;
                }
                cell.number = CountMines(x, y);
                if (cell.number > 0)
                {
                    cell.type = Cell.Type.Number;
                }
                state[x, y] = cell;
            }
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

                if (nx >= 0 && nx < width && ny >= 0 && ny < height && state[nx, ny].type == Cell.Type.Mine)
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
                tilemap.SetTile(cell.position, GetTile(cell));
            }
        }
        StartCoroutine(endRevealing());
    }

    private Tile GetTile(Cell cell)
    {
        if (cell.revealed == 0.5f) return tileUnknownHaftOpacity;
        if (cell.revealed == 1)
        {
            return GetRevealedTile(cell);
        }
        else if (cell.flagged)
        {
            return tileFlag;
        }
        else
        {
            return tileUnknown;
        }
    }

    private Tile GetRevealedTile(Cell cell)
    {
        switch (cell.type)
        {
            case Cell.Type.Empty: return tileEmpty;
            case Cell.Type.Mine: return cell.exploded ? tileExploded : tileMine;
            case Cell.Type.Number: return GetNumberTile(cell);
            default: return null;
        }
    }

    private Tile GetNumberTile(Cell cell)
    {
        switch (cell.number)
        {
            case 1: return tileNum1;
            case 2: return tileNum2;
            case 3: return tileNum3;
            case 4: return tileNum4;
            case 5: return tileNum5;
            case 6: return tileNum6;
            case 7: return tileNum7;
            case 8: return tileNum8;
            default: return null;
        }
    }
    public void showUnRevealedCell()
    {
        do
        {
            int x = Random.Range(0, state.GetLength(0));
            int y = Random.Range(0, state.GetLength(1));
            Cell predict = state[x, y];
            if (predict.type != Cell.Type.Mine && predict.revealed == 0)
            {
                Flood(x, y);
                return;
            }    
        } while (true);
    }
}