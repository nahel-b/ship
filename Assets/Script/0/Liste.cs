using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liste : MonoBehaviour
{
    //public MissionListe Histoire;

    public DeckList deckList;

    public ObjetList ObjetList;

    public ListEnnemie EnnemieList;

    public ColorList colorList;

    void Start()
    {
        // Debug.Log(JsonUtility.ToJson(Histoire));
        // Debug.Log(JsonUtility.ToJson(deckList));
        // Debug.Log(JsonUtility.ToJson(ObjetList));
        // Debug.Log(JsonUtility.ToJson(EnnemieList));
        // Debug.Log(JsonUtility.ToJson(colorList));
    }
}

