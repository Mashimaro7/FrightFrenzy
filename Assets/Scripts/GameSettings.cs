// Decompiled with JetBrains decompiler
// Type: GameSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class GameSettings : MonoBehaviour
{
  public GameObject info;

  public void UpdateServer()
  {
    if (GLOBALS.CLIENT_MODE && (bool) (Object) GLOBALS.topclient)
      GLOBALS.topclient.ChangeGameSettings(this.GetString());
    if (!GLOBALS.SERVER_MODE || !(bool) (Object) GLOBALS.topserver)
      return;
    GLOBALS.topserver.ChangeGameSettings(this.GetString());
  }

  public virtual void SetString(string data)
  {
  }

  public virtual string GetString() => "";

  public virtual void Init()
  {
  }

  public virtual string GetCleanString() => this.GetString();
}
