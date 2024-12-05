
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class Game : MonoBehaviour
{
    public int width = 16;
    public int height = 16;
    public int mineCount = 32;
    public GameOverScreen gameOverScreen;
    public WinScreen winScreen;
    public Timer timer;
    private Board board;
    private bool paused;
    private bool gameover;
    public delegate void InitDelegate();
    public int hintCount = 2;
    public Popup popupController;
    public SaveBestResult saveBestResult;
    public void setPauseAtFalse() {  paused = false; }
    private void Awake()
    {
        board = GetComponentInChildren<Board>();
    }
    private void Start()
    {
        Debug.Log("start game");
        _start();
        gameOverScreen.setInitDelegate(new InitDelegate(_start));
        winScreen.setInitDelegate(new InitDelegate(_start));
    }
    public void _start()
    {
        board.init(width, height, mineCount);
        hintCount = 2;
        gameover = false;
        paused = false;
        timer.ResetTime();
        timer.Running();
    }
    public void Update()
    {
        if (gameover || paused) return;
        if (Input.GetKeyDown(KeyCode.R))
        {
            _start();
        }
        if (Input.GetMouseButtonDown(1)) Flag();
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition);
            Cell cell = board.getCell(cellPosition.x, cellPosition.y);
            if (cell.type == Cell.Type.Invalid || cell.revealed == 1 || cell.flagged) return;
            Board.BoardResult result = board.Reveal(cellPosition.x, cellPosition.y);
            board.Draw();
            checkWinCondition(result);
        }
    }
    public Board.BoardResult checkWinCondition(Board.BoardResult? result = null)
    {
        if (result == null)
            result = board.CheckWinCondition();
        Board.BoardResult? a = null;
        switch (result)
        {
            case Board.BoardResult.OVER:
                gameover = true;
                timer.NotRunning();
                gameOverScreen.setUp(timer.GetTimer(),width,height);
                a = Board.BoardResult.OVER;
                break;
            case Board.BoardResult.WIN:
                gameover = true;
                timer.NotRunning();
                saveBestResult.saveData(width, height, timer.GetTimer());
                winScreen.setUp(timer.GetTimer(), width, height);
                a = Board.BoardResult.WIN;
                break;
            default:
                a = Board.BoardResult.RUNNING;
                break;
        }

        return a ?? Board.BoardResult.RUNNING;
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
   
    public void click()
    {
        if (hintCount == 0)
        {
            paused = true;
            popupController.ShowPopup("No more hints left!");
            return;
        }
        board.showUnRevealedCell();
        Board.BoardResult a = checkWinCondition();
        Debug.Log(a);
        if(a == Board.BoardResult.WIN)
           popupController.hidePopup();
        hintCount--;
        if (hintCount == 0)
        {
            paused = true;
            popupController.ShowPopup("No more hints left!");
            return;
        }
        
        paused = true;
        Update();
        popupController.ShowPopup("You have " + hintCount + " hints left!");
    }
}
