using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] List<LevelInfo> levelsInfo;
    [SerializeField] private int _maxLevelIndex;
    [SerializeField] List<GameObject> gears;
    
    public static LevelController Instance;
    public static int currentLevelIndex;
    
    private void Awake() {
        if(Instance == null){
            Instance = this;
        }
    }

    public void StartLevel(int index){
        if(index >= _maxLevelIndex || index < 0) return;

        if(levelsInfo[index].gearsPositions.Count > gears.Count) {
            Debug.Log("Нехватает шестеренок, добавь на сцену!");
            return;
        }

        currentLevelIndex = index;

        for(int i = 0; i < levelsInfo[index].gearsPositions.Count; i++){
            gears[i].transform.position = levelsInfo[index].gearsPositions[i];
            gears[i].transform.localScale = levelsInfo[index].gearsScale[i];
            gears[i].SetActive(true);
        }
        
        if(gears.Count > levelsInfo[index].gearsPositions.Count){
            for(int i = levelsInfo[index].gearsPositions.Count; i < gears.Count; i++){
                gears[i].SetActive(false);
            }
        }
    }

    public void NextLevel(){
        StartLevel(Mathf.Clamp(currentLevelIndex + 1, 0, _maxLevelIndex - 1));
    }

    public void HideGears(){
        for(int i = 0; i < gears.Count; i++){
            gears[i].SetActive(false);
        }
    }

}
