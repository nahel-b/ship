using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perso : MonoBehaviour
{
    public Transform player;
    public float speed = 5.0f;
    private bool touchStart = false;
    private Vector2 pointA;
    private Vector2 pointB;
    public float speedRot = 10;
    public Transform circle;
    public Transform outerCircle;
    private Animator animator;
    private float debutTouch;
    private Vector3 debPos;
    public Vector3 LastSpeakPlace;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        //GameObject.Find("worldCam").transform.position = new Vector3(transform.position.x, transform.position.y, GameObject.Find("worldCam").transform.position.z);

        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            pointA = GameObject.Find("worldCam").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, Camera.main.transform.position.z));

            //circle.transform.position = -pointB;
            outerCircle.transform.position = pointA * 1;
            circle.GetComponent<SpriteRenderer>().enabled = false;
            outerCircle.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (Input.touchCount > 0)
        {
            touchStart = true;
            //transform.GetComponent<Rigidbody2D>().AddForce(transform.up * speed * 10 * Time.deltaTime);

            pointB = GameObject.Find("worldCam").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, Camera.main.transform.position.z));
        }
        else
        {
            touchStart = false;
        }


        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) { debPos = transform.position; debutTouch = Time.time; }
        else if (LastSpeakPlace != transform.position && Camera.main.GetComponent<DeplacementPrincipal>().activePerso == null && transform.position == debPos && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended && Time.time - debutTouch < 0.5f && Input.GetTouch(0).position.y < Screen.height / 3)
        {
            debPos = Vector3.zero;
            LastSpeakPlace = transform.position;
            debutTouch = 0;
            print("ooo");

            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, -transform.up, 0.6f);
            foreach (RaycastHit2D obj in hit)
            {
                if(obj.transform.gameObject != gameObject && obj.transform.GetComponent<DeplacementObject>() != null)
                {
                    obj.transform.GetComponent<DeplacementObject>().OnMouseUp();

                }
                else if (obj.transform.gameObject != gameObject && obj.transform.GetComponent<PersoDeplacement>() != null)
                {
//                    obj.transform.GetComponent<PersoDeplacement>().OnMouseUp();

                }
            }
        }




    }
    private void FixedUpdate()
    {
        if (touchStart)
        {



            Vector2 offset = pointB - pointA;
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);

            if (Vector2.Distance(pointA, pointB) > 1)
            {
                outerCircle.transform.position = pointB - direction;
                pointA = pointB - direction;
            }

            //circle.transform.position = new Vector2(pointA.x + direction.x, pointA.y + direction.y);
            float heading = -Mathf.Atan2(offset.x, offset.y);

            if (Vector2.Distance(pointA, pointB) > 0.4f)
            {
                animator.SetBool("run", true);
                //transform.rotation = Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg);
                float rot = Clamp0360( heading * Mathf.Rad2Deg);


                if(rot<=45 || rot>= 360 - 45){ animator.SetInteger("side", 0); transform.Translate(Vector3.up * speed * Time.deltaTime); transform.GetComponent<SpriteRenderer>().flipX = false; }
                else if (rot >= 180 + 45) { animator.SetInteger("side", 2); transform.Translate(Vector3.right * speed * Time.deltaTime); transform.GetComponent<SpriteRenderer>().flipX = false; }
                else if (rot >= 180 - 45) { animator.SetInteger("side", 1); transform.Translate(Vector3.up * -speed * Time.deltaTime); transform.GetComponent<SpriteRenderer>().flipX = false; }
                else { animator.SetInteger("side", 2); transform.Translate(Vector3.right * -speed * Time.deltaTime); transform.GetComponent<SpriteRenderer>().flipX = true; }



            }
            
            else { animator.SetBool("run", false); }

            
            //transform.Translate(Vector3.up * speed * Time.deltaTime);

        }
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool("run", true);

            if (Input.GetKey(KeyCode.UpArrow)) { animator.SetInteger("side", 0); transform.Translate(Vector3.up * speed * Time.deltaTime); transform.GetComponent<SpriteRenderer>().flipX = false; }
            else if (Input.GetKey(KeyCode.RightArrow)) { animator.SetInteger("side", 2); transform.Translate(Vector3.right * speed * Time.deltaTime); transform.GetComponent<SpriteRenderer>().flipX = false; }
            else if (Input.GetKey(KeyCode.DownArrow)) { animator.SetInteger("side", 1); transform.Translate(Vector3.up * -speed * Time.deltaTime); transform.GetComponent<SpriteRenderer>().flipX = false; }
            else { animator.SetInteger("side", 2); transform.Translate(Vector3.right * -speed * Time.deltaTime); transform.GetComponent<SpriteRenderer>().flipX = true; }


        }
        else { animator.SetBool("run", false); }
        

    }
    public static float Clamp0360(float eulerAngles)
    {
        float result = eulerAngles - Mathf.CeilToInt(eulerAngles / 360f) * 360f;
        if (result < 0)
        {
            result += 360f;
        }
        return result;
    }

}
