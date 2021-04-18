using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CubeOperater;

public class CubeInstanceMaker : MonoBehaviour
{
    [SerializeField]
    public GameObject CubesParent;

    private GameObject cube_prefab = null;

    void Awake()
    {
        cube_prefab = (GameObject)Resources.Load("prefab/Cube");
        if(cube_prefab == null)
        {
            Debug.Log("Failed to Load Cube prefab");
        }
    }

    public CubeMapNode MakeInstanceAndGetNode(int id, Vector3 pos, ICubeManager manager)
    {
        if(pos == null || manager == null)
        {
            Debug.Log("invalid args");
            return null;
        }
        GameObject cube_obj;
        cube_obj = Instantiate(cube_prefab, pos, Quaternion.identity);
        cube_obj.transform.parent = CubesParent.transform;
        cube_obj.gameObject.name = "cube_" + id.ToString();

        // Add new node
        CubeCtlr cube_ctlr = cube_obj.GetComponent<CubeCtlr>();
        cube_ctlr.Manager = manager;
        cube_ctlr.CubeID = id;
        CubeMapNode newnode = new CubeMapNode(cube_ctlr);
        return newnode;
    }
}
