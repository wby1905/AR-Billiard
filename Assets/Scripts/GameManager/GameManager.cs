using System.Text;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public BallManager ballManager;
    public Stick stick;

    private bool isEnd;
    private int _winner;
    private float _resetTimer = 0f;
    private float _resetTime = 10f;

    public RuleEnum ruleEnum;
    private Rule rule;

    public TMP_Text playerText;
    public TMP_Text ballText;
    public TMP_Text targetText;
    public Notification notification;

    private IMixedRealitySceneSystem _sceneSystem;

    // Start is called before the first frame update
    void Start()
    {
        isEnd = false;
        switch (ruleEnum)
        {
            case RuleEnum.American:
                rule = new RuleAmerican();
                break;

            // Other rules to be implemented...
            default:
                rule = new RuleAmerican();
                break;
        }
        _sceneSystem = MixedRealityToolkit.Instance.GetService<IMixedRealitySceneSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnd)
        {
            return;
        }

        playerText.text = "Player" + rule.GetCurrentPlayer();
        ballText.text = rule.getNumberOfBallLeft().ToString();
        targetText.text = rule.GetCurrentTarget();

        ballManager.checkIsAllStop();
        if (ballManager.IsAllStop() && stick.HasShot())
        {
            int lastHit = ballManager.GetLastHit();
            int[] ballInHole = ballManager.GetBallInHole();
            Debug.Log("Last hit: " + lastHit);
            Debug.Log("Ball in hole: " + ballInHole);
            RuleResult ruleResult = rule.OnShot(lastHit, ballInHole);
            Debug.Log("Rule result: " + ruleResult.ToString());
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Player" + rule.GetCurrentPlayer() + " First hit: " + lastHit);
            if (ballInHole.Length > 0)
            {
                sb.Append("Balls goal this turn: ");
                foreach (int ball in ballInHole)
                {
                    if (ball == 0)
                    {
                        sb.Append("Cue Ball ");
                    }
                    else
                    {
                        sb.Append(ball.ToString() + " ");
                    }
                }
            }

            switch (ruleResult)
            {
                case RuleResult.WhiteBall:
                    sb.AppendLine("You hit the white ball!");
                    ballManager.ReplaceWhiteBall();
                    break;
                case RuleResult.Foul:
                    sb.AppendLine("You fouled!");
                    break;
                case RuleResult.Win:
                    sb.AppendLine("You Win!");
                    isEnd = true;
                    _winner = rule.GetCurrentPlayer();
                    _resetTimer = _resetTime;
                    break;
                case RuleResult.Lose:
                    sb.AppendLine("You Lose!");
                    isEnd = true;
                    _winner = rule.GetCurrentPlayer();
                    _resetTimer = _resetTime;
                    break;
                case RuleResult.ContinueHit:
                    sb.AppendLine("Continue shot!");
                    break;
                case RuleResult.ChangePlayer:
                    sb.AppendLine("Now change player!");
                    break;
                default:
                    sb.AppendLine("Error");
                    break;
            }

            notification.SetText(sb.ToString());
            if (!ballManager.isReplacing && !isEnd)
            {
                stick.hasShot = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (_resetTimer > 0f)
        {
            _resetTimer -= Time.fixedDeltaTime;
            if (_resetTimer < _resetTime - 3f)
                notification.SetText("Player" + _winner + " is the winner.\nReset in " + _resetTimer.ToString("0.0") + "s");
        }
        else if (isEnd)
        {
            ResetGame();
        }
    }


    public void ResetGame()
    {
        _sceneSystem.UnloadContent("ARPoolGame");
    }

}
