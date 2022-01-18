// Decompiled with JetBrains decompiler
// Type: Robot_Bailey
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class Robot_Bailey : RobotInterface3D
{
  private bool init_done;
  [Header("Bot float settings")]
  public float intake_speed = -750f;
  public float forearm_min = -90f;
  public float forearm_deployed;
  public float forearm_max = 50f;
  public float forearm_speed_start = 300f;
  public float forearm_speed_level = 100f;
  public float bucket_min = 5f;
  public float bucket_max = 35f;
  public float bucket_speed = 75f;
  [Header("Bot Linked Objects")]
  public HingeJoint intake_collector;
  public HingeJoint forearm;
  public HingeJoint forearm2;
  public HingeJoint hand;
  public HingeJoint finger_l;
  public HingeJoint finger_r;
  public float finger_open;
  public float finger_closed = -20f;
  public float finger_speed = 80f;
  public float[,] forearm2_map = new float[3, 2]
  {
    {
      -90f,
      0.0f
    },
    {
      0.0f,
      -140f
    },
    {
      50f,
      -35f
    }
  };
  public float[,] hand_map = new float[5, 2]
  {
    {
      -90f,
      88f
    },
    {
      -75f,
      100f
    },
    {
      0.0f,
      -5f
    },
    {
      30f,
      -100f
    },
    {
      50f,
      -155f
    }
  };
  public HingeJoint bucket;
  [Header("Animation Aid")]
  public Transform intake_spinner1_zaxis;
  private AudioManager audio_manager;
  private Robot_Bailey.intakeStates intake_statemachine;
  private Robot_Bailey.intakeStates old_intake_statemachine;
  private Robot_Bailey.armStates arm_state = Robot_Bailey.armStates.opening;
  private Robot_Bailey.bucketStates bucket_state = Robot_Bailey.bucketStates.rising;
  public double delta_angle_display;
  private bool sound_shooting;

  public void Awake() => this.info = "Gamepad Up/Down: Move Arm up/down when deployed\nGamepad Left/Right: retract/deploy arm.\nButton A: Toggle intake\nButton B: open fingers (while held)\nButton Y: Toggle reverse intake\nButton X: Raise/Lower bucket. Lower bucket to collect freight. Raise bucket for proper hand grab." + this.info;

  public override void Start() => base.Start();

  public override void RobotFixedUpdate()
  {
    if (!(bool) (UnityEngine.Object) this.rb_body || !(bool) (UnityEngine.Object) this.myRobotID || !this.init_done)
      return;
    int num = this.isKinematic ? 1 : 0;
  }

  public override void Update_Robot()
  {
    if (!this.init_done)
    {
      this.audio_manager = this.GetComponentInChildren<AudioManager>();
      this.init_done = true;
    }
    this.DoAnimations(this.isKinematic);
    if (this.isKinematic)
      return;
    bool flag1 = false;
    switch (this.arm_state)
    {
      case Robot_Bailey.armStates.closing:
        if ((double) this.MoveHinge(this.finger_l, this.finger_closed, this.finger_speed) == (double) this.finger_closed & (double) this.MoveHinge(this.finger_r, -1f * this.finger_closed, this.finger_speed) == -1.0 * (double) this.finger_closed)
          this.arm_state = Robot_Bailey.armStates.deploying;
        if ((bool) (UnityEngine.Object) this.audio_manager && !this.audio_manager.IsSoundStarted("arm"))
          this.audio_manager.Play("arm", 0.5f);
        flag1 = true;
        break;
      case Robot_Bailey.armStates.deploying:
        if (this.MoveArm(this.forearm_deployed))
          this.arm_state = Robot_Bailey.armStates.deployed;
        flag1 = true;
        break;
      case Robot_Bailey.armStates.deployed:
        if (this.gamepad1_dpad_down)
        {
          this.MoveArm(this.forearm_max, this.forearm_speed_level);
          flag1 = true;
          break;
        }
        if (this.gamepad1_dpad_up)
        {
          this.MoveArm(this.forearm_deployed, this.forearm_speed_level);
          flag1 = true;
          break;
        }
        break;
      case Robot_Bailey.armStates.opening:
        if ((double) this.MoveHinge(this.finger_l, this.finger_open, this.finger_speed) == (double) this.finger_open & (double) this.MoveHinge(this.finger_r, -1f * this.finger_open, this.finger_speed) == -1.0 * (double) this.finger_open)
          this.arm_state = Robot_Bailey.armStates.returning;
        if ((bool) (UnityEngine.Object) this.audio_manager && !this.audio_manager.IsSoundStarted("arm"))
          this.audio_manager.Play("arm", 0.5f);
        flag1 = true;
        break;
      case Robot_Bailey.armStates.returning:
        double num1 = (double) this.MoveHinge(this.finger_l, this.finger_open, this.finger_speed);
        double num2 = (double) this.MoveHinge(this.finger_r, -1f * this.finger_open, this.finger_speed);
        if (this.MoveArm(this.forearm_min))
          this.arm_state = Robot_Bailey.armStates.home;
        flag1 = true;
        break;
    }
    if (this.gamepad1_dpad_right)
      this.arm_state = Robot_Bailey.armStates.opening;
    if (this.gamepad1_dpad_left)
      this.arm_state = Robot_Bailey.armStates.closing;
    if (this.gamepad1_b)
    {
      double num3 = (double) this.MoveHinge(this.finger_l, 2f * this.finger_open, this.finger_speed);
      double num4 = (double) this.MoveHinge(this.finger_r, -2f * this.finger_open, this.finger_speed);
      if ((bool) (UnityEngine.Object) this.audio_manager && !this.audio_manager.IsSoundStarted("arm"))
        this.audio_manager.Play("arm", 0.5f);
      flag1 = true;
    }
    if (!flag1 && (bool) (UnityEngine.Object) this.audio_manager && this.audio_manager.IsSoundStarted("arm"))
      this.audio_manager.Stop("arm", 0.5f);
    bool flag2 = false;
    switch (this.bucket_state)
    {
      case Robot_Bailey.bucketStates.rising:
        if ((double) this.MoveHinge(this.bucket, this.bucket_max, this.bucket_speed) == (double) this.bucket_max)
          this.bucket_state = Robot_Bailey.bucketStates.up;
        flag2 = true;
        if ((bool) (UnityEngine.Object) this.audio_manager && !this.audio_manager.IsSoundStarted("bucket"))
        {
          this.audio_manager.Play("bucket", 0.5f);
          break;
        }
        break;
      case Robot_Bailey.bucketStates.falling:
        if ((double) this.MoveHinge(this.bucket, this.bucket_min, this.bucket_speed) == (double) this.bucket_min)
          this.bucket_state = Robot_Bailey.bucketStates.down;
        flag2 = true;
        if ((bool) (UnityEngine.Object) this.audio_manager && !this.audio_manager.IsSoundStarted("bucket"))
        {
          this.audio_manager.Play("bucket", 0.5f);
          break;
        }
        break;
    }
    if (this.gamepad1_x_changed && this.gamepad1_x)
      this.bucket_state = this.bucket_state == Robot_Bailey.bucketStates.down || this.bucket_state == Robot_Bailey.bucketStates.falling ? Robot_Bailey.bucketStates.rising : Robot_Bailey.bucketStates.falling;
    if (!flag2 && (bool) (UnityEngine.Object) this.audio_manager && this.audio_manager.IsSoundStarted("bucket"))
      this.audio_manager.Stop("bucket", 0.5f);
    if (this.intake_statemachine == Robot_Bailey.intakeStates.on)
    {
      JointMotor motor = this.intake_collector.motor;
      if ((double) motor.targetVelocity != (double) this.intake_speed)
      {
        motor.targetVelocity = this.intake_speed;
        this.intake_collector.motor = motor;
        this.intake_collector.useMotor = true;
        if ((bool) (UnityEngine.Object) this.audio_manager && !this.audio_manager.IsSoundStarted("intake"))
          this.audio_manager.Play("intake", 0.3f);
      }
    }
    if (this.intake_statemachine == Robot_Bailey.intakeStates.reverse)
    {
      JointMotor motor = this.intake_collector.motor;
      if ((double) motor.targetVelocity != -1.0 * (double) this.intake_speed)
      {
        motor.targetVelocity = -1f * this.intake_speed;
        this.intake_collector.motor = motor;
        this.intake_collector.useMotor = true;
        if ((bool) (UnityEngine.Object) this.audio_manager && !this.audio_manager.IsSoundStarted("intake"))
          this.audio_manager.Play("intake", 0.3f);
      }
    }
    if (this.intake_statemachine == Robot_Bailey.intakeStates.off)
    {
      JointMotor motor = this.intake_collector.motor;
      if ((double) motor.targetVelocity != 0.0)
      {
        motor.targetVelocity = 0.0f;
        this.intake_collector.spring = this.intake_collector.spring with
        {
          targetPosition = this.intake_collector.angle
        };
        this.intake_collector.motor = motor;
        this.intake_collector.useMotor = false;
        if ((bool) (UnityEngine.Object) this.audio_manager && this.audio_manager.IsSoundStarted("intake"))
          this.audio_manager.Stop("intake", 0.3f);
      }
    }
    if (this.gamepad1_a_changed && this.gamepad1_a)
      this.intake_statemachine = this.intake_statemachine != Robot_Bailey.intakeStates.on ? Robot_Bailey.intakeStates.on : Robot_Bailey.intakeStates.off;
    if (!this.gamepad1_y_changed || !this.gamepad1_y)
      return;
    if (this.intake_statemachine == Robot_Bailey.intakeStates.reverse)
      this.intake_statemachine = Robot_Bailey.intakeStates.off;
    else
      this.intake_statemachine = Robot_Bailey.intakeStates.reverse;
  }

  public bool MoveArm(float target, float forearm_speed = -1f)
  {
    if ((double) forearm_speed < 0.0)
      forearm_speed = this.forearm_speed_start;
    double num1 = (double) this.MoveHinge(this.forearm, target, forearm_speed * this.turning_scaler);
    double num2 = (double) this.MoveHinge(this.forearm2, this.GetInterpolatedValue(this.forearm.spring.targetPosition, this.forearm2_map), 10f * forearm_speed);
    double num3 = (double) this.MoveHinge(this.hand, this.GetInterpolatedValue(this.forearm.spring.targetPosition, this.hand_map), 10f * forearm_speed);
    if ((bool) (UnityEngine.Object) this.audio_manager && !this.audio_manager.IsSoundStarted("arm"))
      this.audio_manager.Play("arm", 0.5f);
    double num4 = (double) target;
    return num1 == num4;
  }

  public void DoAnimations(bool client_mode = false)
  {
  }

  public override string GetStates() => base.GetStates() + ";";

  public override void SetStates(string instring) => base.SetStates(instring.Split(';')[0]);

  public float GetInterpolatedValue(float x, float[,] table)
  {
    float x2 = table[0, 0];
    float y2 = table[0, 1];
    for (int index = 1; index < table.Length; ++index)
    {
      float x1 = table[index, 0];
      float y1 = table[index, 1];
      if ((double) x1 >= (double) x && (double) x2 <= (double) x || (double) x1 <= (double) x && (double) x2 >= (double) x)
        return this.LinearInterpolate(x1, y1, x2, y2, x);
      x2 = x1;
      y2 = y1;
    }
    int index1 = table.Length - 1;
    return (double) Math.Abs(x - table[0, 0]) < (double) Math.Abs(table[index1, 0] - x) ? this.LinearInterpolate(table[0, 0], table[0, 1], table[1, 0], table[1, 1], x) : this.LinearInterpolate(table[index1, 0], table[index1, 1], table[index1 - 1, 0], table[index1 - 1, 1], x);
  }

  public float LinearInterpolate(float x1, float y1, float x2, float y2, float x)
  {
    float num = (float) (((double) y2 - (double) y1) / ((double) x2 - (double) x1));
    return (x - x1) * num + y1;
  }

  private enum intakeStates
  {
    off,
    on,
    reverse,
  }

  private enum armStates
  {
    home,
    closing,
    deploying,
    deployed,
    opening,
    returning,
  }

  private enum bucketStates
  {
    down,
    rising,
    up,
    falling,
  }
}
