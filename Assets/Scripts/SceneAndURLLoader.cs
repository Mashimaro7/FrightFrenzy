// Decompiled with JetBrains decompiler
// Type: SceneAndURLLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAndURLLoader : MonoBehaviour
{
  private PauseMenu m_PauseMenu;

  private void Awake() => this.m_PauseMenu = this.GetComponentInChildren<PauseMenu>();

  public void SceneLoad(string sceneName)
  {
    this.m_PauseMenu.MenuOff();
    SceneManager.LoadScene(sceneName);
  }

  public void LoadURL(string url) => Application.OpenURL(url);
}
