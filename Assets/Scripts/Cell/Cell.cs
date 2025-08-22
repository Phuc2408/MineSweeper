using System;
using UnityEngine;
using UnityEngine.Tilemaps;
public abstract class Cell:MonoBehaviour
{
    public enum Type 
    {
        Empty,
        Mine,
        Number,
    }
    public Vector3Int position;
    private float revealed;
    public bool flagged;
    public abstract Type getType();
    public abstract Tile GetRevealedTile();

    public float isRevealed()
    {
        return revealed;
    }

    public void setRevealed(float value)
    {
        this.revealed = value;
    }

    public Tile GetTile()
    {
        if (flagged) return TileFactory.Instance.tileFlag;
        if (revealed == 0.5f) return TileFactory.Instance.tileUnknownHaftOpacity;
        if (revealed == 1)
        {
            Tile tile =  GetRevealedTile();
            print(tile);
            return tile;
        }
        else
        {
            return  TileFactory.Instance.tileUnknown;
        }
    }
}