using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleAmerican : Rule
{
    int[] player1List = { };
    int[] player2List = { };
    int player1 = 7;
    int player2 = 7;
    int curPlayer = 1;

    bool prevFoul = false;
    bool isInitialized = false;

    int[] shixing = { 1, 2, 3, 4, 5, 6, 7 };
    int[] huaqiu = { 9, 10, 11, 12, 13, 14, 15 };

    public RuleAmerican()
    {
    }

    bool containBothColor(int[] ballList)
    {
        bool hua = false, shi = false;
        for (int i = 0; i < ballList.Length; i++)
        {
            if (i == 0) return false;
            else if (i > 0 && i < 8) hua = true;
            else if (i > 8 && i < 16) shi = true;

        }
        return hua && shi;
    }
    void updateList(int[] ballList)
    {
        if (ballList.Length == 0) return;

        int currentPlayer = GetCurrentPlayer();
        if (player1List.Length == 0 && player2List.Length == 0)
        {
            if (!containBothColor(ballList))
            {
                if (ballList[0] >= 1 && ballList[0] <= 7)
                {
                    if (currentPlayer == 1)
                    {
                        player1List = shixing;
                        player2List = huaqiu;
                    }
                    else
                    {
                        player1List = huaqiu;
                        player2List = shixing;
                    }
                }
                else if (ballList[0] >= 9 && ballList[0] <= 15)
                {
                    if (currentPlayer == 1)
                    {
                        player1List = huaqiu;
                        player2List = shixing;
                    }
                    else
                    {
                        player1List = shixing;
                        player2List = huaqiu;
                    }
                }
            }

            // more than one ball get into hole, different colors
            else
            {
                if (currentPlayer == 1)
                {
                    player1List = shixing;
                    player2List = huaqiu;
                }
                else
                {
                    player1List = huaqiu;
                    player2List = shixing;
                }
            }

            isInitialized = true;
        }

        foreach (int ball in ballList)
        {
            if (0 < ball && ball < 8)
            {
                if (getList() == shixing)
                {
                    player1--;
                }
                else
                {
                    player2--;
                }
            }
            else if (8 < ball && ball < 16)
            {
                if (getList() == huaqiu)
                {
                    player1--;
                }
                else
                {
                    player2--;
                }
            }
        }



    }

    // check whether the ballList contains a correct ball
    bool checkCorrect(int[] ballList)
    {
        int[] correctList = getList();
        for (int i = 0; i < ballList.Length; i++)
        {
            if (correctList[0] <= ballList[i] && ballList[i] <= correctList[6]) return true;
        }
        return false;
    }

    //switch player to the other
    void switchPlayer()
    {
        int player = GetCurrentPlayer();
        if (player == 1)
        {
            curPlayer = 2;
        }
        else
        {
            curPlayer = 1;
        }
    }

    int[] getList()
    {
        if (GetCurrentPlayer() == 1) return player1List;
        return player2List;
    }
    bool checkBallListContain(int num, int[] ballList)
    {
        for (int i = 0; i < ballList.Length; i++)
        {
            if (ballList[i] == num) return true;
        }
        return false;
    }
    public override int getNumberOfBallLeft()
    {
        if (GetCurrentPlayer() == 1) return player1;
        return player2;
    }

    public override RuleResult OnShot(int firstHit, int[] ballList)
    {

        updateList(ballList);


        // 5: lose
        // switch player to return the winner
        // white and black all in hole
        if (checkBallListContain(8, ballList) && checkBallListContain(0, ballList))
        {
            switchPlayer();
            return RuleResult.Lose;
        }
        // black in hole, but correct balls are not empty
        else if (checkBallListContain(8, ballList) && getNumberOfBallLeft() != 0)
        {
            switchPlayer();
            return RuleResult.Lose;
        }
        // 4: win; black ball in hole and correct balls are empty
        else if (checkBallListContain(8, ballList) && getNumberOfBallLeft() == 0)
        {
            return RuleResult.Win;
        }
        // 0: baiqiu
        if (checkBallListContain(0, ballList))
        {
            prevFoul = false;
            switchPlayer();
            return RuleResult.WhiteBall;
        }

        // no foul at first hit with different color
        if (isInitialized)
        {
            isInitialized = false;
            return RuleResult.ContinueHit;
        }

        // 1: fangui
        int[] correctList = getList();
        if (firstHit == -1 || correctList.Length > 0 && !(correctList[0] <= firstHit && firstHit <= correctList[6]))
        {
            prevFoul = true;
            switchPlayer();
            return RuleResult.Foul;
        }


        if (correctList.Length > 0 && checkCorrect(ballList)) return RuleResult.ContinueHit;

        if (!prevFoul)
        {
            switchPlayer();
            return RuleResult.ChangePlayer;
        }
        else
        {
            prevFoul = false;
            return RuleResult.ContinueHit;
        }
    }

    //player1 = 1
    //player2 = 2
    public override int GetCurrentPlayer()
    {
        return curPlayer;
    }

    public override string GetCurrentTarget()
    {
        int[] list = getList();
        string result = "";
        if (list.Length == 0) result = "EightBall!";
        if (player1List.Length == 0 && player2List.Length == 0) result = "EveryBall";
        if (list == shixing) result = "Solids";
        if (list == huaqiu) result = "Stripes";
        if (prevFoul) result += "\n (Shot Twice)";
        return result;
    }
}
