using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public string goalType;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TRIGGER ATIVADO! Entrou: " + other.gameObject.name);

        if (other.CompareTag("Puck"))
        {
            Debug.Log("PUCK ENTROU NO GOL: " + goalType);

            GameManager.Score(goalType);

            PuckControl puck = other.GetComponent<PuckControl>();

            if (puck != null)
            {
                puck.ResetBall();
                puck.StartBall();
            }
        }
    }
}