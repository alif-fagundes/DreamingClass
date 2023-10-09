using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] GameObject targetAnimation1;
    [SerializeField] GameObject targetAnimation2;
    private bool canChase = false;
    private bool isScaled = false;

    private void FixedUpdate()
    {
        if (canChase)
        {
            ChaseTarget(PlayerController.Instance.gameObject, targetAnimation1);
        }   
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void StartAnimation()
    {
        StartCoroutine(Orchestrate());

        IEnumerator Orchestrate()
        {
            
            PlayerController.Instance.IsEnabled = false;
            canChase = true;
            yield return new WaitUntil(() => !canChase);
            StartCoroutine(ReScaleObject(targetAnimation1, targetAnimation1.transform.localScale, Vector3.zero, 5));
            yield return new WaitUntil(() => isScaled);
            targetAnimation2.SetActive(true);
            targetAnimation2.GetComponent<PlayAudioSFX>().Play();
            yield return new WaitForSeconds(2);
            targetAnimation2.SetActive(false);
            PlayerController.Instance.IsEnabled = true;
        }
        
    }

    IEnumerator ReScaleObject(GameObject target, Vector3 startScale, Vector3 endScale, float duration)
    {
        float ratio = 0;
        float startTime = Time.time;
        do
        {
            yield return new WaitForEndOfFrame();
            ratio = (Time.time - startTime) / duration;
            target.transform.localScale = Vector3.Lerp(startScale, endScale, ratio);

        } while (ratio < 1);
        isScaled = true;
    }

    IEnumerator RePositionObject(GameObject target, Vector3 startPosition, Vector3 endPosition, float duration)
    {
        float ratio = 0;
        float startTime = Time.time;
        do
        {
            yield return new WaitForEndOfFrame();
            ratio = (Time.time - startTime) / duration;
            target.transform.localPosition = Vector3.Lerp(startPosition, endPosition, ratio);

        } while (ratio < 1);

    }

    void ChaseTarget(GameObject chaser, GameObject chased)
    {
        float distancia = Vector3.Distance(chaser.transform.position, chased.transform.position);

        if (distancia > 3f)
        {
            Move(chaser, chased);
        }
        else
        {
            canChase = false;
        }
      
    }

    void Move(GameObject chaser, GameObject chased)
    {
            float lerpSpeed = 1f;
            Vector3 targetPosition = chased.transform.position;
            Vector3 currentPosition = chaser.transform.position;
            Vector3 direction = targetPosition - currentPosition;
            direction.y = chaser.transform.position.y;
            Quaternion desiredRotation = Quaternion.LookRotation(direction);


            chaser.transform.rotation = Quaternion.Euler(0, desiredRotation.eulerAngles.y, 0);
            Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, lerpSpeed * Time.fixedDeltaTime);

            chaser.transform.position = newPosition;
    }

}
