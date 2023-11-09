using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInput : MonoBehaviour
{
    public FrameInput FrameInput { get; private set; }

    private void Update() {
        FrameInput = GatherInput();
    }

    private FrameInput GatherInput() {
        return new FrameInput {
            MoveX = Input.GetAxisRaw("Horizontal"),
            AimShot = Input.GetAxis("Vertical"),
            Jump = Input.GetButtonDown("Jump"),
            Attack = Input.GetButton("Fire1"),
            ReleaseAttack = Input.GetButtonUp("Fire1")
        };
    }
}

public struct FrameInput {
    public float MoveX;
    public float AimShot;
    public bool Jump;
    public bool Attack;
    public bool ReleaseAttack;
    public bool Pause;
}
