using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class GpuInstancingEnabler : MonoBehaviour{
    private void Awake() {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        MeshRenderer mRend = GetComponent<MeshRenderer>();
        mRend.SetPropertyBlock(mpb);
    }
}
