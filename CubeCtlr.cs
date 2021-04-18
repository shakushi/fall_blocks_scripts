using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCtlr : MonoBehaviour
{

    private MeshRenderer cube;
    private BoxCollider groundCol;

    private int cubeid = -1;
    public int CubeID
    {
        set { if (cubeid == -1) { cubeid = value; } }
        get { return cubeid; }
    }
    private ICubeManager manager = null;
    public ICubeManager Manager
    {
        set { if (manager == null) manager = value; }
    }

    public void SetColor(Color color)
    {
        changeMatColor(color);
    }

    public void SetSleep()
    {
        setActiveCube(false);
    }

    public void WakeUp()
    {
        setActiveCube(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<PlayerCtlr>().IPGetPause() == false)
            {
                manager.InCubeTrigger(cubeid);
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        cube = GetComponent<MeshRenderer>();
        groundCol = GetComponent<BoxCollider>();

    }

    private void changeMatColor(Color input)
    {
        cube.material.color = input;
    }

    private void setActiveCube(bool value)
    {
        groundCol.enabled = value;
        cube.enabled = value;
    }
}
