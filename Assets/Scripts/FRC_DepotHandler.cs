// Decompiled with JetBrains decompiler
// Type: FRC_DepotHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class FRC_DepotHandler : MonoBehaviour
{
  private InfiniteRecharge_Settings ir_settings;
  public bool isRed;
  public bool keep_out_enemies;
  public List<RobotInterface3D> enemies = new List<RobotInterface3D>();
  private List<int> enemies_collisions = new List<int>();
  public List<RobotInterface3D> friends = new List<RobotInterface3D>();
  private List<int> friends_collisions = new List<int>();

  private void Start() => this.ir_settings = GameObject.Find("GameSettings").GetComponent<InfiniteRecharge_Settings>();

  private void FixedUpdate()
  {
    if (!this.keep_out_enemies || !this.ir_settings.ENABLE_DEPOT_PENALTIES)
      return;
    this.PushOppositePlayers();
  }

  private void Update()
  {
    if (!((Object) this.ir_settings == (Object) null))
      return;
    this.ir_settings = GameObject.Find("GameSettings").GetComponent<InfiniteRecharge_Settings>();
  }

  public void Reset() => this.RemoveInvalidItems();

  public void OnTriggerEnter(Collider collision)
  {
    this.RemoveInvalidItems();
    this.AddEnemy(collision);
    this.AddFriend(collision);
  }

  public void OnTriggerExit(Collider collision)
  {
    this.RemoveInvalidItems();
    this.RemoveEnemy(collision);
    this.RemoveFriend(collision);
  }

  private void PushOppositePlayers()
  {
    foreach (Component enemy in this.enemies)
    {
      Rigidbody component = enemy.transform.Find("Body").GetComponent<Rigidbody>();
      if ((Object) component == (Object) null)
        break;
      component.velocity = Vector3.zero;
      component.angularVelocity = Vector3.zero;
      component.AddForce((component.transform.position - this.transform.position).normalized * 200f, ForceMode.Acceleration);
    }
  }

  private bool IsEnemy(Collider collision)
  {
    RobotID component = collision.transform.root.GetComponent<RobotID>();
    return !((Object) component == (Object) null) && (!component.starting_pos.ToString().StartsWith("Red") || !this.isRed) && (!component.starting_pos.ToString().StartsWith("Blue") || this.isRed);
  }

  private bool IsFriend(Collider collision)
  {
    RobotID component = collision.transform.root.GetComponent<RobotID>();
    return !((Object) component == (Object) null) && (!component.starting_pos.ToString().StartsWith("Red") || this.isRed) && (!component.starting_pos.ToString().StartsWith("Blue") || !this.isRed);
  }

  private bool AddEnemy(Collider collision)
  {
    if (!this.IsEnemy(collision))
      return false;
    Transform root = collision.transform.root;
    root.GetComponent<RobotID>();
    RobotInterface3D component = root.GetComponent<RobotInterface3D>();
    int index = this.enemies.IndexOf(component);
    if (index >= 0)
    {
      ++this.enemies_collisions[index];
    }
    else
    {
      this.enemies.Add(component);
      this.enemies_collisions.Add(1);
    }
    return true;
  }

  private bool AddFriend(Collider collision)
  {
    if (!this.IsFriend(collision))
      return false;
    Transform root = collision.transform.root;
    root.GetComponent<RobotID>();
    RobotInterface3D component = root.GetComponent<RobotInterface3D>();
    int index = this.friends.IndexOf(component);
    if (index >= 0)
    {
      ++this.friends_collisions[index];
    }
    else
    {
      this.friends.Add(component);
      this.friends_collisions.Add(1);
    }
    return true;
  }

  private bool RemoveEnemy(Collider collision)
  {
    if (!this.IsEnemy(collision))
      return false;
    Transform root = collision.transform.root;
    root.GetComponent<RobotID>();
    int index = this.enemies.IndexOf(root.GetComponent<RobotInterface3D>());
    if (index >= 0)
    {
      --this.enemies_collisions[index];
      if (this.enemies_collisions[index] <= 0)
      {
        this.enemies.RemoveAt(index);
        this.enemies_collisions.RemoveAt(index);
      }
    }
    return true;
  }

  private bool RemoveFriend(Collider collision)
  {
    if (!this.IsFriend(collision))
      return false;
    Transform root = collision.transform.root;
    root.GetComponent<RobotID>();
    int index = this.friends.IndexOf(root.GetComponent<RobotInterface3D>());
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

  private void RemoveInvalidItems()
  {
    for (int index = this.enemies.Count - 1; index >= 0; --index)
    {
      if ((Object) this.enemies[index] == (Object) null)
      {
        this.enemies.RemoveAt(index);
        this.enemies_collisions.RemoveAt(index);
      }
    }
    for (int index = this.friends.Count - 1; index >= 0; --index)
    {
      if ((Object) this.friends[index] == (Object) null)
      {
        this.friends.RemoveAt(index);
        this.friends_collisions.RemoveAt(index);
      }
    }
  }

  public bool GetEnemiesColliding()
  {
    this.RemoveInvalidItems();
    return this.enemies.Count > 0;
  }

  public bool GetFriendsColliding()
  {
    this.RemoveInvalidItems();
    return this.friends.Count > 0;
  }

  public List<RobotInterface3D> GetAllEnemies()
  {
    this.RemoveInvalidItems();
    return this.enemies;
  }

  public List<RobotInterface3D> GetAllFriends()
  {
    this.RemoveInvalidItems();
    return this.friends;
  }

  public bool IsFriendInside(RobotInterface3D friend) => this.friends.Contains(friend);

  public bool IsEnemyInside(RobotInterface3D friend) => this.enemies.Contains(friend);

  public void Clear()
  {
    this.enemies.Clear();
    this.enemies_collisions.Clear();
    this.friends.Clear();
    this.friends_collisions.Clear();
    this.RemoveInvalidItems();
  }
}
