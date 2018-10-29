using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalAreaCheck : MonoBehaviour {

    private Collider2D[] colliderBuffer = new Collider2D[16];

    public List<CharacterHandler> GetCharactersInArea()
    {
        List<CharacterHandler> charactersInArea = new List<CharacterHandler>(16);

        int count = Physics2D.OverlapBoxNonAlloc(transform.position, transform.localScale, 0, colliderBuffer);

        for (int i = 0; i < count; i++)
        {
            CharacterHandler charHandler = colliderBuffer[i].GetComponent<CharacterHandler>();

            if (charHandler) charactersInArea.Add(charHandler);
        }

        return charactersInArea;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 1f, 0f, 1f);
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
