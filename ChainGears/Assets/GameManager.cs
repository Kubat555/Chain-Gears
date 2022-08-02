using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject winParticles;

    [SerializeField] private Animator textAnim;

    [SerializeField] private TextMeshProUGUI attemptsText;
    [HideInInspector] public int attempts = 3;

    public static bool isWin;
    public static bool isTwisted;
    public static bool inGame;
    ChainManager chainManager;
    private void Start()
    {   
        chainManager = FindObjectOfType<ChainManager>();
        GlobalEventManager.OnWinGame.AddListener(WinGame);
        GlobalEventManager.OnLoseGame.AddListener(LoseGame);
        GlobalEventManager.OnEndDrawing.AddListener(CheckConnectedChain);
        GlobalEventManager.OnChainBreaks.AddListener(ShowConnectChainText);
        isWin = false;
        isTwisted = false;
        inGame = false;
        ChainManager.isCollision = false;
    }

    [ContextMenu( "Rotate")]
    private void CheckConnectedChain()
    {
        if (!ChainManager.isCollision)
        {
            GlobalEventManager.OnChainBreaks.Invoke();
        } 
    }
    public void CheckResults()
    { 
       if (isTwisted)
           GlobalEventManager.OnChainBreaks.Invoke();
       else
           GlobalEventManager.OnWinGame.Invoke(); 
    }

    private void WinGame()
    {
        isWin = true;
        inGame = false;

        StartCoroutine(TimerForWin());
        HidePanel(inGamePanel, 0.5f);
    }

    private void LoseGame()
    {
        /*
        ShowPanel(losePanel, 1.4f, "bounce"); 
        HidePanel(inGamePanel, 0.3f);
        */

        isWin = false;
        isTwisted = false; 
        ChainManager.isCollision = false;
        inGame = false;
    }


    public void StartGame(int level)
    {
        attempts = 3;
        attemptsText.text = "ATTEMPTS: " + attempts.ToString();

        HidePanel(levelSelectPanel, 0.5f);
        ShowPanel(inGamePanel, 0.8f, "bounce");

        StartCoroutine(TimerForGame());

        LevelController.Instance.StartLevel(level);
    }

    public void ReloadScene()
    {
        StartCoroutine(TimerForGame());
        chainManager.DestroyAll();
        ChainManager.chainParentList.Clear();
        GearScript.isRotate = false;
        LevelController.Instance.StartLevel(LevelController.currentLevelIndex);
        ShowPanel(inGamePanel, 1f, "bounce");
    }

    public void StartRotate()
    {
        GlobalEventManager.OnRotateStart.Invoke();
    }

    public void FromMainMenuToLevelSelect()
    {
        HidePanel(mainMenuPanel, 0.5f);
        ShowPanel(levelSelectPanel, 0.8f);
    }

    public void FromLevelSelectToMainmenu()
    {
        HidePanel(levelSelectPanel, 0.5f);
        ShowPanel(mainMenuPanel, 0.8f);
    }

    #region Coroutines
    IEnumerator TimerForWin()
    {
        yield return new WaitForSeconds(1);
        Instantiate(winParticles);
        ShowPanel(winPanel, 1.4f, "bounce"); 
    }

    IEnumerator TimerForGame()
    {
        yield return new WaitForSeconds(0.001f);
        inGame = true;
    }
    #endregion


    #region ShowANDHidePanels 
    //МЕТОДЫ ДЛЯ СКРЫТИЯ И ПОЯВЛЕНИЯ ПАНЕЛЕЙ
    public void HidePanel(GameObject panel)
    {
        panel.transform.localScale = new Vector3(0, 0, 0);
    }
    private void HidePanel(GameObject panel, float time)
    {
        panel.transform.DOScale(new Vector3(0, 0, 0), time)
             .SetEase(Ease.OutCubic);
    }

    private void HidePanel(GameObject panel, float time, string bounce)
    {
        panel.transform.DOScale(new Vector3(0, 0, 0), time)
             .SetEase(Ease.OutCubic)
             .SetEase(Ease.OutBounce);
    }

    private void ShowPanel(GameObject panel, float time)
    {
        panel.transform.DOScale(new Vector3(1, 1, 1), time)
             .SetEase(Ease.OutCubic);
    }

    private void ShowPanel(GameObject panel, float time, string bounce)
    {
        panel.transform.DOScale(new Vector3(1, 1, 1), time)
             .SetEase(Ease.OutCubic)
             .SetEase(Ease.OutBounce);
    }
    #endregion

    private void ShowConnectChainText()
    {
        textAnim.SetTrigger("Show");
        attempts--;
        attemptsText.text = "ATTEMPTS: " + attempts.ToString();
    }
}
