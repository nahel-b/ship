using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Reward : MonoBehaviour
{
    public int index = 0;
    public bool wait = false;
    public RecompenseClass recompense;
    public GameObject PieceObjDeck;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public IEnumerator Recompense(RecompenseClass Recompense)
    {
        index = 0;
        recompense = Recompense;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            GameObject.Find("Main Camera").GetComponent<Principal>().Grandeur.coin += recompense.Argent;
        }
        else
        {
            GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().Grandeur.coin += recompense.Argent;

        }

        //TODO: Ajouter les pieces dans l'inventaire
        // foreach (PieceClass p in Recompense.pieces)
        // {
        //     if (SceneManager.GetActiveScene().buildIndex == 0)
        //     {
        //         if (!GameObject.Find("Main Camera").GetComponent<Principal>().Items.PutInventory(p))
        //         {
        //             print("Pas mis dedans");
        //         }
        //     }
        //     else
        //     {
        //         if (!GameObject.Find("worldCam").GetComponent<DeplacementPrincipal>().Items.PutInventory(p))
        //         {
        //             print("Pas mis dedans");
        //         }
        //     }

        // }
        foreach (string deck in Recompense.Deck)
        {

            GameObject.Find("Liste").GetComponent<Liste>().deckList.Find(deck).debloque = true;
        }
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            GameObject.Find("Main Camera").GetComponent<Principal>().Save();
        }
        else
        {
            GameObject.Find("wolrdCam").GetComponent<DeplacementPrincipal>().Save();

        }
        if (recompense.Argent > 0)
        {

            
            transform.GetChild(3).GetComponent<Text>().text = "";
            transform.GetChild(3).GetComponent<RectTransform>().localPosition = new Vector3(0, -200, 0);
            transform.GetChild(3).gameObject.SetActive(true);
            //45 + 35
            int nb = 50;
            float b = 0;
            float fontSize = 45;
            for (float i = 0; i < nb; i++)
            {
                transform.GetChild(3).GetComponent<RectTransform>().localPosition = new Vector3(0, transform.GetChild(3).GetComponent<RectTransform>().localPosition.y + (400f/ 50f), 0);
                b = b + (recompense.Argent / 50f);
                transform.GetChild(3).GetComponent<Text>().text = "+" + Mathf.Round(b).ToString() +"$";
                transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "+" + Mathf.Round(b).ToString() + "$";

                fontSize = fontSize + (35f / 50f);
                transform.GetChild(3).GetComponent<Text>().fontSize = Mathf.RoundToInt(fontSize);
                transform.GetChild(3).GetChild(0).GetComponent<Text>().fontSize = Mathf.RoundToInt(fontSize);

                yield return new WaitForSeconds(0.001f);
            }

           


        }
        if (recompense.xp > 0)
        {

            transform.GetChild(4).GetComponent<Text>().text = "";
            transform.GetChild(4).GetComponent<RectTransform>().localPosition = new Vector3(0, -200, 0);
            transform.GetChild(4).gameObject.SetActive(true);
            //45 + 35
            int nb = 50;
            float b = 0;
            float fontSize = 45;
            for (float i = 0; i < nb; i++)
            {
                transform.GetChild(4).GetComponent<RectTransform>().localPosition = new Vector3(0, transform.GetChild(4).GetComponent<RectTransform>().localPosition.y + (336f / 50f), 0);
                b = b + (recompense.xp / 50f);
                transform.GetChild(4).GetComponent<Text>().text = "+" + Mathf.Round(b).ToString() + "xp";
                transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "+" + Mathf.Round(b).ToString() + "xp";


                fontSize = fontSize + (35f / 50f);
                transform.GetChild(4).GetComponent<Text>().fontSize = Mathf.RoundToInt(fontSize);
                transform.GetChild(4).GetChild(0).GetComponent<Text>().fontSize = Mathf.RoundToInt(fontSize);
                yield return new WaitForSeconds(0.001f);
            }
        }
        if(recompense.Argent>0 || recompense.xp>0)
        {
            yield return new WaitForSeconds(1);
            transform.GetChild(4).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);

        }
        if(recompense.Argent > 0 && recompense.xp > 0)
        {
            transform.GetChild(4).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(true);
            for(float i = 1; i >= 0; i = i - 0.03f)
            {
                Color Coin1 = transform.GetChild(3).GetComponent<Text>().color;
                Coin1.a = i;
                transform.GetChild(3).GetComponent<Text>().color = Coin1;
                Color Coin2 = transform.GetChild(3).GetChild(0).GetComponent<Text>().color;
                Coin2.a = i;
                transform.GetChild(3).GetChild(0).GetComponent<Text>().color = Coin2;
                Color Xp1 = transform.GetChild(4).GetComponent<Text>().color;
                Xp1.a = i;
                transform.GetChild(4).GetComponent<Text>().color = Xp1;
                Color Xp2 = transform.GetChild(4).GetChild(0).GetComponent<Text>().color;
                Xp2.a = i;
                transform.GetChild(4).GetChild(0).GetComponent<Text>().color = Xp2;
                yield return new WaitForSeconds(0.01f);
            }
            transform.GetChild(4).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
        }
        if(recompense.pieces.Count>0 || recompense.Deck.Count > 0)
        {
            GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(8).gameObject.SetActive(true);
            for (float i = 0; i < 1; i = i + 0.04f)
            {
                Color pieceColor = transform.GetChild(0).gameObject.GetComponent<Image>().color;
                pieceColor.a = i;
                transform.transform.GetChild(0).gameObject.GetComponent<Image>().color = pieceColor;
                yield return new WaitForSeconds(0.01f);
            }
            transform.GetChild(9).gameObject.SetActive(true);
            string str = "Rewards :";
            transform.GetChild(9).GetChild(0).GetComponent<Text>().text = "";
            transform.GetChild(9).GetChild(0).GetChild(0).GetComponent<Text>().text = "";
            for (int i = 0; i < str.Length; i++)
            {
                transform.GetChild(9).GetChild(0).GetComponent<Text>().text = transform.GetChild(9).GetChild(0).GetComponent<Text>().text + str[i];
                transform.GetChild(9).GetChild(0).GetChild(0).GetComponent<Text>().text = transform.GetChild(9).GetChild(0).GetChild(0).GetComponent<Text>().text + str[i];
                yield return new WaitForSeconds(0.06f);
            }
        }
        // foreach (PieceClass piece in Recompense.pieces)
        // {
            

        //     transform.GetChild(2).gameObject.SetActive(true);
        //     transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        //     transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
        //     transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = GameObject.Find("Liste").GetComponent<Liste>().ListePiece.Find(piece.nom).GetComponent<SpriteRenderer>().sprite;
        //     transform.GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        //     for (float i = 0; i < 1; i += 0.01f)
        //     {
        //         Color a = transform.GetChild(2).GetChild(0).GetComponent<Image>().color;
        //         a.a = i;
        //         transform.GetChild(2).GetChild(0).GetComponent<Image>().color = a;
        //         yield return new WaitForSeconds(0.01f);
        //     }

        //     transform.GetChild(1).gameObject.SetActive(true);
        //     transform.GetChild(1).GetComponent<Animator>().SetTrigger("explosion");

        //     yield return new WaitForSeconds(0.4f);
        //     for (float i = 0; i < 1; i += 0.1f)
        //     {
        //         Color a = transform.GetChild(2).GetChild(0).GetComponent<Image>().color;
        //         a.r = i; a.g = i; a.b = i;
        //         transform.GetChild(2).GetChild(0).GetComponent<Image>().color = a;
        //         yield return new WaitForSeconds(0.01f);
        //     }
        //     transform.GetChild(2).GetChild(1).GetComponent<Text>().text = "";
        //     transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
        //     string nom = piece.nom;

        //     for (int i = 0; i < nom.Length; i++)
        //     {
        //         transform.GetChild(2).GetChild(1).GetComponent<Text>().text = transform.GetChild(2).GetChild(1).GetComponent<Text>().text + nom[i];
        //         yield return new WaitForSeconds(0.06f);
        //     }
        //     GameObject obj = Instantiate(GameObject.Find("Liste").GetComponent<Liste>().ListePiece.Find(piece.nom), transform);
        //     if (obj.GetComponent<Piece>() != null) { Destroy(obj.GetComponent<Piece>()); }
        //     if (obj.GetComponent<Canon>() != null) { Destroy(obj.GetComponent<Canon>()); }
        //     if (obj.GetComponent<Vis>() != null) { Destroy(obj.GetComponent<Vis>()); }
        //     if (obj.GetComponent<Collider2D>() != null) { Destroy(obj.GetComponent<Collider2D>()); }
        //     if (obj.GetComponent<BoxCollider2D>() != null) { Destroy(obj.GetComponent<BoxCollider2D>()); }
        //     if (obj.GetComponent<PolygonCollider2D>() != null) { Destroy(obj.GetComponent<PolygonCollider2D>()); }

        //     obj.gameObject.SetActive(true);
        //     if (obj.GetComponent<Animator>() != null)
        //     {
        //         obj.GetComponent<Animator>().SetBool("presentation", true);
        //     }
        //     //transform.GetChild(2).GetChild(0).GetComponent<Image>().material = obj.GetComponent<SpriteRenderer>().material;
        //     wait = true;



        //     while (wait)
        //     {
        //         transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = obj.GetComponent<SpriteRenderer>().sprite;
        //         yield return new WaitForSeconds(0.01f);
        //     }
        //     wait = true;
        //     transform.GetChild(2).GetComponent<Animator>().SetTrigger("slide");
        //     yield return new WaitForSeconds(0.3f);
        //     transform.GetChild(7).gameObject.SetActive(true);
        //     string desc = descritption(piece);
        //     transform.GetChild(7).GetComponent<Text>().text = "";
        //     for (int i = 0; i < desc.Length; i++)
        //     {
        //         transform.GetChild(7).GetComponent<Text>().text = transform.GetChild(7).GetComponent<Text>().text + desc[i];
        //         transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = obj.GetComponent<SpriteRenderer>().sprite;
        //         yield return new WaitForSeconds(0.01f);
        //     }
        //     wait = true;
        //     while (wait)
        //     {
        //         transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = obj.GetComponent<SpriteRenderer>().sprite;
        //         yield return new WaitForSeconds(0.01f);
        //     }
        //     Destroy(obj.gameObject);
        //     transform.GetChild(7).gameObject.SetActive(false);
        //     transform.GetChild(2).GetComponent<Animator>().SetTrigger("inventory");
        //     yield return new WaitForSeconds(0.4f);
        //     if(recompense.Deck.Count == 0)
        //     {
        //         transform.GetChild(0).gameObject.SetActive(false);
        //         transform.GetChild(8).gameObject.SetActive(false);
        //         transform.GetChild(9).gameObject.SetActive(false);
        //         GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(true);

        //     }
        //     yield return new WaitForSeconds(0.5f);

        // }
        transform.GetChild(2).gameObject.SetActive(false);

        //h-1=4.99
        //h-2=1.666
        //h-3=0.999
        //h-4=0.713
        //h-5=0.555
        //h-6=0.454
        //h-7=0.385
        //h-8=0.333
        //h-9=0.293
        foreach (string deck in recompense.Deck)
        {
            transform.GetChild(5).gameObject.SetActive(true);
            Assemblage deckObj = GameObject.Find("Liste").GetComponent<Liste>().deckList.Find(deck);
            float h = 0;
            List<float> taille = new List<float>() { 4.99f, 1.666f, 0.999f, 0.713f, 0.555f, 0.454f, 0.385f, 0.333f, 0.293f };
            foreach (PieceClass p in deckObj.assemblage)
            {
                if (h < (p.position.x + GameObject.Find("Liste").GetComponent<Liste>().ListePiece.Find(p.nom).GetComponent<SpriteRenderer>().sprite.rect.width / 32))
                {
                    h = (p.position.x + GameObject.Find("Liste").GetComponent<Liste>().ListePiece.Find(p.nom).GetComponent<SpriteRenderer>().sprite.rect.width / 32);
                }
                if (h < (p.position.y + GameObject.Find("Liste").GetComponent<Liste>().ListePiece.Find(p.nom).GetComponent<SpriteRenderer>().sprite.rect.height / 32))
                {
                    h = (p.position.y + GameObject.Find("Liste").GetComponent<Liste>().ListePiece.Find(p.nom).GetComponent<SpriteRenderer>().sprite.rect.height / 32);
                }

            }
            print(h);
            transform.GetChild(5).GetComponent<RectTransform>().localScale = new Vector3(taille[Mathf.RoundToInt(h)], taille[Mathf.RoundToInt(h)], 1);
            int indexPiece = 0;
            foreach (PieceClass p in deckObj.assemblage)
            {
                GameObject pieceObj = Instantiate(PieceObjDeck,transform.GetChild(5));
                pieceObj.GetComponent<RectTransform>().localPosition = p.position * 100;
                pieceObj.GetComponent<RectTransform>().eulerAngles = p.eulerAngle;
                if (GameObject.Find("Liste").GetComponent<Liste>().ListePiece.Find(p.nom).GetComponent<SpriteRenderer>().flipX) { pieceObj.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1); }


                pieceObj.GetComponent<Image>().sprite = GameObject.Find("Liste").GetComponent<Liste>().ListePiece.Find(p.nom).GetComponent<SpriteRenderer>().sprite;
                for (float i = 0; i < 1; i = i + 0.02f)
                {
                    Color pieceColor = transform.GetChild(5).GetChild(indexPiece).GetComponent<Image>().color;
                    pieceColor.a = i;
                    transform.GetChild(5).GetChild(indexPiece).GetComponent<Image>().color = pieceColor;
                    yield return new WaitForSeconds(0.01f);
                }
                GameObject obj = Instantiate(GameObject.Find("Liste").GetComponent<Liste>().ListePiece.Find(p.nom), transform.GetChild(5));
                if (obj.GetComponent<Piece>() != null) { Destroy(obj.GetComponent<Piece>()); }
                if (obj.GetComponent<Canon>() != null) { Destroy(obj.GetComponent<Canon>()); }
                if (obj.GetComponent<Vis>() != null) { Destroy(obj.GetComponent<Vis>()); }
                if (obj.GetComponent<Collider2D>() != null) { Destroy(obj.GetComponent<Collider2D>()); }
                if (obj.GetComponent<BoxCollider2D>() != null) { Destroy(obj.GetComponent<BoxCollider2D>()); }
                if (obj.GetComponent<PolygonCollider2D>() != null) { Destroy(obj.GetComponent<PolygonCollider2D>()); }

                obj.gameObject.SetActive(true);
                if (obj.GetComponent<Animator>() != null)
                {
                    obj.GetComponent<Animator>().SetBool("presentation", true);
                }
                indexPiece +=2;


            }
            string nomDeck = deck;
            transform.GetChild(6).GetComponent<Text>().text = "";
            transform.GetChild(6).gameObject.SetActive(true);
            for (int i = 0; i < nomDeck.Length; i++)
            {
                transform.GetChild(6).GetComponent<Text>().text = transform.GetChild(6).GetComponent<Text>().text + nomDeck[i];
                yield return new WaitForSeconds(0.06f);
            }
            wait = true;
            while (wait)
            {
                int index = 0;
                foreach (Transform child in transform.GetChild(5))
                {
                    if (child.GetComponent<Image>() != null)
                    {
                        child.GetComponent<Image>().sprite = transform.GetChild(5).GetChild(index + 1).GetComponent<SpriteRenderer>().sprite;
                    }
                    index++;
                }
                yield return new WaitForSeconds(0.01f);
            }



            foreach (Transform child in transform.GetChild(5))
            {

                Destroy(child.gameObject);
            }

            foreach (Transform child in transform.GetChild(5))
            {
                Destroy(child.gameObject);
            }
            GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(true);
        }






        foreach (Transform child in transform) { child.gameObject.SetActive(false); }

        print("prout");
        //obj.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        //obj.transform.localScale = Vector3.zero;
        //obj.GetComponent<Animator>().SetBool("presentation",true);

        //transform.GetChild(1).gameObject.SetActive(true);
        //transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = GameObject.Find("Liste").GetComponent<Liste>().ListePiece.Find(recompense.pieces[0].nom).GetComponent<SpriteRenderer>().sprite;
        //transform.GetChild(1).GetChild(1).GetComponent<Text>().text = recompense.pieces[0].nom;
        //wait = true;
        //yield return new WaitUntil(() => wait == false);
        //print("stop");

    }

    public void FinExplosion()
    {
        if (name == "Explosion")
        {
            transform.parent.GetComponent<Reward>().FinExplosion();
        }
        //else
        //{

        //    transform.GetChild(1).gameObject.SetActive(true);
        //    transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = GameObject.Find("Liste").GetComponent<Liste>().ListePiece.Find(recompense.pieces[0].nom).GetComponent<SpriteRenderer>().sprite;
        //    transform.GetChild(1).GetChild(1).GetComponent<Text>().text = recompense.pieces[0].nom;
        //}
    }

    public void KillExplosion()
    {

        gameObject.SetActive(false);

    }
    public void HidePiece()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);

    }

    public void NextRecompense()
    {
        transform.parent.GetComponent<Reward>().wait = false;
    }


    public string descritption(PieceClass p)
    {
        Piece scriptObj = GameObject.Find("Liste").GetComponent<Liste>().ListePiece.Find(p.nom).GetComponent<Piece>();
        GameObject PieceObj = GameObject.Find("Liste").GetComponent<Liste>().ListePiece.Find(p.nom);
        string desc = "Level  : " + p.niveau.ToString();if (!p.dependant) { desc = desc + "\nProtection : " + scriptObj.vieListe[p.niveau]; }
        if (scriptObj.stockageListe[p.niveau] > 0) { desc = desc + "\nStockage : ";  desc = desc + scriptObj.stockageListe[p.niveau].ToString();  }
        if(PieceObj.GetComponent<Canon>()!= null) { desc = desc + "\nAutomatique : " + PieceObj.GetComponent<Canon>().automatique.ToString() + "\nShot per second : " + PieceObj.GetComponent<Canon>().TirParSeconde[p.niveau].ToString() + "\nMissile damage : " + PieceObj.GetComponent<Canon>().DegatMissile[p.niveau].ToString(); }

        return desc;
    }


}
