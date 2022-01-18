// Decompiled with JetBrains decompiler
// Type: SS_gameidcounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SS_gameidcounter : MonoBehaviour
{
  public Dictionary<int, gameElement> collisions = new Dictionary<int, gameElement>();
  public Dictionary<int, int> collisions_count = new Dictionary<int, int>();
  public int count;

  private void Update() => this.count = this.collisions.Count;

  private void OnDisable() => this.Reset();

  private void OnDestroy() => this.Reset();

  public void Reset()
  {
    foreach (gameElement gameElement in this.collisions.Values)
    {
      if ((bool) (Object) gameElement && gameElement.enabled && gameElement.held_by_robot > 0)
        --gameElement.held_by_robot;
    }
    this.collisions.Clear();
    this.collisions_count.Clear();
  }

  private void OnTriggerEnter(Collider collision)
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    this.RemoveInvalidItems();
    gameElement gameElement = this.FindGameElement(collision);
    if ((Object) gameElement == (Object) null || gameElement.type != ElementType.Cube)
      return;
    if (this.collisions.ContainsKey(gameElement.id))
    {
      ++this.collisions_count[gameElement.id];
    }
    else
    {
      this.collisions[gameElement.id] = gameElement;
      this.collisions_count[gameElement.id] = 1;
      ++gameElement.held_by_robot;
    }
  }

  private void OnTriggerExit(Collider collision)
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    this.RemoveInvalidItems();
    gameElement gameElement = this.FindGameElement(collision);
    if ((Object) gameElement == (Object) null || !this.collisions.ContainsKey(gameElement.id))
      return;
    --this.collisions_count[gameElement.id];
    if (this.collisions_count[gameElement.id] > 0)
      return;
    this.collisions.Remove(gameElement.id);
    this.collisions_count.Remove(gameElement.id);
    --gameElement.held_by_robot;
  }

  private gameElement FindGameElement(Collider collision)
  {
    Transform transform = collision.transform;
    gameElement component;
    for (component = transform.GetComponent<gameElement>(); (Object) component == (Object) null && (Object) transform.parent != (Object) null; component = transform.GetComponent<gameElement>())
      transform = transform.parent;
    return component;
  }

  public int GetElementCount() => this.collisions.Count;

  public int GetElements(Dictionary<int, gameElement> elements)
  {
    int elements1 = 0;
    foreach (int key in this.collisions.Keys)
    {
      if (!elements.ContainsKey(key))
      {
        ++elements1;
        elements[key] = this.collisions[key];
      }
    }
    return elements1;
  }

  private void RemoveInvalidItems()
  {
    foreach (int key in this.collisions.Keys)
    {
      if ((Object) this.collisions[key] == (Object) null || (Object) this.collisions[key].gameObject == (Object) null || !this.collisions[key].enabled)
      {
        this.collisions.Remove(key);
        this.collisions_count.Remove(key);
      }
    }
  }
}
