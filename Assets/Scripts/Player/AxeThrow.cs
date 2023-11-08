using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using UnityEngine.Timeline;

public class AxeThrow : MonoBehaviour
{
    [SerializeField] Transform _axe;
    [SerializeField] Transform _hand;
    [SerializeField] float _strength;
    [SerializeField] float _returnDelay = 1f;

    [Header("Prediction Trajectoire")]
    [SerializeField, Range(10, 100), Tooltip("La quantit� max de points pour le LineRenderer")]
    int _maxPoints = 50;
    [SerializeField, Range(0.01f, 0.5f), Tooltip("L'incr�ment de temps utilis� pour calculer la trajectoire")]
    float _increment = 0.025f;
    [SerializeField, Range(1.05f, 2f), Tooltip("Le chevauchement entre les points de la trajectoire (multiplicateur de la distance entre les points)")]
    float _rayOverlap = 1.1f;

    [Header("GameObjects")]
    [SerializeField] Light2D _axeLight;

    private LineRenderer _trajectoryLine;

    private Rigidbody2D _axeRb;
    private Vector2 _originalPos;
    private Quaternion _originalRotation;

    private void Awake() {
        _axeRb = _axe.GetComponent<Rigidbody2D>();
        _originalPos = _axe.localPosition;
        _originalRotation = _axe.localRotation;
        _trajectoryLine = GetComponent<LineRenderer>();
    }

    private void Start() {
        _axeLight.enabled = false;
        SetTrajectoryVisible(true);
    }

    private void Update() {
        PredictTrajectory();
    }

    public void SetTrajectoryVisible(bool visible) {
        _trajectoryLine.enabled = visible;
    }

    public void  Throw() {
        _axe.SetParent(null);
        _axeLight.enabled = true;
        _axeRb.bodyType = RigidbodyType2D.Dynamic;
        Vector2 throwDir = new Vector2(_hand.parent.localScale.x, 0f);
        _axeRb.AddForce(throwDir * _strength, ForceMode2D.Impulse);
        StartCoroutine(AxeReturnRoutine());
    }

    private IEnumerator AxeReturnRoutine() {
        yield return new WaitForSeconds(_returnDelay);
        
        _axe.DOMove((Vector2) _hand.position + _originalPos, 1f).SetEase(Ease.InExpo).OnComplete(() => {
            _axe.SetParent(_hand);
            _axeRb.bodyType = RigidbodyType2D.Kinematic;
            _axe.localPosition = _originalPos;
            _axe.localRotation = _originalRotation;
            _axeRb.angularVelocity = 0f;
            _axeRb.velocity = Vector2.zero;
            _axeLight.enabled = false;
        });

    }

    public void PredictTrajectory() {
        Vector3 velocity = new Vector2(_hand.parent.localScale.x, 0f) * (_strength / _axeRb.mass);
        Vector3 position = _hand.position;
        Vector3 nextPosition;
        float overlap;

        UpdateLineRender(_maxPoints, (0, position));

        for (int i = 1; i < _maxPoints; i++) {
            // Estimate velocity and update next predicted position
            velocity = CalculateNewVelocity(velocity, _axeRb.drag, _increment);
            nextPosition = position + velocity * _increment;

            // Overlap our rays by small margin to ensure we never miss a surface
            overlap = Vector3.Distance(position, nextPosition) * _rayOverlap;

            //When hitting a surface we want to show the surface marker and stop updating our line
            if (Physics.Raycast(position, velocity.normalized, out RaycastHit hit, overlap)) {
                UpdateLineRender(i, (i - 1, hit.point));
                break;
            }

            //If nothing is hit, continue rendering the arc without a visual marker
            position = nextPosition;
            UpdateLineRender(_maxPoints, (i, position)); //Unneccesary to set count here, but not harmful
        }
    }

    private Vector3 CalculateNewVelocity(Vector3 velocity, float drag, float increment) {
        velocity += Physics.gravity * increment;
        velocity *= Mathf.Clamp01(1f - drag * increment);
        return velocity;
    }

    private void UpdateLineRender(int count, (int point, Vector3 pos) pointPos) {
        _trajectoryLine.positionCount = count;
        _trajectoryLine.SetPosition(pointPos.point, pointPos.pos);
    }

}