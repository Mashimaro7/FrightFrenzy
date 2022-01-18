// Decompiled with JetBrains decompiler
// Type: Assets.OVR.Scripts.Record
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

namespace Assets.OVR.Scripts
{
  public class Record
  {
    public string category;
    public string message;

    public Record(string cat, string msg)
    {
      this.category = cat;
      this.message = msg;
    }
  }
}
