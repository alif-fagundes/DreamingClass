using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5f;
    public float lerpSpeed = 1f;
    private bool canChasePlayer = false;
    private bool canMove = false;
    public Vector3 initialEnemyPosition;
    public Quaternion initialEnemyRotation;

    private void Start()
    {
        initialEnemyPosition = transform.position;
        initialEnemyRotation = transform.rotation;

    }

    private void FixedUpdate()
    {
        if (canChasePlayer)
        {
            Move(player.position);
            canMove = false;

        } else if (canMove)
        {
            if (transform.position != initialEnemyPosition)
            {
                Move(initialEnemyPosition);
            }
            else
            {
                print("Nao pode mover");
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
        BackToPosition();
    }

    public void Move(Vector3 position)
    {
        if (player != null)
        {
            Vector3 targetPosition = position;
            Vector3 currentPosition = transform.position;
            Vector3 direction = targetPosition - currentPosition;
            Quaternion rotacaoDesejada = Quaternion.LookRotation(direction);


            transform.rotation = Quaternion.Euler(0, rotacaoDesejada.eulerAngles.y, 0);
            Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, lerpSpeed * Time.fixedDeltaTime);

            // Defina a nova posição do inimigo.
            transform.position = newPosition;
        }

    }


    private void BackToPosition()
    {
        StartCoroutine(WaitingPosition());

        IEnumerator WaitingPosition()
        {
            yield return new WaitForSeconds(3);
            canMove = true;
        }

        

    }

}
