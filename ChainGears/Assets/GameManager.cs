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
    [SerializeField] private GameObject pauseIcon;
    [SerializeField] private GameObject resumeIcon;
    [SerializeField] private GameObject settingsPanel;

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

    [ContextMenu("Rotate")]
    private void CheckConnectedChain()
    {
        if (!ChainManager.isCollision)
        {
            GlobalEventManager.OnChainBreaks.Invoke();
        }
    }
    public void CheckResults()
    {
        if (isTwisted || chainManager.gearList.Count < 3)
            GlobalEventManager.OnChainBreaks.Invoke();
        else
            GlobalEventManager.OnWinGame.Invoke();
    }

    private void WinGame()
    {
        isWin = true;
        inGame = false;

        StartCoroutine(TimerForWin());
        HidePanel(inGamePanel, 0f, 0.5f);
    }

    private void LoseGame()
    {

        ShowPanel(losePanel, 1.4f, 1f, "bounce");
        HidePanel(inGamePanel, 0f, 0.3f);


        isWin = false;
        isTwisted = false;
        ChainManager.isCollision = false;
        inGame = false;
    }


    public void StartGame(int level)
    {
        attempts = 3;
        attemptsText.text = "ATTEMPTS: " + attempts.ToString();

        HidePanel(levelSelectPanel, 0.5f, 0f);
        ShowPanel(inGamePanel, 0.8f, 1f, "bounce");

        StartCoroutine(TimerForGame());

        LevelController.Instance.StartLevel(level);
    }

    public void ReloadScene()
    {
        attempts = 3;
        attemptsText.text = "ATTEMPTS: " + attempts.ToString();
        StartCoroutine(TimerForGame());
        chainManager.DestroyAll();
        ChainManager.chainParentList.Clear();
        GearScript.isRotate = false;
        LevelController.Instance.StartLevel(LevelController.currentLevelIndex);

        HidePanel(settingsPanel, 0.8f, 0f);
        ShowPanel(inGamePanel, 1f, 1f, "bounce");
        HidePanel(resumeIcon, 0.5f, 0f);
        ShowPanel(pauseIcon, 0.8f, 0.5f);

        chainManager.DestroyAll();
    }

    public void StartRotate()
    {
        GlobalEventManager.OnRotateStart.Invoke();
    }

    public void FromMainMenuToLevelSelect()
    {
        HidePanel(mainMenuPanel, 0.5f, 0f);
        ShowPanel(levelSelectPanel, 0.8f, 1f);
    }

    public void FromLevelSelectToMainmenu()
    {
        HidePanel(levelSelectPanel, 0.5f, 0f);
        ShowPanel(mainMenuPanel, 0.8f, 1f);
    }

    public void ShowSettingsPanel()
    {
        HidePanel(pauseIcon, 0.5f, 0f);
        ShowPanel(resumeIcon, 0.5f, 0.5f);
        ShowPanel(settingsPanel, 0.8f, 1f, "bounce");
        inGame = false;
    }

    public void HideSettingsPanel()
    {
        HidePanel(settingsPanel, 0.8f, 0f);
        HidePanel(resumeIcon, 0.5f, 0f);
        ShowPanel(pauseIcon, 0.8f, 0.5f);
        inGame = true;
    }

    public void GoToMainMenu()
    {
        HidePanel(resumeIcon, 0.5f, 0f);
        HidePanel(settingsPanel, 0.8f, 0f);
        HidePanel(inGamePanel, 0.8f, 0f);
        ShowPanel(pauseIcon, 0.8f, 0.5f);

        ShowPanel(mainMenuPanel, 0.8f, 1f, "bounce");
    }

    public void GoToLevelSelect()
    {
        HidePanel(resumeIcon, 0.5f, 0f);
        HidePanel(settingsPanel, 0.8f, 0f);
        HidePanel(inGamePanel, 0.8f, 0f);
        ShowPanel(pauseIcon, 0.8f, 0.5f);

        ShowPanel(levelSelectPanel, 0.8f, 1f, "bounce");
    }

    #region Coroutines
    IEnumerator TimerForWin()
    {
        yield return new WaitForSeconds(1);
        Instantiate(winParticles);
        ShowPanel(winPanel, 1.4f, 1, "bounce");
    }

    IEnumerator TimerForGame()
    {
        yield return new WaitForSeconds(0.001f);
        inGame = true;
    }
    IEnumerator TimerStopRotateGears()
    {
        yield return new WaitForSeconds(1f);
        GlobalEventManager.OnRotateStop.Invoke();
    }
    #endregion


    #region ShowANDHidePanels 
    //МЕТОДЫ ДЛЯ СКРЫТИЯ И ПОЯВЛЕНИЯ ПАНЕЛЕЙ
    public void HidePanel(GameObject panel)
    {
        panel.transform.localScale = new Vector3(0, 0, 0);
    }
    private void HidePanel(GameObject panel, float time, float size)
    {
        panel.transform.DOScale(new Vector3(size, size, 0), time)
             .SetEase(Ease.OutCubic);
    }

    private void HidePanel(GameObject panel, float time, float size, string bounce)
    {
        panel.transform.DOScale(new Vector3(size, size, 0), time)
             .SetEase(Ease.OutCubic)
             .SetEase(Ease.OutBounce);
    }

    private void ShowPanel(GameObject panel, float time, float size)
    {
        panel.transform.DOScale(new Vector3(size, size, 1), time)
             .SetEase(Ease.OutCubic);
    }

    private void ShowPanel(GameObject panel, float time, float size, string bounce)
    {
        panel.transform.DOScale(new Vector3(size, size, 1), time)
             .SetEase(Ease.OutCubic)
             .SetEase(Ease.OutBounce);
    }
    #endregion

    private void ShowConnectChainText()
    {
        textAnim.SetTrigger("Show");
        attempts--;
        attemptsText.text = "ATTEMPTS: " + attempts.ToString();
        if (attempts <= 0)
        {
            GlobalEventManager.OnLoseGame.Invoke();
        }
        StartCoroutine(TimerStopRotateGears());
    }
}
