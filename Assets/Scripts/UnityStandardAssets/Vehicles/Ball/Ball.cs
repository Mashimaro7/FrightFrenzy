// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Vehicles.Ball.Ball
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Vehicles.Ball
{
  public class Ball : MonoBehaviour
  {
    [SerializeField]
    private float m_MovePower = 5f;
    [SerializeField]
    private bool m_UseTorque = true;
    [SerializeField]
    private float m_MaxAngularVelocity = 25f;
    [SerializeField]
    private float m_JumpPower = 2f;
    private const float k_GroundRayLength = 1f;
    private Rigidbody m_Rigidbody;

    private void Start()
    {
      this.m_Rigidbody = this.GetComponent<Rigidbody>();
      this.GetComponent<Rigidbody>().maxAngularVelocity = this.m_MaxAngularVelocity;
    }

    public void Move(Vector3 moveDirection, bool jump)
    {
      if (this.m_UseTorque)
        this.m_Rigidbody.AddTorque(new Vector3(moveDirection.z, 0.0f, -moveDirection.x) * this.m_MovePower);
      else
        this.m_Rigidbody.AddForce(moveDirection * this.m_MovePower);
      if (!(Physics.Raycast(this.transform.position, -Vector3.up, 1f) & jump))
        return;
      this.m_Rigidbody.AddForce(Vector3.up * this.m_JumpPower, ForceMode.Impulse);
    }
  }
}
