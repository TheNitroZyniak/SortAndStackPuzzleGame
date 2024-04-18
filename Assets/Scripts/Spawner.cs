using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Spawner : MonoBehaviour{

    [Inject] MainGameManager _mainGameManager;
    [Inject] ObjectPooler _objectPooler;
    [Inject] Timer _timer;

    [SerializeField] Level[] _levelsSO;


    private int currentLevel;

    int currentSpawner;

    private void Start() {
        CreateLevel();
    }


    public void CreateLevel() {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
 
        SpawnObjects("Cubes", _levelsSO[currentLevel].amountOfCubes);
        SpawnObjects("Spheres", _levelsSO[currentLevel].amountOfSpheres);
        SpawnObjects("Capsules", _levelsSO[currentLevel].amountOfCapsules);
        _timer.StartTimer(_levelsSO[currentLevel].secondsToComplete);
    }

    private void SpawnObjects(string tag, int amount) {
        currentSpawner++;

        for (int i = 0; i < amount; i++) {
            GameObject obj = _objectPooler.SpawnFromPool(tag,
                new Vector3(Random.Range(-2, 2), Random.Range(-3, 3), 2), 
                //new Vector3(0, 10, 0),
                Quaternion.Euler(Random.Range(0, 90), Random.Range(0, 90), Random.Range(0, 90)));
            _mainGameManager.AddToList(obj.GetComponent<SelectableObject>());

        }

    }


}
