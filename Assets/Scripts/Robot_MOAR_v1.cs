// Decompiled with JetBrains decompiler
// Type: Robot_MOAR_v1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Robot_MOAR_v1 : RobotInterface3D
{
  public ConfigurableJoint collector_slide_1;
  public ConfigurableJoint collector_slide_2;
  public ConfigurableJoint collector_slide_3;
  public HingeJoint collector_hinge;
  public HingeJoint collector_sweeper;
  public HingeJoint collector_sweeper2;
  public float collector_slide_startx;
  public float collector_slide_stopx = 1f;
  public float collector_slide_speed = 1f;
  public float collector_sweeper_speed = -1000f;
  public float collector_hinge_start;
  public float collector_hinge_down;
  public float collector_hinge_speed = 1f;
  public HingeJoint bucket_left_slide_1;
  public ConfigurableJoint bucket_left_slide_2;
  public ConfigurableJoint bucket_left_slide_3;
  public HingeJoint bucket_left_hinge;
  public HingeJoint bucket_right_slide_1;
  public ConfigurableJoint bucket_right_slide_2;
  public ConfigurableJoint bucket_right_slide_3;
  public HingeJoint bucket_right_hinge;
  public float bucket_slide_startx;
  public float bucket_slide_stopx = 1f;
  public float bucket_slide_speed = 1f;
  public float bucket_left_hinge_start;
  public float bucket_right_hinge_start;
  public float bucket_hinge_down;
  public float bucket_hinge_upmax;
  public float bucket_hinge_speed = 1f;
  public float bucket_tilt_down = 60f;
  public float bucket_tilt_up = -60f;
  public float bucket_col_tracking_slope = 1.5f;
  public float bucket_col_tracking_offset = 0.1578f;
  private bool init_done;
  private float turn_scale_original = -1f;
  private bool state_collector_down;
  private bool button_y;
  private Robot_MOAR_v1.sweeper_states sweeper_state = Robot_MOAR_v1.sweeper_states.OFF;
  private bool button_x;
  private bool button_a;

  public void Awake() => this.info = "<Missing Robot Specific Function: TBD>" + this.info;

  public override void Update_Robot()
  {
    if (!this.init_done)
    {
      this.init_done = true;
      this.turn_scale_original = this.turn_scale;
    }
    if (this.gamepad1_dpad_left)
    {
      Vector3 targetPosition = this.collector_slide_1.targetPosition;
      if ((double) targetPosition.x > (double) this.collector_slide_stopx)
      {
        targetPosition.x -= Time.deltaTime * this.collector_slide_speed;
        if ((double) targetPosition.x < (double) this.collector_slide_stopx)
          targetPosition.x = this.collector_slide_stopx;
        this.collector_slide_1.targetPosition = targetPosition;
        this.collector_slide_2.targetPosition = targetPosition * 2f;
        this.collector_slide_3.targetPosition = targetPosition * 3f;
      }
    }
    if (this.gamepad1_dpad_right)
    {
      Vector3 targetPosition = this.collector_slide_1.targetPosition;
      if ((double) targetPosition.x < 0.0)
      {
        targetPosition.x += Time.deltaTime * this.collector_slide_speed;
        if ((double) targetPosition.x > 0.0)
          targetPosition.x = 0.0f;
        this.collector_slide_1.targetPosition = targetPosition;
        this.collector_slide_2.targetPosition = targetPosition * 2f;
        this.collector_slide_3.targetPosition = targetPosition * 3f;
      }
    }
    if ((double) this.gamepad1_left_trigger > 0.1)
    {
      Vector3 targetPosition = this.bucket_left_slide_2.targetPosition with
      {
        x = this.MoveTowards(this.bucket_slide_startx, this.bucket_slide_stopx, this.bucket_left_slide_2.targetPosition.x, Time.deltaTime * this.bucket_slide_speed * this.gamepad1_left_trigger)
      };
      this.bucket_left_slide_2.targetPosition = targetPosition;
      this.bucket_left_slide_3.targetPosition = targetPosition * 2f;
    }
    if (this.gamepad1_left_bumper)
    {
      Vector3 targetPosition = this.bucket_left_slide_2.targetPosition with
      {
        x = this.MoveTowards(this.bucket_slide_stopx, this.bucket_slide_startx, this.bucket_left_slide_2.targetPosition.x, Time.deltaTime * this.bucket_slide_speed)
      };
      this.bucket_left_slide_2.targetPosition = targetPosition;
      this.bucket_left_slide_3.targetPosition = targetPosition * 2f;
    }
    if (this.gamepad1_dpad_up)
    {
      JointSpring spring = this.bucket_left_slide_1.spring;
      float num = this.MoveTowards(this.bucket_hinge_down + this.bucket_left_hinge_start, this.bucket_hinge_upmax + this.bucket_left_hinge_start, spring.targetPosition, Time.deltaTime * this.bucket_hinge_speed);
      spring.targetPosition = num;
      this.bucket_left_slide_1.spring = spring;
      this.bucket_left_hinge.spring = this.bucket_left_hinge.spring with
      {
        targetPosition = (float) (((double) num - (double) this.bucket_hinge_down) / ((double) this.bucket_hinge_upmax - (double) this.bucket_hinge_down) * ((double) this.bucket_tilt_up - (double) this.bucket_tilt_down)) + this.bucket_tilt_down
      };
    }
    if (this.gamepad1_dpad_down)
    {
      JointSpring spring = this.bucket_left_slide_1.spring;
      float num = this.MoveTowards(this.bucket_hinge_upmax + this.bucket_left_hinge_start, this.bucket_hinge_down + this.bucket_left_hinge_start, spring.targetPosition, Time.deltaTime * this.bucket_hinge_speed);
      spring.targetPosition = num;
      this.bucket_left_slide_1.spring = spring;
      this.bucket_left_hinge.spring = this.bucket_left_hinge.spring with
      {
        targetPosition = (float) (((double) num - (double) this.bucket_hinge_down) / ((double) this.bucket_hinge_upmax - (double) this.bucket_hinge_down) * ((double) this.bucket_tilt_up - (double) this.bucket_tilt_down)) + this.bucket_tilt_down
      };
    }
    if (this.gamepad1_x)
    {
      if (!this.button_x)
        this.sweeper_state = this.sweeper_state != Robot_MOAR_v1.sweeper_states.IN ? Robot_MOAR_v1.sweeper_states.IN : Robot_MOAR_v1.sweeper_states.OFF;
      this.button_x = true;
    }
    else
      this.button_x = false;
    if (this.gamepad1_a)
    {
      if (!this.button_a)
        this.sweeper_state = this.sweeper_state != Robot_MOAR_v1.sweeper_states.OUT ? Robot_MOAR_v1.sweeper_states.OUT : Robot_MOAR_v1.sweeper_states.OFF;
      this.button_a = true;
    }
    else
      this.button_a = false;
    JointMotor motor = this.collector_sweeper.motor;
    switch (this.sweeper_state)
    {
      case Robot_MOAR_v1.sweeper_states.IN:
        motor.targetVelocity = this.collector_sweeper_speed;
        break;
      case Robot_MOAR_v1.sweeper_states.OUT:
        motor.targetVelocity = -this.collector_sweeper_speed;
        break;
      default:
        motor.targetVelocity = 0.0f;
        break;
    }
    this.collector_sweeper.motor = motor;
    this.collector_sweeper2.motor = motor;
    if (this.gamepad1_b)
    {
      JointSpring spring = this.bucket_left_hinge.spring;
      spring.targetPosition = this.MoveTowards(spring.targetPosition, this.bucket_tilt_down, spring.targetPosition, (float) ((double) Time.deltaTime * (double) this.bucket_hinge_speed * 10.0));
      this.bucket_left_hinge.spring = spring;
    }
    if (this.gamepad1_y)
    {
      if (!this.button_y)
        this.state_collector_down = !this.state_collector_down;
      this.button_y = true;
    }
    else
      this.button_y = false;
    if (this.state_collector_down)
      this.collector_hinge.spring = this.collector_hinge.spring with
      {
        targetPosition = this.MoveTowards(this.collector_hinge_start, this.collector_hinge_down, this.collector_hinge.spring.targetPosition, Time.deltaTime * this.collector_hinge_speed)
      };
    if (!this.state_collector_down)
      this.collector_hinge.spring = this.collector_hinge.spring with
      {
        targetPosition = this.MoveTowards(this.collector_hinge_down, this.collector_hinge_start, this.collector_hinge.spring.targetPosition, Time.deltaTime * this.collector_hinge_speed)
      };
    if (((double) this.bucket_left_slide_1.spring.targetPosition - (double) this.bucket_hinge_down) / ((double) this.bucket_hinge_upmax - (double) this.bucket_hinge_down) < 0.3)
    {
      float stop = this.collector_slide_1.targetPosition.x * this.bucket_col_tracking_slope + this.bucket_col_tracking_offset;
      if (this.gamepad1_dpad_down)
        stop += 0.05f;
      Vector3 targetPosition = this.bucket_left_slide_2.targetPosition with
      {
        x = this.MoveTowards(this.bucket_left_slide_2.targetPosition.x, stop, this.bucket_left_slide_2.targetPosition.x, Time.deltaTime * this.bucket_slide_speed)
      };
      this.bucket_left_slide_2.targetPosition = targetPosition;
      this.bucket_left_slide_3.targetPosition = targetPosition * 2f;
    }
    this.turn_scale = this.turn_scale_original * (float) (((double) this.collector_slide_2.targetPosition.x + 1.79999995231628) / 1.79999995231628 * 0.660000026226044 + 0.330000013113022);
  }

  private enum sweeper_states
  {
    IN,
    OFF,
    OUT,
  }
}
