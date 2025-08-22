using UnityEngine;
using UnityEngine.Tilemaps;

public class MineCell : Cell
{
    public bool isExploded;
    public MineCell()
    {
        this.isExploded = false;
    }
    public override Tile GetRevealedTile()
    {
        if (isExploded)
            return TileFactory.Instance.tileExploded;
        return TileFactory.Instance.tileMine;
    }
    public override Type getType()
    {
        return Type.Mine;
    }
}