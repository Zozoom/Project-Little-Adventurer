using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Character : MonoBehaviour
{
    [Header("Character Details")]
    public string charName = "Jhonny";
    public GameObject _playerNameText3D;
    public Camera mainCamera;
    public int playerCoins;

    [Header("Character Settings")]
    public float MoveSpeed = 5f;
    public float gravity = -9.8f;
    private CharacterController _cc;
    private Vector3 _movementVelocity;
    private PlayerInput _playerInput;
    private float _verticalVelocity;
    private Animator _animator;
    private float attackStartTime;
    public float attackSlideDuration = 0.4f;
    public float attackSlideSpeed = 0.06f;
    private Vector3 impactOnCharacter;
    public bool isInvincible;
    public float invincibleDuration = 2f;
    private float attackAnimationDuration;
    public float slideSpeed = 9f;

    [Header("Enemy Settings")]
    public bool IsPlayer = true;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Transform TargetPlayer;

    [Header("Health")]
    private Health _health;

    [Header("Damage Caster")]
    private DamageCaster _damageCaster;

    [Header("State Machine")]
    public CharacterState currentState;
    public enum CharacterState { Normal, Attacking, Dead, BeingHit, Slide };

    [Header("Material Animation")]
    private MaterialPropertyBlock _materialPropertyBlock;
    private SkinnedMeshRenderer _skinnedMeshRenderer;

    [Header("Item To Drop")]
    public GameObject itemToDrop;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _health = GetComponent<Health>();
        _damageCaster = GetComponentInChildren<DamageCaster>();

        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
        _skinnedMeshRenderer.GetPropertyBlock(_materialPropertyBlock);

        if (!IsPlayer)
        {
            _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            TargetPlayer = GameObject.FindWithTag("Player").transform;
            _navMeshAgent.speed = MoveSpeed;
        }
        else
        {
            _playerInput = GetComponent<PlayerInput>();
            _playerNameText3D.GetComponent<TextMeshPro>().text = charName;
        }
    }

    private void CalcPlayerMovement()
    {
        if (_playerInput.mouseButtonDown && _cc.isGrounded)
        {
            SwitchStateTo(CharacterState.Attacking);
            return;
        }
        else if (_playerInput.spaceKeyDown && _cc.isGrounded)
        {
            SwitchStateTo(CharacterState.Slide);
            return;
        }

        _movementVelocity.Set(_playerInput.horizontalInput, 0f, _playerInput.verticalInput);
        _movementVelocity.Normalize();
        _movementVelocity = Quaternion.Euler(0, -45f, 0) * _movementVelocity;

        _animator.SetFloat("Speed", _movementVelocity.magnitude);

        _movementVelocity *= MoveSpeed * Time.deltaTime;

        if (_movementVelocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_movementVelocity);

        _animator.SetBool("AirBorne", !_cc.isGrounded);

        _playerNameText3D.transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }

    private void CalcEnemyMovement()
    {
        if (Vector3.Distance(TargetPlayer.position, transform.position) >= _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.SetDestination(TargetPlayer.position);
            _animator.SetFloat("Speed", 0.2f);
        }
        else
        {
            _navMeshAgent.SetDestination(transform.position);
            _animator.SetFloat("Speed", 0f);

            SwitchStateTo(CharacterState.Attacking);
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case CharacterState.Normal:
                if (IsPlayer)
                    CalcPlayerMovement();
                else
                    CalcEnemyMovement();
                break;

            case CharacterState.Attacking:
                if (IsPlayer)
                {
                    _movementVelocity = Vector3.zero;

                    if (Time.time < attackStartTime + attackSlideDuration)
                    {
                        float timePassed = Time.time - attackStartTime;
                        float lerpTime = timePassed / attackSlideDuration;
                        _movementVelocity = Vector3.Lerp(transform.forward * attackSlideSpeed, Vector3.zero, lerpTime);
                    }

                    if (_playerInput.mouseButtonDown && _cc.isGrounded)
                    {
                        string currentClipName = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                        attackAnimationDuration = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                        if (currentClipName != "LittleAdventurerAndie_ATTACK_03" && attackAnimationDuration > 0.5f && attackAnimationDuration < 0.7f)
                        {
                            _playerInput.mouseButtonDown = false;
                            SwitchStateTo(CharacterState.Attacking);
                            CalcPlayerMovement();
                        }
                    }
                }
                break;

            case CharacterState.Dead:
                return;

            case CharacterState.BeingHit:
                if (impactOnCharacter.magnitude > 0.2f)
                {
                    _movementVelocity = impactOnCharacter * Time.deltaTime;
                }
                impactOnCharacter = Vector3.Lerp(impactOnCharacter, Vector3.zero, Time.deltaTime * 5);
                break;

            case CharacterState.Slide:
                _movementVelocity = transform.forward * slideSpeed * Time.deltaTime;
                break;
        }

        if (IsPlayer)
        {
            if (_cc.isGrounded == false)
                _verticalVelocity = gravity;
            else
                _verticalVelocity = gravity * 0.3f;

            _movementVelocity += _verticalVelocity * Vector3.up * Time.deltaTime;

            _cc.Move(_movementVelocity);
            _movementVelocity = Vector3.zero;
        }


    }

    public void SwitchStateTo(CharacterState newState)
    {
        //Clear Cache
        if (IsPlayer)
            _playerInput.ClearChace();

        //Exit State
        switch (currentState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attacking:
                if (_damageCaster != null)
                    _damageCaster.DisableDamageCaster();
                if (IsPlayer)
                    GetComponent<PlayerVfxManager>().StopBlade();
                break;
            case CharacterState.Dead:
                return;
            case CharacterState.BeingHit:
                break;
            case CharacterState.Slide:
                break;
        }

        //Enter State
        switch (newState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attacking:
                if (!IsPlayer)
                {
                    Quaternion newRotation = Quaternion.LookRotation(TargetPlayer.position - transform.position);
                    transform.rotation = newRotation;
                }
                _animator.SetTrigger("Attack");
                if (IsPlayer)
                    attackStartTime = Time.time;
                break;
            case CharacterState.Dead:
                _cc.enabled = false;
                _animator.SetTrigger("Dead");
                StartCoroutine(MaterialDissolve());
                break;
            case CharacterState.BeingHit:
                _animator.SetTrigger("BeingHit");
                if (IsPlayer)
                {
                    isInvincible = true;
                    StartCoroutine(DealyCancelInvincible());
                }

                break;
            case CharacterState.Slide:
                _animator.SetTrigger("Slide");
                break;
        }

        currentState = newState;

        // Debug.Log("State switched to: " + currentState);
    }
    public void SlideAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }
    public void AttackAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    public void BeingHitAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    public void ApplyDamage(int damage, Vector3 attackerPos = new Vector3())
    {
        if (isInvincible)
            return;

        if (_health != null)
            _health.ApplyDamage(damage);

        if (!IsPlayer)
        {
            GetComponent<EnemyVfxManager>().PlayBeingHitVFX(attackerPos);
        }
        else
        {
            SwitchStateTo(CharacterState.BeingHit);
            AddImpact(attackerPos, 10f);
        }

        StartCoroutine(MaterialBlink());
    }

    IEnumerator DealyCancelInvincible()
    {
        yield return new WaitForSeconds(2f);
        isInvincible = false;
    }

    private void AddImpact(Vector3 attackerPos, float force)
    {
        Vector3 impactDir = transform.position - attackerPos;
        impactDir.Normalize();
        impactDir.y = 0;
        impactOnCharacter = impactDir * force;
    }

    public void EnableDamageCaster()
    {
        _damageCaster.EnableDamageCaster();
    }
    public void DisableDamageCaster()
    {
        _damageCaster.DisableDamageCaster();
    }

    IEnumerator MaterialBlink()
    {
        _materialPropertyBlock.SetFloat("_blink", 0.4f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

        yield return new WaitForSeconds(0.2f);

        _materialPropertyBlock.SetFloat("_blink", 0f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }

    IEnumerator MaterialDissolve()
    {
        yield return new WaitForSeconds(2f);

        float dissolveTimeDuration = 2f;
        float currentDissolveTime = 0;
        float dissolveHight_start = 20f;
        float dissolveHight_target = -10f;
        float dissolveHight;

        _materialPropertyBlock.SetFloat("_enabledDissolve", 1f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

        while (currentDissolveTime < dissolveTimeDuration)
        {
            currentDissolveTime += Time.deltaTime;
            dissolveHight = Mathf.Lerp(dissolveHight_start, dissolveHight_target, currentDissolveTime / dissolveTimeDuration);
            _materialPropertyBlock.SetFloat("_dissolve_height", dissolveHight);
            _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
            yield return null;
        }

        DropItem();
    }

    public void DropItem()
    {
        if (itemToDrop != null)
        {
            Instantiate(itemToDrop, transform.position, Quaternion.identity);
        }
    }

    public void PickUpItem(PickUp item)
    {
        switch (item.type)
        {
            case PickUp.PickUpType.Heal:
                AddHealth(item.value);
                break;
            case PickUp.PickUpType.Coin:
                AddCoin(item.value);
                break;
        }
    }

    public void AddHealth(int health)
    {
        _health.AddHealth(health);
        GetComponent<PlayerVfxManager>().PlayHealVFX();
    }
    public void AddCoin(int coin)
    {
        playerCoins += coin;
    }

    public void RotateToTarget()
    {
        if (currentState != CharacterState.Dead)
        {
            transform.LookAt(TargetPlayer, Vector3.up);
        }
    }
}