using UnityEngine;
using UnityEngine.Tilemaps;

public class NumberCell : Cell
{

    public int number;
    public NumberCell(int number)
    {
        this.number = number;
    }

    public override Type getType()
    {
        return Type.Number;
    }

    public override Tile GetRevealedTile()
    {
        switch (number)
        {
            case 1: return TileFactory.Instance.tileNum1;
            case 2: return TileFactory.Instance.tileNum2;
            case 3: return TileFactory.Instance.tileNum3;
            case 4: return TileFactory.Instance.tileNum4;
            case 5: return TileFactory.Instance.tileNum5;
            case 6: return TileFactory.Instance.tileNum6;
            case 7: return TileFactory.Instance.tileNum7;
            case 8: return TileFactory.Instance.tileNum8;
            default: return null;
        }
    }
}
