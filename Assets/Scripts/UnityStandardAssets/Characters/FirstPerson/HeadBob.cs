// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Characters.FirstPerson.HeadBob
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityStandardAssets.Utility;

namespace UnityStandardAssets.Characters.FirstPerson
{
  public class HeadBob : MonoBehaviour
  {
    public Camera Camera;
    public CurveControlledBob motionBob = new CurveControlledBob();
    public LerpControlledBob jumpAndLandingBob = new LerpControlledBob();
    public RigidbodyFirstPersonController rigidbodyFirstPersonController;
    public float StrideInterval;
    [Range(0.0f, 1f)]
    public float RunningStrideLengthen;
    private bool m_PreviouslyGrounded;
    private Vector3 m_OriginalCameraPosition;

    private void Start()
    {
      this.motionBob.Setup(this.Camera, this.StrideInterval);
      this.m_OriginalCameraPosition = this.Camera.transform.localPosition;
    }

    private void Update()
    {
      Vector3 localPosition;
      if ((double) this.rigidbodyFirstPersonController.Velocity.magnitude > 0.0 && this.rigidbodyFirstPersonController.Grounded)
      {
        this.Camera.transform.localPosition = this.motionBob.DoHeadBob(this.rigidbodyFirstPersonController.Velocity.magnitude * (this.rigidbodyFirstPersonController.Running ? this.RunningStrideLengthen : 1f));
        localPosition = this.Camera.transform.localPosition with
        {
          y = this.Camera.transform.localPosition.y - this.jumpAndLandingBob.Offset()
        };
      }
      else
        localPosition = this.Camera.transform.localPosition with
        {
          y = this.m_OriginalCameraPosition.y - this.jumpAndLandingBob.Offset()
        };
      this.Camera.transform.localPosition = localPosition;
      if (!this.m_PreviouslyGrounded && this.rigidbodyFirstPersonController.Grounded)
        this.StartCoroutine(this.jumpAndLandingBob.DoBobCycle());
      this.m_PreviouslyGrounded = this.rigidbodyFirstPersonController.Grounded;
    }
  }
}
