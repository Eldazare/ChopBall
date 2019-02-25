using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterColorSetter  {

	public static Material[] SetMainColor(Material[] rendererMaterials, Material mainMaterial){
		rendererMaterials [0] = mainMaterial;
		rendererMaterials [2] = mainMaterial;
		rendererMaterials [4] = mainMaterial;
		return rendererMaterials;
	}

	public static Material[] SetColorPalette(Material[] rendererMaterials, PaletteContainer container){
		rendererMaterials [1] = container.skin;
		rendererMaterials [3] = container.hair;
		rendererMaterials [5] = container.eyes;
		rendererMaterials [6] = container.eyes;
		rendererMaterials [7] = container.eyes;
		return rendererMaterials;
	}
}
