using UnityEngine;
using UnityEngine.Tilemaps;

public class TileFactory : MonoBehaviour
{
    public Tile tileUnknown;
    public Tile tileUnknownHaftOpacity;
    public Tile tileFlag;
    public Tile tileMine;
    public Tile tileExploded;
    public Tile tileEmpty;
    public Tile tileNum1;
    public Tile tileNum2;
    public Tile tileNum3;
    public Tile tileNum4;
    public Tile tileNum5;
    public Tile tileNum6;
    public Tile tileNum7;
    public Tile tileNum8;

    public static TileFactory Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}