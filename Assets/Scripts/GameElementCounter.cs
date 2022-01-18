// Decompiled with JetBrains decompiler
// Type: GameElementCounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class GameElementCounter : MonoBehaviour
{
  public List<gameElement> collisions = new List<gameElement>();
  public List<int> collisions_count = new List<int>();
  public ElementType filter_element = ElementType.Off;

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

  private gameElement FindGameElement(Collider collision)
  {
    Transform transform = collision.transform;
    gameElement component;
    for (component = transform.GetComponent<gameElement>(); (Object) component == (Object) null && (Object) transform.parent != (Object) null; component = transform.GetComponent<gameElement>())
      transform = transform.parent;
    if ((Object) component == (Object) null)
      return (gameElement) null;
    return this.filter_element == ElementType.Off || component.type == this.filter_element ? component : (gameElement) null;
  }

  private void OnTriggerEnter(Collider collision)
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    this.RemoveInvalidItems();
    gameElement gameElement = this.FindGameElement(collision);
    if (!(bool) (Object) gameElement)
      return;
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

  private void OnTriggerExit(Collider collision)
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    this.RemoveInvalidItems();
    gameElement gameElement = this.FindGameElement(collision);
    if (!(bool) (Object) gameElement || !this.collisions.Contains(gameElement))
      return;
    int index = this.collisions.IndexOf(gameElement);
    --this.collisions_count[index];
    if (this.collisions_count[index] > 0)
      return;
    this.collisions_count.RemoveAt(index);
    this.collisions.RemoveAt(index);
  }

  public int GetElementCount() => this.collisions.Count;

  public bool IsGameElementInside(gameElement item) => this.collisions.Contains(item);

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
