// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.SimpleMouseRotator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Utility
{
  public class SimpleMouseRotator : MonoBehaviour
  {
    public Vector2 rotationRange = (Vector2) new Vector3(70f, 70f);
    public float rotationSpeed = 10f;
    public float dampingTime = 0.2f;
    public bool autoZeroVerticalOnMobile = true;
    public bool autoZeroHorizontalOnMobile;
    public bool relative = true;
    private Vector3 m_TargetAngles;
    private Vector3 m_FollowAngles;
    private Vector3 m_FollowVelocity;
    private Quaternion m_OriginalRotation;

    private void Start() => this.m_OriginalRotation = this.transform.localRotation;

    private void Update()
    {
      this.transform.localRotation = this.m_OriginalRotation;
      if (this.relative)
      {
        float axis1 = CrossPlatformInputManager.GetAxis("Mouse X");
        float axis2 = CrossPlatformInputManager.GetAxis("Mouse Y");
        if ((double) this.m_TargetAngles.y > 180.0)
        {
          this.m_TargetAngles.y -= 360f;
          this.m_FollowAngles.y -= 360f;
        }
        if ((double) this.m_TargetAngles.x > 180.0)
        {
          this.m_TargetAngles.x -= 360f;
          this.m_FollowAngles.x -= 360f;
        }
        if ((double) this.m_TargetAngles.y < -180.0)
        {
          this.m_TargetAngles.y += 360f;
          this.m_FollowAngles.y += 360f;
        }
        if ((double) this.m_TargetAngles.x < -180.0)
        {
          this.m_TargetAngles.x += 360f;
          this.m_FollowAngles.x += 360f;
        }
        this.m_TargetAngles.y += axis1 * this.rotationSpeed;
        this.m_TargetAngles.x += axis2 * this.rotationSpeed;
        this.m_TargetAngles.y = Mathf.Clamp(this.m_TargetAngles.y, (float) (-(double) this.rotationRange.y * 0.5), this.rotationRange.y * 0.5f);
        this.m_TargetAngles.x = Mathf.Clamp(this.m_TargetAngles.x, (float) (-(double) this.rotationRange.x * 0.5), this.rotationRange.x * 0.5f);
      }
      else
      {
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;
        this.m_TargetAngles.y = Mathf.Lerp((float) (-(double) this.rotationRange.y * 0.5), this.rotationRange.y * 0.5f, x / (float) Screen.width);
        this.m_TargetAngles.x = Mathf.Lerp((float) (-(double) this.rotationRange.x * 0.5), this.rotationRange.x * 0.5f, y / (float) Screen.height);
      }
      this.m_FollowAngles = Vector3.SmoothDamp(this.m_FollowAngles, this.m_TargetAngles, ref this.m_FollowVelocity, this.dampingTime);
      this.transform.localRotation = this.m_OriginalRotation * Quaternion.Euler(-this.m_FollowAngles.x, this.m_FollowAngles.y, 0.0f);
    }
  }
}
