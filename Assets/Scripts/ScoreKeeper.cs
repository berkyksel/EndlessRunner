using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public delegate void OnScoreChanged(int newVal);

    public event OnScoreChanged onScoreChanged;
    private int score;

    public void ChangeScore(int amt)
    {
        score += amt;
        Debug.Log($" {score} ");
        onScoreChanged?.Invoke(score);
    }
}
