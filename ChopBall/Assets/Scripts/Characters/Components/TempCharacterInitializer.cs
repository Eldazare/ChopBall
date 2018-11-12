using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCharacterInitializer : MonoBehaviour {

    public GameEvent OnGameStartEvent;

    private CharacterHandler handler;

	private void Start()
    {
        handler = GetComponent<CharacterHandler>();
        handler.Initialize(new Color32(255, 0, 0, 255));


        OnGameStartEvent.Raise();

    }
}
