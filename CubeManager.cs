using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    [SerializeField]
    public GameObject CubesParent;
    [SerializeField]
    public ScoreCtlr ScoreCtlr;
    [SerializeField]
    public SEPlayer SEPlayer;

    private List<CubeMapNode> cube_map = new List<CubeMapNode>();
    private GameObject cube_prefab;
    private bool isPause = false;
    private List<CubeMapNode> connect_list = new List<CubeMapNode>();
    private List<CubeMapNode> vanish_list = new List<CubeMapNode>();
    private float bonusNum;

    private static int inSleepColor = 0;
    private static int connectNum = 4; //繋がって消えるようになる数

    public void ISetPause(bool value)
    {
        isPause = value;
    }

    public void InChangeColor(int cubeid, int color)
    {
        int count = 0;
        connect_list.Clear();

        CubeMapNode thisnode = cube_map.Find(_ => _.Index == cubeid);
        thisnode.Color = color;
        count += checkRightNode(thisnode, connect_list);
        count += checkLeftNode(thisnode, connect_list);
        count += checkUpNode(thisnode, connect_list);
        count += checkDownNode(thisnode, connect_list);
        if (count >= connectNum - 1)
        {
            connect_list.Add(thisnode);
            foreach (CubeMapNode node in connect_list)
            {
                node.Obj.ChangeState(CubeCtlr.CubeState.in_vanish);
            }
            StartCoroutine("vanishCubeWithWait", thisnode.Index);
            SEPlayer.PlaySound(3);
        }
        else
        {
            SEPlayer.PlaySound(2);
        }
        //Debug.Log("(id, color)=(" + cubeid + ", " + color + ") count=" + count);
    }

    public Color InWakeup(int cubeid)
    {
        CubeMapNode thisnode = cube_map.Find(_ => _.Index == cubeid);
        Color new_color = initColor();
        thisnode.Color = new_color.GetHashCode();
        return new_color;
    }

    // Start is called before the first frame update
    void Awake()
    {
        cube_prefab = (GameObject)Resources.Load("Cube");
        makeCubes();
    }

    private void makeCubes()
    {
        GameObject cube_obj;
        for (int i = 0; i < 25; i++)
        {
            cube_obj = Instantiate(cube_prefab, new Vector3((-4f + i%5*2), -2f, (-4f + i/5*2)), Quaternion.identity);
            cube_obj.transform.parent = CubesParent.transform;
            cube_obj.gameObject.name = "cube_" + i.ToString();
            CubeCtlr cube_ctlr = cube_obj.GetComponent<CubeCtlr>();
            cube_ctlr.Manager = this;
            cube_ctlr.CubeID = i;
            Color color = initColor();
            cube_ctlr.SetInitColor(color);
            CubeMapNode newnode = new CubeMapNode(i, color.GetHashCode());
            newnode.Obj = cube_ctlr;
            cube_map.Add(newnode);
        }
        foreach (CubeMapNode node in cube_map)
        {
            if (node.Index % 5 != 4)
            {
                node.RightNode = cube_map.Find(_ => _.Index == node.Index + 1);
            }
            else
            {
                node.RightNode = null;
            }
            if (node.Index % 5 != 0)
            {
                node.LeftNode = cube_map.Find(_ => _.Index == node.Index - 1);
            }
            else
            {
                node.LeftNode = null;
            }
            if (node.Index < 5 * (5-1))
            {
                node.UpNode = cube_map.Find(_ => _.Index == node.Index + 5);
            }
            else
            {
                node.UpNode = null;
            }
            if (node.Index > 5-1 )
            {
                node.DownNode = cube_map.Find(_ => _.Index == node.Index - 5);
            }
            else
            {
                node.DownNode = null;
            }

        }
    }

    private Color initColor()
    {
        int index = Random.Range(0, 4);
        switch (index)
        {
            case 0:
                return Color.red;
            case 1:
                return Color.yellow;
            case 2:
                return Color.green;
            case 3:
                return Color.blue;
            default:
                return Color.black;
        }
    }

    private IEnumerator vanishCubeWithWait(int index)
    {
        yield return new WaitForSeconds(3f);
        vanishCube(index);
    }

    private void vanishCube(int cubeid)
    {
        Debug.Log("vanishCube Enter");
        int count = 0;
        vanish_list.Clear();
        bonusNum = 1f;

        CubeMapNode thisnode = cube_map.Find(_ => _.Index == cubeid);
        count += checkRightNode(thisnode, vanish_list);
        count += checkLeftNode(thisnode, vanish_list);
        count += checkUpNode(thisnode, vanish_list);
        count += checkDownNode(thisnode, vanish_list);
        if (count >= connectNum - 1)
        {
            vanish_list.Add(thisnode);
            foreach (CubeMapNode node in vanish_list)
            {
                node.Obj.ChangeState(CubeCtlr.CubeState.sleep);
                node.Color = inSleepColor;
            }
            SEPlayer.PlaySound(4);
        }

        bonusNum = checkBonus(vanish_list);

        ScoreCtlr.AddScore(vanish_list.Count, bonusNum);
    }

    private float checkBonus(List<CubeMapNode> list)
    {
        /* ボーナス：１列揃うごとに2.45倍　上限は15倍 */
        float ret = 1f;

        /* Check Vertical */
        for (int i = 0; i < 5; i++)
        {
            if(
               list.Find(_ => _.Index == i     ) != null &&
               list.Find(_ => _.Index == i + 5 ) != null &&
               list.Find(_ => _.Index == i + 10) != null &&
               list.Find(_ => _.Index == i + 15) != null &&
               list.Find(_ => _.Index == i + 20) != null
               )
            {
                ret *= 2.45f;
            }
        }
        /* Check Holizontal */
        for (int i = 0; i < 5; i++)
        {
            if (
               list.Find(_ => _.Index == i * 5) != null &&
               list.Find(_ => _.Index == i * 5 + 1) != null &&
               list.Find(_ => _.Index == i * 5 + 2) != null &&
               list.Find(_ => _.Index == i * 5 + 3) != null &&
               list.Find(_ => _.Index == i * 5 + 4) != null
               )
            {
                ret *= 2.45f;
            }
        }

        return Mathf.Min(ret, 15f);
    }

    private int checkRightNode(CubeMapNode node, List<CubeMapNode> list)
    {
        /* 右のノードがなくなるか、colorが一致しなくなるまで進み、数を返す */
        if (node.RightNode == null)
        {
            return 0;
        }
        else if (node.Color == node.RightNode.Color)
        {
            if (!list.Contains(node.RightNode)) { list.Add(node.RightNode); }
            return 1 + checkUpNodeStraight(node.RightNode, list) + checkDownNodeStraight(node.RightNode, list) + checkRightNode(node.RightNode, list);
        }
        return 0;
    }
    private int checkLeftNode(CubeMapNode node, List<CubeMapNode> list)
    {
        if (node.LeftNode == null)
        {
            return 0;
        }
        else if (node.Color == node.LeftNode.Color)
        {
            if (!list.Contains(node.LeftNode)) { list.Add(node.LeftNode); }
            return 1 + checkUpNodeStraight(node.LeftNode, list) + checkDownNodeStraight(node.LeftNode, list) + checkLeftNode(node.LeftNode, list);
        }
        return 0;
    }
    private int checkUpNode(CubeMapNode node, List<CubeMapNode> list)
    {
        if (node.UpNode == null)
        {
            return 0;
        }
        else if (node.Color == node.UpNode.Color)
        {
            if (!list.Contains(node.UpNode)) { list.Add(node.UpNode); }
            return 1 + checkRightNodeStraight(node.UpNode, list) + checkLeftNodeStraight(node.UpNode, list) + checkUpNode(node.UpNode, list);
        }
        return 0;
    }
    private int checkDownNode(CubeMapNode node, List<CubeMapNode> list)
    {
        if (node.DownNode == null)
        {
            return 0;
        }
        else if (node.Color == node.DownNode.Color)
        {
            if (!list.Contains(node.DownNode)) { list.Add(node.DownNode); }
            return 1 + checkRightNodeStraight(node.DownNode, list) + checkLeftNodeStraight(node.DownNode, list) + checkDownNode(node.DownNode, list);
        }
        return 0;
    }

    private int checkRightNodeStraight(CubeMapNode node, List<CubeMapNode> list)
    {
        if (node.RightNode == null)
        {
            return 0;
        }
        else if (node.Color == node.RightNode.Color)
        {
            if (!list.Contains(node.RightNode)) { list.Add(node.RightNode); }
            //Debug.Log("right detect(" + node.Color + ")" + "|thisnode:" + node.Index + " right:" + node.RightNode.Index);
            return 1 + checkRightNodeStraight(node.RightNode, list);
        }
        return 0;
    }
    private int checkLeftNodeStraight(CubeMapNode node, List<CubeMapNode> list)
    {
        if (node.LeftNode == null)
        {
            return 0;
        }
        else if (node.Color == node.LeftNode.Color)
        {
            if (!list.Contains(node.LeftNode)) { list.Add(node.LeftNode); }
            //Debug.Log("left detect(" + node.Color + ")" + "|thisnode:" + node.Index + " left:" + node.LeftNode.Index);
            return 1 + checkLeftNodeStraight(node.LeftNode, list);
        }
        return 0;
    }
    private int checkUpNodeStraight(CubeMapNode node, List<CubeMapNode> list)
    {
        if (node.UpNode == null)
        {
            return 0;
        }
        else if (node.Color == node.UpNode.Color)
        {
            if (!list.Contains(node.UpNode)) { list.Add(node.UpNode); }
            //Debug.Log("up detect(" + node.Color + ")" + "|thisnode:" + node.Index + " up:" + node.UpNode.Index);
            return 1 + checkUpNodeStraight(node.UpNode, list);
        }
        return 0;
    }
    private int checkDownNodeStraight(CubeMapNode node, List<CubeMapNode> list)
    {
        if (node.DownNode == null)
        {
            return 0;
        }
        else if (node.Color == node.DownNode.Color)
        {
            if (!list.Contains(node.DownNode)) { list.Add(node.DownNode); }
            //Debug.Log("down detect(" + node.Color + ")" + "|thisnode:" + node.Index + " down:" + node.DownNode.Index);
            return 1 + checkDownNodeStraight(node.DownNode, list);
        }
        return 0;
    }

}
