// Decompiled with JetBrains decompiler
// Type: SS_scoringBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class SS_scoringBox : MonoBehaviour
{
  private Skystone_Settings ss_settings;
  public bool isRed;
  public List<gameElement> collisions = new List<gameElement>();
  public List<int> collisions_count = new List<int>();
  public int score;
  public int penalties;
  public bool foundation_box;
  public bool non_scoring;
  public bool reset_enemies = true;
  public List<Transform> friends = new List<Transform>();
  private List<int> friends_collisions = new List<int>();
  public float block_height = 0.2f;

  private void Start()
  {
    if (!(bool) (UnityEngine.Object) GameObject.Find("GameSettings"))
      return;
    this.ss_settings = GameObject.Find("GameSettings").GetComponent<Skystone_Settings>();
  }

  public void Reset()
  {
    this.score = 0;
    this.penalties = 0;
  }

  private void Update()
  {
    if (GLOBALS.CLIENT_MODE || !((UnityEngine.Object) this.ss_settings == (UnityEngine.Object) null))
      return;
    this.ss_settings = GameObject.Find("GameSettings").GetComponent<Skystone_Settings>();
  }

  private gameElement FindGameElement(Collider collision)
  {
    Transform transform = collision.transform;
    gameElement component;
    for (component = transform.GetComponent<gameElement>(); (UnityEngine.Object) component == (UnityEngine.Object) null && (UnityEngine.Object) transform.parent != (UnityEngine.Object) null; component = transform.GetComponent<gameElement>())
      transform = transform.parent;
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return (gameElement) null;
    return component.type == ElementType.Cube ? component : (gameElement) null;
  }

  private void OnTriggerEnter(Collider collision)
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    this.RemoveInvalidItems();
    if (collision.transform.name.StartsWith("collisionBoundry") || (UnityEngine.Object) collision.GetComponent<Collider>() == (UnityEngine.Object) null)
      return;
    if (this.reset_enemies && this.IsEnemy(collision))
    {
      if (collision.transform.root.GetComponent<RobotInterface3D>().GetNeedsReset())
        return;
      if (this.ss_settings.ENABLE_SKYBRIDGE_PENALTIES)
        collision.transform.root.GetComponent<RobotInterface3D>().MarkForReset(this.ss_settings.RESET_DURATION_UNDER_SKYBRIDGE);
      this.penalties += this.foundation_box ? this.ss_settings.PENALTY_SCORING : this.ss_settings.PENALTY_SKYBRIDGE;
    }
    else
    {
      if (this.AddFriend(collision))
        return;
      gameElement gameElement = this.FindGameElement(collision);
      if ((UnityEngine.Object) gameElement == (UnityEngine.Object) null)
        return;
      if (!this.foundation_box && !this.non_scoring)
        gameElement.GetComponent<block_score_data>().StartTransition(this.transform.position.x);
      if (this.collisions.Contains(gameElement))
      {
        ++this.collisions_count[this.collisions.IndexOf(gameElement)];
      }
      else
      {
        this.collisions.Add(gameElement);
        this.collisions_count.Add(1);
      }
    }
  }

  public bool IsFriendInside(Transform friend) => this.friends.Contains(friend);

  private bool IsEnemy(Collider collision)
  {
    if (collision.name.Equals("Body"))
      return false;
    RobotID component = collision.transform.root.GetComponent<RobotID>();
    return !((UnityEngine.Object) component == (UnityEngine.Object) null) && (!component.starting_pos.ToString().StartsWith("Red") || !this.isRed) && (!component.starting_pos.ToString().StartsWith("Blue") || this.isRed);
  }

  private bool AddFriend(Collider collision)
  {
    Transform root = collision.transform.root;
    RobotID component = root.GetComponent<RobotID>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || (!component.starting_pos.ToString().StartsWith("Blue") || this.isRed) && (!component.starting_pos.ToString().StartsWith("Red") || !this.isRed))
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
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || (!component.starting_pos.ToString().StartsWith("Blue") || this.isRed) && (!component.starting_pos.ToString().StartsWith("Red") || !this.isRed))
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
    if (GLOBALS.CLIENT_MODE)
      return;
    this.RemoveInvalidItems();
    if (this.RemoveFriend(collision))
      return;
    gameElement gameElement = this.FindGameElement(collision);
    if ((UnityEngine.Object) gameElement == (UnityEngine.Object) null)
      return;
    if (!this.foundation_box && !this.non_scoring)
      this.score += gameElement.GetComponent<block_score_data>().EndTransition(this.transform.position.x);
    if (!this.collisions.Contains(gameElement))
      return;
    if (this.foundation_box)
    {
      int index = this.collisions.IndexOf(gameElement);
      --this.collisions_count[index];
      if (this.collisions_count[index] > 0)
        return;
      this.collisions_count.RemoveAt(index);
      this.collisions.RemoveAt(index);
    }
    else
    {
      if (!gameElement.GetComponent<block_score_data>().OutOfRegion())
        return;
      this.collisions.Remove(gameElement);
    }
  }

  public int GetElementCount() => this.collisions.Count;

  public int GetHighestBlock()
  {
    if (this.non_scoring)
      return 0;
    int highestBlock = 0;
    foreach (gameElement collision in this.collisions)
    {
      block_score_data component = collision.GetComponent<block_score_data>();
      if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && component.currposition != PositionState.off && component.held_by_robot <= 0)
      {
        int num = (int) Math.Floor(((double) collision.transform.position.y - ((double) this.transform.position.y - (double) this.transform.localScale.y / 2.0)) / (double) this.block_height + (double) this.block_height) + 1;
        if (num > highestBlock)
          highestBlock = num;
      }
    }
    return highestBlock;
  }

  public bool IsGameElementInside(gameElement item) => this.collisions.Contains(item);

  private void RemoveInvalidItems()
  {
    for (int index = this.collisions.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) this.collisions[index] == (UnityEngine.Object) null)
      {
        this.collisions.RemoveAt(index);
        this.collisions_count.RemoveAt(index);
      }
    }
    for (int index = this.friends.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) this.friends[index] == (UnityEngine.Object) null)
      {
        this.friends.RemoveAt(index);
        this.friends_collisions.RemoveAt(index);
      }
    }
  }
}
