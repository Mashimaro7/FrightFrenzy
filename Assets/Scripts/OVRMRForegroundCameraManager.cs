// Decompiled with JetBrains decompiler
// Type: OVRMRForegroundCameraManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

internal class OVRMRForegroundCameraManager : MonoBehaviour
{
  public GameObject clipPlaneGameObj;
  private Material clipPlaneMaterial;

  private void OnPreRender()
  {
    if (!(bool) (Object) this.clipPlaneGameObj)
      return;
    if ((Object) this.clipPlaneMaterial == (Object) null)
      this.clipPlaneMaterial = this.clipPlaneGameObj.GetComponent<MeshRenderer>().material;
    this.clipPlaneGameObj.GetComponent<MeshRenderer>().material.SetFloat("_Visible", 1f);
  }

  private void OnPostRender()
  {
    if (!(bool) (Object) this.clipPlaneGameObj)
      return;
    this.clipPlaneGameObj.GetComponent<MeshRenderer>().material.SetFloat("_Visible", 0.0f);
  }
}
