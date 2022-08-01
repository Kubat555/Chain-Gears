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

    private void Start()
    {
        GlobalEventManager.OnWinGame.AddListener(WinGame);
        GlobalEventManager.OnLoseGame.AddListener(LoseGame);
        GlobalEventManager.OnEndDrawing.AddListener(CheckResults);
    }

    private void CheckResults()
    {
        if (isTwisted)
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
        StartCoroutine(TimerForWin());
        HideInGamePanel();
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
    }

    private void LoseGame()
    {
        losePanel.transform.DOScale(new Vector3(1, 1, 1), 1.4f)
            .SetEase(Ease.OutCubic)
            .SetEase(Ease.OutBounce);

        HideInGamePanel();
    }

   public void ReloadScene()
    {
        isWin = false;
        isTwisted = false;
        ChainManager.chainParentList.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void HideInGamePanel()
    {
        inGamePanel.transform.DOScale(new Vector3(0, 0, 0), 0.5f)
            .SetEase(Ease.OutCubic);
    }

    IEnumerator TimerForWin()
    {
        yield return new WaitForSeconds(1);

        Instantiate(winParticles);
        winPanel.transform.DOScale(new Vector3(1, 1, 1), 1.4f)
            .SetEase(Ease.OutCubic)
            .SetEase(Ease.OutBounce);
    }
}
