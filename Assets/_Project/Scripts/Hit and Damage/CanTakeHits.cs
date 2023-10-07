using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CanTakeHits : MonoBehaviour
{
    [Header("Flash")]
    [SerializeField] Color _flashColor = Color.white;
    [SerializeField] float _flashTime = 0.1f;

    [Header("Knockback")]
    [SerializeField] float _knockbackDistance = 2f;
    [SerializeField] float _knockbackSpeed = 2f;
    [SerializeField] BaseController _controllerToDisable;

    [Header("Events")]
    public UnityEvent OnHit;


    private List<Renderer> _renderes = new List<Renderer>();
    private List<Material> _materials = new List<Material>();
    private List<Color> _originalColors = new List<Color>();

    private Coroutine _resetColorsCoroutine;
    private Coroutine _knockBackCoroutine;

    private void Awake()
    {
        _renderes.AddRange(GetComponentsInChildren<Renderer>());

        SetupOriginalColors();
    }

    public void TakeHit(Vector3 dealerPosition)
    {
        if (_flashTime > 0)
        {
            Flash();
        }

        if (_knockbackDistance > 0)
        {
            ApplyKnockback(dealerPosition);
        }

        OnHit?.Invoke();
        //Debug.Log($"{gameObject.name} took a hit");
    }

    private void ApplyKnockback(Vector3 dealerPosition)
    {
        if (_knockBackCoroutine != null)
        {
            StopCoroutine(_knockBackCoroutine);
        }

        _knockBackCoroutine = StartCoroutine(HandleKnockback(dealerPosition));
    }

    private IEnumerator HandleKnockback(Vector3 originPosition)
    {
        if (_controllerToDisable != null)
        {
            _controllerToDisable.ToggleEnabled(false);
        }

        Vector3 direction = transform.position - originPosition;

        Vector3 startPosition = transform.position;
        Vector3 finalPosition = transform.position + (direction * _knockbackDistance);
        finalPosition.y = transform.position.y;

        float time = 0f;
        while (time < 1f)
        {
            transform.position = Vector3.Lerp(startPosition, finalPosition, time);

            time += Time.deltaTime * _knockbackSpeed;
            yield return null;
        }


        if (_controllerToDisable != null)
        {
            _controllerToDisable.ToggleEnabled(true);
        }
    }

    private void SetupOriginalColors()
    {
        foreach (var _r in _renderes)
        {
            foreach (var _rMaterial in _r.materials)
            {
                _materials.Add(_rMaterial);
                _originalColors.Add(_rMaterial.color);
            }
        }

    }

    private void Flash()
    {
        foreach (var _m in _materials)
        {
            _m.color = _flashColor;
        }

        if (_resetColorsCoroutine != null)
        {
            StopCoroutine(_resetColorsCoroutine);
        }

        StartCoroutine(ResetColors());
    }

    private IEnumerator ResetColors()
    {
        yield return new WaitForSeconds(_flashTime);

        for (int i = 0; i < _materials.Count; i++)
        {
            _materials[i].color = _originalColors[i];
        }
    }
}
