// Decompiled with JetBrains decompiler
// Type: PlatformCounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class PlatformCounter : MonoBehaviour
{
  public List<PlatformID> collisions = new List<PlatformID>();
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

  private PlatformID FindPlatformID(Collider collision)
  {
    Transform transform = collision.transform;
    PlatformID component;
    for (component = transform.GetComponent<PlatformID>(); (Object) component == (Object) null && (Object) transform.parent != (Object) null; component = transform.GetComponent<PlatformID>())
      transform = transform.parent;
    return component;
  }

  private void OnTriggerEnter(Collider collision)
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    this.RemoveInvalidItems();
    PlatformID platformId = this.FindPlatformID(collision);
    if (!(bool) (Object) platformId)
      return;
    if (this.collisions.Contains(platformId))
    {
      ++this.collisions_count[this.collisions.IndexOf(platformId)];
    }
    else
    {
      this.collisions.Add(platformId);
      this.collisions_count.Add(1);
    }
  }

  private void OnTriggerExit(Collider collision)
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    this.RemoveInvalidItems();
    PlatformID platformId = this.FindPlatformID(collision);
    if (!(bool) (Object) platformId || !this.collisions.Contains(platformId))
      return;
    int index = this.collisions.IndexOf(platformId);
    --this.collisions_count[index];
    if (this.collisions_count[index] > 0)
      return;
    this.collisions_count.RemoveAt(index);
    this.collisions.RemoveAt(index);
  }

  public int GetPlatformCount() => this.collisions.Count;

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
