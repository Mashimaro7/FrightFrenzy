// Decompiled with JetBrains decompiler
// Type: keyinfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;

public class keyinfo
{
  public string key;
  public string details;

  public keyinfo(string inkey, string info)
  {
    this.key = inkey;
    this.details = info;
  }

  public string GetString() => this.key + "$!" + this.details;

  public void FromString(string input)
  {
    string[] separator = new string[1]{ "$!" };
    string[] strArray = input.Split(separator, StringSplitOptions.None);
    if (strArray.Length < 2)
      return;
    this.key = strArray[0];
    this.details = strArray[1];
  }
}
