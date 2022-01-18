// Decompiled with JetBrains decompiler
// Type: Sound_Controller_Movement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Sound_Controller_Movement : MonoBehaviour
{
  public Sound driveSound;
  public float master_volume = 0.2f;
  private AudioManager mySoundPlayer;
  private RobotInterface3D myRI3D;
  private bool wasPlayingForwardsSoundLast;
  public float targetVelocity;
  public float currVelocity;

  private void Start()
  {
    this.mySoundPlayer = this.GetComponentInParent<AudioManager>();
    if (!(bool) (Object) this.mySoundPlayer)
      this.mySoundPlayer = this.GetComponentInChildren<AudioManager>();
    this.myRI3D = this.GetComponentInParent<RobotInterface3D>();
    if (!(bool) (Object) this.myRI3D)
      this.myRI3D = this.GetComponentInChildren<RobotInterface3D>();
    if (!(bool) (Object) this.mySoundPlayer || !(bool) (Object) this.myRI3D)
      return;
    AudioClip audioClip = Resources.Load<AudioClip>("Sounds/SampledSounds/MovementStraif_loop");
    this.driveSound = new Sound();
    this.driveSound.name = "move_strafe";
    this.driveSound._clip = audioClip;
    this.driveSound._volume = this.master_volume;
    this.driveSound._pitch = 1f;
    this.driveSound._spatialBlend = 1f;
    this.driveSound._loop = false;
    this.driveSound.sourceLocation = this.myRI3D.transform.Find("Body");
    this.driveSound.Init(this.mySoundPlayer);
    this.mySoundPlayer.AddSound(this.driveSound);
  }

  private void Update()
  {
    if (!(bool) (Object) this.myRI3D || this.driveSound == null)
      return;
    this.PlayMovementSound();
  }

  private void PlayMovementSound()
  {
    this.targetVelocity = this.myRI3D.getAverageMovementForceMagnitude();
    this.currVelocity = this.myRI3D.getAveragVelocityMagnitude();
    if ((double) this.currVelocity > 10.0)
      return;
    if ((double) this.currVelocity > 0.100000001490116)
    {
      if (!this.wasPlayingForwardsSoundLast)
      {
        this.mySoundPlayer.Play(this.driveSound.name, server_request: true);
        if ((double) this.targetVelocity > 0.850000023841858)
        {
          this.driveSound.time = 0.0f;
        }
        else
        {
          this.driveSound.time = 5f;
          this.driveSound._pitch = 0.8f;
        }
      }
      this.wasPlayingForwardsSoundLast = true;
      this.driveSound._volume = (double) this.currVelocity < 1.0 ? this.currVelocity * this.master_volume : this.master_volume;
    }
    else
    {
      if (this.wasPlayingForwardsSoundLast)
        this.mySoundPlayer.Stop(this.driveSound.name, 0.1f, true);
      this.wasPlayingForwardsSoundLast = false;
    }
    this.driveSound._pitch = (float) ((double) this.driveSound._pitch * 0.899999976158142 + 0.100000001490116 * (0.349999994039536 + (double) this.currVelocity * 0.649999976158142));
    if ((double) this.driveSound._pitch <= 1.0)
      return;
    this.driveSound._pitch = 1f;
  }

  private float AngleWrapDeg(float degrees)
  {
    float num = degrees % 360f;
    if ((double) num > 180.0)
      num -= 360f;
    return num;
  }

  private float AngleWrapRad(float rad)
  {
    float num = rad % 6.283185f;
    if ((double) num > 3.14159274101257)
      num -= 6.283185f;
    return num;
  }
}
