// Decompiled with JetBrains decompiler
// Type: OVRWaitCursor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class OVRWaitCursor : MonoBehaviour
{
  public Vector3 rotateSpeeds = new Vector3(0.0f, 0.0f, -60f);

  private void Update() => this.transform.Rotate(this.rotateSpeeds * Time.smoothDeltaTime);
}
