using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoadLevelSystem : MonoBehaviour
{
    public static LoadLevelSystem Instance;
     public List<Button> buttons;

    int levelComplete
    {
        get
        {
            return PlayerPrefs.GetInt("LevelComplete");
        }
        set
        {
            PlayerPrefs.SetInt("LevelComplete", value);
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        TurnOffButtons(buttons.Count);

        Refresh();
    }

    public void Refresh()
    {
        switch (levelComplete)
        {
            case 1:
                TurnOnButtons(2);
                break;
            case 2:
                TurnOnButtons(3);
                break;
            case 3:
                TurnOnButtons(4);
                break;
            case 4:
                TurnOnButtons(5);
                break;
            case 5:
                TurnOnButtons(6);
                break;
            case 6:
                TurnOnButtons(7);
                break;
            case 7:
                TurnOnButtons(8);
                break;
            case 8:
                TurnOnButtons(9);
                break;
            case 9:
                TurnOnButtons(10);
                break;
            case 10:
                TurnOnButtons(11);
                break;
            case 11:
                TurnOnButtons(12);
                break;
            case 12:
                TurnOnButtons(13);
                break;
            case 13:
                TurnOnButtons(14);
                break;
            case 14:
                TurnOnButtons(15);
                break;
            case 15:
                TurnOnButtons(15);
                break;
        }
    }
    
    private void TurnOffButtons(int toWhatButton)
    {
        for (int i = 1; i < toWhatButton; i++)
        {
            buttons[i].interactable = false;
        }
    }

    private void TurnOnButtons(int toWhatButton)
    {
        for (int i = 0; i < toWhatButton; i++)
        {
            buttons[i].interactable = true;
        }
    }

}
