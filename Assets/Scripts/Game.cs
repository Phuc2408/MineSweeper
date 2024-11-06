
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int width = 16;
    public int height = 16;
    public int mineCount = 32;

    private Board board;
    private bool gameover;

    private void Awake()
    {
        board = GetComponentInChildren<Board>();
    }
    private void Start()
    {
        board.init(width, height, mineCount);
        gameover = false;
    }

   
    private void Update()
    {
        if (gameover) return;
        if (Input.GetKeyDown(KeyCode.R)) board.init(width, height, mineCount);
        if (Input.GetMouseButtonDown(1)) Flag();
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition);
            Cell cell = board.getCell(cellPosition.x, cellPosition.y);
            if (cell.type == Cell.Type.Invalid || cell.revealed == 1 || cell.flagged) return;
            Board.BoardResult result= board.Reveal(cellPosition.x, cellPosition.y);
            board.Draw();
            switch (result)
            {
                case Board.BoardResult.OVER:
                    Debug.Log("Over");
                    break;
                case Board.BoardResult.WIN:
                    Debug.Log("Win");
                    break;
                default:
                    break;
            }
        }
    }
    private void Flag()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition);
        Cell cell = board.getCell(cellPosition.x, cellPosition.y);

        if (cell.type == Cell.Type.Invalid || cell.revealed == 1) {
            return;
        }

        cell.flagged = !cell.flagged;
        board.setCell(cellPosition.x, cellPosition.y, cell);
        board.Draw();
    }
}
