// Decompiled with JetBrains decompiler
// Type: PhysicsDebug.DelayDestroy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

namespace PhysicsDebug
{
  public class DelayDestroy : MonoBehaviour
  {
    public float delay = 7.5f;

    public void StartUp() => this.StartCoroutine(this.DestroyCo());

    private IEnumerator DestroyCo()
    {
      // ISSUE: reference to a compiler-generated field
      int num = this.\u003C\u003E1__state;
      DelayDestroy delayDestroy = this;
      if (num != 0)
      {
        if (num != 1)
          return false;
        // ISSUE: reference to a compiler-generated field
        this.\u003C\u003E1__state = -1;
        Object.Destroy((Object) delayDestroy.gameObject);
        return false;
      }
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E2__current = (object) new WaitForSeconds(delayDestroy.delay);
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = 1;
      return true;
    }
  }
}
