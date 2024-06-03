using System.Collections;
using UnityEngine;

public class TestFill : MonoBehaviour
{
    [SerializeField] int Amount;
    [SerializeField] float Scale;
    [SerializeField] string objectType;
    [SerializeField] Transform spawnPoint;

    

    void Start(){
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn() {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < Amount; i++) {
            Vector3 pos = new Vector3(Random.Range(-1 + spawnPoint.position.x, 1 + spawnPoint.position.x),
                Random.Range(-1 + spawnPoint.position.y, 1 + spawnPoint.position.y),
                spawnPoint.position.z);

            SelectableObject obj = ObjectPooler.Instance.SpawnFromPool(objectType, pos, Quaternion.identity).GetComponent<SelectableObject>();
            obj.transform.localScale = Vector3.one * Scale;
            
            yield return new WaitForSeconds(0.5f);
        }
    }

}
