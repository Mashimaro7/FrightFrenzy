// Decompiled with JetBrains decompiler
// Type: Scorekeeper_TippingPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class Scorekeeper_TippingPoint : Scorekeeper
{
  private TippingPoint_Settings tp_settings;
  private ClientLow client;
  public GoalCounter red_goal_counter;
  public GoalCounter blue_goal_counter;
  public Transform platform_detector_blue;
  public Transform platform_detector_red;
  private PlatformCounter bd_plat_red;
  private PlatformCounter bd_plat_blue;
  private RobotCounter bd_robot_red;
  private RobotCounter bd_robot_blue;
  private GoalCounter bd_goal_red;
  private GoalCounter bd_goal_blue;
  public GoalCounter Balance_Goal_Counter_Red;
  public RobotCounter Balance_Robot_Counter_Red;
  public GoalCounter Balance_Goal_Counter_Blue;
  public RobotCounter Balance_Robot_Counter_Blue;
  public RobotCollision Balance_Robot_Collision_Red;
  public RobotCollision Balance_Robot_Collision_Blue;
  public Transform preload_redl;
  public Transform preload_redr;
  public Transform preload_bluel;
  public Transform preload_bluer;
  public Transform extra_rings_red;
  public Transform extra_rings_blue;
  public Transform ringreturn_bl;
  public Transform ringreturn_br;
  public Transform ringreturn_rl;
  public Transform ringreturn_rr;
  public FRC_DepotHandler detect_bl;
  public FRC_DepotHandler detect_br;
  public FRC_DepotHandler detect_rl;
  public FRC_DepotHandler detect_rr;
  public bool blue_rings_returned;
  public bool red_rings_returned;
  public GameObject pushback;
  public float score_red;
  public float score_blue;
  public GoalScorer[] all_goals;
  private Vector3 old_gravity;
  private int loadrings_delay;
  private Dictionary<int, gameElement> found_elements = new Dictionary<int, gameElement>();
  private List<RobotStates> robotstates = new List<RobotStates>();
  public int red_auto;
  public int blue_auto;
  public bool blue_winpoint;
  public bool red_winpoint;
  public int rings_red_low;
  public int rings_red_mid;
  public int rings_red_high;
  public int rings_blue_low;
  public int rings_blue_mid;
  public int rings_blue_high;
  public int goals_red_balanced;
  public int goals_blue_balanced;
  public int robots_red_balanced;
  public int robots_blue_balanced;
  public int goals_red_scored;
  public int goals_blue_scored;
  public bool blue_balanced;
  public bool red_balanced;
  private Vector3 delta_ring = new Vector3(0.0f, 0.12f, 0.0f);

  private void Awake()
  {
    GLOBALS.PlayerCount = 4;
    GLOBALS.TIMER_TOTAL = 120;
    GLOBALS.TIMER_AUTO = 15;
    GLOBALS.TIMER_ENDGAME = 0;
    this.old_gravity = Physics.gravity;
    Physics.gravity = new Vector3(0.0f, -19.62f, 0.0f);
  }

  private void OnDestroy() => Physics.gravity = this.old_gravity;

  public override void ScorerInit()
  {
    this.ScorerReset();
    GameObject gameObject1 = GameObject.Find("GameSettings");
    if ((bool) (Object) gameObject1)
      this.tp_settings = gameObject1.GetComponent<TippingPoint_Settings>();
    GameObject gameObject2 = GameObject.Find("Client");
    if ((bool) (Object) gameObject2)
      this.client = gameObject2.GetComponent<ClientLow>();
    this.all_goals = Object.FindObjectsOfType<GoalScorer>();
    this.bd_plat_red = this.platform_detector_red.GetComponent<PlatformCounter>();
    this.bd_plat_blue = this.platform_detector_blue.GetComponent<PlatformCounter>();
    this.bd_robot_red = this.platform_detector_red.GetComponent<RobotCounter>();
    this.bd_robot_blue = this.platform_detector_blue.GetComponent<RobotCounter>();
    this.bd_goal_red = this.platform_detector_red.GetComponent<GoalCounter>();
    this.bd_goal_blue = this.platform_detector_blue.GetComponent<GoalCounter>();
  }

  public override void ScorerReset()
  {
    base.ScorerReset();
    this.red_auto = 0;
    this.blue_auto = 0;
    this.blue_winpoint = false;
    this.red_winpoint = false;
  }

  public override void Restart()
  {
    base.Restart();
    if (GLOBALS.SINGLEPLAYER_MODE && (bool) (Object) GLOBALS.topsingleplayer && GLOBALS.game_option == 2)
    {
      GLOBALS.TIMER_TOTAL = 60;
      GLOBALS.TIMER_AUTO = 0;
    }
    else
    {
      GLOBALS.TIMER_TOTAL = 120;
      GLOBALS.TIMER_AUTO = 15;
    }
  }

  private void LoadAllRings()
  {
    foreach (RobotInterface3D allrobot in this.allrobots)
    {
      string startingPos = allrobot.myRobotID.starting_pos;
      if (!(startingPos == "Red Left"))
      {
        if (!(startingPos == "Red Right"))
        {
          if (!(startingPos == "Blue Left"))
          {
            if (startingPos == "Blue Right")
              this.LoadStartingRings(allrobot, this.preload_bluer);
          }
          else
            this.LoadStartingRings(allrobot, this.preload_bluel);
        }
        else
          this.LoadStartingRings(allrobot, this.preload_redr);
      }
      else
        this.LoadStartingRings(allrobot, this.preload_redl);
    }
  }

  public override void OnTimerStart()
  {
    this.loadrings_delay = 2;
    this.detect_bl.GetComponent<MeshRenderer>().enabled = true;
    this.detect_br.GetComponent<MeshRenderer>().enabled = true;
    this.detect_rl.GetComponent<MeshRenderer>().enabled = true;
    this.detect_rr.GetComponent<MeshRenderer>().enabled = true;
    this.blue_rings_returned = false;
    this.red_rings_returned = false;
    foreach (Component component1 in this.extra_rings_red)
    {
      Rigidbody component2 = component1.GetComponent<Rigidbody>();
      if ((bool) (Object) component2)
        component2.isKinematic = true;
    }
    foreach (Component component3 in this.extra_rings_blue)
    {
      Rigidbody component4 = component3.GetComponent<Rigidbody>();
      if ((bool) (Object) component4)
        component4.isKinematic = true;
    }
    this.detect_bl.Clear();
    this.detect_br.Clear();
    this.detect_rl.Clear();
    this.detect_rr.Clear();
  }

  private void LoadStartingRings(RobotInterface3D bot, Transform rings)
  {
    PreloadID componentInChildren = bot.GetComponentInChildren<PreloadID>();
    if (!(bool) (Object) componentInChildren)
      return;
    float num = 0.0f;
    foreach (Transform ring in rings)
    {
      ring.SetPositionAndRotation(componentInChildren.transform.position + num * this.delta_ring.magnitude * componentInChildren.transform.up, componentInChildren.transform.rotation * ring.rotation);
      ++num;
    }
  }

  public override void ScorerUpdate()
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    if (this.timerstate != Scorekeeper.TimerState.PAUSED && this.timerstate != Scorekeeper.TimerState.RUNNING)
      this.pushback.SetActive(false);
    else if (this.time_total.TotalSeconds > (double) (GLOBALS.TIMER_TOTAL - GLOBALS.TIMER_AUTO))
      this.pushback.SetActive(true);
    else
      this.pushback.SetActive(false);
    this.CalculateScores();
    if (this.loadrings_delay <= 0)
      return;
    --this.loadrings_delay;
    if (this.loadrings_delay != 0)
      return;
    this.LoadAllRings();
  }

  public override void FieldChangedTrigger()
  {
  }

  public override void GetScoreDetails(Dictionary<string, string> data)
  {
    base.GetScoreDetails(data);
    data["ScoreR"] = ((int) this.score_red + this.score_redadj).ToString();
    data["ScoreB"] = ((int) this.score_blue + this.score_blueadj).ToString();
    data["RedWP"] = this.red_winpoint ? "1" : "0";
    data["BlueWP"] = this.blue_winpoint ? "1" : "0";
    data["AutoR"] = this.red_auto.ToString();
    Dictionary<string, string> dictionary1 = data;
    int num = this.blue_auto;
    string str1 = num.ToString();
    dictionary1["AutoB"] = str1;
    Dictionary<string, string> dictionary2 = data;
    num = this.rings_red_low;
    string str2 = num.ToString();
    dictionary2["r_low_rings"] = str2;
    Dictionary<string, string> dictionary3 = data;
    num = this.rings_red_mid;
    string str3 = num.ToString();
    dictionary3["r_mid_rings"] = str3;
    Dictionary<string, string> dictionary4 = data;
    num = this.rings_red_high;
    string str4 = num.ToString();
    dictionary4["r_high_rings"] = str4;
    Dictionary<string, string> dictionary5 = data;
    num = this.rings_blue_low;
    string str5 = num.ToString();
    dictionary5["b_low_rings"] = str5;
    Dictionary<string, string> dictionary6 = data;
    num = this.rings_blue_mid;
    string str6 = num.ToString();
    dictionary6["b_mid_rings"] = str6;
    Dictionary<string, string> dictionary7 = data;
    num = this.rings_blue_high;
    string str7 = num.ToString();
    dictionary7["b_high_rings"] = str7;
    Dictionary<string, string> dictionary8 = data;
    num = this.goals_red_scored;
    string str8 = num.ToString();
    dictionary8["r_goals"] = str8;
    Dictionary<string, string> dictionary9 = data;
    num = this.goals_blue_scored;
    string str9 = num.ToString();
    dictionary9["b_goals"] = str9;
    Dictionary<string, string> dictionary10 = data;
    num = this.goals_red_balanced;
    string str10 = num.ToString();
    dictionary10["r_goals_bal"] = str10;
    Dictionary<string, string> dictionary11 = data;
    num = this.goals_blue_balanced;
    string str11 = num.ToString();
    dictionary11["b_goals_bal"] = str11;
    Dictionary<string, string> dictionary12 = data;
    num = this.robots_red_balanced;
    string str12 = num.ToString();
    dictionary12["r_bots_bal"] = str12;
    Dictionary<string, string> dictionary13 = data;
    num = this.robots_blue_balanced;
    string str13 = num.ToString();
    dictionary13["b_bots_bal"] = str13;
  }

  private void CalculateScores()
  {
    if (this.timerstate != Scorekeeper.TimerState.RUNNING || !(bool) (Object) this.tp_settings)
      return;
    this.score_red = 0.0f;
    this.score_blue = 0.0f;
    this.rings_red_low = 0;
    this.rings_red_mid = 0;
    this.rings_red_high = 0;
    this.rings_blue_low = 0;
    this.rings_blue_mid = 0;
    this.rings_blue_high = 0;
    foreach (GoalScorer allGoal in this.all_goals)
    {
      if (this.red_goal_counter.IsGoalInside(allGoal))
      {
        this.rings_red_low += allGoal.GetLowCount();
        this.rings_red_mid += allGoal.GetMidCount();
        this.rings_red_high += allGoal.GetHighCount();
      }
      else if (this.blue_goal_counter.IsGoalInside(allGoal))
      {
        this.rings_blue_low += allGoal.GetLowCount();
        this.rings_blue_mid += allGoal.GetMidCount();
        this.rings_blue_high += allGoal.GetHighCount();
      }
    }
    this.score_red += (float) (this.rings_red_low + 3 * this.rings_red_mid + 10 * this.rings_red_high);
    this.score_blue += (float) (this.rings_blue_low + 3 * this.rings_blue_mid + 10 * this.rings_blue_high);
    this.goals_red_scored = this.red_goal_counter.GetGoalCount();
    this.score_red += (float) (20 * this.goals_red_scored);
    this.goals_blue_scored = this.blue_goal_counter.GetGoalCount();
    this.score_blue += (float) (20 * this.goals_blue_scored);
    if (!GLOBALS.SINGLEPLAYER_MODE && this.time_total.TotalSeconds > (double) (GLOBALS.TIMER_TOTAL - GLOBALS.TIMER_AUTO))
    {
      this.red_auto = 0;
      this.blue_auto = 0;
      if ((double) this.score_red > (double) this.score_blue)
        this.red_auto = 6;
      else if ((double) this.score_blue > (double) this.score_red)
      {
        this.blue_auto = 6;
      }
      else
      {
        this.red_auto = 3;
        this.blue_auto = 3;
      }
    }
    this.score_red += (float) this.red_auto;
    this.score_blue += (float) this.blue_auto;
    this.goals_red_balanced = 0;
    this.goals_blue_balanced = 0;
    this.robots_red_balanced = 0;
    this.robots_blue_balanced = 0;
    if (!GLOBALS.SINGLEPLAYER_MODE && this.time_total.TotalSeconds > (double) (GLOBALS.TIMER_TOTAL - GLOBALS.TIMER_AUTO))
      return;
    this.blue_balanced = true;
    this.red_balanced = true;
    if (this.bd_plat_red.GetPlatformCount() > 0)
      this.red_balanced = false;
    if (this.bd_plat_blue.GetPlatformCount() > 0)
      this.blue_balanced = false;
    if (this.red_balanced)
    {
      foreach (RobotID collision in this.Balance_Robot_Counter_Red.collisions)
      {
        if (this.bd_robot_red.collisions.Contains(collision))
        {
          this.red_balanced = false;
          break;
        }
      }
    }
    if (this.blue_balanced)
    {
      foreach (RobotID collision in this.Balance_Robot_Counter_Blue.collisions)
      {
        if (this.bd_robot_blue.collisions.Contains(collision))
        {
          this.blue_balanced = false;
          break;
        }
      }
    }
    if (this.red_balanced)
    {
      foreach (GoalScorer collision in this.Balance_Goal_Counter_Red.collisions)
      {
        if (this.bd_goal_red.collisions.Contains(collision))
        {
          this.red_balanced = false;
          break;
        }
      }
    }
    if (this.blue_balanced)
    {
      foreach (GoalScorer collision in this.Balance_Goal_Counter_Blue.collisions)
      {
        if (this.bd_goal_blue.collisions.Contains(collision))
        {
          this.blue_balanced = false;
          break;
        }
      }
    }
    if (this.red_balanced)
    {
      foreach (RobotID collision in this.Balance_Robot_Counter_Red.collisions)
      {
        if (this.bd_robot_red.collisions.Contains(collision))
        {
          this.red_balanced = false;
          break;
        }
      }
    }
    if (this.blue_balanced)
    {
      foreach (RobotID collision in this.Balance_Robot_Counter_Blue.collisions)
      {
        if (this.bd_robot_blue.collisions.Contains(collision))
        {
          this.red_balanced = false;
          break;
        }
      }
    }
    if (this.red_balanced)
    {
      this.goals_red_balanced = this.Balance_Goal_Counter_Red.GetGoalCount();
      this.robots_red_balanced = this.Balance_Robot_Counter_Red.GetRobotCount();
    }
    if (this.blue_balanced)
    {
      this.goals_blue_balanced = this.Balance_Goal_Counter_Blue.GetGoalCount();
      this.robots_blue_balanced = this.Balance_Robot_Counter_Blue.GetRobotCount();
    }
    this.score_red += (float) (this.goals_red_balanced * 20 + this.robots_red_balanced * 30);
    this.score_blue += (float) (this.goals_blue_balanced * 20 + this.robots_blue_balanced * 30);
    this.ReturnRings();
  }

  public void ReturnRings()
  {
    if (!this.blue_rings_returned && this.detect_bl.GetFriendsColliding())
    {
      this.blue_rings_returned = true;
      this.detect_bl.GetComponent<MeshRenderer>().enabled = false;
      this.detect_br.GetComponent<MeshRenderer>().enabled = false;
      float num = 0.0f;
      foreach (Transform transform in this.extra_rings_blue)
      {
        Rigidbody component = transform.GetComponent<Rigidbody>();
        if ((bool) (Object) component)
        {
          component.isKinematic = false;
          transform.SetPositionAndRotation(this.ringreturn_bl.position + num * this.delta_ring, this.ringreturn_bl.rotation * transform.rotation);
          ++num;
        }
      }
    }
    if (!this.blue_rings_returned && this.detect_br.GetFriendsColliding())
    {
      this.blue_rings_returned = true;
      this.detect_bl.GetComponent<MeshRenderer>().enabled = false;
      this.detect_br.GetComponent<MeshRenderer>().enabled = false;
      float num = 0.0f;
      foreach (Transform transform in this.extra_rings_blue)
      {
        Rigidbody component = transform.GetComponent<Rigidbody>();
        if ((bool) (Object) component)
        {
          component.isKinematic = false;
          transform.SetPositionAndRotation(this.ringreturn_br.position + num * this.delta_ring, this.ringreturn_br.rotation * transform.rotation);
          ++num;
        }
      }
    }
    if (!this.red_rings_returned && this.detect_rl.GetFriendsColliding())
    {
      this.red_rings_returned = true;
      this.detect_rl.GetComponent<MeshRenderer>().enabled = false;
      this.detect_rr.GetComponent<MeshRenderer>().enabled = false;
      float num = 0.0f;
      foreach (Transform transform in this.extra_rings_red)
      {
        Rigidbody component = transform.GetComponent<Rigidbody>();
        if ((bool) (Object) component)
        {
          component.isKinematic = false;
          transform.SetPositionAndRotation(this.ringreturn_rl.position + num * this.delta_ring, this.ringreturn_rl.rotation * transform.rotation);
          ++num;
        }
      }
    }
    if (this.red_rings_returned || !this.detect_rr.GetFriendsColliding())
      return;
    this.red_rings_returned = true;
    this.detect_rl.GetComponent<MeshRenderer>().enabled = false;
    this.detect_rr.GetComponent<MeshRenderer>().enabled = false;
    float num1 = 0.0f;
    foreach (Transform transform in this.extra_rings_red)
    {
      Rigidbody component = transform.GetComponent<Rigidbody>();
      if ((bool) (Object) component)
      {
        component.isKinematic = false;
        transform.SetPositionAndRotation(this.ringreturn_rr.position + num1 * this.delta_ring, this.ringreturn_rr.rotation * transform.rotation);
        ++num1;
      }
    }
  }

  public override int GetRedScore() => this.timerstate == Scorekeeper.TimerState.FINISHED ? this.score_redfinal : (int) this.score_red + this.score_redadj;

  public override int GetBlueScore() => this.timerstate == Scorekeeper.TimerState.FINISHED ? this.score_bluefinal : (int) this.score_blue + this.score_blueadj;

  public override void SendServerData(Dictionary<string, string> serverFlags)
  {
    base.SendServerData(serverFlags);
    serverFlags["ST_RR"] = (this.blue_rings_returned ? "1" : "0") + ":" + (this.red_rings_returned ? "1" : "0");
  }

  public override void ReceiveServerData(Dictionary<string, string> serverFlags)
  {
    base.ReceiveServerData(serverFlags);
    int num1 = this.blue_rings_returned ? 1 : 0;
    bool redRingsReturned = this.red_rings_returned;
    if (serverFlags.ContainsKey("ST_RR"))
    {
      string serverFlag = serverFlags["ST_RR"];
      if (serverFlag.Length >= 3)
      {
        this.blue_rings_returned = serverFlag[0] == '1';
        this.red_rings_returned = serverFlag[2] == '1';
      }
    }
    int num2 = this.blue_rings_returned ? 1 : 0;
    if (num1 == num2 && redRingsReturned == this.red_rings_returned)
      return;
    this.detect_bl.GetComponent<MeshRenderer>().enabled = !this.blue_rings_returned;
    this.detect_br.GetComponent<MeshRenderer>().enabled = !this.blue_rings_returned;
    this.detect_rl.GetComponent<MeshRenderer>().enabled = !this.red_rings_returned;
    this.detect_rr.GetComponent<MeshRenderer>().enabled = !this.red_rings_returned;
  }
}
