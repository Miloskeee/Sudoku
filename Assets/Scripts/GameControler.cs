using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControler : MonoBehaviour
{
    public enum GAME
    {
        MENU = 0,
        READY = 1,
        PLAYING = 2,
        GAME_OVER = 3
    }

    public enum DIFF
    {
        EASY = 0,
        HARD = 1
    }

    private int score;

    [SerializeField]
    private GAME currentState = GAME.MENU;
    [SerializeField]
    private DIFF currentDiff = DIFF.EASY;
    [SerializeField]
    private Sprite easy;
    [SerializeField]
    private Sprite hard;
    [SerializeField]
    private Button easyButton;
    [SerializeField]
    private Button hardButton;
    [SerializeField]
    private Button diffButton;
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private GameObject playPanel;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private SudokuControler sudoku;
    [SerializeField]
    private Text youTriedScore;
    [SerializeField]
    private Text correctFieldsScore;
    [SerializeField]
    private Text totalScore;

    [SerializeField]
    private GameObject scoreLabels;

    //Biramo tezinu,lako ili tesko,razlika je u popunjenim poljima
    public void ChangeDiff()
    {
        if (currentDiff == DIFF.EASY)
        {
            currentDiff = DIFF.HARD;
            hardButton.gameObject.SetActive(true);
            easyButton.gameObject.SetActive(false);
            sudoku.setK(40);
            sudoku.ResetValues();
        }
        else
        {
            currentDiff = DIFF.EASY;
            hardButton.gameObject.SetActive(false);
            easyButton.gameObject.SetActive(true);
            sudoku.setK(20);
            sudoku.ResetValues();

        }



    }


    //Setujemo scene
    public void SetGameState(GAME state)
    {
        switch (state)
        {
            case GAME.MENU:
                menuPanel.SetActive(true);
                gameOverPanel.SetActive(false);
                playPanel.SetActive(false);
                break;
            case GAME.READY:
                menuPanel.SetActive(false);
                gameOverPanel.SetActive(false);
                playPanel.SetActive(true);
                break;
            case GAME.GAME_OVER:
                menuPanel.SetActive(false);
                gameOverPanel.SetActive(true);
                playPanel.SetActive(true);
                sudoku.fillValues();
                break;
            case GAME.PLAYING:
                break;


        }


    }

    //Setujemo labele na kraju igre
    public void SetLabels(int tried, int correctFields, int sum)
    {
        scoreLabels.SetActive(true);
        youTriedScore.text = tried.ToString();
        correctFieldsScore.text = correctFields + " x 100";
        totalScore.text = sum.ToString();

    }



    public void toReady()
    {
        SetGameState(GAME.READY);
    }

    public void toMenu()
    {
        SetGameState(GAME.MENU);
    }
    public void GameOver()
    {
        CalculateScore();
    }
    public void toGameOver()
    {
        SetGameState(GAME.GAME_OVER);
    }

    public void Play()
    {
        score = 0;
        Invoke("toReady", 1f);
    }

    //Iyracunavanje score na kraju
    public void CalculateScore()
    {
        int correctFields = sudoku.getK() - sudoku.missingFieldsCount();
        int tried = 100;
        score = tried + Mathf.Abs(correctFields) * 100;
        SetLabels(tried,correctFields,score);
        Invoke("toGameOver", 0.5f);
    }

    public bool isEasy()
    {
        if (currentDiff == DIFF.EASY)
        {
            return true;
        }
        else
        { return false; }
    }


}
