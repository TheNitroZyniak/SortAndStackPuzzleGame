using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class Spawner : MonoBehaviour{

    [Inject] MainGameManager _mainGameManager;
    //[Inject] ObjectPooler _objectPooler;
    [Inject] Timer _timer;
    [Inject] StackManager _stackManager;
    [Inject] BoxesManager _boxesManager;
    [SerializeField] Level[] _levelsSO;
    [SerializeField] Transform[] tutorPoints;

    private int currentLevel;

    private void Start() {
        Application.targetFrameRate = 60;
    }

    public void CreateLevel() {

        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        //currentLevel = 357;

        List<LevelObjectData> lst;

        if (currentLevel >= 20) {
            lst = _levelsSO[20 + (currentLevel - 20) % 6].Get12Objects();
        } 
        else
            lst = _levelsSO[currentLevel].objects;

        _boxesManager.SetStackParams(lst.Count);

        c = 0;

        

        for (int i = 0; i < lst.Count; i++) {
            if(currentLevel == 0)
                SpawnObjects(lst[i].objectType, lst[i].objectAmount, true);
            else
                SpawnObjects(lst[i].objectType, lst[i].objectAmount, false);

            _boxesManager.CreateStacks(lst[i].objectType);

        }
        if (currentLevel < 20) {
            _timer.levelTime = _levelsSO[currentLevel].secondsToComplete;
            _timer.StartTimer(_levelsSO[currentLevel].secondsToComplete);
        } 
        else {
            _timer.levelTime = _levelsSO[20 + (currentLevel - 20) % 6].secondsToComplete;
            _timer.StartTimer(_levelsSO[20 + (currentLevel - 20) % 6].secondsToComplete);
        }

    }

    public void StartGame() {
        _timer.levelTime = _levelsSO[currentLevel].secondsToComplete;
        _timer.StartTimer(_levelsSO[currentLevel].secondsToComplete);
    }
    int c;
    private void SpawnObjects(string tag, int amount, bool firstScene) {
        for (int i = 0; i < amount; i++) {
            GameObject obj;
            if (!firstScene) {
                obj = ObjectPooler.Instance.SpawnFromPool(tag,
                    new Vector3(Random.Range(-4f, 4f), Random.Range(-3, 6), Random.Range(-3, 3)),
                    Quaternion.Euler(0, Random.Range(0, 360), 0));
            } 
            else {
                obj = ObjectPooler.Instance.SpawnFromPool(tag,
                    tutorPoints[c].position,
                    Quaternion.Euler(0, 180, 0));

                c++;
            }

            _mainGameManager.AddToList(obj.GetComponent<SelectableObject>());

        }
    }
}
