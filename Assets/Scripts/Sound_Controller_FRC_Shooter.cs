// Decompiled with JetBrains decompiler
// Type: Sound_Controller_FRC_Shooter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Sound_Controller_FRC_Shooter : MonoBehaviour
{
  public AudioManager mySoundPlayer;
  private Sound_Controller_FRC_Shooter.shooterStates myShooterState;
  private bool shooterStateFinished;

  private void Start() => this.mySoundPlayer = this.GetComponent<AudioManager>();

  private void Update() => this.updateShooterSoundStateMachine();

  public void revup() => this.nextShooterState(1);

  public void revdown() => this.nextShooterState(0);

  private void nextShooterState() => this.nextShooterState((int) (this.myShooterState + 1));

  private void nextShooterState(int number)
  {
    this.myShooterState = (Sound_Controller_FRC_Shooter.shooterStates) number;
    this.shooterStateFinished = true;
  }

  private void updateShooterSoundStateMachine()
  {
    if (this.myShooterState == Sound_Controller_FRC_Shooter.shooterStates.off && this.shooterStateFinished)
    {
      this.shooterStateFinished = false;
      this.mySoundPlayer.Stop("shooter_hover", 0.0f);
    }
    if (this.myShooterState != Sound_Controller_FRC_Shooter.shooterStates.hovering || !this.shooterStateFinished)
      return;
    this.shooterStateFinished = false;
    this.mySoundPlayer.Play("shooter_hover");
  }

  private enum shooterStates
  {
    off,
    hovering,
  }
}
