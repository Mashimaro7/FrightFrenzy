// Decompiled with JetBrains decompiler
// Type: IR_scoringBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class IR_scoringBox : MonoBehaviour
{
  public int number_of_items;
  public int number_of_balls;
  public bool is_red = true;
  public int sound_to_play = 1;
  public Dictionary<int, int> user_ball_count = new Dictionary<int, int>();
  public bool ignore_Bad_elements;
  public bool penalize_Bad_elements;
  public Dictionary<int, int> bad_ball_count = new Dictionary<int, int>();
  public int red_bad_elements;
  public int blue_bad_elements;
  public bool play_animation;
  public bool disable_in_client = true;
  private Animator my_animation;
  public List<Transform> balls = new List<Transform>();
  private AudioManager my_top_audiomanager;

  private void OnEnable() => this.my_animation = this.GetComponent<Animator>();

  public void Reset()
  {
    this.number_of_balls = 0;
    this.balls.Clear();
    this.user_ball_count.Clear();
    this.bad_ball_count.Clear();
    this.number_of_items = 0;
    this.red_bad_elements = 0;
    this.blue_bad_elements = 0;
  }

  public bool IsSomethingInside() => this.number_of_items > 0;

  private void Update()
  {
  }

  private gameElement FindGameElement(Collider collision)
  {
    Transform transform = collision.transform;
    gameElement gameElement = transform.GetComponent<gameElement>();
    if ((Object) gameElement == (Object) null)
      gameElement = transform.GetComponentInParent<gameElement>();
    if ((Object) gameElement == (Object) null)
      return (gameElement) null;
    return gameElement.type == ElementType.Jewel ? gameElement : (gameElement) null;
  }

  private void OnTriggerEnter(Collider collision)
  {
    if (this.disable_in_client && GLOBALS.CLIENT_MODE)
      return;
    ++this.number_of_items;
    gameElement gameElement = this.FindGameElement(collision);
    if ((Object) gameElement == (Object) null)
      return;
    ball_data component = gameElement.GetComponent<ball_data>();
    if ((Object) component == (Object) null || this.balls.Contains(gameElement.transform) || (Object) gameElement == (Object) null || (Object) component == (Object) null || this.ignore_Bad_elements && component.flags.ContainsKey("Bad"))
      return;
    if (this.penalize_Bad_elements && component.flags.ContainsKey("Bad"))
    {
      if (this.bad_ball_count.ContainsKey(component.thrown_by_id))
        ++this.bad_ball_count[component.thrown_by_id];
      else
        this.bad_ball_count[component.thrown_by_id] = 1;
      if (component.thrown_robotid.is_red)
        ++this.red_bad_elements;
      else
        ++this.blue_bad_elements;
    }
    ++this.number_of_balls;
    if (this.user_ball_count.ContainsKey(component.thrown_by_id))
      ++this.user_ball_count[component.thrown_by_id];
    else
      this.user_ball_count[component.thrown_by_id] = 1;
    if (this.is_red)
      component.scored_red = true;
    else
      component.scored_blue = true;
    this.balls.Add(gameElement.transform);
    if ((Object) this.my_top_audiomanager == (Object) null)
    {
      GameObject gameObject = GameObject.Find("AudioManager");
      if (!(bool) (Object) gameObject)
        return;
      this.my_top_audiomanager = gameObject.GetComponent<AudioManager>();
      if ((Object) this.my_top_audiomanager == (Object) null)
        return;
    }
    this.PlayAnimation();
    if (this.sound_to_play == 1)
    {
      this.my_top_audiomanager.Play("ballscored");
    }
    else
    {
      if (this.sound_to_play != 2)
        return;
      this.my_top_audiomanager.Play("ballscored_high");
    }
  }

  public void PlayAnimation()
  {
    if (!this.play_animation || !(bool) (Object) this.my_animation)
      return;
    this.my_animation.Play("Base Layer.flash");
  }

  private void OnTriggerExit(Collider collision)
  {
    if (this.disable_in_client && GLOBALS.CLIENT_MODE)
      return;
    --this.number_of_items;
  }

  public Transform GetNextBall()
  {
    if (this.balls.Count <= 0)
      return (Transform) null;
    Transform ball = this.balls[0];
    this.balls.RemoveAt(0);
    return ball;
  }
}
