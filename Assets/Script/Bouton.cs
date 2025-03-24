using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Bouton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler

{
    public bool isMouseDown = false;
    public bool Mouse = false;
    public Sprite[] PanelImg;
    public Sprite[] BoutonMissionImg;
    public GameObject FrameMission;
    public GameObject ItemObjPrefab;

    void Start()
    {
       // if(name == "Mission-Bouton") { StartCoroutine(Refresh(true)); }
    }

    void Update()
    {
        // Is the mouse still being held down?
        if (name == "ButtonFeu")
        {
        if (isMouseDown && GameObject.Find("Main Camera").GetComponent<Principal>().Grandeur.fuel > 0)
        {

            foreach (Transform child in GameObject.Find("Main Camera").GetComponent<Principal>().Vaisseau.transform.GetChild(0))
            {
                if (child.name.Contains("eacteur"))
                {
                    child.transform.GetComponent<Reacteur>().feu = true;
                    GameObject.Find("Main Camera").GetComponent<Principal>().Grandeur.fuel = GameObject.Find("Main Camera").GetComponent<Principal>().Grandeur.fuel - 2.5f * Time.deltaTime;

                }

            }
                GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Reacteur",0.5f);
                GameObject.Find("FireBarUI").GetComponent<Image>().enabled = true;

            }
            else

        {
                GameObject.Find("AudioManager").GetComponent<AudioManager>().Stop("Reacteur",true);
                GameObject.Find("FireBarUI").GetComponent<Image>().enabled = false;
                foreach (Transform child in GameObject.Find("Main Camera").GetComponent<Principal>().Vaisseau.transform.GetChild(0))
            {
                if (child.name.Contains("eacteur"))
                {
                    child.transform.GetComponent<Reacteur>().feu = false;
                }

            }
        }
        
        }
        

        

        


        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && press && !isMouseDown && Input.GetTouch(0).position.y > Screen.height / 5)
        //{
        //    press = false;
        //    GameObject.Find("Scroll").GetComponent<ScrollRect>().horizontal = false;
        //    spawnPiece();


        //}

    }

    // IEnumerator Refresh(bool refreshAfter)
    // {
    //     if (name == "Mission-Bouton")
    //     {
            
    //         MissionListe missionListe;
    //         if (SceneManager.GetActiveScene().buildIndex == 0) { missionListe = GameObject.Find("Main Camera").GetComponent<Principal>().missions; }
    //         else { missionListe = GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().missions; }
    //         foreach (MissionClass mission in missionListe.missions)
    //         {
    //             if (mission.Avancement()[0] == mission.Avancement()[1])
    //             {
    //                 if (transform.GetChild(0).gameObject.activeInHierarchy)
    //                 {
    //                     if (GameObject.Find("Main Camera") != null)
    //                     {
    //                         GameObject.Find("Main Camera").GetComponent<Principal>().FinPanel = true;
    //                     }
    //                     GameObject.Find("Mission-Parent").transform.GetChild(1).gameObject.SetActive(false);
    //                     foreach (Transform child in GameObject.Find("Mission-Parent").transform.GetChild(1)) { Destroy(child.gameObject); }
    //                     GameObject.Find("Mission-Parent").transform.GetChild(0).gameObject.SetActive(false);
    //                 }

    //                 GetComponent<Image>().sprite = BoutonMissionImg[1];

    //             }
    //         }

    //         yield return new WaitForSeconds(1);
    //         if (refreshAfter)
    //         {
    //             StartCoroutine(Refresh(true));
    //         }
    //     }
    // }



    public void OnPointerDown(PointerEventData data)
    {
         isMouseDown = true;

        // if (name == "Mission-Bouton" && isMouseDown)
        // {
        //     MissionRepository repo =  GameObject.Find("[MissionManager]").GetComponent<MissionManager>().repository;

        //     foreach (MissionData missionData in repo.missions)
        //     {
        //         MissionBase mission = missionData.Deserialize();
                
        //     }

        // }

        // if (name == "Mission-Bouton" && isMouseDown && GetComponent<Image>().sprite == BoutonMissionImg[0])
        // {
        //     MissionListe missionListe;
        //     if (SceneManager.GetActiveScene().buildIndex == 0) { missionListe = GameObject.Find("Main Camera").GetComponent<Principal>().missions; }
        //     else { missionListe = GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().missions; }

        //     GameObject.Find("Mission-Parent").transform.GetChild(0).gameObject.SetActive(true);
        //     GameObject.Find("Mission-Parent").transform.GetChild(1).gameObject.SetActive(true);
        //     foreach (Transform child in GameObject.Find("Mission-Parent").transform.GetChild(1)) { DestroyImmediate(child.gameObject); }
        //     if (missionListe.missions.Count == 1)
        //     {
        //         GameObject.Find("Mission-Parent").transform.GetChild(1).GetComponent<Image>().sprite = PanelImg[0];
        //         GameObject frame = Instantiate(FrameMission, GameObject.Find("Mission-Parent").transform.GetChild(1).transform);
        //         frame.GetComponent<RectTransform>().localPosition = Vector3.zero;
        //         GameObject.Find("Mission-Parent").transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(540, 155);
        //     }
        //     else if (missionListe.missions.Count == 2)
        //     {
        //         GameObject.Find("Mission-Parent").transform.GetChild(1).GetComponent<Image>().sprite = PanelImg[1];
        //         GameObject frame = Instantiate(FrameMission, GameObject.Find("Mission-Parent").transform.GetChild(1).transform);
        //         frame.GetComponent<RectTransform>().localPosition = new Vector3(0, 64, 0);
        //         frame = Instantiate(FrameMission, GameObject.Find("Mission-Parent").transform.GetChild(1).transform);
        //         frame.GetComponent<RectTransform>().localPosition = new Vector3(0, -64, 0);
        //         GameObject.Find("Mission-Parent").transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(540, 290);
        //     }
        //     else if (missionListe.missions.Count == 3)
        //     {
        //         GameObject.Find("Mission-Parent").transform.GetChild(1).GetComponent<Image>().sprite = PanelImg[2];
        //         GameObject frame = Instantiate(FrameMission, GameObject.Find("Mission-Parent").transform.GetChild(1).transform);
        //         frame.GetComponent<RectTransform>().localPosition = new Vector3(0, 128, 0);
        //         frame = Instantiate(FrameMission, GameObject.Find("Mission-Parent").transform.GetChild(1).transform);
        //         frame.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        //         frame = Instantiate(FrameMission, GameObject.Find("Mission-Parent").transform.GetChild(1).transform);
        //         frame.GetComponent<RectTransform>().localPosition = new Vector3(0, -128, 0);
        //         GameObject.Find("Mission-Parent").transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(540, 430);
        //     }
        //     int i = 0;
        //     foreach (Transform child in GameObject.Find("Mission-Parent").transform.GetChild(1))
        //     {
        //         print(i);
        //         MissionClass mission = missionListe.missions[i];
        //         child.GetChild(0).GetComponent<Text>().text = mission.titre;
        //         child.GetChild(1).GetComponent<Text>().text = mission.chapitre;
        //         child.GetChild(2).GetComponent<Text>().text = "Reward je ferais + tard bibou";
        //         child.GetChild(3).GetComponent<Text>().text = mission.descritption;
        //         float max = 503;
        //         GameObject bar = child.GetChild(4).GetChild(0).GetChild(0).gameObject;
        //         bar.GetComponent<RectTransform>().sizeDelta = new Vector2(mission.Avancement()[0] * max / mission.Avancement()[1], bar.GetComponent<RectTransform>().sizeDelta.y);
        //         child.GetChild(4).GetChild(1).GetComponent<Text>().text = mission.Avancement()[0].ToString() + "/" + mission.Avancement()[1].ToString();
        //         i++;
        //     }
        // }
        // else if (name == "Mission-Bouton"  && GetComponent<Image>().sprite == BoutonMissionImg[1])
        // {
        //     MissionListe missionListe;
        //     if (SceneManager.GetActiveScene().buildIndex == 0) { missionListe = GameObject.Find("Main Camera").GetComponent<Principal>().missions; }
        //     else { missionListe = GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().missions; }
        //     MissionClass m = new MissionClass();
        //     foreach (MissionClass mission in missionListe.missions)
        //     {
        //         if (mission.Avancement()[0] == mission.Avancement()[1])
        //         {
        //             m = mission;
                    
        //         }
        //     }
        //     m.Finish();
        //     GetComponent<Image>().sprite = BoutonMissionImg[0];
        // }
        // else if (name == "Inventory-Bouton" && isMouseDown)
        // {
        //     ItemClass items;
        //     if (SceneManager.GetActiveScene().buildIndex == 0) { items = GameObject.Find("Main Camera").GetComponent<Principal>().Items; }
        //     else { items = GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().Items; }
        //     GameObject.Find("Inventory-Parent").transform.GetChild(0).gameObject.SetActive(true);
        //     GameObject.Find("Inventory-Parent").transform.GetChild(1).gameObject.SetActive(true);
        //     GameObject.Find("Inventory-Parent").transform.GetChild(2).gameObject.SetActive(true);
        //     int i = 0;
        //     foreach(string obj in items.Objets)
        //     {
        //         print(obj);
        //         GameObject objItem = Instantiate(ItemObjPrefab, GameObject.Find("Inventory-Parent").transform.GetChild(1).GetChild(0).GetChild(4).GetChild(0));
        //         print(objItem.name);
        //         objItem.transform.GetComponent<Image>().sprite = GameObject.Find("Liste").GetComponent<Liste>().ObjetList.Find(obj).obj.GetComponent<SpriteRenderer>().sprite;
                
        //         objItem.transform.GetChild(1).GetComponent<Text>().text = items.stacks[i].ToString()+ " x " +  obj;
        //         i++;
        //     }
        //     foreach (PieceClass p in items.Pieces)
        //     {
        //         GameObject objItem = Instantiate(ItemObjPrefab, GameObject.Find("Inventory-Parent").transform.GetChild(1).GetChild(1).GetChild(4).GetChild(0));
        //         print(objItem.name);
        //         objItem.transform.GetComponent<Image>().sprite = GameObject.Find("Liste").GetComponent<Liste>().ListePiece.Find(p.nom).GetComponent<SpriteRenderer>().sprite;
        //         objItem.transform.GetChild(1).GetComponent<Text>().text = p.nom;
        //     }
        //     GameObject.Find("Inventory-Parent").transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "Total items: " + items.Count().ToString() + "/" + items.maxItem().ToString();
        // }
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (isMouseDown && name == "VoidMission")
        {
            if (GameObject.Find("Main Camera") != null)
            {
                GameObject.Find("Main Camera").GetComponent<Principal>().FinPanel = true;
            }
            GameObject.Find("Mission-Parent").transform.GetChild(1).gameObject.SetActive(false);
            foreach (Transform child in GameObject.Find("Mission-Parent").transform.GetChild(1)) { Destroy(child.gameObject); }
            GameObject.Find("Mission-Parent").transform.GetChild(0).gameObject.SetActive(false);
        }
        else if (isMouseDown && name == "Voidinventory")
        {
            foreach (Transform child in GameObject.Find("Inventory-Parent").transform.GetChild(1).GetChild(0).GetChild(4).GetChild(0)) { Destroy(child.gameObject); }
            foreach (Transform child in GameObject.Find("Inventory-Parent").transform.GetChild(1).GetChild(1).GetChild(4).GetChild(0)) { Destroy(child.gameObject); }
            GameObject.Find("Inventory-Parent").transform.GetChild(1).gameObject.SetActive(false);
            GameObject.Find("Inventory-Parent").transform.GetChild(2).gameObject.SetActive(false);
            GameObject.Find("Inventory-Parent").transform.GetChild(0).gameObject.SetActive(false);
        }
        isMouseDown = false;
        // Stop blocking
    }




}