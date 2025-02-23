﻿// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
  [RequireComponent(typeof (ThirdPersonCharacter))]
  public class ThirdPersonUserControl : MonoBehaviour
  {
    private ThirdPersonCharacter m_Character;
    private Transform m_Cam;
    private Vector3 m_CamForward;
    private Vector3 m_Move;
    private bool m_Jump;

    private void Start()
    {
      if ((Object) Camera.main != (Object) null)
        this.m_Cam = Camera.main.transform;
      else
        Debug.LogWarning((object) "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", (Object) this.gameObject);
      this.m_Character = this.GetComponent<ThirdPersonCharacter>();
    }

    private void Update()
    {
      if (this.m_Jump)
        return;
      this.m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
    }

    private void FixedUpdate()
    {
      float axis1 = CrossPlatformInputManager.GetAxis("Horizontal");
      float axis2 = CrossPlatformInputManager.GetAxis("Vertical");
      bool key = Input.GetKey(KeyCode.C);
      if ((Object) this.m_Cam != (Object) null)
      {
        this.m_CamForward = Vector3.Scale(this.m_Cam.forward, new Vector3(1f, 0.0f, 1f)).normalized;
        this.m_Move = axis2 * this.m_CamForward + axis1 * this.m_Cam.right;
      }
      else
        this.m_Move = axis2 * Vector3.forward + axis1 * Vector3.right;
      if (Input.GetKey(KeyCode.LeftShift))
        this.m_Move *= 0.5f;
      this.m_Character.Move(this.m_Move, key, this.m_Jump);
      this.m_Jump = false;
    }
  }
}
