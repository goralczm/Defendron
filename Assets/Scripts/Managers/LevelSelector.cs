using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[System.Serializable]
public class Act
{
    public string name;
    public Transform levelsParent;
    public GameObject lockedPanel;
    public TextMeshProUGUI actTitle;
    public bool completed;
}

public class LevelSelector : MonoBehaviour
{
    public List<Act> acts;

    private void Start()
    {
        for (int i = 1; i <= acts.Count; i++)
        {
            acts[i - 1].actTitle.text = "Act " + i + " - " + acts[i - 1].name;

            Button[] buttons = acts[i - 1].levelsParent.GetComponentsInChildren<Button>();
            for (int j = 1; j <= buttons.Length; j++)
            {
                buttons[j - 1].onClick.RemoveAllListeners();
                string levelName = "act" + i + "_level" + j;
                buttons[j - 1].onClick.AddListener(delegate { TransitionManager.instance.ChangeScene(levelName); });

                if (j == 1)
                {
                    if (i == 1)
                    {
                        buttons[j - 1].interactable = true;
                        continue;
                    }

                    string previousAct = "act" + (i - 1).ToString();
                    buttons[j - 1].interactable = Convert.ToBoolean(PlayerPrefs.GetInt(previousAct, 0));
                    continue;
                }

                string previousLevel = "act" + i + "_level" + (j - 1).ToString();
                buttons[j - 1].interactable = Convert.ToBoolean(PlayerPrefs.GetInt(previousLevel, 0));
            }

            if (i > 1)
            {
                acts[i - 1].lockedPanel.SetActive(true);
                continue;
            }

            if (i > 1 && !Convert.ToBoolean(PlayerPrefs.GetInt("act" + (i - 1).ToString())))
                acts[i - 1].lockedPanel.SetActive(true);
        }
    }
}
