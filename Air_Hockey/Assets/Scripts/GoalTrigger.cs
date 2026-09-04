using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public string goalType;

    private bool goalActivated = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Puck"))
            return;

        if (goalActivated)
            return;

        goalActivated = true;

        GameManager gameManager =
            FindFirstObjectByType<GameManager>();

        if (gameManager != null)
        {
            gameManager.ScoreGoal(goalType);
        }

        Invoke(nameof(ResetTrigger), 0.5f);
    }

    void ResetTrigger()
    {
        goalActivated = false;
    }
}