// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets._2D.PlatformerCharacter2D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets._2D
{
  public class PlatformerCharacter2D : MonoBehaviour
  {
    [SerializeField]
    private float m_MaxSpeed = 10f;
    [SerializeField]
    private float m_JumpForce = 400f;
    [Range(0.0f, 1f)]
    [SerializeField]
    private float m_CrouchSpeed = 0.36f;
    [SerializeField]
    private bool m_AirControl;
    [SerializeField]
    private LayerMask m_WhatIsGround;
    private Transform m_GroundCheck;
    private const float k_GroundedRadius = 0.2f;
    private bool m_Grounded;
    private Transform m_CeilingCheck;
    private const float k_CeilingRadius = 0.01f;
    private Animator m_Anim;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;

    private void Awake()
    {
      this.m_GroundCheck = this.transform.Find("GroundCheck");
      this.m_CeilingCheck = this.transform.Find("CeilingCheck");
      this.m_Anim = this.GetComponent<Animator>();
      this.m_Rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
      this.m_Grounded = false;
      foreach (Component component in Physics2D.OverlapCircleAll((Vector2) this.m_GroundCheck.position, 0.2f, (int) this.m_WhatIsGround))
      {
        if ((Object) component.gameObject != (Object) this.gameObject)
          this.m_Grounded = true;
      }
      this.m_Anim.SetBool("Ground", this.m_Grounded);
      this.m_Anim.SetFloat("vSpeed", this.m_Rigidbody2D.velocity.y);
    }

    public void Move(float move, bool crouch, bool jump)
    {
      if (!crouch && this.m_Anim.GetBool("Crouch") && (bool) (Object) Physics2D.OverlapCircle((Vector2) this.m_CeilingCheck.position, 0.01f, (int) this.m_WhatIsGround))
        crouch = true;
      this.m_Anim.SetBool("Crouch", crouch);
      if (this.m_Grounded || this.m_AirControl)
      {
        move = crouch ? move * this.m_CrouchSpeed : move;
        this.m_Anim.SetFloat("Speed", Mathf.Abs(move));
        this.m_Rigidbody2D.velocity = new Vector2(move * this.m_MaxSpeed, this.m_Rigidbody2D.velocity.y);
        if ((double) move > 0.0 && !this.m_FacingRight)
          this.Flip();
        else if ((double) move < 0.0 && this.m_FacingRight)
          this.Flip();
      }
      if (!(this.m_Grounded & jump) || !this.m_Anim.GetBool("Ground"))
        return;
      this.m_Grounded = false;
      this.m_Anim.SetBool("Ground", false);
      this.m_Rigidbody2D.AddForce(new Vector2(0.0f, this.m_JumpForce));
    }

    private void Flip()
    {
      this.m_FacingRight = !this.m_FacingRight;
      Vector3 localScale = this.transform.localScale;
      localScale.x *= -1f;
      this.transform.localScale = localScale;
    }
  }
}
