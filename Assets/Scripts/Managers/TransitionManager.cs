using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    #region Singleton

    public static TransitionManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    /*private void Start()
    {
        for (int i = 1; i <= 10; i++)
        {
            PlayerPrefs.SetInt("act1_level" + i, 0);
        }
        PlayerPrefs.SetInt("act1", 0);
        PlayerPrefs.SetInt("act2", 0);
        PlayerPrefs.SetInt("act3", 0);
        PlayerPrefs.SetInt("act4", 0);
        PlayerPrefs.SetInt("act5", 0);
    }*/

    public void ChangeScene(string name)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(name);
    }
}
