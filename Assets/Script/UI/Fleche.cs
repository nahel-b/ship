using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fleche : MonoBehaviour
{
    public bool isOffScreen;
    public Transform target;
    public string targetName;
    public List<Sprite> sp;
    [HideInInspector]
    public bool showWayPoint;
    public Transform inseree;
    //public float borderSize;
    //public float texteBorderSize;
    // Start is called before the first frame update
    void Start()
    {

        if (GameObject.Find(targetName) != null)
        {
            if(targetName.Contains("Ennemie"))
            {
                GetComponent<Image>().enabled = true; transform.GetChild(0).GetComponent<Text>().enabled = true;
                target = GameObject.Find(targetName).transform.GetChild(0);
            }
            else {
                GetComponent<Image>().enabled = true; transform.GetChild(0).GetComponent<Text>().enabled = true;
                target = GameObject.Find(targetName).transform;
            }
        }
        else
        {
            StartCoroutine(WaitObjSpawn());
        }
    }

    // Update is called once per frame
     void Update()
    {
        if (target != null)
        {

            float borderSize = 60;
            float borderSizeHaut = borderSize;
            if (GameObject.Find("EnnemieBarBorder") != null) {  borderSizeHaut = 110; } 
            
            float texteBorderSize = 100;
            Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(target.position);
            isOffScreen = targetPositionScreenPoint.x <= borderSize || targetPositionScreenPoint.x >= Screen.width - borderSize || targetPositionScreenPoint.y <= borderSize || targetPositionScreenPoint.y >= Screen.height - borderSize;

            if (isOffScreen)
            {
                transform.GetComponent<Image>().enabled = true;
                transform.GetComponent<Image>().sprite = sp[0];
                transform.GetComponent<RectTransform>().localScale = Vector3.one;
                transform.GetChild(0).GetComponent<RectTransform>().localScale = Vector3.one * 0.5f;


                Vector3 direction = target.position - Camera.main.transform.position;
                Vector3 a = Quaternion.LookRotation(Vector3.forward, direction).eulerAngles;

                direction = target.position - Camera.main.transform.parent.position;
                //transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
                transform.GetComponent<RectTransform>().rotation = Quaternion.LookRotation(Vector3.forward, direction);


                float x = Mathf.Cos(Mathf.Deg2Rad * a.z);
                float y = Mathf.Sin(Mathf.Deg2Rad * a.z);

                Vector3 screenPos = new Vector3(0, 0, 0);
                Vector3 txtPos = new Vector3(0, 0, 0);

                float racine = Mathf.Sqrt(2) / 2;
                if (-racine <= y && y <= racine && x > 0)
                {
                    screenPos = new Vector3(((Screen.width) - ((y + racine) * ((Screen.width) / (2 * racine)))) / (Screen.width / (Screen.width - 2 * borderSize)) + borderSize, Screen.height - borderSizeHaut, 10);
                    txtPos = new Vector3(((Screen.width) - ((y + racine) * ((Screen.width) / (2 * racine)))) / (Screen.width / (Screen.width - 2 * (texteBorderSize))) + (texteBorderSize), Screen.height - (texteBorderSize + (borderSizeHaut- borderSize)), 20);
                }
                else if (-Mathf.Sqrt(2) / 2 <= y && y <= Mathf.Sqrt(2) / 2 && x < 0)
                {
                    screenPos = new Vector3(((Screen.width) - ((y + racine) * ((Screen.width) / (2 * racine)))) / (Screen.width / (Screen.width - 2 * borderSize)) + borderSize, borderSize, 20);
                    txtPos = new Vector3(((Screen.width) - ((y + racine) * ((Screen.width) / (2 * racine)))) / (Screen.width / (Screen.width - 2 * texteBorderSize)) + texteBorderSize, texteBorderSize, 20);

                }
                else if (-Mathf.Sqrt(2) / 2 <= x && x <= Mathf.Sqrt(2) / 2 && y > 0)
                {
                    screenPos = new Vector3(borderSize, (((x + racine) * ((Screen.height) / (2 * racine)))) / (Screen.height / (Screen.height - 2 * borderSize)) + borderSize, 20);
                    txtPos = new Vector3(texteBorderSize, (((x + racine) * ((Screen.height) / (2 * racine)))) / (Screen.height / (Screen.height - 2 * texteBorderSize)) + texteBorderSize, 20);

                }
                else if (-Mathf.Sqrt(2) / 2 <= x && x <= Mathf.Sqrt(2) / 2 && y < 0)
                {
                    screenPos = new Vector3(Screen.width - borderSize, (((x + racine) * ((Screen.height) / (2 * racine)))) / (Screen.height / (Screen.height - 2 * borderSize)) + borderSize, 20);
                    txtPos = new Vector3(Screen.width - texteBorderSize, (((x + racine) * ((Screen.height) / (2 * racine)))) / (Screen.height / (Screen.height - 2 * texteBorderSize)) + texteBorderSize, 20);

                }
                transform.position = screenPos;
                transform.GetChild(0).position = txtPos;
                transform.GetChild(0).GetComponent<RectTransform>().rotation = Quaternion.identity;
                transform.GetChild(0).GetComponent<Text>().text = Mathf.Round(((target.position - Camera.main.transform.position) / 60).magnitude).ToString() + "u";
                if (Time.time > 0.5f)
                {
                    foreach (Transform child in transform.parent)
                    {
                        if (Vector2.Distance(GetComponent<RectTransform>().position, child.GetComponent<RectTransform>().position) <= 30 && child.GetComponent<Fleche>().inseree == null && inseree == null && child != transform && child.GetComponent<Fleche>().isOffScreen)
                        {
                            inseree = child;
                            transform.GetChild(0).gameObject.SetActive(false);
                            //GetComponent<Image>().enabled = false;

                        }
                        else if (Vector2.Distance(GetComponent<RectTransform>().position, child.GetComponent<RectTransform>().position) <= 30 && child.GetComponent<Fleche>().inseree == transform && inseree == null && child != transform)
                        {
                            transform.GetChild(0).GetComponent<Text>().text = transform.GetChild(0).GetComponent<Text>().text + "," + Mathf.Round(((child.GetComponent<Fleche>().target.position - Camera.main.transform.position) / 60).magnitude).ToString() + "u";

                        }
                        else if (Vector2.Distance(GetComponent<RectTransform>().position, child.GetComponent<RectTransform>().position) > 30 && child.GetComponent<Fleche>().inseree == transform && inseree == null && child != transform)
                        {
                            inseree = null;
                            child.transform.GetChild(0).gameObject.SetActive(true);
                            child.GetComponent<Image>().enabled = true;
                        }
                    }

                }
                if (inseree)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                    if (!isOffScreen) { inseree = null; }
                    if (!inseree.GetComponent<Fleche>().isOffScreen) { inseree = null; }
                }
                else
                {
                   
                    transform.GetChild(0).gameObject.SetActive(true);

                }
            }
            else if(showWayPoint)
            {
                if(inseree && !isOffScreen) { inseree = null; }

                transform.GetComponent<Image>().enabled = true;
                transform.GetComponent<Image>().sprite = sp[1];
                transform.GetComponent<RectTransform>().localScale = Vector3.one * 0.7f;
                transform.GetComponent<RectTransform>().rotation = Quaternion.identity;
                transform.position = Camera.main.WorldToScreenPoint(target.position);
                transform.GetChild(0).GetComponent<RectTransform>().localScale = Vector3.zero;
            }
            else if (!showWayPoint)
            {
                if (inseree && !isOffScreen) { inseree = null; }

                transform.GetComponent<Image>().enabled = false; transform.GetChild(0).GetComponent<RectTransform>().localScale = Vector3.zero;
            }











            //if (isOffScreen)
            //{
            //    //transform.LookAt(targetPositionScreenPoint);
            //    Vector3 direction = targetPosition - transform.position;
            //    transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

            //    //pointerImage.sprite = arrowSprite;
            //    Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
            //    if (cappedTargetScreenPosition.x <= borderSize) cappedTargetScreenPosition.x = borderSize;
            //    if (cappedTargetScreenPosition.x >= Screen.width - borderSize) cappedTargetScreenPosition.x = Screen.width - borderSize;
            //    if (cappedTargetScreenPosition.y <= borderSize) cappedTargetScreenPosition.y = borderSize;
            //    if (cappedTargetScreenPosition.y >= Screen.height - borderSize) cappedTargetScreenPosition.y = Screen.height - borderSize;

            //    Vector3 pointerWorldPosition = Camera.main.ScreenToWorldPoint(cappedTargetScreenPosition);
            //    transform.position = pointerWorldPosition;




            //   // transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);
            //}
            //else
            //{
            //    //pointerImage.sprite = crossSprite;
            //    Vector3 pointerWorldPosition = Camera.main.ScreenToWorldPoint(targetPositionScreenPoint);
            //    transform.position = pointerWorldPosition;
            //    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);

            //    transform.localEulerAngles = Vector3.zero;
            //}
        }
        else { transform.GetComponent<Image>().enabled = false; transform.GetChild(0).GetComponent<Text>().enabled = false; }
    }


    private IEnumerator WaitObjSpawn()
    {
        yield return new WaitForSeconds(1);
        if (GameObject.Find(targetName) != null)
        {
            if (targetName.Contains("Ennemie"))
            {
                GetComponent<Image>().enabled = true; transform.GetChild(0).GetComponent<Text>().enabled = true;
                target = GameObject.Find(targetName).transform.GetChild(0);
            }
            else
            {
                GetComponent<Image>().enabled = true; transform.GetChild(0).GetComponent<Text>().enabled = true;
                target = GameObject.Find(targetName).transform;
            }
        }

    }
}
