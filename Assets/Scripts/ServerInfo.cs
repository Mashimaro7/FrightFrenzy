// Decompiled with JetBrains decompiler
// Type: ServerInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ServerInfo
{
  public string IP = "0.0.0.0";
  public int PORT = 1446;
  public string GAME = "Unknown";
  public float PING = 9.999f;
  public int PLAYERS;
  public int MAXPLAYERS;
  public string VERSION = "0.0";
  public string COMMENT = "";
  public string PASSWORD = "0";
  public GameObject gui_line;
  public Ping pingobject;
}
