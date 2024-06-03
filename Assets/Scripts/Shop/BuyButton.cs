//using Firebase.Analytics;
using TMPro;
using UnityEngine;

public class BuyButton : MonoBehaviour{

    public void OnBuy() {
        TextMeshProUGUI tm = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        //ItemPurchased itemPurchesd = new ItemPurchased();
        //itemPurchesd.itemId = tm.text;
        //string json = JsonUtility.ToJson(itemPurchesd);
        //AppMetrica.Instance.ReportEvent("Item_Purchased", json);

        //FirebaseAnalytics.LogEvent("Item_Purchased", new Parameter("Price", tm.text));
    }

}
