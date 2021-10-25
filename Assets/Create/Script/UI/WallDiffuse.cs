using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDiffuse : MonoBehaviour
{
    public Material[] matArr;

    MeshRenderer wallRenderer;

    public bool isDiffuse;

    private void Awake()
    {
        wallRenderer = GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        matArr[1].color = new Vector4(1.0f, 1.0f, 1.0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        //Collider[] player = Physics.OverlapSphere(transform.position, 5.0f, layerMask);
        //
        //if(player.Length == 1)
        //{
        //    if (isDiffuse)
        //    {
        //        FadeMaterial();
        //    }
        //}
        //else
        //{
        //    isDiffuse = false;
        //    OpaqueMaterial();
        //}
    }

    public void FadeMaterial()
    {
        wallRenderer.material = matArr[1];
    }

    public void OpaqueMaterial()
    {
        wallRenderer.material = matArr[0];
    }

    public void NeedDiffuse()
    {
        isDiffuse = true;
    }
}
