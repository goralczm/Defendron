using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        for (int i = 0; i < acts.Count; i++)
        {
            acts[i].actTitle.text = "Act " + i + " - " + acts[i].name;

            Button[] buttons = acts[i].levelsParent.GetComponentsInChildren<Button>();
            for (int j = 0; j < buttons.Length; j++)
            {
                buttons[j].onClick.RemoveAllListeners();
                string levelName = "act" + i + "_level" + (j + 1).ToString();
                buttons[j].onClick.AddListener(delegate { TransitionManager.instance.ChangeScene(levelName); });
            }

            if (i > 0 && !acts[i - 1].completed)
                acts[i].lockedPanel.SetActive(true);
        }
    }
}
