// Decompiled with JetBrains decompiler
// Type: RobotCounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class RobotCounter : MonoBehaviour
{
  public List<RobotID> collisions = new List<RobotID>();
  public List<int> collisions_count = new List<int>();

  private void Start()
  {
  }

  public void Reset()
  {
  }

  private void Update()
  {
    int num = GLOBALS.CLIENT_MODE ? 1 : 0;
  }

  private RobotID FindRobotID(Collider collision)
  {
    Transform transform = collision.transform;
    RobotID component;
    for (component = transform.GetComponent<RobotID>(); (Object) component == (Object) null && (Object) transform.parent != (Object) null; component = transform.GetComponent<RobotID>())
      transform = transform.parent;
    return component;
  }

  private void OnTriggerEnter(Collider collision)
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    this.RemoveInvalidItems();
    RobotID robotId = this.FindRobotID(collision);
    if (!(bool) (Object) robotId)
      return;
    if (this.collisions.Contains(robotId))
    {
      ++this.collisions_count[this.collisions.IndexOf(robotId)];
    }
    else
    {
      this.collisions.Add(robotId);
      this.collisions_count.Add(1);
    }
  }

  private void OnTriggerExit(Collider collision)
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    this.RemoveInvalidItems();
    RobotID robotId = this.FindRobotID(collision);
    if (!(bool) (Object) robotId || !this.collisions.Contains(robotId))
      return;
    int index = this.collisions.IndexOf(robotId);
    --this.collisions_count[index];
    if (this.collisions_count[index] > 0)
      return;
    this.collisions_count.RemoveAt(index);
    this.collisions.RemoveAt(index);
  }

  public int GetRobotCount() => this.collisions.Count;

  public int GetBlueRobotCount()
  {
    int blueRobotCount = 0;
    foreach (RobotID collision in this.collisions)
    {
      if (!collision.is_red)
        ++blueRobotCount;
    }
    return blueRobotCount;
  }

  public int GetRedRobotCount()
  {
    int redRobotCount = 0;
    foreach (RobotID collision in this.collisions)
    {
      if (collision.is_red)
        ++redRobotCount;
    }
    return redRobotCount;
  }

  private void RemoveInvalidItems()
  {
    for (int index = this.collisions.Count - 1; index >= 0; --index)
    {
      if ((Object) this.collisions[index] == (Object) null)
      {
        this.collisions.RemoveAt(index);
        this.collisions_count.RemoveAt(index);
      }
    }
  }
}
