using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCtlr : MonoBehaviour
{
    [SerializeField]
    public PlayerCtlr Player;
    [SerializeField]
    public GameSceneCtlr GameSceneCtlr;

    private bool preStart = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (preStart)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                preStart = false;
                GameSceneCtlr.StartGame();
            }
            return;
        }

        Vector3 input = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            input.z += 1f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            input.x += 1f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            input.z -= 1f;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            input.x -= 1f;
        }

        if (input != Vector3.zero)
        {
            Player.IPOnStickInput(input.normalized);
        }
    }
}
