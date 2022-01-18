// Decompiled with JetBrains decompiler
// Type: RotateObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class RotateObject : MonoBehaviour
{
  public bool run;
  public string tag = "";
  public float speed = 1f;
  public float offspeed;
  public Transform centerpoint;
  public Vector3 axis = new Vector3(1f, 0.0f, 0.0f);
  private float previous_time;

  private void Start()
  {
  }

  private void Update()
  {
    float time = Time.time;
    Vector3 position = this.transform.position;
    if ((Object) this.centerpoint != (Object) null)
      position = this.centerpoint.position;
    float num = this.run ? this.speed : this.offspeed;
    if ((double) this.speed != 0.0)
      this.transform.RotateAround(position, this.transform.TransformDirection(this.axis), num * (time - this.previous_time));
    this.previous_time = time;
  }
}
