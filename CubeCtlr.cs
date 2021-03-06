using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCtlr : MonoBehaviour
{
    public enum CubeState
    {
        active,
        in_vanish,
        sleep
    }
    private Color this_color;
    private MeshRenderer cube;
    private CubeState nowstate = CubeState.active;
    private BoxCollider groundCol;

    private int cubeid = -1;
    public int CubeID
    {
        set { if (cubeid == -1) { cubeid = value; } }
        get { return cubeid; }
    }
    private CubeManager manager = null;
    public CubeManager Manager
    {
        set { if (manager == null) manager = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (nowstate == CubeState.active)
            {
                if(other.gameObject.GetComponent<PlayerCtlr>().IPGetPause() == false) transColor();
            }
        }
    }

    public void SetInitColor(Color input)
    {
        changeColor(input);
    }

    public void ChangeState(CubeState state)
    {
        if (nowstate == CubeState.active && state == CubeState.in_vanish)
        {
            nowstate = state;
            cube.material.color -= new Color(0.2f, 0.2f, 0.2f);
        }
        if (nowstate != CubeState.sleep && state == CubeState.sleep)
        {
            nowstate = state;
            setActiveCube(false);
            StartCoroutine("wakeupCube");
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        cube = GetComponent<MeshRenderer>();
        groundCol = GetComponent<BoxCollider>();
    }

    private void transColor()
    {
        Color next_color;

        if (this_color == Color.red)
        {
            next_color = Color.yellow;
        }
        else if (this_color == Color.yellow)
        {
            next_color = Color.green;
        }
        else if (this_color == Color.green)
        {
            next_color = Color.blue;
        }
        else if (this_color == Color.blue)
        {
            next_color = Color.red;
        }
        else
        {
            next_color = Color.black;
        }
        changeColor(next_color);

        manager.InChangeColor(CubeID, next_color.GetHashCode());
    }

    private void changeColor(Color color)
    {
        this_color = color;
        cube.material.color = color + new Color(0.2f, 0.2f, 0.2f);
    }

    private IEnumerator wakeupCube()
    {
        yield return new WaitForSeconds(2f);
        nowstate = CubeState.active;
        changeColor(manager.InWakeup(CubeID));
        setActiveCube(true);
    }

    private void setActiveCube(bool value)
    {
        groundCol.enabled = value;
        cube.enabled = value;
    }
}
