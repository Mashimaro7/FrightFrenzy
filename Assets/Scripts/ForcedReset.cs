// Decompiled with JetBrains decompiler
// Type: ForcedReset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (GUITexture))]
public class ForcedReset : MonoBehaviour
{
  private void Update()
  {
    if (!CrossPlatformInputManager.GetButtonDown("ResetObject"))
      return;
    SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
  }
}
