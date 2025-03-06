using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildBouton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isMouseDown = false;

    public bool Mouse = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isMouseDown)
        {

            Mouse = true;
        }
        if (name.Contains("Vide") && Camera.main.GetComponent<BuildPrincipal>().selectObj != null && Camera.main.GetComponent<BuildPrincipal>().selectObj.GetComponent<BuildPiece>().moove) { Destroy(transform.parent.gameObject); }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && Mouse && name.Contains("ItemObjPrefab"))
        {
            Mouse = false;
            print(Camera.main.GetComponent<BuildPrincipal>().Items.Pieces[transform.GetSiblingIndex()].nom);

            GameObject.Find("Scroll").GetComponent<ScrollRect>().horizontal = true;

        }
        if (Mouse && !isMouseDown && Input.GetTouch(0).position.y > Screen.height / 5 && Mouse && name.Contains("ItemObjPrefab"))
        {
            GameObject.Find("Scroll").GetComponent<ScrollRect>().horizontal = false;
            spawnPiece();

            Destroy(gameObject);
        }


    }

    private void OnMouseUp()
    {
        print(name);
        if (name.Contains("turnBtn"))
        {
            GameObject selectObj = Camera.main.GetComponent<BuildPrincipal>().selectObj;

            selectObj.transform.eulerAngles = new Vector3(0, 0, selectObj.transform.eulerAngles.z - selectObj.GetComponent<BuildPiece>().rotFrame);
            if (selectObj.GetComponent<BuildPiece>().Verify() != "") { selectObj.GetComponent<SpriteRenderer>().color = new Color(1, 0.345098f, 0.345098f); GameObject.Find("VerifyTexte").GetComponent<Text>().text = "Warning : " + selectObj.name + " " + selectObj.GetComponent<BuildPiece>().Verify(); }
            else if (selectObj.GetComponent<BuildPiece>().Verify() == "") { selectObj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1); GameObject.Find("VerifyTexte").GetComponent<Text>().text = ""; }
        }
        else if (name.Contains("Vide"))
        {
            GameObject selectObj = Camera.main.GetComponent<BuildPrincipal>().selectObj;

            if (selectObj.GetComponent<BuildPiece>().Verify() != "") { selectObj.GetComponent<SpriteRenderer>().color = new Color(1, 0.345098f, 0.345098f); GameObject.Find("VerifyTexte").GetComponent<Text>().text = "Warning : " + selectObj.name + " " + selectObj.GetComponent<BuildPiece>().Verify(); }
            else if (selectObj.GetComponent<BuildPiece>().Verify() == "") { selectObj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1); GameObject.Find("VerifyTexte").GetComponent<Text>().text = ""; }
            Camera.main.GetComponent<BuildPrincipal>().selectObj = null; Destroy(transform.parent.gameObject, 0.1f);
        }
    }
    public void spawnPiece()
    {
        BuildPrincipal buildPrincipal = Camera.main.GetComponent<BuildPrincipal>();
        
        // Get piece from inventory
        PieceClass p = buildPrincipal.Items.Pieces[transform.GetSiblingIndex()];
        print(p.nom);
        
        // Calculate position based on mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 position = new Vector3(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), transform.position.z);
        
        // Place the piece using BuildPrincipal's method
        GameObject pObj = buildPrincipal.PlacePiece(
            p, 
            GameObject.Find("Vaisseau").transform,
            true,  // Make interactive
            position  // Use mouse position
        );
        
        // Remove the piece from inventory
        buildPrincipal.Items.Pieces.RemoveAt(transform.GetSiblingIndex());
    }
    public void OnPointerDown(PointerEventData data)
    {
        isMouseDown = true;
        // Start blocking
    }

    public void OnPointerUp(PointerEventData data)
    {
        isMouseDown = false;
        // Stop blocking
    }


}
