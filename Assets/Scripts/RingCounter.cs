// Decompiled with JetBrains decompiler
// Type: RingCounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class RingCounter : MonoBehaviour
{
  public List<gameElement> collisions = new List<gameElement>();
  public List<int> collisions_count = new List<int>();
  public List<gameElement> collisions_ir = new List<gameElement>();
  public List<int> collisions_ir_count = new List<int>();

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
    return component.type == ElementType.Jewel ? component : (gameElement) null;
  }

  private gameElement FindGameElementFromInnerRingID(Collider collision)
  {
    Transform transform = collision.transform;
    return (bool) (Object) transform.GetComponent<InnerRingID>() || (bool) (Object) transform.GetComponentInParent<InnerRingID>() ? this.FindGameElement(collision) : (gameElement) null;
  }

  private void OnTriggerEnter(Collider collision)
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    this.RemoveInvalidItems();
    gameElement gameElement = this.FindGameElement(collision);
    if ((bool) (Object) gameElement)
    {
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
    gameElement elementFromInnerRingId = this.FindGameElementFromInnerRingID(collision);
    if (!(bool) (Object) elementFromInnerRingId)
      return;
    if (this.collisions_ir.Contains(elementFromInnerRingId))
    {
      ++this.collisions_ir_count[this.collisions_ir.IndexOf(elementFromInnerRingId)];
    }
    else
    {
      this.collisions_ir.Add(elementFromInnerRingId);
      this.collisions_ir_count.Add(1);
    }
  }

  private void OnTriggerExit(Collider collision)
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    this.RemoveInvalidItems();
    gameElement gameElement = this.FindGameElement(collision);
    if ((bool) (Object) gameElement && this.collisions.Contains(gameElement))
    {
      int index = this.collisions.IndexOf(gameElement);
      --this.collisions_count[index];
      if (this.collisions_count[index] <= 0)
      {
        this.collisions_count.RemoveAt(index);
        this.collisions.RemoveAt(index);
      }
    }
    gameElement elementFromInnerRingId = this.FindGameElementFromInnerRingID(collision);
    if (!(bool) (Object) elementFromInnerRingId || !this.collisions_ir.Contains(elementFromInnerRingId))
      return;
    int index1 = this.collisions_ir.IndexOf(elementFromInnerRingId);
    --this.collisions_ir_count[index1];
    if (this.collisions_ir_count[index1] > 0)
      return;
    this.collisions_ir_count.RemoveAt(index1);
    this.collisions_ir.RemoveAt(index1);
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
    for (int index = this.collisions_ir.Count - 1; index >= 0; --index)
    {
      if ((Object) this.collisions_ir[index] == (Object) null)
      {
        this.collisions_ir.RemoveAt(index);
        this.collisions_ir_count.RemoveAt(index);
      }
    }
  }
}
