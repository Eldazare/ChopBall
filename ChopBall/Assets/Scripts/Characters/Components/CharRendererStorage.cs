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
        foreach (MeshRenderer m in tipRenderers) {
            m.material = mainMaterial;
        }
    }
}
