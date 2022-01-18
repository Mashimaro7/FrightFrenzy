// Decompiled with JetBrains decompiler
// Type: IR_fieldtracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class IR_fieldtracker : MonoBehaviour
{
  private InfiniteRecharge_Settings ir_settings;
  public bool isRed;
  public bool everyone_is_friend;
  public List<gameElement> collisions = new List<gameElement>();
  public bool reset_enemies;
  public bool disable_in_client = true;
  public List<Transform> friends = new List<Transform>();
  private List<int> friends_collisions = new List<int>();

  private void Start()
  {
    if (this.everyone_is_friend)
      return;
    this.ir_settings = GameObject.Find("GameSettings").GetComponent<InfiniteRecharge_Settings>();
  }

  public void Reset()
  {
  }

  private void Update()
  {
    if (this.everyone_is_friend || !((Object) this.ir_settings == (Object) null))
      return;
    this.ir_settings = GameObject.Find("GameSettings").GetComponent<InfiniteRecharge_Settings>();
  }

  private void OnTriggerEnter(Collider collision)
  {
    if (this.disable_in_client && GLOBALS.CLIENT_MODE)
      return;
    this.RemoveInvalidItems();
    if (!this.everyone_is_friend && this.reset_enemies && this.IsEnemy(collision))
    {
      if (collision.transform.root.GetComponent<RobotInterface3D>().GetNeedsReset() || !this.ir_settings.ENABLE_BLOCKING)
        return;
      collision.transform.root.GetComponent<RobotInterface3D>().MarkForReset(this.ir_settings.RESET_DURATION_UNDER_SKYBRIDGE);
    }
    else
      this.AddFriend(collision);
  }

  public bool IsFriendInside(Transform friend) => this.friends.Contains(friend);

  public float GetClosestDistance(Vector3 point)
  {
    float closestDistance = 99999f;
    foreach (Collider componentsInChild in this.GetComponentsInChildren<Collider>())
    {
      Vector3 b = componentsInChild.ClosestPoint(point);
      if ((double) Vector3.Distance(point, b) < (double) closestDistance)
        closestDistance = Vector3.Distance(point, b);
    }
    return closestDistance;
  }

  private bool IsEnemy(Collider collision)
  {
    if (this.everyone_is_friend || collision.name.Equals("Body"))
      return false;
    RobotID component = collision.transform.root.GetComponent<RobotID>();
    return !((Object) component == (Object) null) && (!component.starting_pos.ToString().StartsWith("Red") || !this.isRed) && (!component.starting_pos.ToString().StartsWith("Blue") || this.isRed);
  }

  private bool AddFriend(Collider collision)
  {
    Transform root = collision.transform.root;
    RobotID component = root.GetComponent<RobotID>();
    if ((Object) component == (Object) null || !this.everyone_is_friend && (!component.starting_pos.ToString().StartsWith("Blue") || this.isRed) && (!component.starting_pos.ToString().StartsWith("Red") || !this.isRed))
      return false;
    int index = this.friends.IndexOf(root);
    if (index >= 0)
    {
      ++this.friends_collisions[index];
    }
    else
    {
      this.friends.Add(root);
      this.friends_collisions.Add(1);
    }
    return true;
  }

  private bool RemoveFriend(Collider collision)
  {
    Transform root = collision.transform.root;
    RobotID component = root.GetComponent<RobotID>();
    if ((Object) component == (Object) null || !this.everyone_is_friend && (!component.starting_pos.ToString().StartsWith("Blue") || this.isRed) && (!component.starting_pos.ToString().StartsWith("Red") || !this.isRed))
      return false;
    int index = this.friends.IndexOf(root);
    if (index >= 0)
    {
      --this.friends_collisions[index];
      if (this.friends_collisions[index] <= 0)
      {
        this.friends.RemoveAt(index);
        this.friends_collisions.RemoveAt(index);
      }
    }
    return true;
  }

  private void OnTriggerExit(Collider collision)
  {
    if (this.disable_in_client && GLOBALS.CLIENT_MODE)
      return;
    this.RemoveInvalidItems();
    this.RemoveFriend(collision);
  }

  private void RemoveInvalidItems()
  {
    for (int index = this.friends.Count - 1; index >= 0; --index)
    {
      if ((Object) this.friends[index] == (Object) null)
      {
        this.friends.RemoveAt(index);
        this.friends_collisions.RemoveAt(index);
      }
    }
  }
}
