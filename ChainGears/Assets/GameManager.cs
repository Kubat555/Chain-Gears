using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
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
        Instantiate(winParticles);
        winPanel.transform.DOScale(new Vector3(1, 1, 1), 1.4f)
            .SetEase(Ease.OutCubic)
            .SetEase(Ease.OutBounce)
            .SetDelay(1);
    }

    private void LoseGame()
    {
        losePanel.transform.DOScale(new Vector3(1, 1, 1), 1.4f)
            .SetEase(Ease.OutCubic)
            .SetEase(Ease.OutBounce);
    }

   public void ReloadScene()
    {
        isWin = false;
        isTwisted = false;
        ChainManager.chainParentList.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
