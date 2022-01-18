// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Vehicles.Ball.BallUserControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Ball
{
  public class BallUserControl : MonoBehaviour
  {
    private UnityStandardAssets.Vehicles.Ball.Ball ball;
    private Vector3 move;
    private Transform cam;
    private Vector3 camForward;
    private bool jump;

    private void Awake()
    {
      this.ball = this.GetComponent<UnityStandardAssets.Vehicles.Ball.Ball>();
      if ((Object) Camera.main != (Object) null)
        this.cam = Camera.main.transform;
      else
        Debug.LogWarning((object) "Warning: no main camera found. Ball needs a Camera tagged \"MainCamera\", for camera-relative controls.");
    }

    private void Update()
    {
      float axis1 = CrossPlatformInputManager.GetAxis("Horizontal");
      float axis2 = CrossPlatformInputManager.GetAxis("Vertical");
      this.jump = CrossPlatformInputManager.GetButton("Jump");
      if ((Object) this.cam != (Object) null)
      {
        this.camForward = Vector3.Scale(this.cam.forward, new Vector3(1f, 0.0f, 1f)).normalized;
        this.move = (axis2 * this.camForward + axis1 * this.cam.right).normalized;
      }
      else
        this.move = (axis2 * Vector3.forward + axis1 * Vector3.right).normalized;
    }

    private void FixedUpdate()
    {
      this.ball.Move(this.move, this.jump);
      this.jump = false;
    }
  }
}
