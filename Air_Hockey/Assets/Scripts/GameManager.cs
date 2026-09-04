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

        if (goalType == "TopGoal")
        {
            // A bola entrou no gol de cima
            // Jogador de baixo marca
            playerScore++;
        }
        else if (goalType == "BottomGoal")
        {
            // A bola entrou no gol de baixo
            // IA marca
            aiScore++;
        }

        Debug.Log(
            "Player: " + playerScore +
            " | AI: " + aiScore
        );

        CheckWinner();

if (!gameOver && theBall != null)
{
    PuckControl puckControl =
        theBall.GetComponent<PuckControl>();

    if (puckControl != null)
    {
        puckControl.RestartGame();
    }
}
    }

    void CheckWinner()
    {
        if (playerScore >= 10)
        {
            gameOver = true;
        }
        else if (aiScore >= 10)
        {
            gameOver = true;
        }
    }

    void OnGUI()
    {
        if (layout != null)
            GUI.skin = layout;

        // Jogador
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
            string winner =
                playerScore >= 10
                ? "PLAYER WINS!"
                : "AI WINS!";

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

        // Botão de restart durante o jogo
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
        playerScore = 0;
        aiScore = 0;
        gameOver = false;

        if (theBall != null)
        {
            PuckControl puckControl =
                theBall.GetComponent<PuckControl>();

            if (puckControl != null)
            {
                puckControl.RestartGame();
            }
        }
    }
}