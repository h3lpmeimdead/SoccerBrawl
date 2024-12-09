using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AI", menuName = "AIMovement")]
public class AIMovementStats : ScriptableObject
{
    [Header("Walk")]
    [Range(1f, 100f)] public float _maxWalkSpeed = 12.5f;
    [Range(0.25f, 50f)] public float _groundAcceleration = 5f;
    [Range(0.25f, 50f)] public float _groundDeceleration = 20f;
    [Range(0.25f, 50f)] public float _airAcceleration = 5f;
    [Range(0.25f, 50f)] public float _airDeceleration = 5f;

    [Header("Ground/Collision checks")]
    public LayerMask _groundLayer;
    public float _groundDetectionRayLength = 0.02f;
    public float _headDetectionRayLength = 0.02f;
    [Range(0f, 1f)] public float _headWidth = 0.75f;

    [Header("Jump")]
    public float _jumpHeight = 6.5f;
    [Range(1f, 1.1f)] public float _jumpHeightCompensationFactor = 1.054f;
    public float _timeTillJumpApex = 0.35f;
    [Range(0.01f, 5f)] public float _gravityOnReleaseMultiplier = 2f;
    public float _maxFallSpeed = 26f;
    [Range(1, 5)] public int _numberofJumpsAllowed = 2;

    [Header("Jump Cut")]
    [Range(0.02f, 0.3f)] public float _timeForUpwardsCancel = 0.027f;

    [Header("Jump Apex")]
    [Range(0.5f, 1f)] public float _apexThreshHold = 0.97f;
    [Range(0.01f, 1f)] public float _apexHangTime = 0.075f;

    [Header("Jump Buffer")]
    [Range(0f, 1f)] public float _jumpBufferTime = 0.125f;

    [Header("Jump Coyote Time")]
    [Range(0f, 1f)] public float _jumpCoyoteTime = 0.1f;

    [Header("Debug")]
    public bool _debugShowIsGroundedBox;
    public bool _debugShowHeadBumpBox;

    [Header("JumpVisualization Tool")]
    public bool _showWalkJumpArc = false;
    public bool _showRunJumpArc = false;
    public bool _stopOnCollision = true;
    public bool _drawRight = true;
    [Range(5, 100)] public int _arcResolution = 20;
    [Range(0, 500)] public int _visualizationSteps = 90;

    [Header("Kick")]
    public float _force = 100f;

    public float Gravity { get; private set; }
    public float InitialJumpVelocity { get; private set; }
    public float AdjustedJumpHeight { get; private set; }

    private void OnValidate()
    {
        CalculateValues();
    }

    private void OnEnable()
    {
        CalculateValues();
    }

    private void CalculateValues()
    {
        AdjustedJumpHeight = _jumpHeight * _jumpHeightCompensationFactor;
        Gravity = -(2f * AdjustedJumpHeight) / Mathf.Pow(_timeTillJumpApex, 2f);
        InitialJumpVelocity = Mathf.Abs(Gravity) * _timeTillJumpApex;
    }
}
