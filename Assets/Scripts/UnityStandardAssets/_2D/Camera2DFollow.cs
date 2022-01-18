// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets._2D.Camera2DFollow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets._2D
{
  public class Camera2DFollow : MonoBehaviour
  {
    public Transform target;
    public float damping = 1f;
    public float lookAheadFactor = 3f;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;
    private float m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;

    private void Start()
    {
      this.m_LastTargetPosition = this.target.position;
      this.m_OffsetZ = (this.transform.position - this.target.position).z;
      this.transform.parent = (Transform) null;
    }

    private void Update()
    {
      float x = (this.target.position - this.m_LastTargetPosition).x;
      this.m_LookAheadPos = (double) Mathf.Abs(x) <= (double) this.lookAheadMoveThreshold ? Vector3.MoveTowards(this.m_LookAheadPos, Vector3.zero, Time.deltaTime * this.lookAheadReturnSpeed) : this.lookAheadFactor * Vector3.right * Mathf.Sign(x);
      this.transform.position = Vector3.SmoothDamp(this.transform.position, this.target.position + this.m_LookAheadPos + Vector3.forward * this.m_OffsetZ, ref this.m_CurrentVelocity, this.damping);
      this.m_LastTargetPosition = this.target.position;
    }
  }
}
