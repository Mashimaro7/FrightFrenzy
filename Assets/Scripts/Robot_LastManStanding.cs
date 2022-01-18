// Decompiled with JetBrains decompiler
// Type: Robot_LastManStanding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Robot_LastManStanding : RobotInterface3D
{
  private bool init_done;
  [Header("FRC Bot float settings")]
  public float intake_speed = -1000f;
  public float ball_feeder_speed = 1.5f;
  public float ball_feeder_hinge_speed = 50f;
  public float ball_out_speed = 2f;
  public float aim_tilt_speed = -300f;
  public float aim_target_pos = 10f;
  public float outhinge_min = -30f;
  public float outhinge_max = 30f;
  [Header("FRC Bot Linked Objects")]
  public HingeJoint intake_collector;
  public HingeJoint ball_feeder_hinge1;
  public HingeJoint ball_feeder_hinge2;
  public ballshooting ball_feeder;
  public ballshooting ball_feeder2;
  public ballshooting ball_feeder3;
  public ballshooting ball_feeder4;
  public ballshooting ball_feeder5;
  public HingeJoint ball_aimer;
  public ballshooting ball_out_forcer;
  public GameObject indicator;
  public Transform ballFeedingPos;
  public ballshooting hopperBalls;
  [Header("Animation Aid")]
  public Transform intake_spinner1_zaxis;
  public Transform ball_feeder1_animation;
  public Transform ball_feeder2_animation;
  private bool a_button_last;
  private Vector3 indicator_starting_pos;
  private Quaternion indicator_rotation;
  private Sound_Controller_FRC_Shooter sound_controller;
  private AudioManager audio_manager;
  private Animation death_animation;
  private Robot_LastManStanding.intakeStates intake_statemachine;
  private Robot_LastManStanding.intakeStates old_intake_statemachine;
  private bool ballfeeder_feeding;
  private bool death_animation_playing;
  public double delta_angle_display;
  private bool pauseUpdates_old;
  private bool sound_shooting;
  private bool animation_playing_old;

  public void Awake() => this.info = "<Missing Robot Specific Function: TBD>" + this.info;

  public override void Start()
  {
    base.Start();
    this.indicator_starting_pos = this.indicator.transform.localPosition;
  }

  public override void RobotFixedUpdate()
  {
    if (!(bool) (Object) this.rb_body || !(bool) (Object) this.myRobotID || !this.init_done || this.isKinematic || !(bool) (Object) this.indicator)
      return;
    this.indicator.transform.localRotation = this.rb_body.transform.localRotation * this.indicator_rotation;
    this.indicator.transform.localPosition = this.rb_body.transform.localPosition + this.rb_body.transform.localRotation * this.indicator_starting_pos;
  }

  public override void Update_Robot()
  {
    if (!this.init_done)
    {
      this.ball_out_forcer.speed = this.ball_out_speed;
      this.sound_controller = this.GetComponent<Sound_Controller_FRC_Shooter>();
      this.audio_manager = this.GetComponent<AudioManager>();
      this.death_animation = this.GetComponent<Animation>();
      this.init_done = true;
    }
    if (this.death_animation_playing)
    {
      this.death_animation_playing = this.death_animation.isPlaying;
    }
    else
    {
      this.pauseUpdates = false;
      if (this.pauseUpdates_old != this.pauseUpdates)
      {
        foreach (BandwidthHelper componentsInChild in this.GetComponentsInChildren<BandwidthHelper>())
          componentsInChild.pauseUpdates = false;
      }
      this.pauseUpdates_old = this.pauseUpdates;
      if (GLOBALS.CLIENT_MODE)
      {
        this.DoAnimations();
      }
      else
      {
        if (this.gamepad1_y)
          this.intake_statemachine = Robot_LastManStanding.intakeStates.reverse;
        else if (this.intake_statemachine == Robot_LastManStanding.intakeStates.reverse)
          this.intake_statemachine = Robot_LastManStanding.intakeStates.onNormal;
        if (this.gamepad1_a && !this.a_button_last)
          this.intake_statemachine = this.intake_statemachine != Robot_LastManStanding.intakeStates.onNormal ? Robot_LastManStanding.intakeStates.onNormal : Robot_LastManStanding.intakeStates.off;
        this.a_button_last = this.gamepad1_a;
        if (this.intake_statemachine != this.old_intake_statemachine)
        {
          if (this.intake_statemachine == Robot_LastManStanding.intakeStates.off)
          {
            if ((bool) (Object) this.sound_controller)
              this.sound_controller.revdown();
          }
          else if (this.old_intake_statemachine == Robot_LastManStanding.intakeStates.off && (bool) (Object) this.sound_controller)
            this.sound_controller.revup();
        }
        this.old_intake_statemachine = this.intake_statemachine;
        bool flag = false;
        float num1 = 0.0f;
        if (this.gamepad1_dpad_down)
        {
          flag = true;
          num1 = this.MoveHinge(this.ball_aimer, this.outhinge_min, this.aim_tilt_speed * this.turning_scaler);
        }
        if (this.gamepad1_dpad_up)
        {
          flag = true;
          num1 = this.MoveHinge(this.ball_aimer, this.outhinge_max, this.aim_tilt_speed * this.turning_scaler);
        }
        if (flag && (double) num1 != (double) this.outhinge_min && (double) num1 != (double) this.outhinge_max)
        {
          if ((bool) (Object) this.audio_manager && !this.audio_manager.IsSoundStarted("adjustangle"))
            this.audio_manager.Play("adjustangle");
        }
        else if ((bool) (Object) this.audio_manager && this.audio_manager.IsSoundStarted("adjustangle"))
          this.audio_manager.Stop("adjustangle", 0.3f);
        if (this.gamepad1_x)
        {
          this.ball_feeder.speed = this.ball_feeder_speed;
          this.ballfeeder_feeding = true;
          if ((bool) (Object) this.ball_feeder_hinge1)
            this.ball_feeder_hinge1.motor = this.ball_feeder_hinge1.motor with
            {
              targetVelocity = this.ball_feeder_hinge_speed
            };
          if ((bool) (Object) this.ball_feeder_hinge2)
            this.ball_feeder_hinge2.motor = this.ball_feeder_hinge2.motor with
            {
              targetVelocity = this.ball_feeder_hinge_speed
            };
          if (!this.sound_shooting)
          {
            if ((bool) (Object) this.audio_manager)
              this.audio_manager.Play("shooting", 0.3f);
            this.sound_shooting = true;
          }
        }
        else
        {
          this.ball_feeder.speed = -0.1f * this.ball_feeder_speed;
          this.ballfeeder_feeding = false;
          if ((bool) (Object) this.ball_feeder_hinge1)
            this.ball_feeder_hinge1.motor = this.ball_feeder_hinge1.motor with
            {
              targetVelocity = -0.25f * this.ball_feeder_hinge_speed
            };
          if ((bool) (Object) this.ball_feeder_hinge2)
            this.ball_feeder_hinge2.motor = this.ball_feeder_hinge2.motor with
            {
              targetVelocity = -0.25f * this.ball_feeder_hinge_speed
            };
          if (this.sound_shooting)
          {
            if ((bool) (Object) this.audio_manager)
              this.audio_manager.Stop("shooting", 0.3f);
            this.sound_shooting = false;
          }
        }
        if (this.gamepad1_b)
        {
          double num2 = (double) this.MoveHinge(this.ball_aimer, this.aim_target_pos, this.aim_tilt_speed);
        }
        if (this.intake_statemachine == Robot_LastManStanding.intakeStates.onNormal)
          this.intake_collector.motor = this.intake_collector.motor with
          {
            targetVelocity = this.intake_speed
          };
        if (this.intake_statemachine == Robot_LastManStanding.intakeStates.off)
          this.intake_collector.motor = this.intake_collector.motor with
          {
            targetVelocity = 0.0f
          };
        if (this.intake_statemachine == Robot_LastManStanding.intakeStates.reverse)
        {
          this.intake_collector.motor = this.intake_collector.motor with
          {
            targetVelocity = -1f * this.intake_speed
          };
          if ((bool) (Object) this.ball_feeder_hinge1)
            this.ball_feeder_hinge1.motor = this.ball_feeder_hinge1.motor with
            {
              targetVelocity = -1f * this.ball_feeder_hinge_speed
            };
          if ((bool) (Object) this.ball_feeder_hinge2)
            this.ball_feeder_hinge2.motor = this.ball_feeder_hinge2.motor with
            {
              targetVelocity = -1f * this.ball_feeder_hinge_speed
            };
        }
        if (!(bool) (Object) this.indicator.GetComponent<interpolation>() || !this.indicator.GetComponent<interpolation>().enabled)
        {
          this.indicator_rotation.eulerAngles = new Vector3(-1f * (float) (((double) this.ball_aimer.spring.targetPosition - (double) this.outhinge_min) / ((double) this.outhinge_max - (double) this.outhinge_min) * 180.0 - 90.0), 0.0f, 0.0f);
          this.RobotFixedUpdate();
        }
        if (this.ball_feeder.AnyBallsInside() || this.ball_out_forcer.AnyBallsInside() || this.ball_feeder2.AnyBallsInside() || this.ball_feeder3.AnyBallsInside() || this.ball_feeder4.AnyBallsInside() || this.ball_feeder5.AnyBallsInside() || !this.hopperBalls.AnyBallsInside())
          return;
        gameElement ballInside = this.hopperBalls.GetBallInside();
        if (!(bool) (Object) ballInside)
          return;
        ballInside.transform.position = this.ballFeedingPos.position;
      }
    }
  }

  public void DoAnimations()
  {
    float zAngle = this.intake_speed * Time.deltaTime;
    if (this.intake_statemachine != Robot_LastManStanding.intakeStates.off)
    {
      if (this.intake_statemachine == Robot_LastManStanding.intakeStates.reverse)
        zAngle *= -1f;
      if ((bool) (Object) this.intake_spinner1_zaxis)
        this.intake_spinner1_zaxis.Rotate(0.0f, 0.0f, zAngle, Space.Self);
    }
    float yAngle = this.ball_feeder_hinge_speed * Time.deltaTime;
    if (this.ballfeeder_feeding)
    {
      this.ball_feeder1_animation.Rotate(0.0f, yAngle, 0.0f, Space.Self);
      this.ball_feeder2_animation.Rotate(0.0f, yAngle, 0.0f, Space.Self);
    }
    else
    {
      this.ball_feeder1_animation.Rotate(0.0f, -0.5f * yAngle, 0.0f, Space.Self);
      this.ball_feeder2_animation.Rotate(0.0f, -0.5f * yAngle, 0.0f, Space.Self);
    }
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

  public override string GetStates() => base.GetStates() + ";" + (object) (int) this.intake_statemachine + ";" + (this.ballfeeder_feeding ? "1" : "0") + ";" + (this.death_animation_playing ? "1" : "0");

  public override void SetStates(string instring)
  {
    string[] strArray = instring.Split(';');
    base.SetStates(strArray[0]);
    if (strArray.Length > 1)
      this.intake_statemachine = (Robot_LastManStanding.intakeStates) int.Parse(strArray[1]);
    if (strArray.Length > 2)
      this.ballfeeder_feeding = strArray[2][0] == '1';
    if (strArray.Length > 3)
      this.death_animation_playing = strArray[3][0] == '1';
    if (this.death_animation_playing && this.death_animation_playing != this.animation_playing_old)
      this.PlayDeathAnimation();
    this.animation_playing_old = this.death_animation_playing;
  }

  public void ResetPosition_w_y_3()
  {
    this.ResetPosition(y_off: 3f);
    this.transform.position = Vector3.zero;
    this.transform.rotation = Quaternion.identity;
  }

  public void PlayDeathAnimation()
  {
    this.death_animation_playing = true;
    this.SetKinematic();
    this.HoldRobot();
    if (!GLOBALS.CLIENT_MODE)
      this.rb_body.GetComponentInParent<AudioManager>().Play("Death1", volume: 1f);
    else
      this.pauseUpdates = true;
    this.transform.position = this.rb_body.transform.position;
    this.transform.rotation = this.rb_body.transform.rotation;
    this.ResetPosition();
    this.death_animation.Play();
  }

  public override void EnableTopObjects()
  {
    if ((bool) (Object) this.death_animation && this.death_animation.isPlaying)
      this.death_animation.Stop();
    this.death_animation_playing = false;
    base.EnableTopObjects();
    this.ResetPosition();
  }

  private enum intakeStates
  {
    off,
    onNormal,
    reverse,
  }
}
