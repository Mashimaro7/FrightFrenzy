// Decompiled with JetBrains decompiler
// Type: RR_Rule1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class RR_Rule1 : MonoBehaviour
{
  public static bool enable_rules;
  public List<gameElement> collisions = new List<gameElement>();
  public RobotID robotid;
  private MeshRenderer mesh;
  public Transform goal;
  private bool redpos = true;

  private void Update()
  {
    if (!RR_Rule1.enable_rules)
      return;
    if ((UnityEngine.Object) this.mesh == (UnityEngine.Object) null)
      this.mesh = this.GetComponent<MeshRenderer>();
    if ((UnityEngine.Object) this.robotid == (UnityEngine.Object) null)
    {
      this.mesh.enabled = false;
    }
    else
    {
      this.mesh.enabled = true;
      this.redpos = !(this.robotid.starting_pos == "Blue Front") && !(this.robotid.starting_pos == "Blue Back");
      this.transform.localScale = this.transform.localScale with
      {
        x = 0.15f,
        z = 0.075f
      };
      this.transform.position = ((this.goal.position + this.robotid.position) / 2f) with
      {
        y = this.robotid.position.y
      };
      this.transform.position = this.robotid.position;
      float num1 = Vector3.SignedAngle(new Vector3(1f, 0.0f, 0.0f), (this.goal.position - this.robotid.position) with
      {
        y = 0.0f
      }, new Vector3(0.0f, 1f, 0.0f));
      this.transform.eulerAngles = this.transform.eulerAngles with
      {
        y = num1
      };
      float num2 = (float) (DateTime.Now.Ticks / 10000L % 2000L);
      if ((double) num2 <= 1000.0)
        return;
      float num3 = 2000f - num2;
    }
  }

  private gameElement FindGameElement(Collider collision)
  {
    Transform transform = collision.transform;
    return (gameElement) null;
  }

  private void OnTriggerEnter(Collider collision)
  {
    int num = (UnityEngine.Object) this.robotid == (UnityEngine.Object) null ? 1 : 0;
  }

  private void OnTriggerExit(Collider collision)
  {
    int num = (UnityEngine.Object) this.robotid == (UnityEngine.Object) null ? 1 : 0;
  }

  public int GetElementCount() => this.collisions.Count;
}
