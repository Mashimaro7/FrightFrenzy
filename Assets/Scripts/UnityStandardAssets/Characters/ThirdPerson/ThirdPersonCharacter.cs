// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
  [RequireComponent(typeof (Rigidbody))]
  [RequireComponent(typeof (CapsuleCollider))]
  [RequireComponent(typeof (Animator))]
  public class ThirdPersonCharacter : MonoBehaviour
  {
    [SerializeField]
    private float m_MovingTurnSpeed = 360f;
    [SerializeField]
    private float m_StationaryTurnSpeed = 180f;
    [SerializeField]
    private float m_JumpPower = 12f;
    [Range(1f, 4f)]
    [SerializeField]
    private float m_GravityMultiplier = 2f;
    [SerializeField]
    private float m_RunCycleLegOffset = 0.2f;
    [SerializeField]
    private float m_MoveSpeedMultiplier = 1f;
    [SerializeField]
    private float m_AnimSpeedMultiplier = 1f;
    [SerializeField]
    private float m_GroundCheckDistance = 0.1f;
    private Rigidbody m_Rigidbody;
    private Animator m_Animator;
    private bool m_IsGrounded;
    private float m_OrigGroundCheckDistance;
    private const float k_Half = 0.5f;
    private float m_TurnAmount;
    private float m_ForwardAmount;
    private Vector3 m_GroundNormal;
    private float m_CapsuleHeight;
    private Vector3 m_CapsuleCenter;
    private CapsuleCollider m_Capsule;
    private bool m_Crouching;

    private void Start()
    {
      this.m_Animator = this.GetComponent<Animator>();
      this.m_Rigidbody = this.GetComponent<Rigidbody>();
      this.m_Capsule = this.GetComponent<CapsuleCollider>();
      this.m_CapsuleHeight = this.m_Capsule.height;
      this.m_CapsuleCenter = this.m_Capsule.center;
      this.m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
      this.m_OrigGroundCheckDistance = this.m_GroundCheckDistance;
    }

    public void Move(Vector3 move, bool crouch, bool jump)
    {
      if ((double) move.magnitude > 1.0)
        move.Normalize();
      move = this.transform.InverseTransformDirection(move);
      this.CheckGroundStatus();
      move = Vector3.ProjectOnPlane(move, this.m_GroundNormal);
      this.m_TurnAmount = Mathf.Atan2(move.x, move.z);
      this.m_ForwardAmount = move.z;
      this.ApplyExtraTurnRotation();
      if (this.m_IsGrounded)
        this.HandleGroundedMovement(crouch, jump);
      else
        this.HandleAirborneMovement();
      this.ScaleCapsuleForCrouching(crouch);
      this.PreventStandingInLowHeadroom();
      this.UpdateAnimator(move);
    }

    private void ScaleCapsuleForCrouching(bool crouch)
    {
      if (this.m_IsGrounded & crouch)
      {
        if (this.m_Crouching)
          return;
        this.m_Capsule.height /= 2f;
        this.m_Capsule.center /= 2f;
        this.m_Crouching = true;
      }
      else
      {
        Ray ray = new Ray(this.m_Rigidbody.position + Vector3.up * this.m_Capsule.radius * 0.5f, Vector3.up);
        float num = this.m_CapsuleHeight - this.m_Capsule.radius * 0.5f;
        double radius = (double) this.m_Capsule.radius * 0.5;
        double maxDistance = (double) num;
        if (Physics.SphereCast(ray, (float) radius, (float) maxDistance, -1, QueryTriggerInteraction.Ignore))
        {
          this.m_Crouching = true;
        }
        else
        {
          this.m_Capsule.height = this.m_CapsuleHeight;
          this.m_Capsule.center = this.m_CapsuleCenter;
          this.m_Crouching = false;
        }
      }
    }

    private void PreventStandingInLowHeadroom()
    {
      if (this.m_Crouching)
        return;
      Ray ray = new Ray(this.m_Rigidbody.position + Vector3.up * this.m_Capsule.radius * 0.5f, Vector3.up);
      float num = this.m_CapsuleHeight - this.m_Capsule.radius * 0.5f;
      double radius = (double) this.m_Capsule.radius * 0.5;
      double maxDistance = (double) num;
      if (!Physics.SphereCast(ray, (float) radius, (float) maxDistance, -1, QueryTriggerInteraction.Ignore))
        return;
      this.m_Crouching = true;
    }

    private void UpdateAnimator(Vector3 move)
    {
      this.m_Animator.SetFloat("Forward", this.m_ForwardAmount, 0.1f, Time.deltaTime);
      this.m_Animator.SetFloat("Turn", this.m_TurnAmount, 0.1f, Time.deltaTime);
      this.m_Animator.SetBool("Crouch", this.m_Crouching);
      this.m_Animator.SetBool("OnGround", this.m_IsGrounded);
      if (!this.m_IsGrounded)
        this.m_Animator.SetFloat("Jump", this.m_Rigidbody.velocity.y);
      float num = ((double) Mathf.Repeat(this.m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + this.m_RunCycleLegOffset, 1f) < 0.5 ? 1f : -1f) * this.m_ForwardAmount;
      if (this.m_IsGrounded)
        this.m_Animator.SetFloat("JumpLeg", num);
      if (this.m_IsGrounded && (double) move.magnitude > 0.0)
        this.m_Animator.speed = this.m_AnimSpeedMultiplier;
      else
        this.m_Animator.speed = 1f;
    }

    private void HandleAirborneMovement()
    {
      this.m_Rigidbody.AddForce(Physics.gravity * this.m_GravityMultiplier - Physics.gravity);
      this.m_GroundCheckDistance = (double) this.m_Rigidbody.velocity.y < 0.0 ? this.m_OrigGroundCheckDistance : 0.01f;
    }

    private void HandleGroundedMovement(bool crouch, bool jump)
    {
      if (!jump || crouch || !this.m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        return;
      this.m_Rigidbody.velocity = new Vector3(this.m_Rigidbody.velocity.x, this.m_JumpPower, this.m_Rigidbody.velocity.z);
      this.m_IsGrounded = false;
      this.m_Animator.applyRootMotion = false;
      this.m_GroundCheckDistance = 0.1f;
    }

    private void ApplyExtraTurnRotation() => this.transform.Rotate(0.0f, this.m_TurnAmount * Mathf.Lerp(this.m_StationaryTurnSpeed, this.m_MovingTurnSpeed, this.m_ForwardAmount) * Time.deltaTime, 0.0f);

    public void OnAnimatorMove()
    {
      if (!this.m_IsGrounded || (double) Time.deltaTime <= 0.0)
        return;
      this.m_Rigidbody.velocity = (this.m_Animator.deltaPosition * this.m_MoveSpeedMultiplier / Time.deltaTime) with
      {
        y = this.m_Rigidbody.velocity.y
      };
    }

    private void CheckGroundStatus()
    {
      RaycastHit hitInfo;
      if (Physics.Raycast(this.transform.position + Vector3.up * 0.1f, Vector3.down, out hitInfo, this.m_GroundCheckDistance))
      {
        this.m_GroundNormal = hitInfo.normal;
        this.m_IsGrounded = true;
        this.m_Animator.applyRootMotion = true;
      }
      else
      {
        this.m_IsGrounded = false;
        this.m_GroundNormal = Vector3.up;
        this.m_Animator.applyRootMotion = false;
      }
    }
  }
}
