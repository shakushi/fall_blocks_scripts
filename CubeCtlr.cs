using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCtlr : MonoBehaviour
{
    public enum CubeState
    {
        active,
        goto_vanish,
        sleep
    }
    private MeshRenderer cube;
    private CubeState nowstate = CubeState.active;
    public CubeState State
    {
        get { return nowstate; }
    }
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

    public void ChangeState(CubeState state)
    {
        if (nowstate != CubeState.sleep && state == CubeState.sleep)
        {
            nowstate = state;
            setActiveCube(false);
            StartCoroutine("wakeupCube");
        }
        else
        {
            nowstate = state;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && nowstate == CubeState.active)
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

    private IEnumerator wakeupCube()
    {
        yield return new WaitForSeconds(2f);
        changeMatColor(manager.InWakeup(CubeID));
        ChangeState(CubeState.active);
        setActiveCube(true);
    }

    private void setActiveCube(bool value)
    {
        groundCol.enabled = value;
        cube.enabled = value;
    }
}
