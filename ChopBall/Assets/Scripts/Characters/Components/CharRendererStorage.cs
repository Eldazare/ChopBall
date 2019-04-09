using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharRendererStorage : MonoBehaviour {

    public MeshRenderer mainRenderer;
    public List<MeshRenderer> skinRenderers;
    public List<MeshRenderer> tipRenderers;

    public void SetMaterialToSkin(Material skinMaterial) {
        foreach (MeshRenderer m in skinRenderers) {
            m.material = skinMaterial;
        }
    }

    public void SetMainMaterialToTip(Material mainMaterial) {
        LoadFickleMaterials(mainMaterial);
        foreach (MeshRenderer m in tipRenderers) {
            m.material = mainMaterial;
        }
    }

    // FICKLE
    private Material fickleSecondaryMaterial;
    private Material fickleMainMaterial;
    private Coroutine fickle;

    private void LoadFickleMaterials(Material mainMaterial) {
    if (fickleMainMaterial == null){
        fickleMainMaterial = mainMaterial;
        fickleSecondaryMaterial = ((CharacterBaseData)Resources.Load("Scriptables/_BaseDatas/CharacterBaseData", typeof(CharacterBaseData))).ChargeAnimationSecondaryMaterial;
        }
    }

    public void StartTipFickle() {
        if (fickle == null) {
            fickle = StartCoroutine(ChargeAnimation());
        }
       
    }

    public void StopTipFickle() {
        if (fickle != null) {
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
    //END FICKLE
}
