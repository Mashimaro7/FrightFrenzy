// Decompiled with JetBrains decompiler
// Type: Robot_RR_MiniDrone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class Robot_RR_MiniDrone : RobotInterface3D
{
  private bool init_done;
  [Header("Float settings")]
  public float ball_feeder_speed = 1.5f;
  public float ball_feeder_hinge_speed = 50f;
  public float ball_out_speed = 2f;
  public float aim_tilt_speed = -300f;
  public float arm_start;
  public float arm_end = -1f;
  public float arm_speed = 1f;
  public float aim_target_pos = 10f;
  public float outhinge_min = -30f;
  public float outhinge_max = 30f;
  [Header("FRC Bot Linked Objects")]
  public ballshooting ball_feeder;
  public HingeJoint ball_aimer;
  public ballshooting ball_out_forcer;
  public ballshooting ball_intake_forcer;
  public GameObject indicator;
  public ConfigurableJoint arm;
  public ConfigurableJoint armdummy;
  private Vector3 indicator_starting_pos;
  private Quaternion indicator_rotation;
  private Sound_Controller_FRC_Shooter sound_controller;
  private AudioManager audio_manager;
  private bool movedLift_sound;
  public double delta_angle_display;
  private bool sound_shooting;
  public float rotation;

  public void Awake() => this.info = "Gamepad Up/Down: Aim Up/Down\nGamepad Left/Right: Lift Down/Up\nButton Y: Spit balls out\nButton X: Shoot balls\nButton B: Aim to home position (while held)" + this.info;

  public override void Start()
  {
    base.Start();
    this.indicator_starting_pos = this.indicator.transform.localPosition;
  }

  public override void RobotFixedUpdate()
  {
    if (!(bool) (UnityEngine.Object) this.rb_body || !(bool) (UnityEngine.Object) this.myRobotID || !this.init_done || this.isKinematic || !(bool) (UnityEngine.Object) this.indicator)
      return;
    this.indicator.transform.localRotation = this.rb_body.transform.localRotation * this.indicator_rotation;
    this.indicator.transform.localPosition = this.rb_body.transform.localPosition + this.rb_body.transform.localRotation * this.indicator_starting_pos;
  }

  public override void Update_Robot()
  {
    if (!this.init_done)
    {
      this.ball_out_forcer.speed = this.ball_out_speed;
      this.sound_controller = this.GetComponentInChildren<Sound_Controller_FRC_Shooter>();
      this.audio_manager = this.GetComponentInChildren<AudioManager>();
      this.init_done = true;
    }
    this.DoAnimations(this.isKinematic);
    if (this.isKinematic)
      return;
    this.ball_intake_forcer.speed = !this.gamepad1_y ? Math.Abs(this.ball_intake_forcer.speed) : Math.Abs(this.ball_intake_forcer.speed) * -1f;
    bool flag1 = false;
    if (this.gamepad1_dpad_right)
    {
      flag1 = true;
      float x = this.MoveTowards(this.arm_start, this.arm_end, this.arm.targetPosition.x, Time.deltaTime * this.arm_speed);
      this.arm.targetPosition = new Vector3(x, 0.0f, 0.0f);
      if ((bool) (UnityEngine.Object) this.armdummy)
        this.armdummy.targetPosition = new Vector3((float) (((double) x - (double) this.arm_start) / 2.0) + this.arm_start, 0.0f, 0.0f);
    }
    if (this.gamepad1_dpad_left)
    {
      flag1 = true;
      float x = this.MoveTowards(this.arm_end, this.arm_start, this.arm.targetPosition.x, Time.deltaTime * this.arm_speed);
      this.arm.targetPosition = new Vector3(x, 0.0f, 0.0f);
      if ((bool) (UnityEngine.Object) this.armdummy)
        this.armdummy.targetPosition = new Vector3((float) (((double) x - (double) this.arm_start) / 2.0) + this.arm_start, 0.0f, 0.0f);
    }
    if (flag1 && (double) this.arm.targetPosition.x != (double) this.arm_start && (double) this.arm.targetPosition.x != (double) this.arm_end)
    {
      if (!this.movedLift_sound)
      {
        if ((bool) (UnityEngine.Object) this.audio_manager)
          this.audio_manager.Play("hangerraise");
        this.movedLift_sound = true;
      }
    }
    else
    {
      if ((bool) (UnityEngine.Object) this.audio_manager)
        this.audio_manager.Stop("hangerraise", 0.3f);
      this.movedLift_sound = false;
    }
    bool flag2 = false;
    float num1 = 0.0f;
    if (this.gamepad1_dpad_down)
    {
      flag2 = true;
      num1 = this.MoveHinge(this.ball_aimer, this.outhinge_min, this.aim_tilt_speed * this.turning_scaler);
    }
    if (this.gamepad1_dpad_up)
    {
      flag2 = true;
      num1 = this.MoveHinge(this.ball_aimer, this.outhinge_max, this.aim_tilt_speed * this.turning_scaler);
    }
    if (this.gamepad1_b)
    {
      flag2 = true;
      num1 = this.MoveHinge(this.ball_aimer, this.aim_target_pos, this.aim_tilt_speed);
    }
    if (flag2 && (double) num1 != (double) this.outhinge_min && (double) num1 != (double) this.outhinge_max && (double) num1 != (double) this.aim_target_pos)
    {
      if ((bool) (UnityEngine.Object) this.audio_manager && !this.audio_manager.IsSoundStarted("adjustangle"))
        this.audio_manager.Play("adjustangle");
    }
    else if ((bool) (UnityEngine.Object) this.audio_manager)
      this.audio_manager.Stop("adjustangle", 0.3f);
    float num2 = 1f;
    if ((double) this.ball_aimer.spring.targetPosition < ((double) this.outhinge_max + (double) this.outhinge_min) * 2.0 / 3.0 - 0.150000005960464)
      num2 = (float) (((double) this.ball_aimer.spring.targetPosition - (double) this.outhinge_min) / ((double) this.outhinge_max - (double) this.outhinge_min) * 3.0 / 2.0 + 0.150000005960464);
    this.ball_out_forcer.speed = this.ball_out_speed * num2;
    if (this.gamepad1_x)
    {
      this.ball_feeder.speed = this.ball_feeder_speed;
      if (!this.sound_shooting)
      {
        if ((bool) (UnityEngine.Object) this.audio_manager)
          this.audio_manager.Play("shooting", 0.3f);
        this.sound_shooting = true;
      }
    }
    else
    {
      this.ball_feeder.speed = -0.5f * this.ball_feeder_speed;
      if (this.sound_shooting)
      {
        if ((bool) (UnityEngine.Object) this.audio_manager)
          this.audio_manager.Stop("shooting", 0.3f);
        this.sound_shooting = false;
      }
    }
    if ((bool) (UnityEngine.Object) this.indicator.GetComponent<interpolation>() && this.indicator.GetComponent<interpolation>().enabled)
      return;
    this.rotation = this.ball_aimer.spring.targetPosition;
    this.rotation = (float) (((double) this.rotation - (double) this.outhinge_min) / ((double) this.outhinge_max - (double) this.outhinge_min) * 180.0 - 90.0);
    this.indicator_rotation.eulerAngles = new Vector3(-1f * this.rotation, 0.0f, 0.0f);
    this.RobotFixedUpdate();
  }

  public void DoAnimations(bool client_mode = false)
  {
  }

  private float MoveHingeCJ(ConfigurableJoint hinge, float target, float speed)
  {
    SoftJointLimit softJointLimit1 = hinge.lowAngularXLimit;
    double limit1 = (double) softJointLimit1.limit;
    softJointLimit1 = hinge.highAngularXLimit;
    double limit2 = (double) softJointLimit1.limit;
    if (limit1 != limit2)
    {
      SoftJointLimit softJointLimit2 = hinge.highAngularXLimit;
      if ((double) softJointLimit2.limit < (double) target)
      {
        softJointLimit2 = hinge.highAngularXLimit;
        target = softJointLimit2.limit;
      }
      softJointLimit2 = hinge.lowAngularXLimit;
      if ((double) softJointLimit2.limit > (double) target)
      {
        softJointLimit2 = hinge.lowAngularXLimit;
        target = softJointLimit2.limit;
      }
    }
    Quaternion targetRotation = hinge.targetRotation;
    if ((double) MyUtils.AngleWrap(targetRotation.eulerAngles.x) == (double) target)
      return target;
    targetRotation = hinge.targetRotation;
    float num1 = MyUtils.AngleWrap(targetRotation.eulerAngles.x);
    float num2 = this.MoveTowards(num1, target, num1, Time.deltaTime * speed);
    targetRotation = hinge.targetRotation;
    Vector3 eulerAngles = targetRotation.eulerAngles with
    {
      x = num2
    };
    hinge.targetRotation = Quaternion.Euler(eulerAngles);
    return num2;
  }

  public override string GetStates() => base.GetStates();

  public override void SetStates(string instring) => base.SetStates(instring.Split(';')[0]);
}
