using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TouchableSpaceObject : MonoBehaviour
{

    public InfoSpaceObject info;

    public UnityEvent onTouchAction = null;

    static GameObject texteImage;
    static GameObject descriptionImage;
    static GameObject bouton;
    


    private void Awake()
    {
        if (texteImage == null)
        {
            texteImage = Resources.Load<GameObject>("Prefab/UI/[Info-ligne] texte - image");
            if (texteImage == null)
            {
                Debug.LogWarning("texteImage prefab not found in Resources/Prefabs folder.");
            }
        }
        if (descriptionImage == null)
        {
            descriptionImage = Resources.Load<GameObject>("Prefab/UI/[Info-ligne] description - image");
            if (descriptionImage == null)
            {
                Debug.LogWarning("descriptionImage prefab not found in Resources/Prefabs folder.");
            }
        }
        if (bouton == null)
        {
            bouton = Resources.Load<GameObject>("Prefab/UI/[Info-ligne] bouton");
            if (bouton == null)
            {
                Debug.LogWarning("bouton prefab not found in Resources/Prefabs folder.");
            }
        }
    }

    public void OnTouch()
    {
        if(onTouchAction.GetPersistentEventCount() > 0)
        {
            onTouchAction.Invoke();
        }
        else
        {
            createUIMenu();

            GameObject.Find("[Rendu]").GetComponent<UIFollowObject>().target = transform;
        }
    }

    public void createUIMenu()
    {
        GameObject.Find("[ligne-info-OBJ]").transform.GetChild(1).gameObject.SetActive(true);
        GameObject parent = GameObject.Find("[ligne-info-OBJ]").transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        
         foreach(Transform child in parent.transform) 
        {
            Destroy(child.gameObject);
        }
        
        GameObject.Find("[ligne-info-OBJ]").transform.GetChild(0).GetComponent<Text>().text = info.titre;

        foreach (ligneInfo ligne in info.lignes)
        {
            if(ligne.bouton)
            {
                GameObject newBouton = Instantiate(bouton, parent.transform);
                newBouton.transform.GetChild(0).GetComponent<Text>().text = ligne.text;
                
                // Use a lambda to capture this.gameObject and pass it to ExecuteTouchAction
                GameObject targetObject = this.gameObject;
                newBouton.GetComponent<Button>().onClick.AddListener(() => ligne.ExecuteTouchAction(targetObject));
            }
            else if(ligne.description)
            {
                GameObject newDescription = Instantiate(descriptionImage, parent.transform);
                newDescription.transform.GetChild(1).GetComponent<Text>().text = ligne.text;
                newDescription.transform.GetChild(0).GetComponent<Image>().sprite = ligne.image;
            }
            else
            {
                GameObject newTexteImage = Instantiate(texteImage, parent.transform);
                newTexteImage.transform.GetChild(1).GetComponent<Text>().text = ligne.text;
                newTexteImage.transform.GetChild(0).GetComponent<Image>().sprite = ligne.image;
            }
        }

        parent.GetComponent<VerticalLayoutGroup>().childScaleHeight = false;

        parent.GetComponent<VerticalLayoutGroup>().childScaleHeight = true;
    }


    void Start()
    {
        foreach(Transform child in GameObject.Find("[ligne-info-OBJ]").transform.GetChild(1).GetChild(0).GetChild(0) )
        {
            Destroy(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


[System.Serializable]
public class InfoSpaceObject 
{
    public string titre;
    public List<ligneInfo> lignes;
}

[System.Serializable]

public class ligneInfo
{
    public Sprite image;
    public string text;
    public float unlockLevel; // niveau de la vision pour voir cette ligne

    public bool bouton;

    public bool description;

    // Change to UnityEvent<GameObject> to accept a GameObject parameter
    public UnityEvent<GameObject> onTouch;

    public void ExecuteTouchAction(GameObject targetObject)
    {
        if (onTouch != null)
            onTouch.Invoke(targetObject);
    }

 
}