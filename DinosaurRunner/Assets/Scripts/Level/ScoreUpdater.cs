public class ScoreUpdater
{
    private float _updateScoreCoefficient;
    private float _score;

    public ScoreUpdater(float updateScoreCoefficient)
    {
        _updateScoreCoefficient = updateScoreCoefficient;
    }

    public void UpdateScore()
    {
        _score += 1 * _updateScoreCoefficient;
    }

    public void ResetScore()
    {
        _score = 0;
    }

    public int GetIntegerScore()
    {
        return (int)_score;
    }
}