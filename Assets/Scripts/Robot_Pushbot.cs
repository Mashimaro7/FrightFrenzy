// Decompiled with JetBrains decompiler
// Type: Robot_Pushbot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class Robot_Pushbot : RobotInterface3D
{
  public float pushbot_arm_speed = 75f;
  public HingeJoint mainArmJoint;
  public HingeJoint finger_r_joint;
  public HingeJoint finger_l_joint;
  public float arm_max_limit = 180f;
  public float arm_min_limit = -180f;

  public void Awake() => this.info = "<Missing Robot Specific Function: TBD>" + this.info;

  public override void Update_Robot()
  {
    HingeJoint mainArmJoint = this.mainArmJoint;
    JointSpring spring = mainArmJoint.spring;
    if (this.gamepad1_dpad_down)
    {
      spring.targetPosition += this.pushbot_arm_speed * Time.deltaTime;
      if ((double) spring.targetPosition > (double) this.arm_max_limit)
        spring.targetPosition = this.arm_max_limit;
    }
    if (this.gamepad1_dpad_up)
    {
      spring.targetPosition -= this.pushbot_arm_speed * Time.deltaTime;
      if ((double) spring.targetPosition < (double) this.arm_min_limit)
        spring.targetPosition = this.arm_min_limit;
    }
    mainArmJoint.spring = spring;
    this.mainArmJoint = mainArmJoint;
    if (this.gamepad1_x)
    {
      this.finger_l_joint.spring = this.finger_l_joint.spring with
      {
        targetPosition = 30f
      };
      this.finger_r_joint.spring = this.finger_r_joint.spring with
      {
        targetPosition = -30f
      };
    }
    if (!this.gamepad1_a)
      return;
    this.finger_l_joint.spring = this.finger_l_joint.spring with
    {
      targetPosition = -15f
    };
    this.finger_r_joint.spring = this.finger_r_joint.spring with
    {
      targetPosition = 15f
    };
  }

  public override void SetName(string name)
  {
    base.SetName(name);
    bool flag = false;
    Transform transform1 = this.transform.Find("Body/NametagB1");
    if ((bool) (Object) transform1)
    {
      transform1.GetComponent<TextMeshPro>().text = name;
      flag = true;
    }
    Transform transform2 = this.transform.Find("Body/NametagB2");
    if ((bool) (Object) transform2)
    {
      transform2.GetComponent<TextMeshPro>().text = name;
      flag = true;
    }
    if (!flag)
      return;
    this.transform.Find("Body/Nametag").gameObject.SetActive(false);
  }
}
