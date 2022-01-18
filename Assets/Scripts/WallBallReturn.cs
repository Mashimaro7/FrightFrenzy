// Decompiled with JetBrains decompiler
// Type: WallBallReturn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class WallBallReturn : MonoBehaviour
{
  private Dictionary<Rigidbody, float> all_bodies = new Dictionary<Rigidbody, float>();
  public float time_to_hold = 3f;
  public float velocity = 0.1f;

  private void OnTriggerEnter(Collider other)
  {
    Rigidbody attachedRigidbody = other.attachedRigidbody;
    if ((Object) attachedRigidbody == (Object) null || this.all_bodies.ContainsKey(other.attachedRigidbody) || (Object) other.GetComponentInParent<ball_data>() == (Object) null)
      return;
    this.all_bodies.Add(attachedRigidbody, Time.time);
    attachedRigidbody.velocity = Vector3.zero;
  }

  private void OnTriggerExit(Collider other)
  {
    Rigidbody attachedRigidbody = other.attachedRigidbody;
    if ((Object) attachedRigidbody == (Object) null || !this.all_bodies.ContainsKey(other.attachedRigidbody))
      return;
    this.all_bodies.Remove(attachedRigidbody);
  }

  private void FixedUpdate()
  {
    foreach (KeyValuePair<Rigidbody, float> allBody in this.all_bodies)
    {
      if ((double) Time.time - (double) allBody.Value < (double) this.time_to_hold)
      {
        allBody.Key.velocity = Vector3.zero;
      }
      else
      {
        Vector3 normalized = (new Vector3(0.0f, 1f, 0.0f) - allBody.Key.transform.position).normalized;
        allBody.Key.velocity = normalized * this.velocity;
      }
    }
  }
}
