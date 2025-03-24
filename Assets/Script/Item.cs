using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PutInventory(GameObject target)
    {
        if (GameObject.Find("Main Camera").GetComponent<Principal>().Items.PutInventory(target.GetComponent<Item>().itemName))
        {
            // foreach (MissionClass m in GameObject.Find("Main Camera").GetComponent<Principal>().missions.missions)
            // {
            //     if (m.minerType.Item == name)
            //     {
            //         m.minerType.avancement[0]++;                    
            //     }

            // }
            if(target.transform.gameObject.activeInHierarchy)
                Destroy(target.transform.gameObject, 0.1f);
        }
        

    }
}
