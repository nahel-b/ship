//||
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


public class SpaceTouchManager : MonoBehaviour
{
    private GameObject Vaisseau;

    private Principal principal;
    private float touchtime;




    void Start()
    {
        principal = GameObject.Find("Main Camera").GetComponent<Principal>();
        Vaisseau = principal.Vaisseau;
    }

    void Update()
    {


            if ((Input.touchCount > 0)&& Input.GetTouch(0).phase == TouchPhase.Began )
            {
                touchtime = Time.time;
            }
            if  ( (Input.touchCount > 0) && 
                (Input.GetTouch(0).phase == TouchPhase.Ended) //&& Input.GetTouch(0).position.y > Screen.height / 4 
                && touchtime > Time.time-0.2f && 

                GameObject.Find("MissionPanel")== null&& GameObject.Find("InventoryPanel") == null
                
                )
            {

                RaycastHit2D[] hit = Physics2D.CircleCastAll(UIFollowObject.ScreenToWorldWithDistortion(Input.GetTouch(0).position,10) , 1, Vector2.zero);
                // Debug visualization of the circle cast
                

                // Sort hits by distance from the center of the circle
                System.Array.Sort(hit, (a, b) => a.distance.CompareTo(b.distance));
                
                // Process the hits in order of distance
                foreach (RaycastHit2D obj in hit)
                {
                    if(obj.transform.GetComponent<TouchableSpaceObject>() != null)
                    {
                        obj.transform.GetComponent<TouchableSpaceObject>().OnTouch();
                        break;
                    }
                }
            }
            bool boutonFeuMouseDown = GameObject.Find("ButtonFeu").GetComponent<Bouton>().isMouseDown;
            bool boutonTournerMouseDown = GameObject.Find("[bouton-tourner]").GetComponent<Bouton>().isMouseDown;
            // if (FinPanel) { FinPanel = false; }
            if ( Input.touchCount == 1 && boutonTournerMouseDown  && !boutonFeuMouseDown )
            {

                var touch = Input.GetTouch(0);
                if (touch.position.x < Screen.width / 4)
                {

                    foreach (Transform child in Vaisseau.transform.GetChild(0).transform)
                    {
                        if (child.name.Contains("eacteur"))
                        {
                            child.transform.GetComponent<Rigidbody2D>().angularVelocity = 20000 * Time.deltaTime;
                        }
                    }

                }
                else if (touch.position.x > Screen.width * 3/4)
                {
                    foreach (Transform child in Vaisseau.transform.GetChild(0).transform)
                    {
                        if (child.name.Contains("eacteur"))
                        {
                            child.transform.GetComponent<Rigidbody2D>().angularVelocity = -20000 * Time.deltaTime;
                        }
                    }
                }
            }
            else if (Input.touchCount == 2 && boutonTournerMouseDown && boutonFeuMouseDown )
            {
                var touch = Input.GetTouch(1);
                if (touch.position.x < Screen.width / 4)
                {

                    foreach (Transform child in Vaisseau.transform.GetChild(0).transform)
                    {
                        if (child.name.Contains("eacteur"))
                        {
                            //child.transform.GetComponent<Rigidbody2D>().AddTorque(100 * Time.deltaTime);
                            child.transform.GetComponent<Rigidbody2D>().angularVelocity = 20000 * Time.deltaTime;
                        }
                    }

                }
                else if (touch.position.x > Screen.width * 3/ 4 )
                {
                    foreach (Transform child in Vaisseau.transform.GetChild(0).transform)
                    {
                        if (child.name.Contains("eacteur"))
                        {
                            child.transform.GetComponent<Rigidbody2D>().angularVelocity = -20000 * Time.deltaTime;
                        }
                    }
                }
            }
            else
            {
                foreach (Transform child in Vaisseau.transform.GetChild(0).transform)
                {
                    if (child.name.Contains("eacteur"))
                    {
                        child.transform.GetComponent<Rigidbody2D>().angularVelocity = 0;
                    }
                }


                if (Input.touchCount == 2)
                {
                    Touch touch0 = Input.GetTouch(0);
                    Touch touch1 = Input.GetTouch(1);

                    Vector2 touch0prevPos = touch0.position - touch0.deltaPosition;
                    Vector2 touch1prevPos = touch1.position - touch1.deltaPosition;

                    float prevMagnitude = (touch0prevPos - touch1prevPos).magnitude;
                    float curentMagnitude = (touch0.position - touch1.position).magnitude;

                    Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - 0.05f * (curentMagnitude - prevMagnitude), 8, 30);


                }

            }
    }


}