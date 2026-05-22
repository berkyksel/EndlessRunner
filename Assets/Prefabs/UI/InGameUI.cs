using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ScoreKeeper scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
        if (scoreKeeper != null)
        {
            scoreKeeper.onScoreChanged += UpdateScoreText;
        }
    }

    private void UpdateScoreText(int newVal)
    {
        ScoreText.SetText($"Score: {newVal}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
