using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Dependencies */
[RequireComponent(typeof(CharacterController))]

public class PlayerCtlr : MonoBehaviour
{
    /* SETTING VALUE */
    private float moveSpeed = 3f;
    private Vector3 gravity = new Vector3(0, -1.5f, 0);

    private SimpleAnimator animator;
    private CharacterController character;
    private bool tmpFlagMove = false;
    private bool isPause = true;
    private Vector3 inputDir = Vector3.zero;
    private bool isResult = false;

    private enum AnimState
    {
        stand,
        run,
        fall
    }
    private AnimState animationState = AnimState.stand;

    public void IPOnStickInput(Vector3 normalInput)
    {
        if(normalInput == null) { return; }

        tmpFlagMove = true;
        inputDir = normalInput;
    }
    public bool IPGetPause()
    {
        return isPause;
    }
    public void IPSetPause(bool value)
    {
        isPause = value;
    }
    public void InResult()
    {
        isResult = true;
    }
    public void IPRespone()
    {
        StartCoroutine("pauseSec", 3);
        Vector3 restartPos = new Vector3(0, 2f, 0);
        this.gameObject.transform.position = restartPos;
    }

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<SimpleAnimator>();
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isResult) return;
        if (isPause)
        {
            animSwitchIfNotSame(AnimState.stand);
            return;
        }

        if (tmpFlagMove)
        {
            tmpFlagMove = false;
            moveWithRotation(inputDir + gravity);
            if (character.isGrounded)
            {
                animSwitchIfNotSame(AnimState.run);
            }
            else
            {
                animSwitchIfNotSame(AnimState.fall);
            }
        }
        else
        {
            moveNoRotation(gravity);
            if (character.isGrounded)
            {
                animSwitchIfNotSame(AnimState.stand);
            }
            else
            {
                animSwitchIfNotSame(AnimState.fall);
            }
        }
    }

    private void animSwitchIfNotSame(AnimState state)
    {
        if (animationState != state)
        {
            animationState = state;
            switch(state)
            {
                case AnimState.stand:
                    animator.CrossFade("Standing@loop");
                    break;
                case AnimState.run:
                    animator.CrossFade("Running@loop");
                    break;
                case AnimState.fall:
                    animator.CrossFade("Jumping@loop");
                    break;
            }
        }
    }

    private void moveNoRotation(Vector3 moveDirection)
    {
        character.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
    private void moveWithRotation(Vector3 moveDirection)
    {
        Quaternion lookRotation;
        character.Move(moveDirection * moveSpeed * Time.deltaTime);

        lookRotation = Quaternion.LookRotation(moveDirection);

        lookRotation.x = 0;
        lookRotation.z = 0;
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, lookRotation, 0.2f);
    }

    private IEnumerator pauseSec(int sec)
    {
        isPause = true;
        yield return new WaitForSeconds(sec);
        isPause = false;
    }
}
