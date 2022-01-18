// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.CameraRefocus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Utility
{
  public class CameraRefocus
  {
    public Camera Camera;
    public Vector3 Lookatpoint;
    public Transform Parent;
    private Vector3 m_OrigCameraPos;
    private bool m_Refocus;

    public CameraRefocus(Camera camera, Transform parent, Vector3 origCameraPos)
    {
      this.m_OrigCameraPos = origCameraPos;
      this.Camera = camera;
      this.Parent = parent;
    }

    public void ChangeCamera(Camera camera) => this.Camera = camera;

    public void ChangeParent(Transform parent) => this.Parent = parent;

    public void GetFocusPoint()
    {
      RaycastHit hitInfo;
      if (Physics.Raycast(this.Parent.transform.position + this.m_OrigCameraPos, this.Parent.transform.forward, out hitInfo, 100f))
      {
        this.Lookatpoint = hitInfo.point;
        this.m_Refocus = true;
      }
      else
        this.m_Refocus = false;
    }

    public void SetFocusPoint()
    {
      if (!this.m_Refocus)
        return;
      this.Camera.transform.LookAt(this.Lookatpoint);
    }
  }
}
