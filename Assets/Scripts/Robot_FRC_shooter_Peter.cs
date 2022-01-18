// Decompiled with JetBrains decompiler
// Type: Robot_FRC_shooter_Peter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class Robot_FRC_shooter_Peter : RobotInterface3D
{
  private bool init_done;
  [Header("FRC Bot float settings")]
  public float intake_speed = -1000f;
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
  public float ball_velocity = 6f;
  [Header("FRC Bot Linked Objects")]
  public HingeJoint intake_collector;
  public HingeJoint intake_collector2;
  public HingeJoint intake_collector3;
  public ballshooting ball_feeder;
  public ballshooting ball_feeder2;
  public ballshooting ball_feeder3;
  public HingeJoint ball_feeder_hinge1;
  public HingeJoint ball_feeder_hinge2;
  public HingeJoint ball_aimer;
  public ConfigurableJoint ball_aimer_cj;
  public ballshooting ball_out_forcer;
  public GameObject indicator;
  public ConfigurableJoint arm;
  public ConfigurableJoint armdummy;
  public Transform flywheel;
  public float flywheel_speed = 1f;
  public Transform collectorWheelVisual_Left;
  public Transform collectorWheelVisual_Right;
  [Header("Turret Options")]
  public ConfigurableJoint turret;
  public float turret_averager = 10f;
  public float turret_angle_correction;
  public Vector3 red_target;
  public Vector3 blue_target;
  [Header("Turret Visuals")]
  public HingeJoint inertia_shooter_flywheel;
  public Robot_FRC_shooter_Peter.PullyData[] inertia_shooter_pulleys;
  [Header("Animation Aid")]
  public Transform intake_spinner1_zaxis;
  public Transform intake_spinner2_zaxis;
  public Transform intake_spinner3_zaxis;
  private bool a_button_last;
  private Vector3 indicator_starting_pos;
  private Quaternion indicator_rotation;
  private Sound_Controller_FRC_Shooter sound_controller;
  private AudioManager audio_manager;
  private Robot_FRC_shooter_Peter.intakeStates intake_statemachine;
  private Robot_FRC_shooter_Peter.intakeStates old_intake_statemachine;
  private bool movedLift_sound;
  public double delta_angle_display;
  private bool gamepad1_x_old;
  private bool sound_shooting;

  public void Awake() => this.info = "<Missing Robot Specific Function: TBD>" + this.info;

  public override void Start()
  {
    base.Start();
    this.indicator_starting_pos = this.indicator.transform.localPosition;
  }

  public override void RobotFixedUpdate()
  {
    if (!(bool) (UnityEngine.Object) this.rb_body || !(bool) (UnityEngine.Object) this.myRobotID || !this.init_done || this.isKinematic)
      return;
    if ((bool) (UnityEngine.Object) this.indicator)
    {
      this.indicator.transform.localRotation = this.rb_body.transform.localRotation * this.indicator_rotation;
      this.indicator.transform.localPosition = this.rb_body.transform.localPosition + this.rb_body.transform.localRotation * this.indicator_starting_pos;
    }
    if (!(bool) (UnityEngine.Object) this.turret || !(bool) (UnityEngine.Object) this.myRobotID)
      return;
    Vector2 vector2_1;
    vector2_1.x = this.ball_out_forcer.transform.position.x;
    vector2_1.y = this.ball_out_forcer.transform.position.z;
    Vector3 localPosition = this.ball_out_forcer.transform.localPosition;
    --localPosition.x;
    Vector3 vector3 = this.ball_out_forcer.transform.TransformPoint(localPosition);
    Vector2 vector2_2;
    vector2_2.x = vector3.x;
    vector2_2.y = vector3.z;
    Vector2 vector2_3;
    if (this.myRobotID.is_red)
    {
      vector2_3.x = this.red_target.x;
      vector2_3.y = this.red_target.z;
    }
    else
    {
      vector2_3.x = this.blue_target.x;
      vector2_3.y = this.blue_target.z;
    }
    Vector2 inDirection = vector2_3 - vector2_1;
    float num1 = inDirection.magnitude / this.ball_velocity;
    Vector2 rhs = Vector2.Perpendicular(inDirection);
    rhs.Normalize();
    Vector2 lhs;
    lhs.x = this.rb_body.velocity.x;
    lhs.y = this.rb_body.velocity.y;
    float num2 = Vector2.Dot(lhs, rhs);
    Vector2 vector2_4 = vector2_3 - rhs * num2 * num1;
    this.turret.targetRotation *= Quaternion.Euler(-1f * (Vector2.SignedAngle(vector2_2 - vector2_1, vector2_4 - vector2_1) + this.turret_angle_correction) / this.turret_averager, 0.0f, 0.0f);
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
    if (this.gamepad1_y)
      this.intake_statemachine = Robot_FRC_shooter_Peter.intakeStates.reverse;
    else if (this.intake_statemachine == Robot_FRC_shooter_Peter.intakeStates.reverse)
      this.intake_statemachine = Robot_FRC_shooter_Peter.intakeStates.onNormal;
    if (this.gamepad1_a && !this.a_button_last)
      this.intake_statemachine = this.intake_statemachine != Robot_FRC_shooter_Peter.intakeStates.onNormal ? Robot_FRC_shooter_Peter.intakeStates.onNormal : Robot_FRC_shooter_Peter.intakeStates.off;
    this.a_button_last = this.gamepad1_a;
    if (this.intake_statemachine != this.old_intake_statemachine)
    {
      if (this.intake_statemachine == Robot_FRC_shooter_Peter.intakeStates.off)
      {
        if ((bool) (UnityEngine.Object) this.sound_controller)
          this.sound_controller.revdown();
      }
      else if (this.old_intake_statemachine == Robot_FRC_shooter_Peter.intakeStates.off && (bool) (UnityEngine.Object) this.sound_controller)
        this.sound_controller.revup();
    }
    this.old_intake_statemachine = this.intake_statemachine;
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
      num1 = !(bool) (UnityEngine.Object) this.ball_aimer_cj ? this.MoveHinge(this.ball_aimer, this.outhinge_min, this.aim_tilt_speed * this.turning_scaler) : this.MoveHingeCJ(this.ball_aimer_cj, this.outhinge_min, this.aim_tilt_speed * this.turning_scaler);
    }
    if (this.gamepad1_dpad_up)
    {
      flag2 = true;
      num1 = !(bool) (UnityEngine.Object) this.ball_aimer_cj ? this.MoveHinge(this.ball_aimer, this.outhinge_max, this.aim_tilt_speed * this.turning_scaler) : this.MoveHingeCJ(this.ball_aimer_cj, this.outhinge_max, this.aim_tilt_speed * this.turning_scaler);
    }
    if (flag2 && (double) num1 != (double) this.outhinge_min && (double) num1 != (double) this.outhinge_max)
    {
      if ((bool) (UnityEngine.Object) this.audio_manager && !this.audio_manager.findSound("adjustangle").isPlaying)
        this.audio_manager.Play("adjustangle");
    }
    else if ((bool) (UnityEngine.Object) this.audio_manager && this.audio_manager.findSound("adjustangle").isPlaying)
      this.audio_manager.Stop("adjustangle", 0.3f);
    if (this.gamepad1_x)
    {
      this.Inertia_SpinUpFlywheel();
      this.ball_feeder.speed = this.ball_feeder_speed;
      if ((bool) (UnityEngine.Object) this.ball_feeder2)
        this.ball_feeder2.speed = this.ball_feeder_speed;
      if ((bool) (UnityEngine.Object) this.ball_feeder3)
        this.ball_feeder3.speed = this.ball_feeder_speed;
      if ((bool) (UnityEngine.Object) this.ball_feeder_hinge1)
        this.ball_feeder_hinge1.motor = this.ball_feeder_hinge1.motor with
        {
          targetVelocity = this.ball_feeder_hinge_speed
        };
      if ((bool) (UnityEngine.Object) this.ball_feeder_hinge2)
        this.ball_feeder_hinge2.motor = this.ball_feeder_hinge2.motor with
        {
          targetVelocity = this.ball_feeder_hinge_speed
        };
      if ((bool) (UnityEngine.Object) this.collectorWheelVisual_Left)
        this.collectorWheelVisual_Left.Rotate(0.0f, 0.0f, Time.deltaTime * 180f, Space.Self);
      if ((bool) (UnityEngine.Object) this.collectorWheelVisual_Right)
        this.collectorWheelVisual_Right.Rotate(0.0f, 0.0f, (float) (-(double) Time.deltaTime * 180.0), Space.Self);
      if (!this.sound_shooting)
      {
        if ((bool) (UnityEngine.Object) this.audio_manager)
          this.audio_manager.Play("shooting", 0.3f);
        this.sound_shooting = true;
      }
    }
    else
    {
      if ((bool) (UnityEngine.Object) this.collectorWheelVisual_Left)
        this.collectorWheelVisual_Left.Rotate(0.0f, 0.0f, (float) (-(double) Time.deltaTime * 180.0), Space.Self);
      if ((bool) (UnityEngine.Object) this.collectorWheelVisual_Right)
        this.collectorWheelVisual_Right.Rotate(0.0f, 0.0f, Time.deltaTime * 180f, Space.Self);
      this.Inertia_StopFlywheel();
      this.ball_feeder.speed = -0.5f * this.ball_feeder_speed;
      float num2 = 0.0f;
      if (this.intake_statemachine == Robot_FRC_shooter_Peter.intakeStates.reverse)
        num2 = -1f * this.ball_feeder_speed;
      if ((bool) (UnityEngine.Object) this.ball_feeder2)
        this.ball_feeder2.speed = num2;
      if ((bool) (UnityEngine.Object) this.ball_feeder3)
        this.ball_feeder3.speed = num2;
      if ((bool) (UnityEngine.Object) this.ball_feeder_hinge1)
        this.ball_feeder_hinge1.motor = this.ball_feeder_hinge1.motor with
        {
          targetVelocity = -0.25f * this.ball_feeder_hinge_speed
        };
      if ((bool) (UnityEngine.Object) this.ball_feeder_hinge2)
        this.ball_feeder_hinge2.motor = this.ball_feeder_hinge2.motor with
        {
          targetVelocity = -0.25f * this.ball_feeder_hinge_speed
        };
      if (this.sound_shooting)
      {
        if ((bool) (UnityEngine.Object) this.audio_manager)
          this.audio_manager.Stop("shooting", 0.3f);
        this.sound_shooting = false;
      }
    }
    this.Inertia_MoveTurretPulleys();
    this.gamepad1_x_old = this.gamepad1_x;
    if (this.gamepad1_b)
    {
      if ((bool) (UnityEngine.Object) this.ball_aimer_cj)
      {
        double num3 = (double) this.MoveHingeCJ(this.ball_aimer_cj, this.aim_target_pos, this.aim_tilt_speed);
      }
      else
      {
        double num4 = (double) this.MoveHinge(this.ball_aimer, this.aim_target_pos, this.aim_tilt_speed);
      }
    }
    if (this.intake_statemachine == Robot_FRC_shooter_Peter.intakeStates.onNormal)
    {
      this.intake_collector.motor = this.intake_collector.motor with
      {
        targetVelocity = this.intake_speed
      };
      if ((bool) (UnityEngine.Object) this.intake_collector2)
        this.intake_collector2.motor = this.intake_collector2.motor with
        {
          targetVelocity = this.intake_speed
        };
      if ((bool) (UnityEngine.Object) this.intake_collector3)
        this.intake_collector3.motor = this.intake_collector3.motor with
        {
          targetVelocity = this.intake_speed
        };
    }
    if (this.intake_statemachine == Robot_FRC_shooter_Peter.intakeStates.off)
    {
      this.intake_collector.motor = this.intake_collector.motor with
      {
        targetVelocity = 0.0f
      };
      if ((bool) (UnityEngine.Object) this.intake_collector2)
        this.intake_collector2.motor = this.intake_collector2.motor with
        {
          targetVelocity = 0.0f
        };
      if ((bool) (UnityEngine.Object) this.intake_collector3)
        this.intake_collector3.motor = this.intake_collector3.motor with
        {
          targetVelocity = 0.0f
        };
    }
    if (this.intake_statemachine == Robot_FRC_shooter_Peter.intakeStates.reverse)
    {
      this.intake_collector.motor = this.intake_collector.motor with
      {
        targetVelocity = -1f * this.intake_speed
      };
      if ((bool) (UnityEngine.Object) this.intake_collector2)
        this.intake_collector2.motor = this.intake_collector2.motor with
        {
          targetVelocity = -1f * this.intake_speed
        };
      if ((bool) (UnityEngine.Object) this.intake_collector3)
        this.intake_collector3.motor = this.intake_collector3.motor with
        {
          targetVelocity = -1f * this.intake_speed
        };
    }
    this.UpdateVisualBallFeeder();
    if ((bool) (UnityEngine.Object) this.indicator.GetComponent<interpolation>() && this.indicator.GetComponent<interpolation>().enabled)
      return;
    this.indicator_rotation.eulerAngles = new Vector3(-1f * (float) ((((bool) (UnityEngine.Object) this.ball_aimer_cj ? (double) this.ball_aimer_cj.targetRotation.eulerAngles.x : (double) this.ball_aimer.spring.targetPosition) - (double) this.outhinge_min) / ((double) this.outhinge_max - (double) this.outhinge_min) * 180.0 - 90.0), 0.0f, 0.0f);
    this.RobotFixedUpdate();
  }

  private void UpdateVisualBallFeeder()
  {
  }

  private void Inertia_MoveTurretPulleys()
  {
    foreach (Robot_FRC_shooter_Peter.PullyData inertiaShooterPulley in this.inertia_shooter_pulleys)
    {
      Quaternion quaternion = Quaternion.Inverse(this.turret.transform.rotation) * this.inertia_shooter_flywheel.transform.rotation;
      if (inertiaShooterPulley.isReversed)
        quaternion.eulerAngles = new Vector3(quaternion.eulerAngles.x, -quaternion.eulerAngles.y, quaternion.eulerAngles.z);
      inertiaShooterPulley.myTransform.localRotation = quaternion;
    }
  }

  private void Inertia_SpinUpFlywheel()
  {
    if (!(bool) (UnityEngine.Object) this.inertia_shooter_flywheel)
      return;
    this.inertia_shooter_flywheel.motor = this.inertia_shooter_flywheel.motor with
    {
      force = 0.5f,
      targetVelocity = -900f,
      freeSpin = true
    };
  }

  private void Inertia_StopFlywheel()
  {
    if (!(bool) (UnityEngine.Object) this.inertia_shooter_flywheel)
      return;
    this.inertia_shooter_flywheel.motor = this.inertia_shooter_flywheel.motor with
    {
      force = 0.0f,
      targetVelocity = -900f,
      freeSpin = true
    };
  }

  public void DoAnimations(bool client_mode = false)
  {
    if ((bool) (UnityEngine.Object) this.flywheel)
      this.flywheel.transform.localRotation = this.flywheel.transform.localRotation * Quaternion.identity with
      {
        y = this.flywheel_speed * Time.deltaTime
      };
    if (this.intake_statemachine == Robot_FRC_shooter_Peter.intakeStates.off)
      return;
    float yAngle = this.intake_speed * Time.deltaTime;
    if (this.intake_statemachine == Robot_FRC_shooter_Peter.intakeStates.reverse)
      yAngle *= -1f;
    if ((bool) (UnityEngine.Object) this.intake_spinner1_zaxis)
      this.intake_spinner1_zaxis.Rotate(0.0f, yAngle, 0.0f, Space.Self);
    if ((bool) (UnityEngine.Object) this.intake_spinner2_zaxis)
      this.intake_spinner2_zaxis.Rotate(0.0f, yAngle, 0.0f, Space.Self);
    if (!(bool) (UnityEngine.Object) this.intake_spinner3_zaxis)
      return;
    this.intake_spinner3_zaxis.Rotate(0.0f, yAngle, 0.0f, Space.Self);
  }

  private new float MoveHinge(HingeJoint hinge, float target, float speed)
  {
    JointLimits limits1 = hinge.limits;
    double max = (double) limits1.max;
    limits1 = hinge.limits;
    double min = (double) limits1.min;
    if (max != min)
    {
      JointLimits limits2 = hinge.limits;
      if ((double) limits2.max < (double) target)
      {
        limits2 = hinge.limits;
        target = limits2.max;
      }
      limits2 = hinge.limits;
      if ((double) limits2.min > (double) target)
      {
        limits2 = hinge.limits;
        target = limits2.min;
      }
    }
    if ((double) hinge.spring.targetPosition == (double) target)
      return target;
    float targetPosition = hinge.spring.targetPosition;
    float num = this.MoveTowards(targetPosition, target, targetPosition, Time.deltaTime * speed);
    JointSpring spring = hinge.spring with
    {
      targetPosition = num
    };
    hinge.spring = spring;
    return num;
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

  public override string GetStates() => base.GetStates() + ";" + (object) (int) this.intake_statemachine;

  public override void SetStates(string instring)
  {
    string[] strArray = instring.Split(';');
    base.SetStates(strArray[0]);
    if (strArray.Length <= 1)
      return;
    this.intake_statemachine = (Robot_FRC_shooter_Peter.intakeStates) int.Parse(strArray[1]);
  }

  [Serializable]
  public struct PullyData
  {
    public Transform myTransform;
    public bool isReversed;
  }

  private enum intakeStates
  {
    off,
    onNormal,
    reverse,
  }
}
