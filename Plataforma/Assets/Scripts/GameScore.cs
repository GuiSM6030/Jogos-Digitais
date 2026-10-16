using UnityEngine;

public static class GameScore
{
    public static int CurrentScore { get; private set; }

    public static void AddPoints(int points)
    {
        if (points <= 0)
        {
            Debug.LogWarning("A pontuacao adicionada precisa ser maior que zero.");
            return;
        }

        CurrentScore = checked(CurrentScore + points);
    }

    public static void Reset()
    {
        CurrentScore = 0;
    }
}
