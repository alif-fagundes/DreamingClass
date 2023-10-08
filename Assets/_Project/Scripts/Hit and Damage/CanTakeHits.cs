using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CanTakeHits : MonoBehaviour
{
    [Header("Flash")]
    [ColorUsageAttribute(false, true), SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float _flashTime = 0.1f;

    [Header("Knockback")]
    [SerializeField] private float _knockbackDistance = 2f;
    [SerializeField] private float _knockbackSpeed = 2f;
    [SerializeField] private BaseController _controllerToDisable;
    [SerializeField] private LayerMask _wallLayerMask;

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
        if (_controllerToDisable != null && _controllerToDisable.IsEnabled)
        {
            _controllerToDisable.ToggleEnabled(false);
        }

        Vector3 direction = transform.position - originPosition;

        Vector3 startPosition = transform.position;
        Vector3 finalPosition = transform.position + (direction * _knockbackDistance);
        finalPosition.y = transform.position.y;

        if (Physics.Linecast(new Vector3(startPosition.x, startPosition.y + 0.3f, startPosition.z),
            new Vector3(finalPosition.x, finalPosition.y + 0.3f, finalPosition.z), out RaycastHit hit, _wallLayerMask))
        {
            // void knockback pushing the unit into a wall

            direction = transform.position - hit.point;
            finalPosition = transform.position + (direction * 0.3f);
            finalPosition.y = transform.position.y;
        }

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
                _originalColors.Add(_rMaterial.GetColor("_EmissionColor"));
            }
        }

    }

    private void Flash()
    {
        foreach (var m in _materials)
        {
            m.SetColor("_EmissionColor", _flashColor);
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
            _materials[i].SetColor("_EmissionColor", _originalColors[i]);
        }
    }
}
