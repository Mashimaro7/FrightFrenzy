// Decompiled with JetBrains decompiler
// Type: DebugTransform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DebugTransform : MonoBehaviour
{
  public Vector3 onenable_pos;
  public Quaternion onenable_rot;
  public Vector3 reset_pos;
  public Quaternion reset_rot;
  public Vector3 start_pos;
  public Quaternion start_rot;
  public Vector3 fix_pos;
  public Quaternion fix_rot;
  public Vector3 curr_pos;
  public Quaternion curr_rot;

  private void OnEnable()
  {
    this.fix_pos = this.transform.position;
    this.fix_rot = this.transform.rotation;
  }

  private void Reset()
  {
    this.reset_pos = this.transform.position;
    this.reset_rot = this.transform.rotation;
  }

  public void FixVars()
  {
    this.onenable_pos = this.transform.position;
    this.onenable_rot = this.transform.rotation;
  }

  private void Start()
  {
    this.start_pos = this.transform.position;
    this.start_rot = this.transform.rotation;
  }

  private void Update()
  {
    this.curr_pos = this.transform.position;
    this.curr_rot = this.transform.rotation;
    int num = this.curr_pos != this.start_pos ? 1 : 0;
  }
}
