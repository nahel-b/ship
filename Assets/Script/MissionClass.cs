using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Events;

[System.Serializable]
public class MissionListe
{
    public List<MissionClass> missions;
    public MissionListe() { missions = new List<MissionClass>(); }
    public void Add(MissionClass m)
    {
        missions.Add(m);
        if (m.Debut != null)
        {
            m.Debut.Invoke();
        }
    }
}

[System.Serializable]
public class MissionClass 
{

    public string titre;
    public string chapitre;
    public string descritption;
    [Header("Attaque/livraison/mining/parler")]
    public string type;
    [Header("Type :")]
    public AttaqueType attaqueType;
    public LivraisonType livraisonType;
    public MinerType minerType;
    public ParlerType parlerType;
    [Header("Recompense :")]
    public RecompenseClass recompenseClass;
    [Header("Void :")]
    public UnityEvent Debut;
    public UnityEvent Milieu;
    public UnityEvent Fin;

    public List<float> Avancement()
    {
        
        List<float> a = new List<float> { 0, 0 };
        if(type == "livraison")
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                foreach(string item in GameObject.Find("Main Camera").GetComponent<Principal>().Items.Objets) {if(livraisonType.Item == item) { livraisonType.avancement[0] = true; } }
            }
            else
            {
                foreach (string item in GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().Items.Objets) { if (livraisonType.Item == item) { livraisonType.avancement[0] = true; } }
            }
            a = new List<float> { 0,2};
            if (livraisonType.avancement[1]) { a[0] = 2; }
            else if (livraisonType.avancement[0]) { a[0] = 1; }
        }
        if(type == "parler") { a = new List<float> { 0, 1 }; if (parlerType.parler) { a = new List<float> { 1, 1 }; } }
        if(type == "miner") { a = new List<float> { 0, 1 }; a = minerType.avancement; }
        if(type == "attaque") { a = new List<float> { 0, 1 }; if (attaqueType.kill) { a = new List<float> { 1, 1 }; } }
        return a;
    }

    public void Finish()
    {
        RecompenseClass rc = recompenseClass;

        if(chapitre == "Story")
        {
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                int i = 0;
                int a = 0;



                foreach (MissionClass mission in GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().missions.missions)
                {
                    if (this.titre == mission.titre) { a = i; }
                    i++;
                }
                i = 0;
                int b = 1;
                foreach (MissionClass mission in GameObject.Find("Liste").GetComponent<Liste>().Histoire.missions)
                {
                    if (this.titre == mission.titre) { b = i; }
                    i++;
                }
                GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().missions.missions[a] = GameObject.Find("Liste").GetComponent<Liste>().Histoire.missions[b + 1];
                if (GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().missions.missions[a].Debut != null)
                {
                    GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().missions.missions[a].Debut.Invoke();
                }
            }
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                int i = 0;
                int a = 0;



                foreach (MissionClass mission in GameObject.Find("Main Camera").GetComponent<Principal>().missions.missions)
                {
                    if (this.titre == mission.titre) { a = i; }
                    i++;
                }
                i = 0;
                int b = 1;
                foreach (MissionClass mission in GameObject.Find("Liste").GetComponent<Liste>().Histoire.missions)
                {
                    if (this.titre == mission.titre) { b = i; }
                    i++;
                }
                GameObject.Find("Main Camera").GetComponent<Principal>().missions.missions[a] = GameObject.Find("Liste").GetComponent<Liste>().Histoire.missions[b + 1];
                if (GameObject.Find("Main Camera").GetComponent<Principal>().missions.missions[a].Debut != null)
                {
                    GameObject.Find("Main Camera").GetComponent<Principal>().missions.missions[a].Debut.Invoke();
                }
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Camera.main.GetComponent<Principal>().RewardVoid(rc);

        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().RewardVoid(rc);

        }

    }
        
    }





[System.Serializable]
public class AttaqueType
{
    public string targetName;
    public bool kill = false;

}
[System.Serializable]
public class LivraisonType
{
    public string DepartName;
    public string ArriveName;
    public string Item;
    public List<bool> avancement ;
}
[System.Serializable]
public class ParlerType
{
    public string PersoName;
    public bool parler=false;
    public int PersoPhase;
}
[System.Serializable]
public class MinerType
{
    public string Item;
    public List<float> avancement;
}

[System.Serializable]
public class RecompenseClass
{
    public float xp;
    public int Argent;
    public List<PieceClass> pieces;
    public List<string> Deck;
    public int index = 0;
}

//public class 

//public class PersonnageClass
//{
//    public List<PhasePerso> phases;


//}

//public class PhasePerso
//{
//    public List<string> dialogues;

//    public UnityEvent finish;

//}


//[CustomEditor(typeof(MissionClass))]
//public class MyScriptEditor : Editor
//{
//    void OnInspectorGUI()
//    {
//        //var myScript = target as MissionClass;

//        MissionClass.flag = GUILayout.Toggle(myScript.flag, "Flag");

//        if (myScript.flag)
//            myScript.i = EditorGUILayout.IntSlider("I field:", myScript.i, 1, 100);

//    }
//}




