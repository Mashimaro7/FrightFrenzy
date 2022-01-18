// Decompiled with JetBrains decompiler
// Type: Generic_Robot_Pushback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class Generic_Robot_Pushback : MonoBehaviour
{
  public bool enable = true;
  public bool push_only_body = true;
  public bool push_red;
  public bool push_blue;
  public float force_scale = 200f;
  public List<RobotID> bots_red = new List<RobotID>();
  private List<int> bots_red_count = new List<int>();
  public List<RobotID> bots_blue = new List<RobotID>();
  private List<int> bots_blue_count = new List<int>();

  private void Start()
  {
  }

  private void FixedUpdate() => this.PushRobots();

  private void Update()
  {
  }

  public void Reset() => this.RemoveInvalidItems();

  public void OnTriggerEnter(Collider collision)
  {
    this.RemoveInvalidItems();
    this.AddBot(collision);
  }

  public void OnTriggerExit(Collider collision)
  {
    this.RemoveInvalidItems();
    this.RemoveBot(collision);
  }

  private void PushRobots()
  {
    if (!this.enable)
      return;
    if (this.push_red)
    {
      foreach (RobotID bot in this.bots_red)
        this.PushBot(bot);
    }
    if (!this.push_blue)
      return;
    foreach (RobotID bot in this.bots_blue)
      this.PushBot(bot);
  }

  private void PushBot(RobotID bot)
  {
    if (this.push_only_body)
    {
      Transform transform = bot.transform.Find("Body");
      if (!(bool) (Object) transform)
        return;
      Rigidbody component = transform.GetComponent<Rigidbody>();
      if ((Object) component == (Object) null)
        return;
      component.velocity = Vector3.zero;
      component.angularVelocity = Vector3.zero;
      component.AddForce((component.transform.position - this.transform.position).normalized * this.force_scale, ForceMode.Acceleration);
    }
    else
    {
      foreach (Rigidbody componentsInChild in bot.GetComponentsInChildren<Rigidbody>())
      {
        componentsInChild.velocity = Vector3.zero;
        componentsInChild.angularVelocity = Vector3.zero;
        componentsInChild.AddForce((componentsInChild.transform.position - this.transform.position).normalized * this.force_scale, ForceMode.Acceleration);
      }
    }
  }

  private RobotID GetRobotID(Collider collision, bool isRed, bool isBlue)
  {
    RobotID component = collision.transform.root.GetComponent<RobotID>();
    if ((Object) component == (Object) null)
      return (RobotID) null;
    return component.is_red & isRed || !component.is_red & isBlue ? component : (RobotID) null;
  }

  private bool AddBot(Collider collision)
  {
    RobotID robotId = this.GetRobotID(collision, true, true);
    if (!(bool) (Object) robotId)
      return false;
    List<RobotID> robotIdList = robotId.is_red ? this.bots_red : this.bots_blue;
    List<int> intList = robotId.is_red ? this.bots_red_count : this.bots_blue_count;
    int index = robotIdList.IndexOf(robotId);
    if (index >= 0)
    {
      ++intList[index];
    }
    else
    {
      robotIdList.Add(robotId);
      intList.Add(1);
    }
    return true;
  }

  private bool RemoveBot(Collider collision)
  {
    RobotID robotId = this.GetRobotID(collision, true, true);
    if (!(bool) (Object) robotId)
      return false;
    List<RobotID> robotIdList = robotId.is_red ? this.bots_red : this.bots_blue;
    List<int> intList = robotId.is_red ? this.bots_red_count : this.bots_blue_count;
    int index = robotIdList.IndexOf(robotId);
    if (index >= 0)
    {
      --intList[index];
      if (intList[index] <= 0)
      {
        robotIdList.RemoveAt(index);
        intList.RemoveAt(index);
      }
    }
    return true;
  }

  private void RemoveInvalidItems()
  {
    for (int index = this.bots_red.Count - 1; index >= 0; --index)
    {
      if ((Object) this.bots_red[index] == (Object) null)
      {
        this.bots_red.RemoveAt(index);
        this.bots_red_count.RemoveAt(index);
      }
    }
    for (int index = this.bots_blue.Count - 1; index >= 0; --index)
    {
      if ((Object) this.bots_blue[index] == (Object) null)
      {
        this.bots_blue.RemoveAt(index);
        this.bots_blue_count.RemoveAt(index);
      }
    }
  }

  public bool IsBotInsdide(RobotID bot) => this.bots_blue.Contains(bot) || this.bots_red.Contains(bot);

  public void Clear()
  {
    this.bots_red.Clear();
    this.bots_red_count.Clear();
    this.bots_blue.Clear();
    this.bots_blue_count.Clear();
    this.RemoveInvalidItems();
  }
}
