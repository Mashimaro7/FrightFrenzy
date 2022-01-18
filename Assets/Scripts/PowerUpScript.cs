// Decompiled with JetBrains decompiler
// Type: PowerUpScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
  public Scorekeeper myscorekeeper;
  public GameObject core_object;
  public float speed = 200f;
  public string text = "0";
  public Color text_color = new Color(1f, 1f, 1f);
  public List<Material> text_materials = new List<Material>();
  public int material_to_use;
  public TextMeshPro textfield;
  public PowerUpType myPower = PowerUpType.SPEED;
  private Animator my_animation;
  private AudioManager myaudio;
  private RobotInterface3D robot_owner;
  private long time_of_trigger = -9999999999;
  private bool needs_servicing;
  private bool enabled = true;

  private void Start()
  {
    switch (this.myPower)
    {
      case PowerUpType.SPEED:
        this.text = "S";
        this.text_color.r = 1f;
        this.text_color.g = 1f;
        this.text_color.b = 1f;
        this.text_color.a = 1f;
        this.material_to_use = 0;
        break;
      case PowerUpType.TORQUE:
        this.text = "T";
        this.text_color.r = 1f;
        this.text_color.g = 1f;
        this.text_color.b = 1f;
        this.text_color.a = 1f;
        this.material_to_use = 0;
        break;
      case PowerUpType.INVISIBILITY:
        this.text = "I";
        this.text_color.r = 1f;
        this.text_color.g = 1f;
        this.text_color.b = 1f;
        this.text_color.a = 1f;
        this.material_to_use = 0;
        break;
      case PowerUpType.SLOW:
        this.text = "s";
        this.text_color.r = 0.0f;
        this.text_color.g = 0.0f;
        this.text_color.b = 0.0f;
        this.text_color.a = 1f;
        this.material_to_use = 1;
        break;
      case PowerUpType.WEAK:
        this.text = "W";
        this.text_color.r = 0.0f;
        this.text_color.g = 0.0f;
        this.text_color.b = 0.0f;
        this.text_color.a = 1f;
        this.material_to_use = 1;
        break;
      case PowerUpType.INVERTED:
        this.text = "i";
        this.text_color.r = 0.0f;
        this.text_color.g = 0.0f;
        this.text_color.b = 0.0f;
        this.text_color.a = 1f;
        this.material_to_use = 1;
        break;
    }
    this.textfield.text = this.text;
    this.textfield.color = this.text_color;
    this.my_animation = this.GetComponent<Animator>();
    this.textfield.fontSharedMaterial = this.text_materials[this.material_to_use];
    this.myaudio = this.GetComponent<AudioManager>();
  }

  private void OnEnable() => this.PU_Disable();

  private void Update()
  {
    Vector3 eulerAngles = this.transform.rotation.eulerAngles;
    eulerAngles.y += Time.deltaTime * this.speed;
    eulerAngles.y %= 360f;
    this.transform.rotation = Quaternion.Euler(eulerAngles);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (GLOBALS.CLIENT_MODE || (Object) this.robot_owner != (Object) null)
      return;
    RobotInterface3D componentInParent = other.GetComponentInParent<RobotInterface3D>();
    if (!(bool) (Object) componentInParent || !this.myscorekeeper.PU_CheckIfClearToAssign(this, componentInParent))
      return;
    this.robot_owner = componentInParent;
    this.my_animation.Play("Base Layer.CLOSE");
    this.myaudio.Play("PowerUpGot");
    this.enabled = false;
    this.time_of_trigger = MyUtils.GetTimeMillisSinceStart();
    this.needs_servicing = true;
  }

  public bool NeedsServicing() => this.needs_servicing;

  public void Serviced() => this.needs_servicing = false;

  public RobotInterface3D GetOwner() => (bool) (Object) this.robot_owner ? this.robot_owner : (RobotInterface3D) null;

  public long GetTimeStarted() => this.time_of_trigger;

  public void ClearOwner()
  {
    this.robot_owner = (RobotInterface3D) null;
    this.time_of_trigger = -1L;
    this.needs_servicing = false;
  }

  public void PU_DisableWithAnimation()
  {
    this.my_animation.Play("Base Layer.CLOSE");
    this.enabled = false;
  }

  public void PU_Disable()
  {
    this.core_object.SetActive(false);
    this.enabled = false;
  }

  public bool IsDisabled() => !this.enabled;

  public void PU_Enable(PowerUpType myNewPower = PowerUpType.SPEED)
  {
    this.robot_owner = (RobotInterface3D) null;
    this.time_of_trigger = -9999999999L;
    this.needs_servicing = false;
    this.myPower = myNewPower;
    this.core_object.SetActive(true);
    this.Start();
    this.my_animation.Play("Base Layer.OPEN");
    this.enabled = true;
  }
}
