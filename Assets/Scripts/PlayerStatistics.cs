[System.Serializable]
public class PlayerStatistics
{
    public int procsDestroyed;
    public int towersPlaced;
    public int moneyEarned;
    public int wavesCompleted;
    public int looses;

    public PlayerStatistics(StatisticsManager stats)
    {
        procsDestroyed = stats.procsDestroyed;
        towersPlaced = stats.towersPlaced;
        moneyEarned = stats.moneyEarned;
        wavesCompleted = stats.wavesCompleted;
        looses = stats.looses;
    }
}
