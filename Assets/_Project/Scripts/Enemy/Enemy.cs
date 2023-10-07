using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : BaseController, ITakeDamage, IDoDamage, ICanActivatePressurePlates
{
    public Transform player;
    public float moveSpeed = 5f;
    public float lerpSpeed = 1f;
    public bool canChasePlayer = false;
    private bool canMove = false;
    private bool onCooldown = false;
    public Vector3 initialEnemyPosition;
    public Quaternion initialEnemyRotation;

    [Header("Combat")]
    [SerializeField] private int _maxHealth = 30;
    [SerializeField] private int _damage = 5;
    [SerializeField] private float _cooldownTime = 1.5f;

    [Header("Events")]
    public UnityEvent OnDeath;

    private int _currentHealth;
    private bool _isDead = false;
    private Coroutine _cooldownCoroutine;

    private void Start()
    {
        initialEnemyPosition = transform.position;
        initialEnemyRotation = transform.rotation;

        _currentHealth = _maxHealth;

        ToggleEnabled(true);
    }

    private void FixedUpdate()
    {
        if (!IsEnabled) return;
        if (_isDead) return;


        if (canChasePlayer)
        {
            Move(player.position);
            canMove = false;
        }
        else if (canMove)
        {
            float distancia = Vector3.Distance(transform.position, initialEnemyPosition);

            if (distancia > 0.5f)
            {
                Move(initialEnemyPosition);
            }
            else
            {
                transform.rotation = initialEnemyRotation;
                canMove = false;
            }
        }
    }

    public void StartChasePlayer()
    {
        canChasePlayer = true;
    }

    public void StopChasePlayer()
    {
        canChasePlayer = false;
        canMove = true;
        BackToPosition();
    }

    public void Move(Vector3 position)
    {
        if (onCooldown) return;

        if (player != null)
        {
            Vector3 targetPosition = position;
            Vector3 currentPosition = transform.position;
            Vector3 direction = targetPosition - currentPosition;
            direction.y = transform.position.y;
            Quaternion desiredRotation = Quaternion.LookRotation(direction);


            transform.rotation = Quaternion.Euler(0, desiredRotation.eulerAngles.y, 0);
            Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, lerpSpeed * Time.fixedDeltaTime);

            transform.position = newPosition;
        }
    }


    private void BackToPosition()
    {
        StartCoroutine(WaitingPosition());

        IEnumerator WaitingPosition()
        {
            canMove = false;
            yield return new WaitForSeconds(3);
            canMove = true;
        }

    }

    private void Cooldown()
    {
        if (_cooldownCoroutine != null)
        {
            StopCoroutine(_cooldownCoroutine);
        }
        _cooldownCoroutine = StartCoroutine(HandleCooldown());

        IEnumerator HandleCooldown()
        {
            onCooldown = true;
            yield return new WaitForSeconds(_cooldownTime);
            onCooldown = false;
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
        else
        {
            Cooldown();
        }
    }

    private void Die()
    {
        ToggleEnabled(false);
        _isDead = true;
        OnDeath?.Invoke();
    }

    public void DoDamage(ITakeDamage other)
    {
        other.TakeDamage(_damage);
        Cooldown();
    }
}
