using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject brickPrefab;
    public float radius;
    public int height;
    public int numOfBrickEachLayer;
    public GameObject cam;
    public float lockTime = 2;
    public float explodeTime = 3;
    public float thresholdAngle = 25;
    public Slider bar;
    public LayerMask mask;

    private GameObject lookingAt;
    private GameObject locked;
    private List<GameObject> bricks = new List<GameObject>();
    private float timer;
    private Vector3 lastPos;
    private bool canExplode = true;


    // Use this for initialization
    void Start () {
        float rotDeg = 360.0f / numOfBrickEachLayer;
        for (int i = 0; i < height*3; i++) {
            float degree = 0;
            if (i % 2 == 1)
            {
                degree = rotDeg / 2.0f;
            }

            while (degree < 360)
            {
                GameObject brick = GameObject.Instantiate(brickPrefab);
                Vector3 pos = new Vector3(radius * Mathf.Cos(degree*Mathf.Deg2Rad), i * 0.31f + 0.5f, radius * Mathf.Sin(degree * Mathf.Deg2Rad));
                Vector3 rot = new Vector3(0, -degree, 0);
                brick.transform.SetPositionAndRotation(pos, Quaternion.Euler(rot));
                bricks.Add(brick);
                degree += rotDeg;
            }
        }
        lookingAt = bricks[0];
    }
	
	// Update is called once per frame
	void Update () {
        bar.value = 0;
        foreach(var b in bricks)
        {
            b.GetComponentInParent<Renderer>().material.color = new Color32(200, 200, 200, 255);
        }
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit))
        {
            if (hit.collider.gameObject.tag == "Brick")
            {
                GameObject cur = hit.collider.gameObject;
                if(!locked)
                    cur.GetComponent<Renderer>().material.color = Color.white;
                if (cur.GetInstanceID() == lookingAt.GetInstanceID())
                {
                    timer += Time.deltaTime;
                    bar.value = timer;
                    if (timer >= lockTime)
                    {
                        locked = lookingAt;
                    }
                    if (timer >= explodeTime && canExplode)
                    {
                        Collider[] colliders = Physics.OverlapSphere(lookingAt.transform.position, 1, mask);
                        foreach(Collider c in colliders)
                        {
                            c.GetComponentInParent<Rigidbody>().AddForce(c.transform.position *1000);
                        }
                        canExplode = false;
                    }
                }
                else if(!locked)
                {
                    lookingAt = cur;
                    timer = 0;
                    canExplode = true;
                }
            }
            else if(hit.collider.gameObject.tag == "Reset")
            {
                Reset();
            }
        }

        if (locked)
        {
            if (Vector3.Angle(locked.transform.position - cam.transform.position, cam.transform.forward) > thresholdAngle)
            {
                locked = null;
            }
            else
            {
                locked.GetComponent<Renderer>().material.color = new Color32(20, 20, 20, 255);
                Vector3 d = cam.transform.position - lastPos;
                Vector3 v = Vector3.Normalize(locked.transform.position);
                Vector3 t = Vector3.Project(d, v);
                locked.transform.position += t;
                bar.value = timer;
            }
        }
        
        lastPos = cam.transform.position;
    }

    private void Reset()
    {
        float rotDeg = 360.0f / numOfBrickEachLayer;
        int cnt = 0;
        for (int i = 0; i < height*3; i++)
        {
            float degree = 0;

            if (i % 3 == 0)
            {
                degree = rotDeg / 1.5f;
            }
            else if (i % 3 == 1)
            {
                degree = rotDeg / 2.2f;
            }

            while (degree < 360)
            {
                Vector3 pos = new Vector3(radius * Mathf.Cos(degree * Mathf.Deg2Rad), i * 0.31f+0.5f, radius * Mathf.Sin(degree * Mathf.Deg2Rad));
                Vector3 rot = new Vector3(0, -degree, 0);
                bricks[cnt].GetComponent<Rigidbody>().velocity = Vector3.zero;
                bricks[cnt].transform.SetPositionAndRotation(pos, Quaternion.Euler(rot));
                cnt++;
                degree += rotDeg;
            }
        }
    }
}
