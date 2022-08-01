using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject winParticles;

    public static bool isWin;
    public static bool isTwisted;
    public static bool inGame;

    private void Start()
    {
        GlobalEventManager.OnWinGame.AddListener(WinGame);
        GlobalEventManager.OnLoseGame.AddListener(LoseGame);
        GlobalEventManager.OnEndDrawing.AddListener(CheckResults);
        isWin = false;
        isTwisted = false;
        inGame = false;
        ChainManager.isCollision = false;
    }

    private void CheckResults()
    {
        if (isTwisted||!ChainManager.isCollision)
        {
            GlobalEventManager.OnLoseGame.Invoke();
        }
        else
        {
            GlobalEventManager.OnWinGame.Invoke();
        }
    }

    private void WinGame()
    {
        isWin = true;
        inGame = false;

        StartCoroutine(TimerForWin());
        HideInGamePanel();
    }

    private void LoseGame()
    {
        /*losePanel.transform.DOScale(new Vector3(1, 1, 1), 1.4f)
            .SetEase(Ease.OutCubic)
            .SetEase(Ease.OutBounce);

        HideInGamePanel();
*/
        isWin = false;
        isTwisted = false; 
        ChainManager.isCollision = false;
        inGame = true;
    }


    public void StartGame(int level)
    {
        mainMenuPanel.transform.DOScale(new Vector3(0, 0, 0), 0.5f)
            .SetEase(Ease.OutCubic);

        StartCoroutine(TimerForGame());

        LevelController.Instance.StartLevel(level);
    }

    public void ReloadScene()
    {
        StartCoroutine(TimerForGame());
        ChainManager.chainParentList.Clear();
        LevelController.Instance.StartLevel(LevelController.currentLevelIndex);
    }

    private void HideInGamePanel()
    {
        inGamePanel.transform.DOScale(new Vector3(0, 0, 0), 0.5f)
            .SetEase(Ease.OutCubic);
    }

    #region ��������
    IEnumerator TimerForWin()
    {
        yield return new WaitForSeconds(1);

        Instantiate(winParticles);
        winPanel.transform.DOScale(new Vector3(1, 1, 1), 1.4f)
            .SetEase(Ease.OutCubic)
            .SetEase(Ease.OutBounce); 
    }

    IEnumerator TimerForGame()
    {
        yield return new WaitForSeconds(0.001f);
        inGame = true;
    }
    #endregion
}
