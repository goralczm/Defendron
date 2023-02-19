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

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
