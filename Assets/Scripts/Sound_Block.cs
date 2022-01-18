// Decompiled with JetBrains decompiler
// Type: Sound_Block
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Sound_Block : MonoBehaviour
{
  public float volume = 0.3f;
  private float lastTime;

  private void OnCollisionEnter(Collision collision)
  {
    float magnitude = collision.relativeVelocity.magnitude;
    bool flag = true;
    if ((double) Time.time - (double) this.lastTime < 0.2)
      return;
    if ((double) magnitude > 2.0)
    {
      this.transform.GetComponent<AudioManager>().Play("hit4");
      this.transform.GetComponent<AudioManager>().findSound("hit4")._volume = magnitude * this.volume;
    }
    else if ((double) magnitude > 1.6)
    {
      this.transform.GetComponent<AudioManager>().Play("hit3");
      this.transform.GetComponent<AudioManager>().findSound("hit3")._volume = magnitude * this.volume;
    }
    else if ((double) magnitude > 1.2)
    {
      this.transform.GetComponent<AudioManager>().Play("hit2");
      this.transform.GetComponent<AudioManager>().findSound("hit2")._volume = magnitude * this.volume;
    }
    else if ((double) magnitude > 0.8)
    {
      this.transform.GetComponent<AudioManager>().Play("hit1");
      this.transform.GetComponent<AudioManager>().findSound("hit1")._volume = magnitude * this.volume;
    }
    else
      flag = false;
    if (!flag)
      return;
    this.lastTime = Time.time;
  }
}
