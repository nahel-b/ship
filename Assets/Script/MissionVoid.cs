using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionVoid : MonoBehaviour
{
    // public void RandomArriveLivraison(string titre)
    // {
    //     foreach(MissionClass mi in GameObject.Find("Main Camera").GetComponent<Principal>().missions.missions)
    //     {
    //         if(mi.titre == titre) { mi.livraisonType.DepartName = GameObject.Find("Main Camera").GetComponent<Principal>().Stations[0].name; }
    //     }
    //     print("a");
    // }

    // //public void addfleche(Color32 color)
    // //{
    // //    ListsaveObj sv = JsonUtility.FromJson<ListsaveObj>(PlayerPrefs.GetString("saveObj"));
    // //    sv.fleches.Add(new FlecheClass(color, objName));
    // //}
    // public void addfleche(string objName)
    // {
    //     if (SceneManager.GetActiveScene().buildIndex == 0)
    //     {
    //         GameObject.Find("Main Camera").GetComponent<Principal>().Fleche(objName,true, GameObject.Find("Liste").GetComponent<Liste>().colorList.RandomColor().color);
    //         print("missionFleche");
    //     }
    //     else
    //     {
    //         ListsaveObj sv = JsonUtility.FromJson<ListsaveObj>(PlayerPrefs.GetString("saveObj"));
    //         sv.fleches.Add(new FlecheClass(GameObject.Find("Liste").GetComponent<Liste>().colorList.RandomColor().color, objName, false));
    //         PlayerPrefs.SetString("saveObj", JsonUtility.ToJson(sv));
    //     }
    // }
    // public void ChangePhase(string PersoName)
    // {
    //     if (SceneManager.GetActiveScene().buildIndex == 0)
    //     {
    //         PersoSave p = JsonUtility.FromJson<PersoSave>(PlayerPrefs.GetString("PersoSave"));
    //         p.Find(PersoName).Phase++;
    //         PlayerPrefs.SetString("PersoSave", JsonUtility.ToJson(p));
    //     }
    //     else
    //     {
    //         GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().persoSave.Find(PersoName).Phase++;
    //         GameObject.Find(PersoName).GetComponent<PersoDeplacement>().phase++;
    //     }
    // }

    // public void SpawnEnnemie(string indexVector2)
    // {
    //     if (SceneManager.GetActiveScene().buildIndex == 0)
    //     {
    //         EnnemieClass ennemie = GameObject.Find("Liste").GetComponent<Liste>().EnnemieList.Ennemies[int.Parse(indexVector2.Split(';')[0])];
    //         GameObject a = Instantiate(new GameObject());
    //         a.AddComponent<Ennemie>();
    //         a.GetComponent<Ennemie>().agressif = GameObject.Find("Liste").GetComponent<Liste>().EnnemieList.Ennemies[int.Parse(indexVector2.Split(';')[0])].aggressif;
    //         a.GetComponent<Ennemie>().SpeedRotation = GameObject.Find("Liste").GetComponent<Liste>().EnnemieList.Ennemies[int.Parse(indexVector2.Split(';')[0])].SpeedRotation;
    //         a.GetComponent<Ennemie>().zone = GameObject.Find("Liste").GetComponent<Liste>().EnnemieList.Ennemies[int.Parse(indexVector2.Split(';')[0])].zone;
    //         a.GetComponent<Ennemie>().vaisseau = GameObject.Find("Liste").GetComponent<Liste>().EnnemieList.Ennemies[int.Parse(indexVector2.Split(';')[0])].vaisseau;
    //         a.GetComponent<Ennemie>().speed = GameObject.Find("Liste").GetComponent<Liste>().EnnemieList.Ennemies[int.Parse(indexVector2.Split(';')[0])].speed;
    //         a.GetComponent<Ennemie>().nom = GameObject.Find("Liste").GetComponent<Liste>().EnnemieList.Ennemies[int.Parse(indexVector2.Split(';')[0])].nom;

    //         a.tag = "ennemie";
    //         a.GetComponent<Ennemie>().Rotz = float.Parse(indexVector2.Split(';')[3]);
    //         a.transform.position = new Vector3(float.Parse(indexVector2.Split(';')[1]), float.Parse(indexVector2.Split(';')[2]), 0);
    //     }
    //     else
    //     {
    //         EnnemieClass ennemie = GameObject.Find("Liste").GetComponent<Liste>().EnnemieList.Ennemies[int.Parse(indexVector2.Split(';')[0])];
    //         ListEnnemie e = JsonUtility.FromJson<ListEnnemie>( PlayerPrefs.GetString("Ennemie"));
    //         e.Ennemies.Add(new EnnemieClass(ennemie.nom, ennemie.SpeedRotation, ennemie.zone, ennemie.speed, ennemie.vaisseau, ennemie.aggressif, float.Parse(indexVector2.Split(';')[3]), new Vector3(float.Parse(indexVector2.Split(';')[1]), float.Parse(indexVector2.Split(';')[2]), 0), 0));
    //         PlayerPrefs.SetString("Ennemie",JsonUtility.ToJson(e));
    //     }
    // }


}
