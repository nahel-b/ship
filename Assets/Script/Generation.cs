using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Generation : MonoBehaviour
{

    public bool DestroyAste;

    public GameObject etoile;

    private Vector3 lastPos = new Vector3(0,0,0);

    private GameObject chunk;

    public List<GameObject> Asteroides;
    public List<GameObject> Planete;

    
    private GameObject Vaisseau;



    public List<Sprite> EtoileSprite;

    public Color[] EtoileCouleur;

    private GameObject openConsoleStation;


    void Start()
    {
        Vaisseau =  GameObject.Find("Main Camera").GetComponent<Principal>().Vaisseau;
        chunk = GameObject.Find("Chunk0");

        GenerationEtoile(-60,60,-60,60);
        StartCoroutine(GenerationEtoileCoroutine(60));
        StartCoroutine(GenerationBG());
        StartCoroutine(GenerationAsteroideCoroutineFG());
        
        
        for (int i = 0; i < 1000; i++)
        {
            float Rayon = 1000;
            GameObject planete = Instantiate(Planete[Random.Range(0, Planete.Count)], new Vector3(Random.Range(-Rayon, Rayon), Random.Range(-Rayon, Rayon), -1), Quaternion.Euler(0, 0, Random.Range(0, 360)));
            planete.transform.parent = GameObject.Find("PlaneteBG1").transform;
        }
    }


    void Update()
    {
        GameObject.Find("EtoileBG1").transform.position = new Vector3(GameObject.Find("Main Camera").transform.parent.position.x * 0.2f, GameObject.Find("Main Camera").transform.parent.position.y * 0.2f, 10);
        GameObject.Find("EtoileBG2").transform.position = new Vector3(GameObject.Find("Main Camera").transform.parent.position.x * 0.75f, GameObject.Find("Main Camera").transform.parent.position.y * 0.75f, 10);
        GameObject.Find("EtoileBG3").transform.position = new Vector3(GameObject.Find("Main Camera").transform.parent.position.x * 0.9f, GameObject.Find("Main Camera").transform.parent.position.y * 0.9f, 10);
        GameObject.Find("PlaneteBG1").transform.position = new Vector3(GameObject.Find("Main Camera").transform.parent.position.x * 0.8f, GameObject.Find("Main Camera").transform.parent.position.y * 0.8f, 5);
    
        GameObject.Find("AsteroidesFG").transform.position = new Vector3(GameObject.Find("Main Camera").transform.parent.position.x * -1.5f, GameObject.Find("Main Camera").transform.parent.position.y * -1.5f, 8);

        GameObject.Find("galaxieBG").transform.position = new Vector3(GameObject.Find("Main Camera").transform.parent.position.x * 0.999f, GameObject.Find("Main Camera").transform.parent.position.y * 0.999f, 8);
    }





    void GenerationEtoile(float a, float b, float c, float d)
    {

        //print(Mathf.Abs(a-b));
        //print(Mathf.Abs(c - d));


        //40*40=150*10,6
        int nb = Mathf.RoundToInt((Mathf.Abs(a - b) * Mathf.Abs(c - d)) / 12);

        for (int i = 0; i < nb; i++)
        {
            GameObject etoileObj = Instantiate(etoile, new Vector3(Random.Range(a, b), Random.Range(c, d), 0), Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
            int spriteNb = Random.Range(0, EtoileSprite.Count);
            float taille;
            if (spriteNb <= 2)
            {
                taille = Random.Range(0.05f, 0.2f);
            }
            else
            {
                taille = 1f *Random.Range(0.05f, 0.2f);
            }
            etoileObj.transform.localScale = new Vector3(taille, taille, 1);
            etoileObj.GetComponent<SpriteRenderer>().sprite = EtoileSprite[spriteNb];
            etoileObj.GetComponent<SpriteRenderer>().color = EtoileCouleur[Random.Range(0, EtoileCouleur.Length)];

            int e = Random.Range(0, 6);

            if(e == 0 || e==1 || e==2)
            {
                etoileObj.transform.parent = GameObject.Find("EtoileBG1").transform;
            }
            else if(e == 3 || e == 4 )
            {
                etoileObj.transform.parent = GameObject.Find("EtoileBG2").transform;
            }
            else if(e == 5 )
            {
                etoileObj.transform.parent = GameObject.Find("EtoileBG3").transform;
            }
            
           
        }

    }





    IEnumerator GenerationEtoileCoroutine(float rayon)
    {

        Vector3 pos = GameObject.Find("Main Camera").transform.parent.position;

        if (lastPos.y < pos.y)
        {
            GenerationEtoile(pos.x - rayon, pos.x + rayon, lastPos.y + rayon, pos.y + rayon);
        }
        else { GenerationEtoile(pos.x - rayon, pos.x + rayon, lastPos.y - rayon, pos.y - rayon);
         }
        if (lastPos.x < pos.x) {
            
             GenerationEtoile( lastPos.x + rayon, pos.x + rayon, pos.y - rayon, pos.y + rayon); 
             }
        else { GenerationEtoile(lastPos.x - rayon, pos.x - rayon, pos.y - rayon, pos.y + rayon);
         }


        for (int i = 1; i<=3; i++)
        {
            foreach(Transform child in GameObject.Find("EtoileBG" + i).transform)
            {
                if (child.transform.position.y + rayon < pos.y) { Destroy(child.gameObject); }
                else if (child.transform.position.y - rayon > pos.y) { Destroy(child.gameObject); }
                else if (child.transform.position.x + rayon < pos.x) { Destroy(child.gameObject); }
                else if (child.transform.position.x - rayon > pos.x) { Destroy(child.gameObject); }
            }
        
        }

        


        lastPos = GameObject.Find("Main Camera").transform.parent.position;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(GenerationEtoileCoroutine(rayon));
    }


    
 IEnumerator GenerationAsteroideCoroutineFG()
    {
        float chunkSize = 200;

        Vector3 pos = new Vector3(Mathf.Round(GameObject.Find("Main Camera").transform.position.x / chunkSize), Mathf.Round(GameObject.Find("Main Camera").transform.position.y / chunkSize), 0.1f);
        if (GameObject.Find("ChunkFG " + pos.x + ";" + pos.y) == null) { GameObject a = Instantiate(chunk, pos * chunkSize, Quaternion.identity); a.name = "ChunkFG " + pos.x + ";" + pos.y; a.transform.parent = GameObject.Find("AsteroidesFG").transform; GenerationAsteroidesFG(a); }
        if (GameObject.Find("ChunkFG " + (pos.x + 1) + ";" + pos.y) == null) { GameObject a = Instantiate(chunk, new Vector3(pos.x + 1, pos.y, 0.1f) * chunkSize, Quaternion.identity); a.name = "ChunkFG " + (pos.x + 1) + ";" + pos.y; a.transform.parent = GameObject.Find("AsteroidesFG").transform; GenerationAsteroidesFG(a); }
        if (GameObject.Find("ChunkFG " + (pos.x - 1) + ";" + pos.y) == null) { GameObject a = Instantiate(chunk, new Vector3(pos.x - 1, pos.y, 0.1f) * chunkSize, Quaternion.identity); a.name = "ChunkFG " + (pos.x - 1) + ";" + pos.y; a.transform.parent = GameObject.Find("AsteroidesFG").transform; GenerationAsteroidesFG(a); }
        if (GameObject.Find("ChunkFG " + pos.x + ";" + (pos.y + 1)) == null) { GameObject a = Instantiate(chunk, new Vector3(pos.x, pos.y + 1, 0.1f) * chunkSize, Quaternion.identity); a.name = "ChunkFG " + pos.x + ";" + (pos.y + 1); a.transform.parent = GameObject.Find("AsteroidesFG").transform; GenerationAsteroidesFG(a); }
        if (GameObject.Find("ChunkFG " + pos.x + ";" + (pos.y - 1)) == null) { GameObject a = Instantiate(chunk, new Vector3(pos.x, pos.y - 1, 0.1f) * chunkSize, Quaternion.identity); a.name = "ChunkFG " + pos.x + ";" + (pos.y - 1); a.transform.parent = GameObject.Find("AsteroidesFG").transform; GenerationAsteroidesFG(a); }
        if (GameObject.Find("ChunkFG " + (pos.x + 1) + ";" + (pos.y + 1)) == null) { GameObject a = Instantiate(chunk, new Vector3(pos.x + 1, pos.y + 1, 0.1f) * chunkSize, Quaternion.identity); a.name = "ChunkFG " + (pos.x + 1) + ";" + (pos.y + 1); a.transform.parent = GameObject.Find("AsteroidesFG").transform; GenerationAsteroidesFG(a); }
        if (GameObject.Find("ChunkFG " + (pos.x - 1) + ";" + (pos.y + 1)) == null) { GameObject a = Instantiate(chunk, new Vector3(pos.x - 1, pos.y + 1, 0.1f) * chunkSize, Quaternion.identity); a.name = "ChunkFG " + (pos.x - 1) + ";" + (pos.y + 1); a.transform.parent = GameObject.Find("AsteroidesFG").transform; GenerationAsteroidesFG(a); }
        if (GameObject.Find("ChunkFG " + (pos.x + 1) + ";" + (pos.y - 1)) == null) { GameObject a = Instantiate(chunk, new Vector3(pos.x + 1, pos.y - 1, 0.1f) * chunkSize, Quaternion.identity); a.name = "ChunkFG " + (pos.x + 1) + ";" + (pos.y - 1); a.transform.parent = GameObject.Find("AsteroidesFG").transform; GenerationAsteroidesFG(a); }
        if (GameObject.Find("ChunkFG " + (pos.x - 1) + ";" + (pos.y - 1)) == null) { GameObject a = Instantiate(chunk, new Vector3(pos.x - 1, pos.y - 1, 0.1f) * chunkSize, Quaternion.identity); a.name = "ChunkFG " + (pos.x - 1) + ";" + (pos.y - 1); a.transform.parent = GameObject.Find("AsteroidesFG").transform; GenerationAsteroidesFG(a); }
        foreach (Transform child in GameObject.Find("AsteroidesFG").transform)
        {
            if (child.name != "ChunkFG " + pos.x + ";" + pos.y && child.name != "ChunkFG " + (pos.x + 1) + ";" + pos.y && child.name != "ChunkFG " + (pos.x - 1) + ";" + pos.y && child.name != "ChunkFG " + pos.x + ";" + (pos.y + 1) && child.name != "ChunkFG " + pos.x + ";" + (pos.y - 1) && child.name != "ChunkFG " + (pos.x + 1) + ";" + (pos.y + 1) && child.name != "ChunkFG " + (pos.x - 1) + ";" + (pos.y + 1) && child.name != "ChunkFG " + (pos.x + 1) + ";" + (pos.y - 1) && child.name != "ChunkFG " + (pos.x - 1) + ";" + (pos.y - 1)) { Destroy(child.gameObject); }
        }
        
        
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(GenerationAsteroideCoroutineFG());
    }


    IEnumerator GenerationBG()
    {
        float chunkSize = 200;

        Vector3 pos = new Vector3(Mathf.Round(GameObject.Find("Main Camera").transform.position.x / chunkSize), Mathf.Round(GameObject.Find("Main Camera").transform.position.y / chunkSize), 0.1f);
        if (GameObject.Find("Chunk " + pos.x + ";" + pos.y) == null) { GameObject a = Instantiate(chunk, pos * chunkSize, Quaternion.identity); a.name = "Chunk " + pos.x + ";" + pos.y; a.transform.parent = GameObject.Find("Chunk").transform; GenerationAsteroides(a); }
        if (GameObject.Find("Chunk " + (pos.x + 1) + ";" + pos.y) == null) { GameObject a = Instantiate(chunk, new Vector3(pos.x + 1, pos.y, 0.1f) * chunkSize, Quaternion.identity); a.name = "Chunk " + (pos.x + 1) + ";" + pos.y; a.transform.parent = GameObject.Find("Chunk").transform; GenerationAsteroides(a); }
        if (GameObject.Find("Chunk " + (pos.x - 1) + ";" + pos.y) == null) { GameObject a = Instantiate(chunk, new Vector3(pos.x - 1, pos.y, 0.1f) * chunkSize, Quaternion.identity); a.name = "Chunk " + (pos.x - 1) + ";" + pos.y; a.transform.parent = GameObject.Find("Chunk").transform; GenerationAsteroides(a); }
        if (GameObject.Find("Chunk " + pos.x + ";" + (pos.y + 1)) == null) { GameObject a = Instantiate(chunk, new Vector3(pos.x, pos.y + 1, 0.1f) * chunkSize, Quaternion.identity); a.name = "Chunk " + pos.x + ";" + (pos.y + 1); a.transform.parent = GameObject.Find("Chunk").transform; GenerationAsteroides(a); }
        if (GameObject.Find("Chunk " + pos.x + ";" + (pos.y - 1)) == null) { GameObject a = Instantiate(chunk, new Vector3(pos.x, pos.y - 1, 0.1f) * chunkSize, Quaternion.identity); a.name = "Chunk " + pos.x + ";" + (pos.y - 1); a.transform.parent = GameObject.Find("Chunk").transform; GenerationAsteroides(a); }
        if (GameObject.Find("Chunk " + (pos.x + 1) + ";" + (pos.y + 1)) == null) { GameObject a = Instantiate(chunk, new Vector3(pos.x + 1, pos.y + 1, 0.1f) * chunkSize, Quaternion.identity); a.name = "Chunk " + (pos.x + 1) + ";" + (pos.y + 1); a.transform.parent = GameObject.Find("Chunk").transform; GenerationAsteroides(a); }
        if (GameObject.Find("Chunk " + (pos.x - 1) + ";" + (pos.y + 1)) == null) { GameObject a = Instantiate(chunk, new Vector3(pos.x - 1, pos.y + 1, 0.1f) * chunkSize, Quaternion.identity); a.name = "Chunk " + (pos.x - 1) + ";" + (pos.y + 1); a.transform.parent = GameObject.Find("Chunk").transform; GenerationAsteroides(a); }
        if (GameObject.Find("Chunk " + (pos.x + 1) + ";" + (pos.y - 1)) == null) { GameObject a = Instantiate(chunk, new Vector3(pos.x + 1, pos.y - 1, 0.1f) * chunkSize, Quaternion.identity); a.name = "Chunk " + (pos.x + 1) + ";" + (pos.y - 1); a.transform.parent = GameObject.Find("Chunk").transform; GenerationAsteroides(a); }
        if (GameObject.Find("Chunk " + (pos.x - 1) + ";" + (pos.y - 1)) == null) { GameObject a = Instantiate(chunk, new Vector3(pos.x - 1, pos.y - 1, 0.1f) * chunkSize, Quaternion.identity); a.name = "Chunk " + (pos.x - 1) + ";" + (pos.y - 1); a.transform.parent = GameObject.Find("Chunk").transform; GenerationAsteroides(a); }
        foreach (Transform child in GameObject.Find("Chunk").transform)
        {
            if (child.name != "Chunk " + pos.x + ";" + pos.y && child.name != "Chunk " + (pos.x + 1) + ";" + pos.y && child.name != "Chunk " + (pos.x - 1) + ";" + pos.y && child.name != "Chunk " + pos.x + ";" + (pos.y + 1) && child.name != "Chunk " + pos.x + ";" + (pos.y - 1) && child.name != "Chunk " + (pos.x + 1) + ";" + (pos.y + 1) && child.name != "Chunk " + (pos.x - 1) + ";" + (pos.y + 1) && child.name != "Chunk " + (pos.x + 1) + ";" + (pos.y - 1) && child.name != "Chunk " + (pos.x - 1) + ";" + (pos.y - 1)) { Destroy(child.gameObject); }
        }
        //KIll ASTEROIDE BEHIND
        Vector2 vel = Vector2.zero;
        int nb = 0;
        foreach (Transform piece in GameObject.Find("Vaisseau").transform)
        {
            if (piece.GetComponent<Rigidbody2D>() != null) { vel = vel + piece.GetComponent<Rigidbody2D>().velocity; nb++; }
        }
        foreach (Transform piece in GameObject.Find("Vaisseau").transform.GetChild(0))
        {
            if (piece.GetComponent<Rigidbody2D>() != null) { vel = vel + piece.GetComponent<Rigidbody2D>().velocity; nb++; }
        }
        vel = (vel / nb).normalized;
        Vector3 aV3 = new Vector3(vel.x,vel.y,0);
        if (DestroyAste)
        {
            Collider2D[] hit = Physics2D.OverlapCircleAll(Vaisseau.transform.GetChild(0).GetChild(0).transform.position + (aV3 * 40), 4f);
            foreach (Collider2D aster in hit) { if (aster.transform.GetComponent<Asteroide>() != null && Vector2.Distance(new Vector2(Vaisseau.transform.GetChild(0).GetChild(0).transform.position.x, Vaisseau.transform.GetChild(0).GetChild(0).transform.position.y), new Vector2(aster.transform.position.x, aster.transform.position.y)) > 20f) { Destroy(aster.gameObject); } }
            //Gizmos.DrawSphere(Vaisseau.transform.GetChild(0).GetChild(0).transform.position + aV3, 4f);
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(GenerationBG());
    }


    void GenerationAsteroides(GameObject Chunk)
    {

        //objet asteroide
        float chunkSize = 200f;
        for (int i = 0; i < 70; i++)
        {
            GameObject aste = Instantiate(Asteroides[Random.Range(0, Asteroides.Count)], new Vector3(Random.Range(Chunk.transform.position.x- (chunkSize/2f), Chunk.transform.position.x+ (chunkSize / 2f)), Random.Range(Chunk.transform.position.y- (chunkSize / 2f), Chunk.transform.position.y + (chunkSize / 2f)), 0), Quaternion.Euler(0, 0, Random.Range(0, 360)));
            float taille = Random.Range(1f, 3f);
            aste.transform.localScale = new Vector3(taille, taille, 1);
            aste.transform.parent = Chunk.transform;

        }

    }

    void GenerationAsteroidesFG(GameObject Chunk)
    {

        //objet asteroide
        float chunkSize = 200f;
        for (int i = 0; i < 10; i++)
        {
            GameObject aste = Instantiate(Asteroides[Random.Range(0, Asteroides.Count)], new Vector3(Random.Range(Chunk.transform.position.x- (chunkSize/2f), Chunk.transform.position.x+ (chunkSize / 2f)), Random.Range(Chunk.transform.position.y- (chunkSize / 2f), Chunk.transform.position.y + (chunkSize / 2f)), 0), Quaternion.Euler(0, 0, Random.Range(0, 360)));
            float taille = Random.Range(8f, 10f);
            aste.transform.localScale = new Vector3(taille, taille, 1);
            aste.transform.parent = Chunk.transform;
            Destroy(aste.GetComponent<PolygonCollider2D>());
            Destroy(aste.GetComponent<Rigidbody2D>());
            aste.GetComponent<SpriteRenderer>().sortingLayerName = "vfx";
            aste.GetComponent<SpriteRenderer>().color = new Color32(0x2E, 0x2E, 0x2E, 0xFF);
            Destroy(aste.GetComponent<Asteroide>());
            //aste.GetComponent<SpriteRenderer>().color = Color.black;
        }

    }
    public void SUPASTE() { DestroyAste = !DestroyAste; }
    void GenerationPlanete(GameObject Chunk)
    {
        for (int i = 0; i < 100; i++)
        {
            GameObject aste = Instantiate(Asteroides[Random.Range(0, Asteroides.Count)], new Vector3(Random.Range(Chunk.transform.position.x - 500, Chunk.transform.position.x + 500), Random.Range(Chunk.transform.position.y - 500, Chunk.transform.position.y + 500), 0), Quaternion.Euler(0, 0, Random.Range(0, 360)));
            float taille = Random.Range(1f, 3f);
            aste.transform.localScale = new Vector3(taille, taille, 1);
            aste.transform.parent = Chunk.transform;

        }
        for (int i = 0; i < 50; i++)
        {
            float Rayon = 250 * 0.8f;
            GameObject planete = Instantiate(Planete[Random.Range(0, Planete.Count)], new Vector3(Random.Range(Chunk.transform.position.x - Rayon, Rayon+ Chunk.transform.position.x), Random.Range(Chunk.transform.position.x - Rayon, Rayon+ Chunk.transform.position.x), -1), Quaternion.Euler(0, 0, Random.Range(0, 360)));
            planete.transform.parent = GameObject.Find("PlaneteBG1").transform;
        }
    }


}