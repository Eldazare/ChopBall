using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharRendererStorage : MonoBehaviour {

    public MeshRenderer mainRenderer;
    public List<MeshRenderer> skinRenderers;
    public List<MeshRenderer> tipRenderers;


    public Material fickleSecondaryMaterial;
    private Material fickleMainMaterial;
    private Coroutine fickle;

    public void SetMaterialToSkin(Material skinMaterial) {
        foreach (MeshRenderer m in skinRenderers) {
            m.material = skinMaterial;
        }
    }

    public void SetMainMaterialToTip(Material mainMaterial) {
        if (fickleMainMaterial == null) {
            fickleMainMaterial = mainMaterial;
        }
        foreach (MeshRenderer m in tipRenderers) {
            m.material = mainMaterial;
        }
    }

    public void StartTipFickle() {
        if (fickle == null) {
            Debug.Log("StartedFickle");
            fickle = StartCoroutine(ChargeAnimation());
        }
       
    }

    public void StopTipFickle() {
        if (fickle != null) {
            Debug.Log("StoppedFickle");
            StopCoroutine(fickle);
            SetMainMaterialToTip(fickleMainMaterial);
            fickle = null;
        }
    }

    private IEnumerator ChargeAnimation() {
        float waitTime = 0.1f;
        while (true) {
            SetMainMaterialToTip(fickleSecondaryMaterial);
            yield return new WaitForSeconds(waitTime);
            SetMainMaterialToTip(fickleMainMaterial);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
