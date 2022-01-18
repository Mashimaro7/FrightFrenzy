// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.SceneUtils.PlaceTargetWithMouse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.SceneUtils
{
  public class PlaceTargetWithMouse : MonoBehaviour
  {
    public float surfaceOffset = 1.5f;
    public GameObject setTargetOn;

    private void Update()
    {
      RaycastHit hitInfo;
      if (!Input.GetMouseButtonDown(0) || !Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
        return;
      this.transform.position = hitInfo.point + hitInfo.normal * this.surfaceOffset;
      if (!((Object) this.setTargetOn != (Object) null))
        return;
      this.setTargetOn.SendMessage("SetTarget", (object) this.transform);
    }
  }
}
