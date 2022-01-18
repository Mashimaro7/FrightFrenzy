// Decompiled with JetBrains decompiler
// Type: RobotPushBack_TP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class RobotPushBack_TP : MonoBehaviour
{
  private TippingPoint_Settings tp_settings;
  public List<Transform> enemies = new List<Transform>();
  public List<Rigidbody> enemies_ri3d = new List<Rigidbody>();
  private List<int> enemies_collisions = new List<int>();

  private void Start()
  {
    if (!(bool) (Object) GameObject.Find("GameSettings"))
      return;
    this.tp_settings = GameObject.Find("GameSettings").GetComponent<TippingPoint_Settings>();
  }

  private void FixedUpdate()
  {
    if (!(bool) (Object) this.tp_settings || !this.tp_settings.ENABLE_AUTO_PUSHBACK)
      return;
    this.PushPlayers();
  }

  public void Reset() => this.RemoveInvalidItems();

  private void PushPlayers()
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    foreach (Rigidbody rigidbody in this.enemies_ri3d)
    {
      if ((Object) rigidbody == (Object) null)
        break;
      rigidbody.velocity = Vector3.zero;
      rigidbody.angularVelocity = Vector3.zero;
      rigidbody.AddForce((rigidbody.transform.position - this.transform.position).normalized * 200f, ForceMode.Acceleration);
    }
  }

  private bool IsPartOfBody(Transform start)
  {
    bool flag = false;
    if (start.name.StartsWith("collisionBoundry"))
      return false;
    for (; (bool) (Object) start && !flag; start = start.parent)
    {
      if (start.name == "Body")
        flag = true;
    }
    return flag;
  }

  private void RememberEnemies(Collider collision)
  {
    if (!this.IsPartOfBody(collision.transform))
      return;
    Transform root = collision.transform.root;
    RobotID component1 = root.GetComponent<RobotID>();
    RobotInterface3D component2 = root.GetComponent<RobotInterface3D>();
    if ((Object) component1 == (Object) null || !(bool) (Object) component2)
      return;
    Transform transform = component2.transform.Find("Body");
    if (!(bool) (Object) transform)
      return;
    Rigidbody component3 = transform.GetComponent<Rigidbody>();
    if ((Object) component3 == (Object) null)
      return;
    this.enemies.Add(collision.transform);
    int index = this.enemies_ri3d.IndexOf(component3);
    if (index < 0)
    {
      this.enemies_ri3d.Add(component3);
      this.enemies_collisions.Add(1);
    }
    else
      ++this.enemies_collisions[index];
  }

  private bool RemoveEnemy(Collider collision)
  {
    RobotInterface3D component1 = collision.transform.root.GetComponent<RobotInterface3D>();
    if (!(bool) (Object) component1)
      return false;
    Transform transform = component1.transform.Find("Body");
    if (!(bool) (Object) transform)
      return false;
    Rigidbody component2 = transform.GetComponent<Rigidbody>();
    if ((Object) component2 == (Object) null)
      return false;
    int index = this.enemies_ri3d.IndexOf(component2);
    if (index < 0)
      return false;
    --this.enemies_collisions[index];
    if (this.enemies_collisions[index] < 0)
      return false;
    this.enemies_ri3d.RemoveAt(index);
    this.enemies_collisions.RemoveAt(index);
    return true;
  }

  private void OnTriggerEnter(Collider collision)
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    this.RemoveInvalidItems();
    this.RememberEnemies(collision);
  }

  private void OnTriggerExit(Collider collision)
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    if (this.enemies.Contains(collision.transform))
      this.enemies.Remove(collision.transform);
    this.RemoveInvalidItems();
    this.RemoveEnemy(collision);
  }

  private void RemoveInvalidItems()
  {
    for (int index = this.enemies.Count - 1; index >= 0; --index)
    {
      if ((Object) this.enemies[index] == (Object) null)
        this.enemies.RemoveAt(index);
    }
    for (int index = this.enemies_ri3d.Count - 1; index >= 0; --index)
    {
      if ((Object) this.enemies_ri3d[index] == (Object) null)
      {
        this.enemies_ri3d.RemoveAt(index);
        this.enemies_collisions.RemoveAt(index);
      }
    }
  }
}
