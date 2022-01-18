// Decompiled with JetBrains decompiler
// Type: FSP_ContinuousRotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FSP_ContinuousRotation : MonoBehaviour
{
  public float X;
  public float Y;
  public float Z;
  public bool local;

  private void Update()
  {
    if (!this.local)
      this.transform.Rotate(this.X * Time.deltaTime, this.Y * Time.deltaTime, this.Z * Time.deltaTime, Space.World);
    else
      this.transform.Rotate(this.X * Time.deltaTime, this.Y * Time.deltaTime, this.Z * Time.deltaTime);
  }
}
