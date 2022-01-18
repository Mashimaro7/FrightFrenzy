// Decompiled with JetBrains decompiler
// Type: gameElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class gameElement : MonoBehaviour
{
  public int id;
  public ElementType type;
  public int lastupdateID;
  public string lastPlayerCollision;
  public float maxAngularRotation;
  public Vector3 inertiaTensor = Vector3.zero;
  public Vector3 center_of_mass = Vector3.zero;
  public string note = "";
  public string note2 = "";
  public GenericFieldTracker tracker;
  private Vector3 starting_pos;
  private Quaternion starting_rot;
  private Color starting_color;
  private bool starting_kinematic;
  private bool I_have_color;
  public int held_by_robot;
  public bool use_local_pos;
  public Transform option2;
  public Transform option3;
  public Transform option4;
  public Transform option5;
  public Rigidbody myrb;
  private bool init_done;

  private void Update()
  {
    int num = (bool) (Object) this.myrb ? 1 : 0;
  }

  private void OnEnable()
  {
    if (this.init_done)
      return;
    this.init_done = true;
    this.lastupdateID = 0;
    this.lastPlayerCollision = "";
    if (this.use_local_pos)
    {
      this.starting_pos = this.transform.localPosition;
      this.starting_rot = this.transform.localRotation;
    }
    else
    {
      this.starting_pos = this.transform.position;
      this.starting_rot = this.transform.rotation;
    }
    if (this.type == ElementType.NoRigidBody)
      return;
    if ((bool) (Object) this.GetComponent<Rigidbody>())
    {
      this.starting_kinematic = this.GetComponent<Rigidbody>().isKinematic;
      if ((double) this.maxAngularRotation != 0.0)
        this.GetComponent<Rigidbody>().maxAngularVelocity = this.maxAngularRotation;
      if ((double) this.inertiaTensor.magnitude != 0.0)
        this.GetComponent<Rigidbody>().inertiaTensor = this.inertiaTensor;
      if ((double) this.center_of_mass.magnitude != 0.0)
        this.GetComponent<Rigidbody>().centerOfMass = this.center_of_mass;
    }
    Renderer component = this.GetComponent<Renderer>();
    if ((bool) (Object) component)
    {
      Material material = component.material;
      if ((bool) (Object) material)
      {
        this.starting_color = material.color;
        this.I_have_color = true;
      }
    }
    this.myrb = this.GetComponent<Rigidbody>();
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (GLOBALS.CLIENT_MODE || this.type == ElementType.NoRigidBody || this.type == ElementType.Off)
      return;
    Transform transform1 = collision.transform;
    while ((Object) transform1.parent != (Object) null)
      transform1 = transform1.parent;
    Transform transform2 = transform1.Find("Nametag");
    if (!((Object) transform2 != (Object) null))
      return;
    this.lastPlayerCollision = transform2.GetComponent<TextMesh>().text;
  }

  public void ResetColor()
  {
    if (this.type == ElementType.NoRigidBody || this.type == ElementType.Off || !this.I_have_color)
      return;
    this.GetComponent<Renderer>().material.color = this.starting_color;
  }

  public virtual void ResetPosition(int option = 1)
  {
    if (this.type == ElementType.NoRigidBody)
      return;
    Transform transform = this.transform;
    if (this.use_local_pos)
    {
      transform.localPosition = this.starting_pos;
      transform.localRotation = this.starting_rot;
    }
    else
    {
      transform.position = this.starting_pos;
      transform.rotation = this.starting_rot;
    }
    this.held_by_robot = 0;
    switch (option)
    {
      case 2:
        if ((bool) (Object) this.option2)
        {
          transform = this.option2;
          break;
        }
        break;
      case 3:
        if ((bool) (Object) this.option3)
        {
          transform = this.option3;
          break;
        }
        break;
      case 4:
        if ((bool) (Object) this.option4)
        {
          transform = this.option4;
          break;
        }
        break;
      case 5:
        if ((bool) (Object) this.option5)
        {
          transform = this.option5;
          break;
        }
        break;
    }
    this.transform.position = transform.position;
    this.transform.rotation = transform.rotation;
    if (!(bool) (Object) this.GetComponent<Rigidbody>())
      return;
    this.GetComponent<Rigidbody>().isKinematic = this.starting_kinematic;
    Rigidbody component = this.GetComponent<Rigidbody>();
    component.velocity = Vector3.zero;
    component.angularVelocity = Vector3.zero;
    component.Sleep();
    this.ResetColor();
  }
}
