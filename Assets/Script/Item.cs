using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string name;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnMouseUp()
    {
        if (GameObject.Find("Main Camera").GetComponent<Principal>().Items.PutInventory(name))
        {
            // foreach (MissionClass m in GameObject.Find("Main Camera").GetComponent<Principal>().missions.missions)
            // {
            //     if (m.minerType.Item == name)
            //     {
            //         m.minerType.avancement[0]++;                    
            //     }

            // }
            Destroy(gameObject);
        }
        

    }
}
