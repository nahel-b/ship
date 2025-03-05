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
        PieceClass p = Camera.main.GetComponent<BuildPrincipal>().Items.Pieces[transform.GetSiblingIndex()];
        print(p.nom);
        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject pObj = Instantiate(PieceLoader.GetPiecePrefab(p.nom), new Vector3(Mathf.Round(MousePos.x), Mathf.Round(MousePos.y), transform.position.z), Quaternion.Euler(p.eulerAngle));
        pObj.name = p.nom;
        pObj.AddComponent<BuildPiece>();

        foreach (FixedJoint2D component in pObj.GetComponents<FixedJoint2D>()) { Destroy(component); }
        foreach (Piece component in pObj.GetComponents<Piece>()) { pObj.GetComponent<BuildPiece>().vie = component.vie; Destroy(component); }
        foreach (Reacteur component in pObj.GetComponents<Reacteur>()) { Destroy(component); }
        if (pObj.GetComponent<BoxCollider2D>() != null) { pObj.GetComponent<BoxCollider2D>().isTrigger = true; }
        if (pObj.GetComponent<PolygonCollider2D>() != null) { Destroy(pObj.GetComponent<PolygonCollider2D>()); pObj.AddComponent<BoxCollider2D>(); pObj.GetComponent<BoxCollider2D>().isTrigger = true; }
        pObj.transform.parent = GameObject.Find("Vaisseau").transform;
        pObj.GetComponent<BuildPiece>().objPrefab = PieceLoader.GetPiecePrefab(p.nom);
        pObj.GetComponent<BuildPiece>().niveau = p.niveau;
        pObj.GetComponent<BuildPiece>().attchableSide = p.attchableSide;
        pObj.GetComponent<BuildPiece>().dependant = p.dependant;
        pObj.GetComponent<BuildPiece>().socle = p.socle;
        pObj.GetComponent<BuildPiece>().rotFrame = p.rotFrame;
        if (p.vie == -1 && !p.dependant) { pObj.GetComponent<BuildPiece>().vie = PieceLoader.GetPiecePrefab(p.nom).GetComponent<Piece>().vieListe[p.niveau]; }
        else if (!p.dependant) { pObj.GetComponent<BuildPiece>().vie = p.vie; }

        if (p.dependant)
        {
            print(transform.name);

            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector3.up, 0.3f);
            foreach (RaycastHit2D obj in hit)
            {
                print(obj.transform.name);
                if (obj.transform.parent == GameObject.Find("Vaisseau").transform || obj.transform.parent == pObj.transform.parent) { pObj.transform.parent = obj.transform; }
            }
        }

        //pObj.GetComponent<BuildPiece>().spawnpress = true;

        pObj.GetComponent<BuildPiece>().sp = true;
        pObj.GetComponent<BuildPiece>().Start();
        pObj.GetComponent<BuildPiece>().OnMouseDown();
        pObj.GetComponent<BuildPiece>().OnMouseDrag();

        Camera.main.GetComponent<BuildPrincipal>().Items.Pieces.RemoveAt(transform.GetSiblingIndex());

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
