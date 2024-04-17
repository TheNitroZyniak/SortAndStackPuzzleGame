using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Spawner : MonoBehaviour{
    [Inject] MainGameManager _mainGameManager;

    [SerializeField] private int objectsAmount = 3;
    [SerializeField] private GameObject[] objects;


    private void Start() {
        CreateLevel();
    }


    public void CreateLevel() {
        int ballIndex = 0;
        for (int i = 0; i < objectsAmount * 3; i++) {
            SelectableObject newObject = Instantiate(objects[ballIndex], 
                new Vector3(Random.Range(-2,2), Random.Range(-3, 3), 0), 
                Quaternion.Euler(Random.Range(0, 90), Random.Range(0, 90), Random.Range(0, 90))).GetComponent<SelectableObject>();

            _mainGameManager.AddToList(newObject);

            ballIndex++;
            if(ballIndex > objectsAmount - 1) {
                ballIndex = 0;
            }
        }
    }
}
