using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PersoDeplacement : MonoBehaviour
{

    public int phase;
    public float lastSpeakTime;
    public string dialogue;
    public GameObject dialogueObj;
    public GameObject Button;
    private bool finishDialogue = true;
    private string lastDialogue;
    public int numberItem = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMouseUp()
    {
        if (!GameObject.Find("MecanicienButton").transform.GetChild(0).gameObject.activeInHierarchy && Time.time - lastSpeakTime > 0.5f && Vector2.Distance(transform.position, GameObject.Find("perso").transform.position) < 0.5f && name.Contains("Mecanicien"))
        {
            lastSpeakTime = Time.time;
            Camera.main.GetComponent<DeplacementPrincipal>().activePerso = gameObject;
            foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(true); }
            GameObject.Find("Dialogue-Parent").transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate
            {
                if (Camera.main.GetComponent<DeplacementPrincipal>().activePerso != null)
                {
                    foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(false); }

                    foreach (Transform t in GameObject.Find("MecanicienButton").transform)
                    {
                        t.gameObject.SetActive(false);
                    }
                    Camera.main.GetComponent<DeplacementPrincipal>().activePerso = null;
                }
            });
            foreach (Transform t in GameObject.Find("MecanicienButton").transform)
            {
                t.gameObject.SetActive(true);
            }
            StartCoroutine(Dialogue("Hi ! I'm the mechanic at this station, tell me what you need."));
        }

        ///////////JOE///////////
        else if(name == "Joe Station x-tu9"&&(!GameObject.Find("Dialogue-Parent").transform.GetChild(0).gameObject.activeInHierarchy || lastSpeakTime == 0) && Time.time - lastSpeakTime > 0.5f && Vector2.Distance(transform.position, GameObject.Find("perso").transform.position) < 0.5f)
        {
            lastSpeakTime = Time.time;
            Camera.main.GetComponent<DeplacementPrincipal>().activePerso = gameObject;
            if (phase == 0)
            {
                ParlerYesNo("Hey ! I was waiting for you. Could you do me a favor?", "Pity ! tell me when you can.", "Joe Station x-tu9");
            }
            else if (phase == 1)
            {
                ParlerGiveObject("package", "Great ! I put you a package in your inventory, deliver it to my friend Leïla for a nice reward!", "Thin ! You have too many items, drop in and come back to me after",true);
            }
            else if(phase == 2)
            {
                Parler("So, are you going to bring the package?",false);
            }
            else if(phase == 3)
            {
                Parler("Great ! Now I'll see if you're a good miner, go mine 20 iron and come back to me.",true);
            }
            
        }
        else if (name == "Leïla Station Marcel" && Time.time - lastSpeakTime > 0.5f && Vector2.Distance(transform.position, GameObject.Find("perso").transform.position) < 0.5f)
        {
            lastSpeakTime = Time.time;
            Camera.main.GetComponent<DeplacementPrincipal>().activePerso = gameObject;
            if (phase == 1)
            {
                ParlerTakeObject("package", "Thank you very much for bringing me this package, go get this nice reward!", "You forgot to bring me the package, but don't worry, I won't move.");
            }

        }

    }

    public IEnumerator Dialogue(string texte)
    {


        if (finishDialogue || (texte != lastDialogue))
        {
            lastDialogue = texte;
            finishDialogue = false;
            foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(true); }
            GameObject.Find("Dialogue-Parent").transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;
            GameObject.Find("TexteDialogue").GetComponent<Text>().text = "";
            for (int i = 0; i < texte.Length; i++)
            {
                if (GameObject.Find("TexteDialogue") != null && texte == lastDialogue)
                {
                    GameObject.Find("TexteDialogue").GetComponent<Text>().text = GameObject.Find("TexteDialogue").GetComponent<Text>().text + texte[i];
                    yield return new WaitForSeconds(0.01f);
                }
            }
            finishDialogue = true;
        }
        yield return null;
    }

    public void ParlerYesNo(string question,string no,string name)
    {
        foreach(Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(true); }
        //dialogueObj.SetActive(true);
        StartCoroutine(Dialogue(question));
        GameObject.Find("Dialogue-Parent").transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate
        {
            if (Camera.main.GetComponent<DeplacementPrincipal>().activePerso != null)
            {
                //Camera.main.GetComponent<DeplacementPrincipal>().activePerso.GetComponent<PersoDeplacement>().dialogueObj.gameObject.SetActive(false);
                foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(false); }

                foreach (Transform child in GameObject.Find("YesNoBouton").transform) { child.gameObject.SetActive(false); }

                //foreach (Transform t in GameObject.Find("ButtonParent").transform)
                //{
                //    print("nok");
                //    t.gameObject.SetActive(false);
                //}
                Camera.main.GetComponent<DeplacementPrincipal>().activePerso = null;
            }
        });

        foreach (Transform child in GameObject.Find("YesNoBouton").transform) { child.gameObject.SetActive(true); }

        GameObject.Find("YesNoBouton").transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate
        {
            GameObject.Find(name).GetComponent<PersoDeplacement>().phase++;
            GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().persoSave.Find(name).Phase++;
            GameObject.Find(name).GetComponent<PersoDeplacement>().lastSpeakTime = 0;
            GameObject.Find(name).GetComponent<PersoDeplacement>().OnMouseUp();
            foreach (Transform child in GameObject.Find("YesNoBouton").transform) { child.gameObject.SetActive(false); }
        });
        GameObject.Find("YesNoBouton").transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate
        {
            foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(true); }
            StartCoroutine(Dialogue(no));
            foreach (Transform child in GameObject.Find("YesNoBouton").transform) { child.gameObject.SetActive(false); }
        });
    }
    public void ParlerGiveObject(string ObjectName,string success,string noPlace ,bool MissionParler)
    {

        if (GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().Items.PutInventory(ObjectName))
        {
            foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(true); }
            StartCoroutine(Dialogue(success));
            //Button.SetActive(false);
            if (MissionParler)
            {
                foreach (MissionClass m in GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().missions.missions)
                {
                    print(m.type);
                    if (m.type == "parler" && m.parlerType.PersoName == transform.name && m.parlerType.PersoPhase == phase) { m.parlerType.parler = true; }
                }
                phase++;
                GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().persoSave.Find(name).Phase++;

            }
            //phase++;
            //GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().persoSave.Find(transform.name + " " + JsonUtility.FromJson<ListsaveObj>(PlayerPrefs.GetString("saveObj")).currentSpot).Phase = 1;
            //GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().persoSave.Find("Leïla Station Marcel").Phase = 1;

        }
        else
        {
            foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(true); }
            StartCoroutine(Dialogue(noPlace));
            //Button.SetActive(false);

        }



    }
    public void Parler(string dialogue,bool MissionParler)
    {
        foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(true); }
        StartCoroutine(Dialogue(dialogue));
        GameObject.Find("Dialogue-Parent").transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate
        {
            if (Camera.main.GetComponent<DeplacementPrincipal>().activePerso != null)
            {
                //Camera.main.GetComponent<DeplacementPrincipal>().activePerso.GetComponent<PersoDeplacement>().dialogueObj.gameObject.SetActive(false);
                foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(false); }

                foreach (Transform child in GameObject.Find("YesNoBouton").transform) { child.gameObject.SetActive(false); }

                //foreach (Transform t in GameObject.Find("ButtonParent").transform)
                //{
                //    print("nok");
                //    t.gameObject.SetActive(false);
                //}
                Camera.main.GetComponent<DeplacementPrincipal>().activePerso = null;
            }
        });
        if (MissionParler)
        {
            foreach (MissionClass m in GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().missions.missions)
            {
                print(m.type);
                if (m.type == "parler" && m.parlerType.PersoName == transform.name && m.parlerType.PersoPhase == phase) { m.parlerType.parler = true; }
            }
            GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().persoSave.Find(name).Phase++;
            phase++;
        }
        foreach (Transform child in GameObject.Find("YesNoBouton").transform)
        {
            child.gameObject.SetActive(false);
        }

    }
    public void ParlerTakeObject(string item, string success,string noItem)
    {
        foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(true); }
        if (GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().Items.Contains(item))
        {
            foreach (MissionClass m in GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().missions.missions)
            {
                if (m.livraisonType.Item == item)
                {
                    m.livraisonType.avancement[1] = true;
                    GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().Items.Remove(item);
                    foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(true); }
                    StartCoroutine(Dialogue(success));
                    GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().persoSave.Find(name).Phase = 2;
                    phase++;
                }

            }
        }
        else 
        {
            foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(true); }
            StartCoroutine(Dialogue(noItem));
        }
        GameObject.Find("Dialogue-Parent").transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate
        {
            if (Camera.main.GetComponent<DeplacementPrincipal>().activePerso != null)
            {
                foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(false); }
                foreach (Transform child in GameObject.Find("YesNoBouton").transform) { child.gameObject.SetActive(false); }
                Camera.main.GetComponent<DeplacementPrincipal>().activePerso = null;
            }
        });

        foreach (Transform child in GameObject.Find("YesNoBouton").transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void ParlerTakeObject(string item,int number, string success)
    {
        foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(true); }

        while (GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().Items.Contains(item))
        {
            foreach (MissionClass m in GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().missions.missions)
            {
                if (m.livraisonType.Item == item)
                {
                    m.livraisonType.avancement[1] = true;
                    GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().Items.Remove(item);
                    foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(true); }
                    StartCoroutine(Dialogue(success));
                    GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().persoSave.Find(name).Phase = 2;
                    phase++;
                }

            }
        }
        //else
        //{
        //    foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(true); }
        //    StartCoroutine(Dialogue("eee"));
        //}
        GameObject.Find("Dialogue-Parent").transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate
        {
            if (Camera.main.GetComponent<DeplacementPrincipal>().activePerso != null)
            {
                foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(false); }
                foreach (Transform child in GameObject.Find("YesNoBouton").transform) { child.gameObject.SetActive(false); }
                Camera.main.GetComponent<DeplacementPrincipal>().activePerso = null;
            }
        });

        foreach (Transform child in GameObject.Find("YesNoBouton").transform)
        {
            child.gameObject.SetActive(false);
        }
    }







    public void Refuel()
    {
        foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(true); }
        if (Camera.main.GetComponent<DeplacementPrincipal>().Grandeur.fuel == Camera.main.GetComponent<DeplacementPrincipal>().Grandeur.fuelMax)
        {
            StartCoroutine(GameObject.Find("Mecanicien " + JsonUtility.FromJson<ListsaveObj>(PlayerPrefs.GetString("saveObj")).currentSpot).GetComponent<PersoDeplacement>().Dialogue("Your fuel tank is already full."));

        }
        else if (Camera.main.GetComponent<DeplacementPrincipal>().Grandeur.coin - 50 >= 0)
        {
            Camera.main.GetComponent<DeplacementPrincipal>().Grandeur.coin -= 50;
            StartCoroutine(GameObject.Find("Mecanicien " + JsonUtility.FromJson<ListsaveObj>(PlayerPrefs.GetString("saveObj")).currentSpot).GetComponent<PersoDeplacement>().Dialogue("I have filled in your fuel tank !"));
            Camera.main.GetComponent<DeplacementPrincipal>().Grandeur.fuel = Camera.main.GetComponent<DeplacementPrincipal>().Grandeur.fuelMax;
        }
        else
        {
            StartCoroutine(GameObject.Find("Mecanicien " + JsonUtility.FromJson<ListsaveObj>(PlayerPrefs.GetString("saveObj")).currentSpot).GetComponent<PersoDeplacement>().Dialogue("You must have 50 coin to refuel."));
        }
        foreach (Transform t in GameObject.Find("MecanicienButton").transform)
        {
            t.gameObject.SetActive(false);
        }
        GameObject.Find("Dialogue-Parent").transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate
        {
            if (Camera.main.GetComponent<DeplacementPrincipal>().activePerso != null)
            {
                foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(false); }

                foreach (Transform t in GameObject.Find("MecanicienButton").transform)
                {
                    t.gameObject.SetActive(false);
                }
                Camera.main.GetComponent<DeplacementPrincipal>().activePerso = null;
            }
        });
    }
    public void Repair()
    {

        foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(true); }
        if (Camera.main.GetComponent<DeplacementPrincipal>().Grandeur.health == Camera.main.GetComponent<DeplacementPrincipal>().Grandeur.healthMax)
        {
            StartCoroutine(GameObject.Find("Mecanicien " + JsonUtility.FromJson<ListsaveObj>(PlayerPrefs.GetString("saveObj")).currentSpot).GetComponent<PersoDeplacement>().Dialogue("Your health is already full."));

        }
        else if (Camera.main.GetComponent<DeplacementPrincipal>().Grandeur.coin - 75 >= 0)
        {
            Camera.main.GetComponent<DeplacementPrincipal>().Grandeur.coin -= 75;
            StartCoroutine(GameObject.Find("Mecanicien " + JsonUtility.FromJson<ListsaveObj>(PlayerPrefs.GetString("saveObj")).currentSpot).GetComponent<PersoDeplacement>().Dialogue("I repaired your ship."));
            Camera.main.GetComponent<DeplacementPrincipal>().Grandeur.health = Camera.main.GetComponent<DeplacementPrincipal>().Grandeur.healthMax;
        }
        else
        {
            StartCoroutine(GameObject.Find("Mecanicien "+ JsonUtility.FromJson<ListsaveObj>(PlayerPrefs.GetString("saveObj")).currentSpot).GetComponent<PersoDeplacement>().Dialogue("You must have 75 coin to repair your ship."));
        }
        foreach (Transform t in GameObject.Find("MecanicienButton").transform)
        {
            t.gameObject.SetActive(false);
        }
        GameObject.Find("Dialogue-Parent").transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate
        {
            if (Camera.main.GetComponent<DeplacementPrincipal>().activePerso != null)
            {
                foreach (Transform child in GameObject.Find("Dialogue-Parent").transform) { child.gameObject.SetActive(false); }

                foreach (Transform t in GameObject.Find("MecanicienButton").transform)
                {
                    t.gameObject.SetActive(false);
                }
                Camera.main.GetComponent<DeplacementPrincipal>().activePerso = null;
            }
        });

    }
}
