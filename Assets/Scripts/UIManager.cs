using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using DG.Tweening;


public class UIManager : MonoBehaviour {

    [SerializeField] private BallCell[] ballCells;
    [SerializeField] private GameObject victoryPopup, lostPopup;

    private int selectedBallsCounter = 0;


    int temp = 0;
    public void UpdateBefore(Sprite ballSprite, int id) {
        int insertIndex = selectedBallsCounter;
        for (int i = 0; i < selectedBallsCounter; i++) {
            if (ballCells[i].BallId == id) {
                insertIndex = i + 1;
            }
        }

        for (int i = selectedBallsCounter - 1; i >= insertIndex; i--) {
            ballCells[i + 1].SetCell(ballCells[i].CellImage.sprite, ballCells[i].BallId);
        }

        ballCells[insertIndex].ClearCell();
        temp = selectedBallsCounter;
        selectedBallsCounter = insertIndex;

    }

    public void UpdateSelectedBallsDisplay(Vector3 inputPos, Sprite ballSprite, int id) {
        MoveSprite(inputPos, ballSprite);

        ballCells[selectedBallsCounter].SetCell(ballSprite, id);
        selectedBallsCounter = temp;
        selectedBallsCounter++;
        CheckFor3(id);
       
    }

    private void MoveSprite(Vector3 inputPos, Sprite ballSprite) {
        GameObject go = new GameObject();
        go.transform.position = inputPos;
        go.AddComponent<Image>();
        go.GetComponent<Image>().sprite = ballSprite;
        go.transform.parent = transform;

        go.transform.DOMove(ballCells[selectedBallsCounter].transform.position, 1);
    }

    private void CheckFor3(int id) {
        int count = 0;
        for (int i = 0; i < selectedBallsCounter; i++) {
            if (ballCells[i].BallId == id) count++;
        }

        if (count == 3) {
            RemoveAndShiftCells(id);
        } 
        else if (selectedBallsCounter == ballCells.Length) {
            //GameController.Instance.SetActiveAllBalls(false);

            //LevelFailed levelFailed = new LevelFailed();
            //int currentLvl = PlayerPrefs.GetInt("Level");
            //levelFailed.level = currentLvl;
            //string json = JsonUtility.ToJson(levelFailed);
            //AppMetrica.Instance.ReportEvent("Level_Failed", json);
        }
    }

    private void RemoveAndShiftCells(int id) {

        int first = -1;
        int removeCount = 0;
        for (int i = 0; i < selectedBallsCounter; i++) {
            if (ballCells[i].BallId == id) {
                if (first < 0) first = i;
                ballCells[i].ClearCell();
                removeCount++;
            }
        }

        // Смещаем оставшиеся изображения
        int index = first;

        for (int i = 0; i < ballCells.Length - 3; i++) {
            if (ballCells[i].IsEmpty()) {
                if (!ballCells[i + 3].IsEmpty()) {
                    ballCells[i].SetCell(ballCells[i + 3].CellImage.sprite, ballCells[i + 3].BallId);
                    index = i + 1;
                }
            }
        }

        for (int i = index; i < ballCells.Length; i++) 
            ballCells[i].ClearCell();
        
        selectedBallsCounter = index; 
        //UpdateStarsText(currentStars);
    }

    public Vector3 GetCurrentImagePos() {
        return ballCells[selectedBallsCounter].transform.position;
    }

    public void OpenLosePopup() {
        lostPopup.SetActive(true);
    }

    public void OpenVictoryPopup() {
        victoryPopup.SetActive(true);
    }

    public void SetScene(int scene) {
        SceneManager.LoadScene(scene);
    }
}

