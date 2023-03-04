[System.Serializable]
public class LevelsData
{
    public string[] levelName;
    public bool[] completed;

    public LevelsData(LevelSelector data)
    {
        LevelData[] newData = new LevelData[levelName.Length];
        for (int i = 0; i < newData.Length; i++)
        {
            newData[i].levelName = levelName[i];
            newData[i].completed = completed[i];
        }
    }
}
