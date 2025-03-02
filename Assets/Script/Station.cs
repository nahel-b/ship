using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Station : MonoBehaviour
{


    public StationClass station = new StationClass();
    //public string Nom;
    //["livraison","objet","depart","arrivée","recompense"]
    // ou ["attaque","objet","vaisseau","arrivée","recompense"]
    //private List<string> a = new List<string>() {"",""};
    //private List<List<string>> objets = new List<List<string>>() { new List<string>() { "Super Calculator", "la mère de Louis","Supersonic Reactor" },  new List<string>() { "Black Box", "Module x-t8cv" } };


    //public List<List<string>> jobs = new List<List<string>>();

    // Start is called before the first frame update
    void Start()
    {
        //if (station.name == "")
        //{
        //    print("resetStation");
        //int a = Random.Range(0, Camera.main.GetComponent<Principal>().StationName.Count);
        //transform.name = Camera.main.GetComponent<Principal>().StationName[a];
        //station.name = Camera.main.GetComponent<Principal>().StationName[a];
        //Camera.main.GetComponent<Principal>().StationName.RemoveAt(a);
        //station.index = Camera.main.GetComponent<Principal>().Stations.Count;
        //Camera.main.GetComponent<Principal>().Stations.Add(gameObject);
        //}
        //else
        //{
        //    print("load");
        //    transform.name = station.name;
        //    Camera.main.GetComponent<Principal>().StationName.Remove(station.name);
        //    Camera.main.GetComponent<Principal>().Stations[station.index] = gameObject;


        //}




    }

    // Update is called once per frame
    void Update()
    {

        //if(Time.time>1 && station.jobs.Count == 0)
        //{
        //    List<GameObject> l = Camera.main.GetComponent<Principal>().Stations;
        //    for (int i = 0; i < 4; i++)
        //    {
        //        GameObject a = l[Random.Range(0, l.Count)];
        //        GameObject b = l[Random.Range(0, l.Count)];
        //        //while (b == a) { b = l[Random.Range(0, l.Count)]; }
        //        station.jobs.Add(new metier().RandomMetier());

        //        //if (Random.Range(0, 2) == 0) { jobs.Add(new List<string>() { "livraison", objets[0][Random.Range(0, objets[0].Count)], a.name,b.name, Random.Range(0,300).ToString() }); }
        //        //else { jobs.Add(new List<string>() { "livraison", objets[0][Random.Range(0, objets[0].Count)], a.name, b.name, Random.Range(0, 300).ToString() }); }

        //    }

        //}


        transform.GetChild(1).transform.eulerAngles = new Vector3(0, 0, transform.GetChild(1).transform.eulerAngles.z - 10 * Time.deltaTime);
        transform.GetChild(2).transform.eulerAngles = new Vector3(0, 0, transform.GetChild(2).transform.eulerAngles.z + 20 * Time.deltaTime);

    }


#if UNITY_EDITOR

    //private void OnMouseDown()
    //{
    //    tap();

    //}
#endif
    //public void VerifyJob()
    //{
    //    int i = 0;
    //    List<metier> a = GameObject.Find("Main Camera").GetComponent<Principal>().Jobs;
    //    foreach (metier job in a)
    //    {
    //        //print(job[2]);
    //        if (job.type == "livraison" && job.depart == name && GameObject.Find("Main Camera").GetComponent<Principal>().Items.Count() < GameObject.Find("Main Camera").GetComponent<Principal>().Items.maxItem() )
    //        {
    //            GameObject.Find("Main Camera").GetComponent<Principal>().Jobs[i].recuperer = true;
    //            GameObject.Find("Main Camera").GetComponent<Principal>().notifie.Add("\"" + job.obj.ToString() + "\" added in the cargo");
    //            GameObject.Find("Main Camera").GetComponent<Principal>().Items.Add(job.obj.ToString());
    //            GameObject.Find("Main Camera").GetComponent<Principal>().Fleche(GameObject.Find(GameObject.Find("Main Camera").GetComponent<Principal>().Jobs[i].arrive).transform);
    //        }
    //        else if (job.type == "livraison" && job.depart == name && GameObject.Find("Main Camera").GetComponent<Principal>().Items.Count() >= GameObject.Find("Main Camera").GetComponent<Principal>().Items.maxItem())
    //        {
    //            print("full");
    //            GameObject.Find("Main Camera").GetComponent<Principal>().notifie.Add("Cargo is full, \"" + job.obj.ToString() + "\" cannot be added");
    //        }
    //        else if (job.type == "livraison" && job.recuperer == true && job.arrive == name)
    //        {
    //            print("packet transporté bg");
    //            GameObject.Find("Main Camera").GetComponent<Principal>().Grandeur.coin += a[i].recompense;
    //            GameObject.Find("Main Camera").GetComponent<Principal>().notifie.Add("Job finished, reward : " + a[i].recompense.ToString() + "$");
    //            GameObject.Find("Main Camera").GetComponent<Principal>().Jobs.RemoveAt(i);
    //        }
    //        i++;
    //    }
    //}

    public IEnumerator enter()
    {

        if (GameObject.Find("SceneTransition").GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "SceneTransitionStart")
        {


            GameObject.Find("SceneTransition").GetComponent<Image>().enabled = (true);
            GameObject.Find("Main Camera").GetComponent<Principal>().Save();
            ListsaveObj startListe = new ListsaveObj();
            ListsaveObj svObj = JsonUtility.FromJson<ListsaveObj>(PlayerPrefs.GetString("saveObj",JsonUtility.ToJson(startListe)));
            svObj.currentSpot = transform.name;

            print(name);
            PlayerPrefs.SetString("saveObj", JsonUtility.ToJson(svObj));
            Camera.main.GetComponent<Principal>().Save();

            GameObject.Find("SceneTransition").GetComponent<Animator>().SetTrigger("ferme");
            GameObject.Find("CanvasTransition").GetComponent<CanvasGroup>().blocksRaycasts = true;
            yield return new WaitForSeconds(0.25f);
            string txt = "Docking on " + transform.name + "....";
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


            SceneManager.LoadSceneAsync(2);
        }
        yield return null;
    }







}
