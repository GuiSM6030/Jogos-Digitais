using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public string goalType;

    void Start()
    {
        Debug.Log("GOAL TRIGGER ATIVO: " + gameObject.name);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TRIGGER DETECTOU: " + other.gameObject.name);

        if (other.CompareTag("Puck"))
        {
            Debug.Log("PUCK ENTROU NO GOL: " + goalType);

            GameManager gameManager = FindFirstObjectByType<GameManager>();

            if (gameManager != null)
            {
                gameManager.ScoreGoal(goalType);
            }
        }
    }

    public void ResetGoal()
    {
    }
}