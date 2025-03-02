using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroide : MonoBehaviour
{
    public float vie = 10;
    public RandomListe itemsLoot;
    public RandomListe ObjectCount;
    public List<GameObject> Item;
    private bool mort = false;
    public GameObject Fumee;
    public bool ExplosionMax;
    // Start is called before the first frame update
    void Start()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, 10, Vector2.zero);
        foreach (RaycastHit2D obj in hit)
        {
            if (obj.transform.GetComponent<Station>() != null || obj.transform.GetComponent<Piece>() != null || obj.transform.gameObject.layer == LayerMask.NameToLayer("vfx"))
            {
            Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        

        if (vie <= 0 && !mort)
        {
            mort = true;
            Instantiate(Fumee, transform.position, Quaternion.identity, transform);
            StartCoroutine(GameObject.Find("Main Camera").GetComponent<Principal>().Feedback(1, 0.1f));
            StartCoroutine(GameObject.Find("Main Camera").GetComponent<Principal>().Shake(0.3f, 0.3f));
            //StartCoroutine(Mort());
        }
        if(vie<=0 && mort && ExplosionMax)
        {
            ExplosionMax = false;
            StartCoroutine(Mort());
        } 

    }

    //if (vie <= 0 && !mort)
    //{
    //    mort = true; int a = Random.Range(0, 3);
    //}  //for (int i = 0; i < a; i++) { GameObject item = Instantiate(Item[Random.Range(0, Item.Count)], new Vector3(Random.Range(transform.position.x - 1,transform.position.x+1), Random.Range(transform.position.y - 1, transform.position.y + 1),transform.position.z), Quaternion.Euler(0, 0, Random.Range(0f, 360f))) ; item.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-1f, 1f) * 10f, Random.Range(-1f, 1f)*10f, 0)); }  Destroy(gameObject,0.01f); } 


    IEnumerator Mort()
    {
        


        mort = true;
        
        int a = Mathf.RoundToInt(ObjectCount.RandomFloat());
        for (int i = 0; i < a; i++)
        {
            string itemString = itemsLoot.RandomString();
            
            GameObject item = Instantiate(GameObject.Find("Liste").GetComponent<Liste>().ObjetList.Find(itemString).obj, new Vector3(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y - 1, transform.position.y + 1), transform.position.z), Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
            item.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-1f, 1f) * 10f, Random.Range(-1f, 1f) * 10f, 0));
        }
        GetComponent<PolygonCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);


        yield return null;
    }



    //public void hit(float force)
    //{
    //    if (tps == true && vie>0)
    //    {
    //        StartCoroutine(Hit(force));
    //    }
    //}

    //IEnumerator Hit(float force)
    //{
    //    tps = false;
    //    yield return new WaitForSeconds(1);
    //    vie = vie - force;
    //    tps = true;
    //}
}
