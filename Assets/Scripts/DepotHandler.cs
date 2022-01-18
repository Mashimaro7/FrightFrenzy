// Decompiled with JetBrains decompiler
// Type: DepotHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class DepotHandler : MonoBehaviour
{
  private Skystone_Settings ss_settings;
  public bool isRed;
  public int curr_block = 1;
  public GameObject block_drop_pos;
  public List<Transform> collisions = new List<Transform>();
  public List<Transform> enemies = new List<Transform>();
  public List<Rigidbody> enemies_ri3d = new List<Rigidbody>();
  private List<int> enemies_collisions = new List<int>();
  private long block_blanking;

  private void Start()
  {
    this.Add_Block();
    if (!(bool) (Object) GameObject.Find("GameSettings"))
      return;
    this.ss_settings = GameObject.Find("GameSettings").GetComponent<Skystone_Settings>();
  }

  private void FixedUpdate()
  {
    if (!(bool) (Object) this.ss_settings || !this.ss_settings.ENABLE_DEPOT_PENALTIES)
      return;
    this.PushOppositePlayers();
  }

  private void Update()
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    if ((Object) this.ss_settings == (Object) null)
      this.ss_settings = GameObject.Find("GameSettings").GetComponent<Skystone_Settings>();
    if (this.collisions.Count == 0 && this.curr_block == 1 && this.block_blanking >= 10L)
      this.Add_Block();
    if (this.block_blanking >= 10L)
      return;
    ++this.block_blanking;
  }

  public void Reset()
  {
    this.curr_block = 1;
    this.RemoveInvalidItems();
  }

  private void PushOppositePlayers()
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    foreach (Rigidbody rigidbody in this.enemies_ri3d)
    {
      if ((Object) rigidbody == (Object) null)
        break;
      rigidbody.velocity = Vector3.zero;
      rigidbody.angularVelocity = Vector3.zero;
      Vector3 vector3 = (rigidbody.transform.position - this.transform.position) with
      {
        y = 0.0f
      };
      rigidbody.AddForce(vector3.normalized * 0.5f, ForceMode.VelocityChange);
    }
  }

  private void RememberEnemies(Collider collision)
  {
    Transform root = collision.transform.root;
    RobotID component1 = root.GetComponent<RobotID>();
    RobotInterface3D component2 = root.GetComponent<RobotInterface3D>();
    if ((Object) component1 == (Object) null || !(bool) (Object) component2 || (!component1.starting_pos.ToString().StartsWith("Red") || this.isRed) && (!component1.starting_pos.ToString().StartsWith("Blue") || !this.isRed))
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
    if (this.enemies_collisions[index] > 0)
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
    if (collision.transform.name.StartsWith("collisionBoundry"))
      return;
    this.collisions.Add(collision.transform);
    this.RememberEnemies(collision);
  }

  private void OnTriggerExit(Collider collision)
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    if (this.collisions.Contains(collision.transform))
      this.collisions.Remove(collision.transform);
    if (this.enemies.Contains(collision.transform))
      this.enemies.Remove(collision.transform);
    this.RemoveInvalidItems();
    this.RemoveEnemy(collision);
    if (this.collisions.Count != 0 || this.curr_block > 24)
      return;
    this.Add_Block();
  }

  private void Add_Block()
  {
    if (this.block_blanking < 10L)
      return;
    this.block_blanking = 0L;
    GameObject gameObject = !this.isRed ? GameObject.Find("Extra Blocks Blue/block_extra (" + (object) this.curr_block + ")") : GameObject.Find("Extra Blocks Red/block_extra (" + (object) this.curr_block + ")");
    gameObject.transform.position = this.block_drop_pos.transform.position;
    gameObject.transform.rotation = this.block_drop_pos.transform.rotation;
    gameObject.GetComponent<Rigidbody>().isKinematic = false;
    gameObject.GetComponent<block_score_data>().currposition = PositionState.Loading;
    ++this.curr_block;
  }

  private void RemoveInvalidItems()
  {
    for (int index = this.collisions.Count - 1; index >= 0; --index)
    {
      if ((Object) this.collisions[index] == (Object) null)
        this.collisions.RemoveAt(index);
    }
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
