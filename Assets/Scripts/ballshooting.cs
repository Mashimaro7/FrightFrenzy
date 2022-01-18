// Decompiled with JetBrains decompiler
// Type: ballshooting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ballshooting : MonoBehaviour
{
  public bool play_sound;
  public float force_divider = 20f;
  public bool hard_stop;
  public bool off_when_zero;
  public bool only_additive_force;
  public bool zero_other_velocities;
  public bool set_velocity_directly;
  public List<gameElement> objects_inside = new List<gameElement>();
  public bool disablePushing;
  public bool doNotMarkBall;
  public bool AddTorque;
  public bool UseDeprecatedAlgorithm = true;
  private Rigidbody myRigidBody;
  private RobotInterface3D myrobot;
  private Vector3 old_position = Vector3.zero;
  private Vector3 myVelocity = Vector3.zero;
  private bool playsound;
  private Collider item_triggering;
  public float speed = 1f;

  private void Start()
  {
    this.myrobot = this.GetComponentInParent<RobotInterface3D>();
    this.old_position = this.transform.position;
    this.myRigidBody = this.GetComponent<Rigidbody>();
    this.MyStart();
  }

  public virtual void MyStart()
  {
  }

  private void Update()
  {
    if (!this.playsound)
      return;
    AudioManager componentInParent = this.item_triggering.gameObject.GetComponentInParent<AudioManager>();
    if ((bool) (Object) componentInParent)
      componentInParent.Play("ballpass");
    this.playsound = false;
  }

  private void FixedUpdate()
  {
    if (this.AddTorque)
    {
      if ((bool) (Object) this.myRigidBody)
        this.myVelocity = this.myRigidBody.angularVelocity;
      else
        this.myVelocity = Vector3.zero;
    }
    else
    {
      this.myVelocity = (this.transform.position - this.old_position) / Time.fixedDeltaTime;
      this.old_position = this.transform.position;
    }
  }

  private void OnTriggerStay(Collider collision)
  {
    GameObject gameObject = collision.gameObject;
    gameElement gameElement = gameObject.GetComponent<gameElement>();
    if (!(bool) (Object) gameElement)
      gameElement = gameObject.GetComponentInParent<gameElement>();
    if (!(bool) (Object) gameElement || gameElement.type == ElementType.Stone || gameElement.type == ElementType.Off || gameElement.type == ElementType.NoRigidBody)
      return;
    if (!this.objects_inside.Contains(gameElement))
      this.objects_inside.Add(gameElement);
    ball_data component1 = gameElement.gameObject.GetComponent<ball_data>();
    if (!this.doNotMarkBall && (bool) (Object) component1)
    {
      if ((bool) (Object) this.myrobot)
      {
        component1.thrown_by_id = this.myrobot.myRobotID.id;
        component1.thrown_robotid = this.myrobot.myRobotID;
        this.MarkGameElement(component1);
      }
      else
      {
        component1.thrown_by_id = -1;
        component1.thrown_robotid = (RobotID) null;
      }
    }
    if (this.disablePushing)
      return;
    Rigidbody component2 = gameElement.gameObject.GetComponent<Rigidbody>();
    if (this.AddTorque)
    {
      if (this.UseDeprecatedAlgorithm)
        this.DoTorqueDeprecated(component2);
      else
        this.DoTorque(component2);
    }
    else if (this.UseDeprecatedAlgorithm)
      this.DoVelocityDeprecated(component2);
    else
      this.DoVelocity(component2);
  }

  public virtual void MarkGameElement(ball_data thisballdata)
  {
  }

  private void DoVelocity(Rigidbody rigidbody)
  {
    Vector3 lhs = rigidbody.velocity - this.myVelocity;
    Vector3 rhs1 = this.transform.TransformDirection(-1f, 0.0f, 0.0f);
    Vector3 rhs2 = this.transform.TransformDirection(0.0f, -1f, 0.0f);
    Vector3 rhs3 = this.transform.TransformDirection(0.0f, 0.0f, -1f);
    float num1 = Vector3.Dot(lhs, rhs1);
    float num2 = Vector3.Dot(lhs, rhs2);
    float num3 = Vector3.Dot(lhs, rhs3);
    float num4 = this.speed - num1;
    if (this.hard_stop)
    {
      if (!this.set_velocity_directly)
      {
        rigidbody.AddForce(rhs1 * (num4 - 0.01f * this.speed), ForceMode.VelocityChange);
        if (!this.zero_other_velocities)
          return;
        rigidbody.AddForce(-1f * num2 * rhs2, ForceMode.VelocityChange);
        rigidbody.AddForce(-1f * num3 * rhs3, ForceMode.VelocityChange);
      }
      else
      {
        Vector3 vector3 = this.myVelocity - 0.01f * this.speed * rhs1;
        if (!this.zero_other_velocities)
          vector3 = vector3 + num2 * rhs2 + num3 * rhs3;
        rigidbody.velocity = vector3;
      }
    }
    else
    {
      if ((double) this.speed == 0.0 && this.off_when_zero)
        return;
      if (!this.set_velocity_directly)
      {
        if (this.zero_other_velocities)
        {
          rigidbody.AddForce(-1f * num2 * rhs2 / this.force_divider, ForceMode.VelocityChange);
          rigidbody.AddForce(-1f * num3 * rhs3 / this.force_divider, ForceMode.VelocityChange);
        }
        if (this.only_additive_force && (double) num4 < 0.0)
          return;
        rigidbody.AddForce(rhs1 * num4 / this.force_divider, ForceMode.VelocityChange);
      }
      else
      {
        Vector3 vector3_1 = this.myVelocity + (this.speed + (this.speed - num1) / this.force_divider) * rhs1;
        Vector3 vector3_2 = this.zero_other_velocities ? vector3_1 + num2 * rhs2 * (float) (1.0 - 1.0 / (double) this.force_divider) + num3 * rhs3 * (float) (1.0 - 1.0 / (double) this.force_divider) : vector3_1 + num2 * rhs2 + num3 * rhs3;
        rigidbody.velocity = vector3_2;
      }
    }
  }

  private void DoVelocityDeprecated(Rigidbody rigidbody)
  {
    if (this.hard_stop)
    {
      rigidbody.velocity = this.myVelocity;
      rigidbody.AddForce(this.transform.TransformDirection(500f * this.speed, 0.0f, 0.0f), ForceMode.Acceleration);
    }
    else
    {
      if ((double) this.speed == 0.0 && this.off_when_zero)
        return;
      rigidbody.AddForce(this.transform.TransformDirection(-1f, 0.0f, 0.0f) * this.speed / this.force_divider, ForceMode.VelocityChange);
      if (this.only_additive_force)
        return;
      Vector3 vector3_1 = rigidbody.velocity - this.myVelocity;
      float num1 = vector3_1.magnitude;
      if ((double) num1 < 9.99999997475243E-07)
        num1 = 1E-06f;
      float num2 = this.speed / num1;
      vector3_1 *= (float) (1.0 + ((double) num2 - 1.0) / (double) this.force_divider);
      Vector3 vector3_2 = vector3_1 + this.myVelocity;
      rigidbody.velocity = vector3_2;
    }
  }

  private void DoTorque(Rigidbody rigidbody)
  {
    Vector3 lhs = rigidbody.angularVelocity - this.myVelocity;
    Vector3 rhs1 = this.transform.TransformDirection(-1f, 0.0f, 0.0f);
    Vector3 rhs2 = this.transform.TransformDirection(0.0f, -1f, 0.0f);
    Vector3 rhs3 = this.transform.TransformDirection(0.0f, 0.0f, -1f);
    float num1 = Vector3.Dot(lhs, rhs1);
    float num2 = Vector3.Dot(lhs, rhs2);
    float num3 = Vector3.Dot(lhs, rhs3);
    float num4 = this.speed - num1;
    if (this.hard_stop)
    {
      rigidbody.AddTorque(rhs1 * (num4 - 0.01f * this.speed), ForceMode.VelocityChange);
      if (!this.zero_other_velocities)
        return;
      rigidbody.AddTorque(-1f * num2 * rhs2, ForceMode.VelocityChange);
      rigidbody.AddTorque(-1f * num3 * rhs3, ForceMode.VelocityChange);
    }
    else
    {
      if ((double) this.speed == 0.0 && this.off_when_zero)
        return;
      if (this.zero_other_velocities)
      {
        rigidbody.AddTorque(-1f * num2 * rhs2 / this.force_divider, ForceMode.VelocityChange);
        rigidbody.AddTorque(-1f * num3 * rhs3 / this.force_divider, ForceMode.VelocityChange);
      }
      if (this.only_additive_force && (double) num4 < 0.0)
        return;
      rigidbody.AddTorque(rhs1 * num4 / this.force_divider, ForceMode.VelocityChange);
    }
  }

  private void DoTorqueDeprecated(Rigidbody rigidbody)
  {
    if (this.hard_stop)
    {
      rigidbody.angularVelocity = this.myVelocity;
      rigidbody.AddTorque(this.transform.TransformDirection(500f * this.speed, 0.0f, 0.0f), ForceMode.Acceleration);
    }
    else
    {
      if ((double) this.speed == 0.0 && this.off_when_zero)
        return;
      rigidbody.AddTorque(this.transform.TransformDirection(-1f, 0.0f, 0.0f) * this.speed / this.force_divider, ForceMode.VelocityChange);
      if (this.only_additive_force)
        return;
      Vector3 vector3_1 = rigidbody.angularVelocity - this.myVelocity;
      float num1 = vector3_1.magnitude;
      if ((double) num1 < 9.99999997475243E-07)
        num1 = 1E-06f;
      float num2 = this.speed / num1;
      vector3_1 *= (float) (1.0 + ((double) num2 - 1.0) / (double) this.force_divider);
      Vector3 vector3_2 = vector3_1 + this.myVelocity;
      rigidbody.angularVelocity = vector3_2;
    }
  }

  private void OnTriggerEnter(Collider collision)
  {
    if (!(bool) (Object) collision.gameObject || !(bool) (Object) collision.gameObject.GetComponent<gameElement>() && !(bool) (Object) collision.gameObject.GetComponentInParent<gameElement>())
      return;
    if (this.play_sound)
    {
      this.playsound = true;
      this.item_triggering = collision;
    }
    this.CleanUpList();
  }

  private void OnTriggerExit(Collider collision)
  {
    if (!(bool) (Object) collision.gameObject)
      return;
    gameElement gameElement = collision.gameObject.GetComponent<gameElement>();
    if (!(bool) (Object) gameElement)
      gameElement = collision.gameObject.GetComponentInParent<gameElement>();
    if (!(bool) (Object) gameElement)
      return;
    this.objects_inside.Remove(gameElement);
  }

  private void CleanUpList()
  {
    for (int index = this.objects_inside.Count - 1; index >= 0; --index)
    {
      if (!(bool) (Object) this.objects_inside[index])
        this.objects_inside.RemoveAt(index);
      else if (!this.objects_inside[index].gameObject.activeSelf)
        this.objects_inside.RemoveAt(index);
    }
  }

  public gameElement GetBallInside()
  {
    this.CleanUpList();
    if (this.objects_inside.Count <= 0)
      return (gameElement) null;
    gameElement ballInside = this.objects_inside[0];
    this.objects_inside.RemoveAt(0);
    return ballInside;
  }

  public bool AnyBallsInside() => this.objects_inside.Count > 0;
}
