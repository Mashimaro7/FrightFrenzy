// Decompiled with JetBrains decompiler
// Type: Robot_dual_meta
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Robot_dual_meta : RobotInterface3D
{
  public HingeJoint bucket1;
  public HingeJoint collector1_hinge;
  public PossessionTracker detector1;
  public HingeJoint bucket2;
  public HingeJoint collector2_hinge;
  public PossessionTracker detector2;
  public float bucket_down;
  public float bucket_up = 90f;
  public float bucket_speed = 200f;
  public float intake_speed = -1400f;
  public ConfigurableJoint slide_last;
  public Transform slide2;
  public Transform slide1;
  public Transform slide0;
  public float slide_max = 1.6f;
  public float slide_min;
  public float slide_speed = 1f;
  public HingeJoint out_bucket;
  public float out_bucket_down;
  public float out_bucket_up = 40f;
  public float out_bucket_speed = 200f;
  public HingeJoint flap1;
  public HingeJoint flap2;
  public float flap_down;
  public float flap_up = 90f;
  public float flap_speed = 200f;
  public HingeJoint floor;
  public float floor_up;
  public float floor_down = 60f;
  public float floor_speed = 400f;
  public HingeJoint platform_wheel;
  public float platform_wheel_speed = 1800f;
  private AudioManager audio_manager;
  private Robot_dual_meta.intakeStates collector1_statemachine;
  private Robot_dual_meta.intakeStates collector2_statemachine;
  private Robot_dual_meta.bucketStates bucket1_statemachine = Robot_dual_meta.bucketStates.movingDown;
  private Robot_dual_meta.bucketStates bucket2_statemachine = Robot_dual_meta.bucketStates.movingDown;
  private bool doing_reset;
  private bool force_slide_down;

  public void Awake() => this.info = "Gamepad Up/Down: Move Arm up/down when deployed\nGamepad Left/Right: Rotate depositor when arm is extended. Useful for level 1/2 in team hub.\nButton A: Open depositor floor (while held)\nButton B: Right intake control. Hold to reverse and rotate up. Press to toggle on/off. Will auto rotate up when freight detected.\nButton Y: While held, opens depositor floor (same as Button A). When released, auto-retract arm.\nButton X: Leftt intake control. Hold to reverse and rotate up. Press to toggle on/off. Will auto rotate up when freight detected.\nLeft trigger: Rotate platform wheel counter-clockwise.\nRight trigger: Rotate platform wheel clockwise." + this.info;

  public override void Start() => base.Start();

  public override void Init_Robot() => this.audio_manager = this.GetComponentInChildren<AudioManager>();

  public override void Update_Robot()
  {
    this.DoAnimations(this.isKinematic);
    if (this.isKinematic)
      return;
    this.force_slide_down = false;
    this.ProcessIntake(this.gamepad1_x, this.gamepad1_x_changed, ref this.collector1_statemachine, this.collector1_hinge, this.bucket1, ref this.bucket1_statemachine, this.detector1, this.flap1, ref this.collector2_statemachine);
    this.ProcessIntake(this.gamepad1_b, this.gamepad1_b_changed, ref this.collector2_statemachine, this.collector2_hinge, this.bucket2, ref this.bucket2_statemachine, this.detector2, this.flap2, ref this.collector1_statemachine);
    if (this.gamepad1_y_changed && !this.gamepad1_y)
    {
      this.bucket1_statemachine = Robot_dual_meta.bucketStates.movingDown;
      this.bucket2_statemachine = Robot_dual_meta.bucketStates.movingDown;
      this.doing_reset = true;
    }
    if (this.doing_reset)
    {
      this.force_slide_down = true;
      if (this.gamepad1_dpad_up)
        this.doing_reset = false;
    }
    if (this.gamepad1_dpad_down || this.force_slide_down)
    {
      if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("arm"))
        this.audio_manager.Play("arm", 0.5f);
      if ((double) this.MoveSlide(this.slide_last, RobotInterface3D.Axis.x, this.slide_min, this.slide_speed * this.turning_scaler) == (double) this.slide_min)
        this.doing_reset = false;
    }
    else if (this.gamepad1_dpad_up)
    {
      if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("arm"))
        this.audio_manager.Play("arm", 0.5f);
      double num1 = (double) this.MoveSlide(this.slide_last, RobotInterface3D.Axis.x, this.slide_max, this.slide_speed * this.turning_scaler);
      double num2 = (double) this.MoveHinge(this.flap1, this.flap_down, this.flap_speed);
      double num3 = (double) this.MoveHinge(this.flap2, this.flap_down, this.flap_speed);
    }
    else if ((bool) (Object) this.audio_manager && this.audio_manager.IsSoundStarted("arm"))
      this.audio_manager.Stop("arm", 0.5f);
    if ((this.gamepad1_a || this.gamepad1_y) && (double) this.slide_last.targetPosition.x > ((double) this.slide_max - (double) this.slide_min) * 0.100000001490116 + (double) this.slide_min)
    {
      double num4 = (double) this.MoveHinge(this.floor, this.floor_down - this.out_bucket.spring.targetPosition, this.floor_speed);
    }
    else
    {
      double num5 = (double) this.MoveHinge(this.floor, this.floor_up, this.floor_speed);
    }
    if (!this.doing_reset && this.gamepad1_dpad_right && (double) this.slide_last.targetPosition.x > 0.349999994039536)
    {
      if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("bucket"))
        this.audio_manager.Play("arm", 0.5f);
      double num6 = (double) this.MoveHinge(this.out_bucket, this.out_bucket_up, this.out_bucket_speed);
    }
    else if (this.gamepad1_dpad_left)
    {
      if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("bucket"))
        this.audio_manager.Play("arm", 0.5f);
      double num7 = (double) this.MoveHinge(this.out_bucket, this.out_bucket_down, this.out_bucket_speed);
    }
    else
    {
      if ((bool) (Object) this.audio_manager && this.audio_manager.IsSoundStarted("arm"))
        this.audio_manager.Stop("bucket", 0.5f);
      if (this.doing_reset || (double) this.slide_last.targetPosition.x < 0.349999994039536)
      {
        double num8 = (double) this.MoveHinge(this.out_bucket, this.out_bucket_down, 2f * this.out_bucket_speed);
      }
    }
    bool flag = false;
    if ((double) this.gamepad1_left_trigger > 0.0)
    {
      this.platform_wheel.motor = this.platform_wheel.motor with
      {
        targetVelocity = -1f * this.gamepad1_left_trigger * this.platform_wheel_speed * this.turning_scaler
      };
      flag = true;
    }
    else if ((double) this.gamepad1_right_trigger > 0.0)
    {
      this.platform_wheel.motor = this.platform_wheel.motor with
      {
        targetVelocity = this.gamepad1_right_trigger * this.platform_wheel_speed * this.turning_scaler
      };
      flag = true;
    }
    else
      this.platform_wheel.motor = this.platform_wheel.motor with
      {
        targetVelocity = 0.0f
      };
    if (flag)
    {
      if (!(bool) (Object) this.audio_manager || this.audio_manager.IsSoundStarted("wheels"))
        return;
      this.audio_manager.Play("wheels", 0.5f);
    }
    else
    {
      if (!(bool) (Object) this.audio_manager || !this.audio_manager.IsSoundStarted("wheels"))
        return;
      this.audio_manager.Stop("wheels", 0.2f);
    }
  }

  private void ProcessIntake(
    bool button,
    bool button_changed,
    ref Robot_dual_meta.intakeStates statemachine,
    HingeJoint my_hinge,
    HingeJoint bucket,
    ref Robot_dual_meta.bucketStates bucketstate,
    PossessionTracker detector,
    HingeJoint flap,
    ref Robot_dual_meta.intakeStates other_statemachine)
  {
    if (button_changed & button)
    {
      switch (statemachine)
      {
        case Robot_dual_meta.intakeStates.off:
          statemachine = Robot_dual_meta.intakeStates.on;
          other_statemachine = Robot_dual_meta.intakeStates.off;
          break;
        case Robot_dual_meta.intakeStates.on:
          statemachine = Robot_dual_meta.intakeStates.off;
          break;
      }
    }
    JointMotor motor = my_hinge.motor;
    if (button)
    {
      float x = this.slide_last.targetPosition.x;
      if ((double) bucket.spring.targetPosition < ((double) this.bucket_up - (double) this.bucket_down) / 2.0 + (double) this.bucket_down || (double) x <= (double) this.slide_min)
      {
        if ((double) motor.targetVelocity != -1.0 * (double) this.intake_speed)
        {
          motor.targetVelocity = -1f * this.intake_speed;
          my_hinge.motor = motor;
          my_hinge.useMotor = true;
          if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("intake"))
            this.audio_manager.Play("intake", 0.3f);
        }
      }
      else
        this.force_slide_down = true;
      double num = (double) this.MoveHinge(flap, this.flap_up, this.flap_speed);
    }
    else if (statemachine == Robot_dual_meta.intakeStates.on)
    {
      if ((double) motor.targetVelocity != (double) this.intake_speed)
      {
        motor.targetVelocity = this.intake_speed;
        my_hinge.motor = motor;
        my_hinge.useMotor = true;
        if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("intake"))
          this.audio_manager.Play("intake", 0.3f);
      }
    }
    else
    {
      if ((double) motor.targetVelocity != 0.0)
      {
        motor.targetVelocity = 0.0f;
        JointSpring spring = my_hinge.spring with
        {
          targetPosition = my_hinge.angle
        };
        my_hinge.spring = spring;
        my_hinge.motor = motor;
        my_hinge.useMotor = false;
        if ((bool) (Object) this.audio_manager && this.audio_manager.IsSoundStarted("intake"))
          this.audio_manager.Stop("intake", 0.3f);
      }
      double num = (double) this.MoveHinge(flap, this.flap_down, this.flap_speed);
    }
    if (!button)
    {
      switch (bucketstate)
      {
        case Robot_dual_meta.bucketStates.down:
          double num1 = (double) this.MoveHinge(bucket, this.bucket_down, this.bucket_speed);
          if (!detector.IsAnyGameElementInside())
            break;
          bucketstate = Robot_dual_meta.bucketStates.movingUp;
          break;
        case Robot_dual_meta.bucketStates.movingUp:
          if ((double) this.MoveHinge(bucket, this.bucket_up, this.bucket_speed) != (double) this.bucket_up)
            break;
          bucketstate = Robot_dual_meta.bucketStates.up;
          break;
        case Robot_dual_meta.bucketStates.up:
          if (!button_changed || button)
            break;
          bucketstate = Robot_dual_meta.bucketStates.movingDown;
          break;
        case Robot_dual_meta.bucketStates.movingDown:
          if ((double) this.MoveHinge(bucket, this.bucket_down, this.bucket_speed) != (double) this.bucket_down)
            break;
          bucketstate = Robot_dual_meta.bucketStates.down;
          break;
      }
    }
    else
    {
      double num2 = (double) this.MoveHinge(bucket, this.bucket_up, this.bucket_speed / 2f);
    }
  }

  public void DoAnimations(bool client_mode = false)
  {
    this.slide2.position = this.slide0.position + 0.6666667f * (this.slide_last.transform.position - this.slide0.position);
    this.slide1.position = this.slide0.position + 0.3333333f * (this.slide_last.transform.position - this.slide0.position);
  }

  private enum intakeStates
  {
    off,
    on,
  }

  private enum bucketStates
  {
    down,
    movingUp,
    up,
    movingDown,
  }
}
