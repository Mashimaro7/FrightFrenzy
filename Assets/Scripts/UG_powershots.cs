// Decompiled with JetBrains decompiler
// Type: UG_powershots
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class UG_powershots : MonoBehaviour
{
  public bool penalty;
  public int hit_by_id = -1;
  public bool scored;
  public bool unscored;
  public HingeJoint myjoint;
  public bool up;
  private Vector3 starting_pos;
  private Quaternion starting_rot;
  private AudioManager my_top_audiomanager;
  private bool previous_up;

  private void Start()
  {
    this.starting_pos = this.transform.position;
    this.starting_rot = this.transform.rotation;
    this.myjoint = this.GetComponent<HingeJoint>();
    GameObject gameObject = GameObject.Find("AudioManager");
    if (!(bool) (Object) gameObject)
      return;
    this.my_top_audiomanager = gameObject.GetComponent<AudioManager>();
    int num = (Object) this.my_top_audiomanager == (Object) null ? 1 : 0;
  }

  public void Reset()
  {
    if (!(bool) (Object) this.myjoint)
      return;
    this.penalty = false;
    this.hit_by_id = -1;
    this.transform.position = this.starting_pos;
    this.transform.rotation = this.starting_rot;
    this.scored = false;
    this.unscored = false;
    this.up = true;
    this.previous_up = true;
  }

  public void ClearScore()
  {
    if (this.scored)
    {
      this.penalty = false;
      this.hit_by_id = -1;
    }
    this.scored = false;
    this.unscored = false;
  }

  private void Update()
  {
    if (!(bool) (Object) this.myjoint)
      return;
    this.up = (double) this.myjoint.angle <= 45.0;
    if (!this.scored)
    {
      this.scored = !this.up && this.previous_up;
      if (this.scored)
      {
        this.unscored = false;
        this.my_top_audiomanager.Play("popsound");
      }
    }
    if (!this.unscored)
    {
      this.unscored = this.up && !this.previous_up;
      if (this.unscored)
        this.scored = false;
    }
    this.previous_up = this.up;
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (!(bool) (Object) this.myjoint || (double) this.myjoint.angle > 45.0)
      return;
    gameElement gameElement = this.FindGameElement(collision);
    if ((Object) gameElement == (Object) null)
      return;
    ball_data component = gameElement.GetComponent<ball_data>();
    if ((Object) component == (Object) null || (Object) gameElement == (Object) null || (Object) component == (Object) null || component.thrown_by_id <= 0)
      return;
    if (component.thrown_by_id == this.hit_by_id)
      this.penalty |= component.flags.ContainsKey("Bad");
    else
      this.penalty = component.flags.ContainsKey("Bad");
    this.hit_by_id = component.thrown_by_id;
  }

  private gameElement FindGameElement(Collision collision)
  {
    Transform transform = collision.transform;
    gameElement gameElement = transform.GetComponent<gameElement>();
    if ((Object) gameElement == (Object) null)
      gameElement = transform.GetComponentInParent<gameElement>();
    if ((Object) gameElement == (Object) null)
      return (gameElement) null;
    return gameElement.type == ElementType.Jewel ? gameElement : (gameElement) null;
  }
}
