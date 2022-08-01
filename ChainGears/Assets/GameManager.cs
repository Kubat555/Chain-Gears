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
    public static bool isGame;

    private void Start()
    {
        GlobalEventManager.OnWinGame.AddListener(WinGame);
        GlobalEventManager.OnLoseGame.AddListener(LoseGame);
        GlobalEventManager.OnEndDrawing.AddListener(CheckResults);
        isWin = false;
        isTwisted = false;
        isGame = false;
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
        isGame = false;

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
        isGame = false;
    }


    public void StartGame()
    {
        mainMenuPanel.transform.DOScale(new Vector3(0, 0, 0), 0.5f)
            .SetEase(Ease.OutCubic);

        StartCoroutine(TimerForGame());
    }

    public void ReloadScene()
    {
        StartCoroutine(TimerForGame());
        ChainManager.chainParentList.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void HideInGamePanel()
    {
        inGamePanel.transform.DOScale(new Vector3(0, 0, 0), 0.5f)
            .SetEase(Ease.OutCubic);
    }

    #region  Œ–”“»Õ€
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
        isGame = true;
    }
    #endregion
}
