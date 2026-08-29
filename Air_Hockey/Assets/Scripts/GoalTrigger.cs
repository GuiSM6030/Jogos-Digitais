using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public string goalType;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Puck"))
        {
            GameObject gameManager = GameObject.Find("GameManager");
            if (gameManager != null)
            {
                gameManager.SendMessage("ScoreGoal", goalType);
            }
        }
    }
}