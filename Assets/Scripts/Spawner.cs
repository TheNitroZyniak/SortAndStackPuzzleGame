using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Spawner : MonoBehaviour{

    [Inject] MainGameManager _mainGameManager;
    [Inject] ObjectPooler _objectPooler;
    [Inject] Timer _timer;
    [Inject] StackManager _stackManager;
    [SerializeField] Level[] _levelsSO;


    private int currentLevel;

    int currentSpawner;

    private void Start() {
        Application.targetFrameRate = 60;

        CreateLevel();
    }


    public void CreateLevel() {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");

        _stackManager.SetStackParams(_levelsSO[currentLevel].objects.Count);

        for (int i = 0; i < _levelsSO[currentLevel].objects.Count; i++) {
            SpawnObjects(_levelsSO[currentLevel].objects[i].objectType, _levelsSO[currentLevel].objects[i].objectAmount);
            _stackManager.CreateStacks(_levelsSO[currentLevel].objects[i].objectType);
        }
    }

    public void StartGame() {
        _timer.StartTimer(_levelsSO[currentLevel].secondsToComplete);
    }

    private void SpawnObjects(ObjectType tag, int amount) {
        currentSpawner++;

        for (int i = 0; i < amount; i++) {
            GameObject obj = _objectPooler.SpawnFromPool(tag,
                new Vector3(Random.Range(-3.5f, 3.5f), Random.Range(-3, 6), Random.Range(0, 3)), 
                //new Vector3(0, 10, 0),
                Quaternion.Euler(0, Random.Range(0, 360), 0));
            _mainGameManager.AddToList(obj.GetComponent<SelectableObject>());

        }

    }


}
