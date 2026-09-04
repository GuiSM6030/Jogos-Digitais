using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int playerScore = 0;
    public int aiScore = 0;

    public GUISkin layout;
    public GameObject theBall;

    private bool gameOver = false;

    void Start()
    {
        theBall = GameObject.FindGameObjectWithTag("Puck");

        playerScore = 0;
        aiScore = 0;
    }

    public void ScoreGoal(string goalType)
    {
        if (gameOver)
            return;

        if (goalType == "AIGoal")
        {
            playerScore++;

            Debug.Log(
                "PLAYER MARCOU! Placar: " +
                playerScore + " x " + aiScore
            );
        }
        else if (goalType == "PlayerGoal")
        {
            aiScore++;

            Debug.Log(
                "IA MARCOU! Placar: " +
                playerScore + " x " + aiScore
            );
        }
        else
        {
            Debug.LogError(
                "Goal Type inválido: " + goalType
            );

            return;
        }

        // Verifica vitória
        if (playerScore >= 10)
        {
            gameOver = true;
            ResetBall();
            return;
        }

        if (aiScore >= 10)
        {
            gameOver = true;
            ResetBall();
            return;
        }

        // Reseta a bola
        ResetBall();

        // Libera os gols novamente
        ResetGoals();

        // Lança novamente depois de 1 segundo
        Invoke(nameof(RestartBall), 1f);
    }

    void ResetBall()
    {
        if (theBall == null)
            return;

        PuckControl puck =
            theBall.GetComponent<PuckControl>();

        if (puck != null)
        {
            puck.ResetBall();
        }
    }

    void RestartBall()
    {
        if (gameOver)
            return;

        if (theBall == null)
            return;

        PuckControl puck =
            theBall.GetComponent<PuckControl>();

        if (puck != null)
        {
            puck.StartBall();
        }
    }

    void ResetGoals()
    {
        GoalTrigger[] goals =
            FindObjectsByType<GoalTrigger>(
                FindObjectsSortMode.None
            );

        foreach (GoalTrigger goal in goals)
        {
            goal.ResetGoal();
        }
    }

    void OnGUI()
    {
        if (layout != null)
            GUI.skin = layout;

        // Player
        GUI.Label(
            new Rect(
                Screen.width / 2 - 150,
                20,
                100,
                100
            ),
            playerScore.ToString()
        );

        // IA
        GUI.Label(
            new Rect(
                Screen.width / 2 + 150,
                20,
                100,
                100
            ),
            aiScore.ToString()
        );

        if (gameOver)
        {
            string winner;

            if (playerScore >= 10)
                winner = "PLAYER WINS!";
            else
                winner = "AI WINS!";

            GUI.Label(
                new Rect(
                    Screen.width / 2 - 150,
                    Screen.height / 2 - 50,
                    400,
                    100
                ),
                winner
            );

            if (GUI.Button(
                new Rect(
                    Screen.width / 2 - 60,
                    Screen.height / 2 + 40,
                    120,
                    50
                ),
                "RESTART"
            ))
            {
                RestartGame();
            }

            return;
        }

        if (GUI.Button(
            new Rect(
                Screen.width / 2 - 60,
                35,
                120,
                53
            ),
            "RESTART"
        ))
        {
            RestartGame();
        }
    }

    void RestartGame()
    {
        CancelInvoke();

        playerScore = 0;
        aiScore = 0;
        gameOver = false;

        ResetGoals();
        ResetBall();

        Invoke(nameof(RestartBall), 1f);
    }
}