


public class Rule
{
    public virtual RuleResult OnShot(int firstHit, int[] ballList)
    {

        return RuleResult.Error;
    }

    public virtual int GetCurrentPlayer()
    {
        // 1: player1
        // 2: player2
        return 0;
    }

    public virtual int getNumberOfBallLeft()
    {
        return 0;
    }

    public virtual string GetCurrentTarget()
    {
        return "";
    }
}