using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vfx : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ExplosionAste()
    {
        transform.parent.GetComponent<Asteroide>().ExplosionMax = true;
    }
    public void ExplosionPiece()
    {
        StartCoroutine(transform.parent.GetComponent<Piece>().Mort());

    }
    public void AutoKill() { 
        Destroy(gameObject); 
        }
}
