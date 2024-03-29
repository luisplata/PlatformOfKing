using System;
using UnityEngine;

public class JumpSystem : MonoBehaviour
{
    public Action OnAttack, OnMidAir, OnRelease, OnSustain, OnEndJump;
    [SerializeField] private float timeToAttack, timeToDecreasing, timeToSustain, timeToRelease;
    [SerializeField] private float maxHeighJump, heightDecreasing;
    [SerializeField] private float forceToAttack, forceToDecreasing;
    private TeaTime _attack, _decresing, _sustain, _release;
    private Rigidbody2D _rigidbody;
    private float _deltatimeLocal;
    private RigidbodyConstraints2D _rigidbodyConstraints;
    private bool _jumpingFromAttack;

    public void Configure(Rigidbody2D rigidbody, IFloorController floorController)
    {
        _rigidbody = rigidbody;
        var gameObjectToPlayer = rigidbody.gameObject;
        _rigidbodyConstraints = _rigidbody.constraints;
        _attack = this.tt().Pause().Add(() =>
        {
            _jumpingFromAttack = true;
            _rigidbody.gravityScale = 0;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            Debug.Log("JumpSystem: Attack");
        }).Add(() =>
        {
            OnAttack?.Invoke();
        }).Loop(loop =>
        {
            //Debug.Log("JumpSystem: Attack Loop");
            _deltatimeLocal += loop.deltaTime;
            if (_deltatimeLocal >= timeToAttack)
            {
                loop.Break();
            }

            float t = _deltatimeLocal / timeToAttack;
            float heightMultiplier = Mathf.Cos(t * Mathf.PI * 0.5f);

            var position = gameObjectToPlayer.transform.position;
            position = Vector3.Lerp(position, position + Vector3.up * (maxHeighJump * heightMultiplier),
                forceToAttack * loop.deltaTime);
            gameObjectToPlayer.transform.position = position;
        }).Add(() =>
        {
            _decresing.Play();
        });
        
        _decresing = this.tt().Pause().Add(() =>
        {
            OnMidAir?.Invoke();
        }).Loop(loop =>
        {
            //Debug.Log("JumpSystem: Decreasing Loop");
            _deltatimeLocal += loop.deltaTime;
            if(_deltatimeLocal >= timeToAttack + timeToDecreasing)
            {
                loop.Break();
            }

            float t = (_deltatimeLocal - timeToAttack) / timeToDecreasing;
            float heightMultiplier = Mathf.Log(1 + t * 4);

            var position = gameObjectToPlayer.transform.position;
            position = Vector3.Lerp(position, position - Vector3.up * (heightDecreasing * heightMultiplier), forceToDecreasing * loop.deltaTime);
            gameObjectToPlayer.transform.position = position;
        }).Add(() =>
        {
            _sustain.Play();
        });
        
        _sustain = this.tt().Pause().Add(() =>
        {
            OnSustain?.Invoke();
        }).Loop(loop =>
        {
            //Debug.Log("JumpSystem: Sustain Loop");
            _deltatimeLocal += loop.deltaTime;
            if (_deltatimeLocal >= timeToAttack + timeToDecreasing + timeToSustain)
            {
                loop.Break();
            }
        }).Add(() =>
        {
            _release.Play();
        });
        
        
        _release = this.tt().Pause().Add(() =>
        {
            OnRelease?.Invoke();
        }).Loop(loop=>
        {
            //Debug.Log("JumpSystem: Release Loop");
            _deltatimeLocal += loop.deltaTime;
            if (floorController.IsTouchingFloor())
            {
                loop.Break();
            }

            var t = _deltatimeLocal / timeToAttack;
            var heightMultiplier = Mathf.Log(1 + t * forceToDecreasing);

            var position = gameObjectToPlayer.transform.position;
            position = Vector3.Lerp(position, position - Vector3.up * (maxHeighJump * heightMultiplier), forceToDecreasing * loop.deltaTime);
            gameObjectToPlayer.transform.position = position;
        }).Add(() =>
        {
            _rigidbody.gravityScale = 1;
            _rigidbody.constraints = _rigidbodyConstraints;
            _deltatimeLocal = 0;
            Debug.Log("JumpSystem: Attack End");
            OnEndJump?.Invoke();
            _jumpingFromAttack = false;
        });
    }

    public void Jump()
    {
        Debug.Log("JumpSystem: Jump");
        _attack.Play();
    }

    public void Fall()
    {
        Debug.Log("JumpSystem: Fall");
        if (!_jumpingFromAttack)
        {
            _release.Play();
        }
        
    }
}