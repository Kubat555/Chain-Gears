using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    public static bool isWin;

    private void Start()
    {
        GlobalEventManager.OnWinGame.AddListener(WinGame);
    }

    private void WinGame()
    {
        isWin = true;
        winPanel.transform.DOScale(new Vector3(1, 1, 1), 1.4f)
            .SetEase(Ease.OutCubic)
            .SetEase(Ease.OutBounce)
            .SetDelay(1);
    }

   public void ReloadScene()
    {
        ChainManager.chainParentList.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
