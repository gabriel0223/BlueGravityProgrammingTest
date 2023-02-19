using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Transform _playerSprites;
    [SerializeField] private Animator _anim;
    [SerializeField] private Rigidbody2D _rb2d;

    // Update is called once per frame
    void Update()
    {
        HandleSpriteFlipping();
        HandleAnimation();
    }

    private void HandleSpriteFlipping()
    {
        if (_rb2d.velocity.x == 0)
        {
            return;
        }

        _playerSprites.localScale = new Vector2(_rb2d.velocity.x > 0 ? 1 : -1, _playerSprites.localScale.y);
    }

    private void HandleAnimation()
    {
        _anim.SetBool("Moving", _rb2d.velocity.magnitude > 0);
    }
}
