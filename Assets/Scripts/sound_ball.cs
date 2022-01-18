// Decompiled with JetBrains decompiler
// Type: sound_ball
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class sound_ball : MonoBehaviour
{
  private bool isInRobot;
  public float clipped_volume = 0.6f;
  public float min_speed = 0.5f;
  public float blanking_time = 0.2f;
  public float inside_robot_scale = 0.4f;
  public float sound_scale = 1f;
  private AudioManager sm_cached;
  private float time_of_update;

  private void OnTriggerEnter(Collider other)
  {
    if (!other.name.StartsWith("collisionBoundry"))
      return;
    this.isInRobot = true;
  }

  private void OnTriggerExit(Collider other)
  {
    if (!other.name.StartsWith("collisionBoundry"))
      return;
    this.isInRobot = false;
  }

  private void Start()
  {
    if ((bool) (Object) this.sm_cached)
      return;
    this.sm_cached = this.GetComponent<AudioManager>();
  }

  private void OnCollisionEnter(Collision other)
  {
    if ((double) other.relativeVelocity.magnitude <= (double) this.min_speed || (double) Time.time - (double) this.time_of_update <= (double) this.blanking_time)
      return;
    float num = this.isInRobot ? this.inside_robot_scale : 1f;
    Sound sound = this.sm_cached.findSound("ballhit2");
    if (sound.isPlaying)
      sound = this.sm_cached.findSound("ballhit3");
    float volume = other.relativeVelocity.magnitude * sound.get_init_volume() * num * this.sound_scale;
    if ((double) volume > (double) this.clipped_volume)
      volume = this.clipped_volume;
    this.sm_cached.Play(sound.name, volume: volume);
    this.time_of_update = Time.time;
  }
}
