// Decompiled with JetBrains decompiler
// Type: Sound_Controller_RobotJackets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Sound_Controller_RobotJackets : MonoBehaviour
{
  private AudioManager mySoundPlayer;
  private Robot_RoboJackets myController;
  private Transform myBody;
  public Transform myLift;
  private Vector3 liftLastRelativePosition;

  private void Start()
  {
    this.mySoundPlayer = this.GetComponent<AudioManager>();
    this.myController = this.transform.GetComponent<Robot_RoboJackets>();
  }

  private void Update()
  {
    if (!(bool) (Object) this.myBody)
      this.myBody = this.transform.Find("Body");
    this.PlayFlippingSound();
    this.PlayCollectingSound();
    this.PlayLiftSound();
  }

  private void PlayLiftSound()
  {
    Vector3 vector3 = this.myBody.worldToLocalMatrix.MultiplyPoint(this.myLift.position);
    float magnitude = (vector3 - this.liftLastRelativePosition).magnitude;
    if ((double) magnitude > 1.0 / 1000.0 && (double) magnitude < 10.0)
    {
      if (!this.mySoundPlayer.SoundIsPlaying("Lift"))
        this.mySoundPlayer.Play("Lift", volume: 1f);
    }
    else if (this.mySoundPlayer.SoundIsPlaying("Lift"))
      this.mySoundPlayer.Stop("Lift", 0.1f);
    this.liftLastRelativePosition = vector3;
  }

  private void PlayCollectingSound()
  {
    if (this.myController.collecting_state == Robot_RoboJackets.collectingStates.onNormal || this.myController.collecting_state == Robot_RoboJackets.collectingStates.reverse)
    {
      if (this.mySoundPlayer.findSound("Collector").isPlaying)
        return;
      this.mySoundPlayer.Play("Collector");
    }
    else
    {
      if (!this.mySoundPlayer.findSound("Collector").isPlaying)
        return;
      this.mySoundPlayer.Stop("Collector", 0.1f);
    }
  }

  private void PlayFlippingSound()
  {
    if (this.myController.armstate == 1 && !this.mySoundPlayer.findSound("Flip").isPlaying)
    {
      this.mySoundPlayer.Play("Flip");
      this.mySoundPlayer.Play("Grip");
    }
    if (this.myController.armstate != 6 || this.mySoundPlayer.findSound("FlipReverse").isPlaying)
      return;
    this.mySoundPlayer.Play("FlipReverse");
    this.mySoundPlayer.Play("Grip");
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
