using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int PlayerScore1 = 0;
    public static int PlayerScore2 = 0;

    public GUISkin layout;

    private GameObject puck;
    private Collider2D playerGoal;
    private Collider2D aiGoal;

    private bool goalBeingScored = false;

    void Start()
    {
        PlayerScore1 = 0;
        PlayerScore2 = 0;

        puck = GameObject.Find("puck_0");

        GameObject playerGoalObject = GameObject.Find("PlayerGoal");
        GameObject aiGoalObject = GameObject.Find("AIGoal");

        if (playerGoalObject != null)
        {
            playerGoal = playerGoalObject.GetComponent<Collider2D>();
        }

        if (aiGoalObject != null)
        {
            aiGoal = aiGoalObject.GetComponent<Collider2D>();
        }

        Debug.Log("GameManager iniciado");

        if (puck == null)
            Debug.LogError("ERRO: puck_0 não encontrado!");

        if (playerGoal == null)
            Debug.LogError("ERRO: PlayerGoal não encontrado!");

        if (aiGoal == null)
            Debug.LogError("ERRO: AIGoal não encontrado!");
    }

    void Update()
    {
        if (puck == null)
            return;

        if (goalBeingScored)
            return;

        Vector2 puckPosition = puck.transform.position;

        // Verifica se o puck entrou no gol do jogador
        if (playerGoal != null && playerGoal.bounds.Contains(puckPosition))
        {
            Score("PlayerGoal");
            return;
        }

        // Verifica se o puck entrou no gol da IA
        if (aiGoal != null && aiGoal.bounds.Contains(puckPosition))
        {
            Score("AIGoal");
            return;
        }
    }

    public static void Score(string goalID)
    {
        GameManager manager = FindObjectOfType<GameManager>();

        if (manager != null)
        {
            manager.RegisterScore(goalID);
        }
    }

    void RegisterScore(string goalID)
    {
        if (goalBeingScored)
            return;

        goalBeingScored = true;

        if (goalID == "PlayerGoal")
        {
            // A bola entrou no gol do jogador
            // Ponto para a IA
            PlayerScore2++;

            Debug.Log("GOL! Ponto para PLAYER 2");
            Debug.Log("PLACAR: " + PlayerScore1 + " x " + PlayerScore2);
        }
        else if (goalID == "AIGoal")
        {
            // A bola entrou no gol da IA
            // Ponto para o jogador
            PlayerScore1++;

            Debug.Log("GOL! Ponto para PLAYER 1");
            Debug.Log("PLACAR: " + PlayerScore1 + " x " + PlayerScore2);
        }

        ResetPuck();
    }

    void ResetPuck()
    {
        if (puck == null)
            return;

        PuckControl puckControl = puck.GetComponent<PuckControl>();

        if (puckControl != null)
        {
            puckControl.ResetBall();
        }

        // Espera um pequeno tempo antes de liberar a próxima jogada
        Invoke("StartNextBall", 0.5f);
    }

    void StartNextBall()
    {
        if (puck == null)
            return;

        if (PlayerScore1 >= 10 || PlayerScore2 >= 10)
        {
            return;
        }

        PuckControl puckControl = puck.GetComponent<PuckControl>();

        if (puckControl != null)
        {
            puckControl.StartBall();
        }

        goalBeingScored = false;
    }

    void OnGUI()
    {
        if (layout != null)
        {
            GUI.skin = layout;
        }

        // PLACAR PLAYER 1
        GUI.Label(
            new Rect(Screen.width / 2 - 162, 20, 100, 100),
            "" + PlayerScore1
        );

        // PLACAR PLAYER 2
        GUI.Label(
            new Rect(Screen.width / 2 + 162, 20, 100, 100),
            "" + PlayerScore2
        );

        // BOTÃO RESTART
        if (GUI.Button(
            new Rect(Screen.width / 2 - 60, 35, 120, 53),
            "RESTART"))
        {
            PlayerScore1 = 0;
            PlayerScore2 = 0;

            CancelInvoke("StartNextBall");

            goalBeingScored = false;

            if (puck != null)
            {
                PuckControl puckControl = puck.GetComponent<PuckControl>();

                if (puckControl != null)
                {
                    puckControl.ResetBall();
                    puckControl.StartBall();
                }
            }
        }

        // VITÓRIA PLAYER 1
        if (PlayerScore1 >= 10)
        {
            GUI.Label(
                new Rect(Screen.width / 2 - 150, 200, 2000, 100),
                "PLAYER ONE WINS"
            );

            if (puck != null)
            {
                PuckControl puckControl = puck.GetComponent<PuckControl>();

                if (puckControl != null)
                {
                    puckControl.ResetBall();
                }
            }
        }

        // VITÓRIA PLAYER 2
        else if (PlayerScore2 >= 10)
        {
            GUI.Label(
                new Rect(Screen.width / 2 - 150, 200, 2000, 100),
                "PLAYER TWO WINS"
            );

            if (puck != null)
            {
                PuckControl puckControl = puck.GetComponent<PuckControl>();

                if (puckControl != null)
                {
                    puckControl.ResetBall();
                }
            }
        }
    }
}