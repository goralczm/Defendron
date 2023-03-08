using System.Collections.Generic;
using System.Collections;
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

    private Animator _anim;
    public bool isChangingScene;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        isChangingScene = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            for (int i = 1; i <= 5; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    PlayerPrefs.SetInt("act" + i + "_level" + j, 0);
                }
                PlayerPrefs.SetInt("act" + i, 0);
            }
            ChangeScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            for (int i = 1; i <= 5; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    PlayerPrefs.SetInt("act" + i + "_level" + j, 1);
                }
                PlayerPrefs.SetInt("act" + i, 1);
            }
            ChangeScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ChangeScene(string name)
    {
        Time.timeScale = 1f;

        if (isChangingScene)
            return;

        StartCoroutine(Transition(name));
    }

    IEnumerator Transition(string name)
    {
        isChangingScene = true;
        _anim.Play("out");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(name);
    }

    public void EndTranstion()
    {
        isChangingScene = false;
    }
}
