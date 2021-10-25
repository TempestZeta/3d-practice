using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public Transform playerTr;
    Transform charaTr;
    Transform cameraTr;
    Camera mainCam;

    bool playerTrace;

    RaycastHit ray;

    GameObject diffuseWall;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Wall")
        {
            other.SendMessage("FadeMaterial");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Wall")
        {
            other.SendMessage("OpaqueMaterial");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        cameraTr = GetComponent<Transform>();
        charaTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        mainCam = GetComponent<Camera>();
        playerTrace = true;
        diffuseWall = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTrace)
        {
            cameraTr.position = playerTr.position;
            cameraTr.rotation = playerTr.rotation;
        }

        if(diffuseWall != null)
        {
            if (diffuseWall != ray.collider.gameObject)
            {
                diffuseWall.SendMessage("OpaqueMaterial");
                diffuseWall = null;
            }
        }
    }

    public void CameraTranslate(Camera subcam)
    {
        mainCam.enabled = false;
        subcam.enabled = true;
    }

    public void ResetCamera(Camera subcam)
    {
        subcam.enabled = false;
        mainCam.enabled = true;
    }

    bool HidePlayer()
    {
        int layer = 1 << 10;

        Vector3 dist = charaTr.position - cameraTr.position;

        Debug.DrawRay(cameraTr.position, dist, Color.black, 10.0f);

        if (Physics.Raycast(cameraTr.position, dist, out ray, 10.0f, layer))
        {
            if (ray.collider.tag == "Wall")
            {
                ray.collider.SendMessage("FadeMaterial");
                diffuseWall = ray.collider.gameObject;
                return true;
            }
        }
        return false;
    }

    void CameraDamage()
    {
        StartCoroutine(TiltCamera());
    }

    IEnumerator TiltCamera()
    {
        playerTrace = false;
        cameraTr.Translate(-0.1f, 0.0f, 0.0f);
        yield return new WaitForSeconds(0.1f);
        cameraTr.Translate(0.2f, 0.0f, 0.0f);
        yield return new WaitForSeconds(0.1f);
        playerTrace = true;
    }

}
