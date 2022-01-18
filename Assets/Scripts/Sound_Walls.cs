// Decompiled with JetBrains decompiler
// Type: Sound_Walls
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Sound_Walls : MonoBehaviour
{
  private float lastTime;
  public float volume = 0.25f;
  public float clipped_volume = 0.6f;
  public float min_speed = 0.5f;
  public float blanking_time = 0.2f;
  private AudioManager myaudiomanager;
  private float time_of_update;

  private void Start() => this.myaudiomanager = this.transform.GetComponent<AudioManager>();

  public void OnCollisionEnter(Collision collision)
  {
    double magnitude = (double) collision.relativeVelocity.magnitude;
    if ((double) collision.relativeVelocity.magnitude <= (double) this.min_speed || (double) Time.time - (double) this.time_of_update <= (double) this.blanking_time)
      return;
    Sound sound = this.myaudiomanager.findSound("hit1");
    float volume = (collision.relativeVelocity.magnitude - this.min_speed) * this.volume;
    if ((double) volume > (double) this.clipped_volume)
      volume = this.clipped_volume;
    this.myaudiomanager.Play(sound.name, volume: volume);
    this.time_of_update = Time.time;
  }
}
