// Decompiled with JetBrains decompiler
// Type: GenericFieldTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class GenericFieldTracker : MonoBehaviour
{
  public List<RobotID> robots = new List<RobotID>();
  public List<int> robot_collisions = new List<int>();
  public List<gameElement> game_elements = new List<gameElement>();
  public List<int> game_elements_collisions = new List<int>();
  public bool dont_clear_exited_items;
  public bool disable_in_client = true;
  public bool only_robot_body;
  public bool track_gameelements = true;
  public bool track_robots = true;
  public bool ignore_self;
  public string note = "";
  public gameElement self_ge;
  public RobotID self_ri;

  protected virtual void Start()
  {
    this.self_ge = this.GetComponentInChildren<gameElement>();
    if (!(bool) (Object) this.self_ge)
      this.self_ge = this.GetComponentInParent<gameElement>();
    this.self_ri = this.GetComponentInChildren<RobotID>();
    if ((bool) (Object) this.self_ri)
      return;
    this.self_ri = this.GetComponentInParent<RobotID>();
  }

  public virtual void Reset()
  {
    this.robots.Clear();
    this.robot_collisions.Clear();
    this.game_elements.Clear();
    this.game_elements_collisions.Clear();
  }

  public bool IsGameElementInside(gameElement element) => this.game_elements.Contains(element);

  public bool IsAnyGameElementInside() => this.game_elements.Count > 0;

  public int GetGameElementCount() => this.game_elements.Count;

  public bool IsGameElementInside(Transform element) => this.game_elements.Contains(element.GetComponentInParent<gameElement>());

  public bool IsRobotInside(RobotID robot) => this.robots.Contains(robot);

  public bool IsRobotInside(Transform robot) => this.robots.Contains(robot.GetComponentInParent<RobotID>());

  public bool IsAnyRobotInside() => this.robots.Count > 0;

  public virtual void OnGameElementTriggerEnter(gameElement ge)
  {
  }

  public virtual void OnRobotTriggerEnter(RobotID robot)
  {
  }

  public virtual void OnGameElementTriggerExit(gameElement ge)
  {
  }

  public virtual void OnRobotTriggerExit(RobotID robot)
  {
  }

  private void OnTriggerEnter(Collider collision)
  {
    if (this.disable_in_client && GLOBALS.CLIENT_MODE)
      return;
    this.RemoveInvalidItems();
    if (this.track_gameelements)
    {
      gameElement componentInParent = collision.GetComponentInParent<gameElement>();
      if ((bool) (Object) componentInParent)
      {
        if (this.ignore_self && (bool) (Object) this.self_ge && (Object) componentInParent == (Object) this.self_ge || this.note.Length > 0 && this.note != componentInParent.note)
          return;
        this.AddGameElement(componentInParent);
        this.OnGameElementTriggerEnter(componentInParent);
        return;
      }
    }
    if (!this.track_robots)
      return;
    RobotID componentInParent1 = collision.GetComponentInParent<RobotID>();
    if (!(bool) (Object) componentInParent1 || this.only_robot_body && !(collision.attachedRigidbody.transform.name == "Body") || collision.transform.name == "collisionBoundry" || this.ignore_self && (bool) (Object) this.self_ri && (Object) componentInParent1 == (Object) this.self_ri)
      return;
    this.AddRobot(componentInParent1);
    this.OnRobotTriggerEnter(componentInParent1);
  }

  private bool AddRobot(RobotID robotElement)
  {
    int index = this.robots.IndexOf(robotElement);
    if (index >= 0)
    {
      ++this.robot_collisions[index];
    }
    else
    {
      this.robots.Add(robotElement);
      this.robot_collisions.Add(1);
    }
    return true;
  }

  private bool AddGameElement(gameElement game_element)
  {
    int index = this.game_elements.IndexOf(game_element);
    if (index >= 0)
    {
      ++this.game_elements_collisions[index];
    }
    else
    {
      this.game_elements.Add(game_element);
      this.game_elements_collisions.Add(1);
    }
    return true;
  }

  private bool RemoveExitedRobot(RobotID robotElement)
  {
    int index = this.robots.IndexOf(robotElement);
    if (index >= 0)
    {
      --this.robot_collisions[index];
      if (!this.dont_clear_exited_items && this.robot_collisions[index] <= 0)
      {
        this.robots.RemoveAt(index);
        this.robot_collisions.RemoveAt(index);
        this.RobotExited(robotElement);
        return true;
      }
    }
    return false;
  }

  private bool RemoveExitedGameElements(gameElement robotElement)
  {
    int index = this.game_elements.IndexOf(robotElement);
    if (index >= 0)
    {
      --this.game_elements_collisions[index];
      if (!this.dont_clear_exited_items && this.game_elements_collisions[index] <= 0)
      {
        this.game_elements.RemoveAt(index);
        this.game_elements_collisions.RemoveAt(index);
        this.GameElementExited(robotElement);
        return true;
      }
    }
    return false;
  }

  public virtual void RobotExited(RobotID robotid)
  {
  }

  public virtual void GameElementExited(gameElement gamelement)
  {
  }

  private void OnTriggerExit(Collider collision)
  {
    if (this.disable_in_client && GLOBALS.CLIENT_MODE)
      return;
    this.RemoveInvalidItems();
    if (this.track_gameelements)
    {
      gameElement componentInParent = collision.GetComponentInParent<gameElement>();
      if ((bool) (Object) componentInParent)
      {
        this.RemoveExitedGameElements(componentInParent);
        this.OnGameElementTriggerExit(componentInParent);
        return;
      }
    }
    if (!this.track_robots)
      return;
    RobotID componentInParent1 = collision.GetComponentInParent<RobotID>();
    if (!(bool) (Object) componentInParent1 || this.only_robot_body && !(collision.attachedRigidbody.transform.name == "Body") || collision.transform.name == "collisionBoundry")
      return;
    this.RemoveExitedRobot(componentInParent1);
    this.OnRobotTriggerExit(componentInParent1);
  }

  public void ClearExitedItem() => this.RemoveInvalidItems();

  private void RemoveInvalidItems()
  {
    for (int index = this.robots.Count - 1; index >= 0; --index)
    {
      if ((Object) this.robots[index] == (Object) null || this.robot_collisions[index] <= 0)
      {
        this.robots.RemoveAt(index);
        this.robot_collisions.RemoveAt(index);
      }
    }
    for (int index = this.game_elements.Count - 1; index >= 0; --index)
    {
      if ((Object) this.game_elements[index] == (Object) null || this.game_elements_collisions[index] <= 0)
      {
        this.game_elements.RemoveAt(index);
        this.game_elements_collisions.RemoveAt(index);
      }
    }
  }
}
