// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets._2D.Platformer2DUserControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
  [RequireComponent(typeof (PlatformerCharacter2D))]
  public class Platformer2DUserControl : MonoBehaviour
  {
    private PlatformerCharacter2D m_Character;
    private bool m_Jump;

    private void Awake() => this.m_Character = this.GetComponent<PlatformerCharacter2D>();

    private void Update()
    {
      if (this.m_Jump)
        return;
      this.m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
    }

    private void FixedUpdate()
    {
      bool key = Input.GetKey(KeyCode.LeftControl);
      this.m_Character.Move(CrossPlatformInputManager.GetAxis("Horizontal"), key, this.m_Jump);
      this.m_Jump = false;
    }
  }
}
