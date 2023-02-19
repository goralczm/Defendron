using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    public int procsDestroyed;
    public int towersPlaced;
    public int moneyEarned;
    public int wavesCompleted;
    public int looses;

    private void SaveStatistics()
    {
        StatisticsData data = new StatisticsData(this);
        SaveSystem.SaveData(data, "statistics");
    }

    private void LoadStatistics()
    {
        StatisticsData data = SaveSystem.LoadData("statistics") as StatisticsData;

        if (data == null)
            return;

        procsDestroyed = data.procsDestroyed;
        towersPlaced = data.towersPlaced;
        moneyEarned = data.moneyEarned;
        wavesCompleted = data.wavesCompleted;
        looses = data.looses;
    }

    private void ResetStatistics()
    {
        procsDestroyed = 0;
        towersPlaced = 0;
        moneyEarned = 0;
        wavesCompleted = 0;
        looses = 0;
    }
}
