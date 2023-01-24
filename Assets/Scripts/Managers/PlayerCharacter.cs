using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private float speed;

    private float _horizontal;
    private float _vertical;

    private Rigidbody2D _rb;
    private SpriteRenderer _rend;
    private Animator _anim;

    private string _currAnimState;
    private string IDLE = "idle";
    private string MOVE = "move";

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rend = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        if (_horizontal > 0)
            _rend.flipX = true;
        else if (_horizontal < 0)
            _rend.flipX = false;

        if (_rb.velocity != Vector2.zero)
            ChangeAnimationState(MOVE);
        else
            ChangeAnimationState(IDLE);
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_horizontal * speed, _vertical * speed);
    }

    private void ChangeAnimationState(string newState)
    {
        if (_currAnimState == newState)
            return;

        _anim.Play(newState);
    }
}
