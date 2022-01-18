// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson
{
  [RequireComponent(typeof (Rigidbody))]
  [RequireComponent(typeof (CapsuleCollider))]
  public class RigidbodyFirstPersonController : MonoBehaviour
  {
    public Camera cam;
    public RigidbodyFirstPersonController.MovementSettings movementSettings = new RigidbodyFirstPersonController.MovementSettings();
    public MouseLook mouseLook = new MouseLook();
    public RigidbodyFirstPersonController.AdvancedSettings advancedSettings = new RigidbodyFirstPersonController.AdvancedSettings();
    private Rigidbody m_RigidBody;
    private CapsuleCollider m_Capsule;
    private float m_YRotation;
    private Vector3 m_GroundContactNormal;
    private bool m_Jump;
    private bool m_PreviouslyGrounded;
    private bool m_Jumping;
    private bool m_IsGrounded;

    public Vector3 Velocity => this.m_RigidBody.velocity;

    public bool Grounded => this.m_IsGrounded;

    public bool Jumping => this.m_Jumping;

    public bool Running => this.movementSettings.Running;

    private void Start()
    {
      this.m_RigidBody = this.GetComponent<Rigidbody>();
      this.m_Capsule = this.GetComponent<CapsuleCollider>();
      this.mouseLook.Init(this.transform, this.cam.transform);
    }

    private void Update()
    {
      this.RotateView();
      if (!CrossPlatformInputManager.GetButtonDown("Jump") || this.m_Jump)
        return;
      this.m_Jump = true;
    }

    private void FixedUpdate()
    {
      this.GroundCheck();
      Vector2 input = this.GetInput();
      Vector3 vector3;
      if (((double) Mathf.Abs(input.x) > 1.40129846432482E-45 || (double) Mathf.Abs(input.y) > 1.40129846432482E-45) && (this.advancedSettings.airControl || this.m_IsGrounded))
      {
        vector3 = Vector3.ProjectOnPlane(this.cam.transform.forward * input.y + this.cam.transform.right * input.x, this.m_GroundContactNormal);
        Vector3 normalized = vector3.normalized;
        normalized.x *= this.movementSettings.CurrentTargetSpeed;
        normalized.z *= this.movementSettings.CurrentTargetSpeed;
        normalized.y *= this.movementSettings.CurrentTargetSpeed;
        vector3 = this.m_RigidBody.velocity;
        if ((double) vector3.sqrMagnitude < (double) this.movementSettings.CurrentTargetSpeed * (double) this.movementSettings.CurrentTargetSpeed)
          this.m_RigidBody.AddForce(normalized * this.SlopeMultiplier(), ForceMode.Impulse);
      }
      if (this.m_IsGrounded)
      {
        this.m_RigidBody.drag = 5f;
        if (this.m_Jump)
        {
          this.m_RigidBody.drag = 0.0f;
          this.m_RigidBody.velocity = new Vector3(this.m_RigidBody.velocity.x, 0.0f, this.m_RigidBody.velocity.z);
          this.m_RigidBody.AddForce(new Vector3(0.0f, this.movementSettings.JumpForce, 0.0f), ForceMode.Impulse);
          this.m_Jumping = true;
        }
        if (!this.m_Jumping && (double) Mathf.Abs(input.x) < 1.40129846432482E-45 && (double) Mathf.Abs(input.y) < 1.40129846432482E-45)
        {
          vector3 = this.m_RigidBody.velocity;
          if ((double) vector3.magnitude < 1.0)
            this.m_RigidBody.Sleep();
        }
      }
      else
      {
        this.m_RigidBody.drag = 0.0f;
        if (this.m_PreviouslyGrounded && !this.m_Jumping)
          this.StickToGroundHelper();
      }
      this.m_Jump = false;
    }

    private float SlopeMultiplier() => this.movementSettings.SlopeCurveModifier.Evaluate(Vector3.Angle(this.m_GroundContactNormal, Vector3.up));

    private void StickToGroundHelper()
    {
      RaycastHit hitInfo;
      if (!Physics.SphereCast(this.transform.position, this.m_Capsule.radius * (1f - this.advancedSettings.shellOffset), Vector3.down, out hitInfo, this.m_Capsule.height / 2f - this.m_Capsule.radius + this.advancedSettings.stickToGroundHelperDistance, -1, QueryTriggerInteraction.Ignore) || (double) Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) >= 85.0)
        return;
      this.m_RigidBody.velocity = Vector3.ProjectOnPlane(this.m_RigidBody.velocity, hitInfo.normal);
    }

    private Vector2 GetInput()
    {
      Vector2 input = new Vector2()
      {
        x = CrossPlatformInputManager.GetAxis("Horizontal"),
        y = CrossPlatformInputManager.GetAxis("Vertical")
      };
      this.movementSettings.UpdateDesiredTargetSpeed(input);
      return input;
    }

    private void RotateView()
    {
      if ((double) Mathf.Abs(Time.timeScale) < 1.40129846432482E-45)
        return;
      float y = this.transform.eulerAngles.y;
      this.mouseLook.LookRotation(this.transform, this.cam.transform);
      if (!this.m_IsGrounded && !this.advancedSettings.airControl)
        return;
      this.m_RigidBody.velocity = Quaternion.AngleAxis(this.transform.eulerAngles.y - y, Vector3.up) * this.m_RigidBody.velocity;
    }

    private void GroundCheck()
    {
      this.m_PreviouslyGrounded = this.m_IsGrounded;
      RaycastHit hitInfo;
      if (Physics.SphereCast(this.transform.position, this.m_Capsule.radius * (1f - this.advancedSettings.shellOffset), Vector3.down, out hitInfo, this.m_Capsule.height / 2f - this.m_Capsule.radius + this.advancedSettings.groundCheckDistance, -1, QueryTriggerInteraction.Ignore))
      {
        this.m_IsGrounded = true;
        this.m_GroundContactNormal = hitInfo.normal;
      }
      else
      {
        this.m_IsGrounded = false;
        this.m_GroundContactNormal = Vector3.up;
      }
      if (this.m_PreviouslyGrounded || !this.m_IsGrounded || !this.m_Jumping)
        return;
      this.m_Jumping = false;
    }

    [Serializable]
    public class MovementSettings
    {
      public float ForwardSpeed = 8f;
      public float BackwardSpeed = 4f;
      public float StrafeSpeed = 4f;
      public float RunMultiplier = 2f;
      public KeyCode RunKey = KeyCode.LeftShift;
      public float JumpForce = 30f;
      public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe[3]
      {
        new Keyframe(-90f, 1f),
        new Keyframe(0.0f, 1f),
        new Keyframe(90f, 0.0f)
      });
      [HideInInspector]
      public float CurrentTargetSpeed = 8f;
      private bool m_Running;

      public void UpdateDesiredTargetSpeed(Vector2 input)
      {
        if (input == Vector2.zero)
          return;
        if ((double) input.x > 0.0 || (double) input.x < 0.0)
          this.CurrentTargetSpeed = this.StrafeSpeed;
        if ((double) input.y < 0.0)
          this.CurrentTargetSpeed = this.BackwardSpeed;
        if ((double) input.y > 0.0)
          this.CurrentTargetSpeed = this.ForwardSpeed;
        if (Input.GetKey(this.RunKey))
        {
          this.CurrentTargetSpeed *= this.RunMultiplier;
          this.m_Running = true;
        }
        else
          this.m_Running = false;
      }

      public bool Running => this.m_Running;
    }

    [Serializable]
    public class AdvancedSettings
    {
      public float groundCheckDistance = 0.01f;
      public float stickToGroundHelperDistance = 0.5f;
      public float slowDownRate = 20f;
      public bool airControl;
      [Tooltip("set it to 0.1 or more if you get stuck in wall")]
      public float shellOffset;
    }
  }
}
