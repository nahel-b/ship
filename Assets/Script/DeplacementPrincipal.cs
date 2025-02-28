using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class DeplacementPrincipal : MonoBehaviour
{
    public DeplacementClass DeplacementClass;
    public GameObject activePerso;
    public Grandeur Grandeur = new Grandeur();
    public MissionListe missions;
    public ItemClass Items;
    public float offset;
    public PersoSave persoSave;
    //etoile
    public List<Sprite> EtoileSprite;
    public Color[] EtoileCouleur;
    public GameObject etoile;
    private Vector3 lastPos = new Vector3(0,0,0);


    // Start is called before the first frame update
    void Start()
    {
        Generation(-5,5,-5,5);
        StartCoroutine(Generation(5));
        GameObject.Find("Liste").GetComponent<Liste>().colorList = JsonUtility.FromJson<ColorList>(PlayerPrefs.GetString("Color"));
        persoSave = JsonUtility.FromJson<PersoSave>(PlayerPrefs.GetString("PersoSave",JsonUtility.ToJson( persoSave)));
        foreach (PersoClass perso in persoSave.Perso) { GameObject.Find(perso.name).GetComponent<PersoDeplacement>().phase = perso.Phase; }
        ListsaveObj svObj = JsonUtility.FromJson<ListsaveObj>(PlayerPrefs.GetString("saveObj"));
        missions = JsonUtility.FromJson<MissionListe>(PlayerPrefs.GetString("Missions"));
        GameObject.Find("perso").transform.position = DeplacementClass.Find(svObj.currentSpot).apparitionPos;
        print(svObj.currentSpot);
        Items = svObj.Items;
        Grandeur = JsonUtility.FromJson<Grandeur>(PlayerPrefs.GetString("Grandeur", JsonUtility.ToJson(Grandeur)));
        GameObject.Find("SceneTransition").GetComponent<Animator>().SetTrigger("ouvre");
    }
    void FixedUpdate()
    {
        Vector3 desiredPosition = new Vector3( GameObject.Find("perso").transform.position.x , GameObject.Find("perso").transform.position.y ,-10);

        if (Vector3.Distance(desiredPosition, transform.position) > offset)
        {
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 0.125f);

            transform.position = smoothedPosition;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 p = transform.position;
        GameObject.Find("EtoileBG1").transform.position = new Vector3(p.x * 0.2f, p.y * 0.2f, 10);
        GameObject.Find("EtoileBG2").transform.position = new Vector3(p.x * 0.4f, p.y * 0.4f, 10);
        GameObject.Find("EtoileBG3").transform.position = new Vector3(p.x * 0.6f, p.y * 0.6f, 10);
        //GameObject.Find("PlaneteBG1").transform.position = new Vector3(transform.parent.position.x * 0.8f, transform.parent.position.y * 0.8f, 5);


        GameObject.Find("CoinObj").GetComponent<Text>().text = Grandeur.coin.ToString() + "$";
    }

   

    public void Save()
    {
        ListsaveObj i = JsonUtility.FromJson<ListsaveObj>(PlayerPrefs.GetString("saveObj"));
        i.Items = Items;
        DeckList d = GameObject.Find("Liste").GetComponent<Liste>().deckList;
        PlayerPrefs.SetString("Deck", JsonUtility.ToJson(d));
        PlayerPrefs.SetString("saveObj", JsonUtility.ToJson(i));
        PlayerPrefs.SetString("Grandeur", JsonUtility.ToJson(Grandeur));
        PlayerPrefs.SetString("Missions", JsonUtility.ToJson(missions));
        PlayerPrefs.SetString("PersoSave", JsonUtility.ToJson(persoSave));
        PlayerPrefs.SetString("Color", JsonUtility.ToJson(GameObject.Find("Liste").GetComponent<Liste>().colorList));
    }





    public IEnumerator exit()
    {
        GameObject.Find("SceneTransition").GetComponent<Image>().enabled = (true);
        Camera.main.GetComponent<DeplacementPrincipal>().Save();
        
       
        GameObject.Find("SceneTransition").GetComponent<Animator>().SetTrigger("ferme");
        yield return new WaitForSeconds(0.25f);
        ListsaveObj svObj = JsonUtility.FromJson<ListsaveObj>(PlayerPrefs.GetString("saveObj"));
        string txt = "Undocking of the " + svObj.currentSpot + "....";
        for (int i = 0; i < txt.Length; i++)
        {
            GameObject.Find("Transitiontxt").GetComponent<Text>().text = GameObject.Find("Transitiontxt").GetComponent<Text>().text + txt[i];
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.5f);
        Color a = GameObject.Find("Transitiontxt").GetComponent<Text>().color;
        for (float i = 1; i >= 0; i -= 0.1f)
        {
            //GameObject.Find("notifieTexte").GetComponent<Text>().color = new Color(GameObject.Find("notifieTexte").GetComponent<Text>().color[0], GameObject.Find("notifieTexte").GetComponent<Text>().color[1], GameObject.Find("notifieTexte").GetComponent<Text>().color[2], i);
            a.a = i;
            GameObject.Find("Transitiontxt").GetComponent<Text>().color = a;
            yield return new WaitForSeconds(0.01f);
        }
        a.a = 0;
        GameObject.Find("Transitiontxt").GetComponent<Text>().color = a;


        SceneManager.LoadSceneAsync(0);


        yield return null;
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save();
        }
    }
    public void OnApplicationQuit()
    {
        Save();
    }
    public void RewardVoid(RecompenseClass rec)
    {
        StartCoroutine(GameObject.Find("Reward-Parent").GetComponent<Reward>().Recompense(rec));
    }

    void Generation(float a, float b, float c, float d)
    {
        int nb = Mathf.RoundToInt((Mathf.Abs(a - b) * Mathf.Abs(c - d)) *8);
        for (int i = 0; i < nb; i++)
        {
            GameObject etoileObj = Instantiate(etoile, new Vector3(Random.Range(a, b), Random.Range(c, d), 0), Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
            int spriteNb = Random.Range(0, EtoileSprite.Count);
            float taille;
            if (spriteNb <= 2)
            {
                taille = Random.Range(0.01f, 0.015f);
            }
            else
            {
                taille = 1f * Random.Range(0.01f, 0.015f);
            }
            etoileObj.transform.localScale = new Vector3(taille, taille, 1);
            etoileObj.GetComponent<SpriteRenderer>().sprite = EtoileSprite[spriteNb];
            etoileObj.GetComponent<SpriteRenderer>().color = EtoileCouleur[Random.Range(0, EtoileCouleur.Length)];
            int e = Random.Range(0, 3);
            switch (e)
            {
                case 0:
                    etoileObj.transform.parent = GameObject.Find("EtoileBG1").transform;
                    break;
                case 1:
                    etoileObj.transform.parent = GameObject.Find("EtoileBG2").transform;
                    break;
                case 2:
                    etoileObj.transform.parent = GameObject.Find("EtoileBG3").transform;
                    break;
            }
        }
    }

    IEnumerator Generation(float rayon)
    {
        Vector3 pos = transform.position;

        if (lastPos.y < pos.y) { Generation(pos.x - rayon, pos.x + rayon, lastPos.y + rayon, pos.y + rayon); }
        else { Generation(pos.x - rayon, pos.x + rayon, lastPos.y - rayon, pos.y - rayon); }
        if (lastPos.x < pos.x) { Generation(lastPos.x + rayon, pos.x + rayon, pos.y - rayon, pos.y + rayon); }
        else { Generation(lastPos.x - rayon, pos.x - rayon, pos.y - rayon, pos.y + rayon); }


        for (int i = 1; i <= 3; i++)
        {
            foreach (Transform child in GameObject.Find("EtoileBG" + i).transform)
            {
                if (child.transform.position.y + rayon < pos.y) { Destroy(child.gameObject); }
                else if (child.transform.position.y - rayon > pos.y) { Destroy(child.gameObject); }
                else if (child.transform.position.x + rayon < pos.x) { Destroy(child.gameObject); }
                else if (child.transform.position.x - rayon > pos.x) { Destroy(child.gameObject); }
            }
        }





        lastPos = transform.position;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Generation(rayon));
    }




}
