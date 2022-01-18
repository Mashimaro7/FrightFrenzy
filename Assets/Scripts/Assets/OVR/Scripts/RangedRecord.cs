// Decompiled with JetBrains decompiler
// Type: Assets.OVR.Scripts.RangedRecord
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

namespace Assets.OVR.Scripts
{
  public class RangedRecord : Record
  {
    public float value;
    public float min;
    public float max;

    public RangedRecord(string cat, string msg, float val, float minVal, float maxVal)
      : base(cat, msg)
    {
      this.value = val;
      this.min = minVal;
      this.max = maxVal;
    }
  }
}
