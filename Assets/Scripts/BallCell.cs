using UnityEngine;
using UnityEngine.UI;

public class BallCell : MonoBehaviour {
    [SerializeField] Sprite empty;
    public Image CellImage { get; private set; }
    public int BallId { get; private set; }
    public Transform point;

    private void Awake() {
        CellImage = GetComponent<Image>();
        ClearCell();
    }

    public void SetCell(Sprite ballSprite, int id) {
        CellImage.sprite = ballSprite;
        CellImage.color = Color.white;
        CellImage.rectTransform.sizeDelta = new Vector2(100, 100);
        BallId = id;
    }

    public void ClearCell() {
        CellImage.sprite = empty; // Или установите спрайт для пустой ячейки
        CellImage.color = new Color(79f, 38f, 21f)/256f;
        CellImage.rectTransform.sizeDelta = new Vector2(150, 150);
        BallId = -1;
    }

    public bool IsEmpty() {
        return CellImage.sprite == empty;
    }
}
