using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void UpdatePlayerPrefs()
    {
        if (PlayerPrefs.GetInt("LevelComplete") < Mathf.Clamp(LevelController.currentLevelIndex + 1, 0, LevelController.Instance.maxLevels - 1))
        {
            PlayerPrefs.SetInt("LevelComplete", LevelController.currentLevelIndex + 1);
            LoadLevelSystem.Instance.Refresh();
        }
    }

}
