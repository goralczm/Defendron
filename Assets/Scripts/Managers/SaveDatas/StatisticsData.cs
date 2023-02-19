[System.Serializable]
public class StatisticsData
{
    public int procsDestroyed;
    public int towersPlaced;
    public int moneyEarned;
    public int wavesCompleted;
    public int looses;

    public StatisticsData(StatisticsManager stats)
    {
        procsDestroyed = stats.procsDestroyed;
        towersPlaced = stats.towersPlaced;
        moneyEarned = stats.moneyEarned;
        wavesCompleted = stats.wavesCompleted;
        looses = stats.looses;
    }
}
