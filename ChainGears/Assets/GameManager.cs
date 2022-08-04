using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private List<GameObject> splineChain;

    [HideInInspector] public int attempts = 3;

    public Button rotateButton;
    public static bool isWin;
    public static bool isTwisted;
    public static bool inGame;

    public static GameManager Instance;
    ChainManager chainManager;

    private void Awake() {
        if(Instance == null){
            Instance = this;
        }
    }
    private void Start()
    {
        Application.targetFrameRate = 120;
        chainManager = FindObjectOfType<ChainManager>();
        GlobalEventManager.OnWinGame.AddListener(WinGame);
        GlobalEventManager.OnLoseGame.AddListener(LoseGame);
        GlobalEventManager.OnEndDrawing.AddListener(CheckConnectedChain);
        GlobalEventManager.OnChainBreaks.AddListener(ShowConnectChainText);
        isWin = false;
        isTwisted = false;
        inGame = false;
        ChainManager.isCollision = false;
        rotateButton.interactable = false;
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
        {
            GlobalEventManager.OnChainBreaks.Invoke();
            rotateButton.interactable = false;
        }
        else
            GlobalEventManager.OnWinGame.Invoke();
    }

    private void WinGame()
    {
        splineChain[LevelController.currentLevelIndex].SetActive(true);

        chainManager.DestroyAll();
        isWin = true;
        inGame = false;
        SaveSystem.Instance.UpdatePlayerPrefs();

        StartCoroutine(TimerForWin());
        HidePanel(inGamePanel, 0.2f, 0f);
    }

    private void LoseGame()
    {

        ShowPanel(losePanel, 0.4f, 1f);
        HidePanel(inGamePanel, 0.2f, 0f);


        isWin = false;
        isTwisted = false;
        ChainManager.isCollision = false;
        inGame = false;
    }


    public void StartGame(int level)
    {
        attempts = 3;
        attemptsText.text = "ATTEMPTS: " + attempts.ToString();

        HidePanel(levelSelectPanel, 0.2f, 0f);
        ShowPanel(inGamePanel, 0.2f, 1f);

        StartCoroutine(TimerForGame(0.001f));

        LevelController.Instance.StartLevel(level);
        rotateButton.interactable = false;
    }

    public void ReloadScene()
    {
        attempts = 3;
        attemptsText.text = "ATTEMPTS: " + attempts.ToString();
        StartCoroutine(TimerForGame(0.001f)); 
        ChainManager.chainParentList.Clear();
        GearScript.isRotate = false;
        LevelController.Instance.StartLevel(LevelController.currentLevelIndex);
        HideSpline();
        HidePanel(settingsPanel, 0.2f, 0f);
        ShowPanel(inGamePanel, 0.2f, 1f);
        HidePanel(resumeIcon, 0.2f, 0f);
        ShowPanel(pauseIcon, 0.2f, 0.5f);
        inGame = true;
        chainManager.DestroyAll();
        rotateButton.interactable = false;
    }

    public void HideSpline()
    {
        splineChain[LevelController.currentLevelIndex].SetActive(false);
    }

    public void StartRotate()
    {
        GlobalEventManager.OnRotateStart.Invoke();
    }

    public void FromMainMenuToLevelSelect()
    {
        HidePanel(mainMenuPanel, 0.2f, 0f);
        ShowPanel(levelSelectPanel, 0.2f, 1f);
    }

    public void FromLevelSelectToMainmenu()
    {
        HidePanel(levelSelectPanel, 0.2f, 0f);
        ShowPanel(mainMenuPanel, 0.2f, 1f);
    }

    public void ControlSettingsPanel()
    {
        if (inGame)
        {
            HidePanel(pauseIcon, 0.01f, 0f);
            ShowPanel(resumeIcon, 0.01f, 0.5f);
            ShowPanel(settingsPanel, 0.1f, 1f);
            inGame = false;
        }
        else
        {
            HidePanel(settingsPanel, 0.1f, 0f);
            HidePanel(resumeIcon, 0.01f, 0f);
            ShowPanel(pauseIcon, 0.01f, 0.5f);
            inGame = true;
        }
        
    }

    public void GoToMainMenu()
    {
        HidePanel(winPanel, 0.2f, 0f);
        HidePanel(losePanel, 0.2f, 0f);
        HidePanel(resumeIcon, 0.2f, 0f);
        HidePanel(settingsPanel, 0.2f, 0f);
        HidePanel(inGamePanel, 0.2f, 0f);
        ShowPanel(pauseIcon, 0.2f, 0.5f);
        ShowPanel(mainMenuPanel, 0.2f, 1f);

        LevelController.Instance.HideGears();
        HideSpline();
        isWin = false;
    }

    public void GoToLevelSelect()
    {
        HidePanel(winPanel, 0.2f, 0f);
        HidePanel(losePanel, 0.2f, 0f);
        HidePanel(resumeIcon, 0.2f, 0f);
        HidePanel(settingsPanel, 0.2f, 0f);
        HidePanel(inGamePanel, 0.2f, 0f);
        ShowPanel(pauseIcon, 0.2f, 0.5f);
        ShowPanel(levelSelectPanel, 0.2f, 1f);

        LevelController.Instance.HideGears();
        HideSpline();
        isWin = false;
    }

    #region Coroutines
    IEnumerator TimerForWin()
    {
        yield return new WaitForSeconds(1);
        Instantiate(winParticles);
        ShowPanel(winPanel, 0.4f, 1);
    }

    IEnumerator TimerForGame(float time)
    {
        yield return new WaitForSeconds(time);
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
        if (panel.transform.localScale.x <= 0) return;
        panel.transform.localScale = new Vector3(0, 0, 0);
    }
    private void HidePanel(GameObject panel, float time, float size)
    {
        if (panel.transform.localScale.x <= 0) return;
        panel.transform.DOScale(new Vector3(size, size, 0), time)
             .SetEase(Ease.OutCubic);
    }

    //private void HidePanel(GameObject panel, float time, float size, string bounce)
    //{
    //    if (panel.transform.localScale.x <= 0) return;
    //    panel.transform.DOScale(new Vector3(size, size, 0), time)
    //         .SetEase(Ease.OutCubic)
    //         .SetEase(Ease.OutBounce);
    //}

    private void ShowPanel(GameObject panel, float time, float size)
    {
        if (panel.transform.localScale.x > 0) return;
        panel.transform.DOScale(new Vector3(size, size, 1), time)
             .SetEase(Ease.OutCubic);
    }

    //private void ShowPanel(GameObject panel, float time, float size, string bounce)
    //{
    //    if (panel.transform.localScale.x > 0) return;
    //    panel.transform.DOScale(new Vector3(size, size, 1), time)
    //         .SetEase(Ease.OutCubic)
    //         .SetEase(Ease.OutBounce);
    //}
    #endregion

    private void ShowConnectChainText()
    {
        inGame = false;
        attempts--;
        attemptsText.text = "ATTEMPTS: " + attempts.ToString();
        if (attempts > 0)
        {
            textAnim.SetTrigger("Show");
            StartCoroutine(TimerForGame(1.5f));
        }
        else if (attempts <= 0)
        {
            
            GlobalEventManager.OnLoseGame.Invoke();
        }
        StartCoroutine(TimerStopRotateGears());   
    }
}
