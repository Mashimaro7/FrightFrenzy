// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Water.WaterBasic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Water
{
  [ExecuteInEditMode]
  public class WaterBasic : MonoBehaviour
  {
    private void Update()
    {
      Renderer component = this.GetComponent<Renderer>();
      if (!(bool) (Object) component)
        return;
      Material sharedMaterial = component.sharedMaterial;
      if (!(bool) (Object) sharedMaterial)
        return;
      Vector4 vector = sharedMaterial.GetVector("WaveSpeed");
      float num1 = sharedMaterial.GetFloat("_WaveScale");
      float num2 = Time.time / 20f;
      Vector4 vector4_1 = vector * (num2 * num1);
      Vector4 vector4_2 = new Vector4(Mathf.Repeat(vector4_1.x, 1f), Mathf.Repeat(vector4_1.y, 1f), Mathf.Repeat(vector4_1.z, 1f), Mathf.Repeat(vector4_1.w, 1f));
      sharedMaterial.SetVector("_WaveOffset", vector4_2);
    }
  }
}
