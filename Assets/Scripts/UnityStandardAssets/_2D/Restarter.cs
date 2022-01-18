// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets._2D.Restarter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityStandardAssets._2D
{
  public class Restarter : MonoBehaviour
  {
    private void OnTriggerEnter2D(Collider2D other)
    {
      if (!(other.tag == "Player"))
        return;
      SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
    }
  }
}
