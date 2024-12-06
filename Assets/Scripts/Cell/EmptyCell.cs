using UnityEngine;
using UnityEngine.Tilemaps;
public class EmptyCell : Cell
{
    public EmptyCell()
    {
    }
    public override Tile GetRevealedTile()
    {
        return TileFactory.Instance.tileEmpty;
    }

    public override Type getType()
    {
        return Type.Empty;
    }
}