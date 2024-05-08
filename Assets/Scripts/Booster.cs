using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Booster : MonoBehaviour{

    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private boosterType _boosterName;

    int _amount;

    private void Start() {
        _amount = PlayerPrefs.GetInt(_boosterName.ToString());
        _amountText.text = _amount.ToString();

    }

    public void UseBooster() {
        if (_amount > 0) {
            _amount--;
            _amountText.text = _amount.ToString();
        }
    }
}

public enum boosterType {
    None,
    Magnet,
    Undo,
    Mix,
    Freeze
}
