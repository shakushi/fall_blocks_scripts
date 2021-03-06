using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMatColor : MonoBehaviour
{
    public Color color = Color.black;

    private MeshRenderer cube;
    private bool random = false;

    private void Awake()
    {
        cube = GetComponent<MeshRenderer>();
        if (color == Color.black)
        {
            if (random)
            {
                cube.material.color = new Color(Random.value, Random.value, Random.value, 1.0f);
            }
            else
            {
                cube.material.color = initColor();
            }

        }
        else
        {
            cube.material.color = color;
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

}
