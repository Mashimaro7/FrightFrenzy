// Decompiled with JetBrains decompiler
// Type: SelectiveWall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SelectiveWall : GenericFieldTracker
{
  public bool disable;
  public bool push_elements = true;
  public ElementType[] elements_to_let_through;
  public bool push_red_bots = true;
  public bool push_blue_bots = true;
  public Vector3 pushback_dir = new Vector3(1f, 0.0f, 0.0f);

  private void FixedUpdate()
  {
    if (this.disable)
      return;
    this.PushObjects();
  }

  private void PushObjects()
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    foreach (RobotID robot in this.robots)
    {
      if (!((Object) robot == (Object) null) && (!robot.is_red || this.push_red_bots) && (robot.is_red || this.push_blue_bots))
      {
        RobotInterface3D component = robot.GetComponent<RobotInterface3D>();
        if (!((Object) component == (Object) null))
        {
          Rigidbody rbBody = component.rb_body;
          if (!((Object) rbBody == (Object) null))
          {
            Vector3 velocity = rbBody.velocity;
            Vector3 angularVelocity = rbBody.angularVelocity;
            rbBody.velocity = Vector3.zero;
            rbBody.angularVelocity = Vector3.zero;
            rbBody.AddForce(this.pushback_dir * 500f, ForceMode.Acceleration);
          }
        }
      }
    }
    if (!this.push_elements)
      return;
    foreach (gameElement gameElement in this.game_elements)
    {
      if (!((Object) gameElement == (Object) null))
      {
        bool flag = false;
        foreach (ElementType elementType in this.elements_to_let_through)
        {
          if (gameElement.type == elementType)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          Rigidbody component = gameElement.GetComponent<Rigidbody>();
          if (!((Object) component == (Object) null))
          {
            component.velocity = Vector3.zero;
            component.angularVelocity = Vector3.zero;
            component.AddForce(this.pushback_dir * 500f, ForceMode.Acceleration);
          }
        }
      }
    }
  }
}
