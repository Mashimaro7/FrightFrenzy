// Decompiled with JetBrains decompiler
// Type: Scorekeeper_ChangeUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class Scorekeeper_ChangeUp : Scorekeeper
{
  private ChangeUp_Settings cu_settings;
  private ClientLow client;
  public CU_scoringBox scorer_tl;
  public CU_scoringBox scorer_tm;
  public CU_scoringBox scorer_tr;
  public CU_scoringBox scorer_l;
  public CU_scoringBox scorer_m;
  public CU_scoringBox scorer_r;
  public CU_scoringBox scorer_bl;
  public CU_scoringBox scorer_bm;
  public CU_scoringBox scorer_br;
  public MeshRenderer line_r1;
  public MeshRenderer line_r2;
  public MeshRenderer line_r3;
  public MeshRenderer line_r4;
  public MeshRenderer line_r5;
  public MeshRenderer line_r6;
  public MeshRenderer line_r7;
  public MeshRenderer line_r8;
  public GameObject pushback;
  public Material color_red;
  public Material color_blue;
  public float score_red;
  public float score_blue;
  private Vector3 old_gravity;
  private Dictionary<int, gameElement> found_elements = new Dictionary<int, gameElement>();
  private List<RobotStates> robotstates = new List<RobotStates>();
  public int red_balls;
  public int blue_balls;
  public int red_rows;
  public int blue_rows;
  public int red_auto;
  public int blue_auto;
  public bool blue_winpoint;
  public bool red_winpoint;

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
      this.cu_settings = gameObject1.GetComponent<ChangeUp_Settings>();
    GameObject gameObject2 = GameObject.Find("Client");
    if (!(bool) (Object) gameObject2)
      return;
    this.client = gameObject2.GetComponent<ClientLow>();
  }

  public override void ScorerReset()
  {
    base.ScorerReset();
    this.red_balls = 0;
    this.blue_balls = 0;
    this.red_rows = 0;
    this.blue_rows = 0;
    this.red_auto = 0;
    this.blue_auto = 0;
    this.blue_winpoint = false;
    this.red_winpoint = false;
  }

  public override void Restart()
  {
    base.Restart();
    if (GLOBALS.SINGLEPLAYER_MODE && (bool) (Object) GLOBALS.topsingleplayer && GLOBALS.game_option == 3)
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
  }

  public override void FieldChangedTrigger()
  {
  }

  public override void GetScoreDetails(Dictionary<string, string> data)
  {
    base.GetScoreDetails(data);
    data["ScoreR"] = ((int) this.score_red + this.score_redadj).ToString();
    data["ScoreB"] = ((int) this.score_blue + this.score_blueadj).ToString();
    data["RBalls"] = this.red_balls.ToString();
    data["BBalls"] = this.blue_balls.ToString();
    data["AutoR"] = this.red_auto.ToString();
    data["AutoB"] = this.blue_auto.ToString();
    data["RRows"] = this.red_rows.ToString();
    data["BRows"] = this.blue_rows.ToString();
    data["RedWP"] = this.red_winpoint ? "1" : "0";
    data["BlueWP"] = this.blue_winpoint ? "1" : "0";
  }

  private void CalculateScores()
  {
    if (this.timerstate != Scorekeeper.TimerState.RUNNING || !(bool) (Object) this.cu_settings)
      return;
    this.red_balls = this.scorer_tl.GetRedCount() + this.scorer_tm.GetRedCount() + this.scorer_tr.GetRedCount() + this.scorer_l.GetRedCount() + this.scorer_m.GetRedCount() + this.scorer_r.GetRedCount() + this.scorer_bl.GetRedCount() + this.scorer_bm.GetRedCount() + this.scorer_br.GetRedCount();
    this.blue_balls = this.scorer_tl.GetBlueCount() + this.scorer_tm.GetBlueCount() + this.scorer_tr.GetBlueCount() + this.scorer_l.GetBlueCount() + this.scorer_m.GetBlueCount() + this.scorer_r.GetBlueCount() + this.scorer_bl.GetBlueCount() + this.scorer_bm.GetBlueCount() + this.scorer_br.GetBlueCount();
    int highestBall1 = this.scorer_tl.GetHighestBall();
    int highestBall2 = this.scorer_tm.GetHighestBall();
    int highestBall3 = this.scorer_tr.GetHighestBall();
    int highestBall4 = this.scorer_l.GetHighestBall();
    int highestBall5 = this.scorer_m.GetHighestBall();
    int highestBall6 = this.scorer_r.GetHighestBall();
    int highestBall7 = this.scorer_bl.GetHighestBall();
    int highestBall8 = this.scorer_bm.GetHighestBall();
    int highestBall9 = this.scorer_br.GetHighestBall();
    this.red_rows = 0;
    this.blue_rows = 0;
    this.line_r1.enabled = false;
    this.line_r2.enabled = false;
    this.line_r3.enabled = false;
    this.line_r4.enabled = false;
    this.line_r5.enabled = false;
    this.line_r6.enabled = false;
    this.line_r7.enabled = false;
    this.line_r8.enabled = false;
    bool flag = this.cu_settings.ENABLE_LINES || this.cu_settings.ENABLE_LINES_SPEC;
    if (highestBall1 == highestBall2 && highestBall1 == highestBall3 && highestBall1 == 1)
    {
      ++this.red_rows;
      Object.Destroy((Object) this.line_r1.material);
      this.line_r1.material = this.color_red;
      this.line_r1.enabled = flag;
    }
    if (highestBall1 == highestBall2 && highestBall1 == highestBall3 && highestBall1 == 2)
    {
      ++this.blue_rows;
      Object.Destroy((Object) this.line_r1.material);
      this.line_r1.material = this.color_blue;
      this.line_r1.enabled = flag;
    }
    if (highestBall4 == highestBall5 && highestBall4 == highestBall6 && highestBall4 == 1)
    {
      ++this.red_rows;
      Object.Destroy((Object) this.line_r2.material);
      this.line_r2.material = this.color_red;
      this.line_r2.enabled = flag;
    }
    if (highestBall4 == highestBall5 && highestBall4 == highestBall6 && highestBall4 == 2)
    {
      ++this.blue_rows;
      Object.Destroy((Object) this.line_r2.material);
      this.line_r2.material = this.color_blue;
      this.line_r2.enabled = flag;
    }
    if (highestBall7 == highestBall8 && highestBall7 == highestBall9 && highestBall7 == 1)
    {
      ++this.red_rows;
      Object.Destroy((Object) this.line_r3.material);
      this.line_r3.material = this.color_red;
      this.line_r3.enabled = flag;
    }
    if (highestBall7 == highestBall8 && highestBall7 == highestBall9 && highestBall7 == 2)
    {
      ++this.blue_rows;
      Object.Destroy((Object) this.line_r3.material);
      this.line_r3.material = this.color_blue;
      this.line_r3.enabled = flag;
    }
    if (highestBall1 == highestBall4 && highestBall1 == highestBall7 && highestBall1 == 1)
    {
      ++this.red_rows;
      Object.Destroy((Object) this.line_r4.material);
      this.line_r4.material = this.color_red;
      this.line_r4.enabled = flag;
    }
    if (highestBall1 == highestBall4 && highestBall1 == highestBall7 && highestBall1 == 2)
    {
      ++this.blue_rows;
      Object.Destroy((Object) this.line_r4.material);
      this.line_r4.material = this.color_blue;
      this.line_r4.enabled = flag;
    }
    if (highestBall2 == highestBall5 && highestBall2 == highestBall8 && highestBall2 == 1)
    {
      ++this.red_rows;
      Object.Destroy((Object) this.line_r5.material);
      this.line_r5.material = this.color_red;
      this.line_r5.enabled = flag;
    }
    if (highestBall2 == highestBall5 && highestBall2 == highestBall8 && highestBall2 == 2)
    {
      ++this.blue_rows;
      Object.Destroy((Object) this.line_r5.material);
      this.line_r5.material = this.color_blue;
      this.line_r5.enabled = flag;
    }
    if (highestBall3 == highestBall6 && highestBall3 == highestBall9 && highestBall3 == 1)
    {
      ++this.red_rows;
      Object.Destroy((Object) this.line_r6.material);
      this.line_r6.material = this.color_red;
      this.line_r6.enabled = flag;
    }
    if (highestBall3 == highestBall6 && highestBall3 == highestBall9 && highestBall3 == 2)
    {
      ++this.blue_rows;
      Object.Destroy((Object) this.line_r6.material);
      this.line_r6.material = this.color_blue;
      this.line_r6.enabled = flag;
    }
    if (highestBall1 == highestBall5 && highestBall1 == highestBall9 && highestBall1 == 1)
    {
      ++this.red_rows;
      Object.Destroy((Object) this.line_r7.material);
      this.line_r7.material = this.color_red;
      this.line_r7.enabled = flag;
    }
    if (highestBall1 == highestBall5 && highestBall1 == highestBall9 && highestBall1 == 2)
    {
      ++this.blue_rows;
      Object.Destroy((Object) this.line_r7.material);
      this.line_r7.material = this.color_blue;
      this.line_r7.enabled = flag;
    }
    if (highestBall3 == highestBall5 && highestBall3 == highestBall7 && highestBall3 == 1)
    {
      ++this.red_rows;
      Object.Destroy((Object) this.line_r8.material);
      this.line_r8.material = this.color_red;
      this.line_r8.enabled = flag;
    }
    if (highestBall3 == highestBall5 && highestBall3 == highestBall7 && highestBall3 == 2)
    {
      ++this.blue_rows;
      Object.Destroy((Object) this.line_r8.material);
      this.line_r8.material = this.color_blue;
      this.line_r8.enabled = flag;
    }
    this.score_red = (float) (this.red_balls + 6 * this.red_rows);
    this.score_blue = (float) (this.blue_balls + 6 * this.blue_rows);
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
    if (this.time_total.TotalSeconds >= (double) (GLOBALS.TIMER_TOTAL - GLOBALS.TIMER_AUTO))
    {
      this.blue_winpoint = this.line_r6.enabled && highestBall3 == 2;
      this.red_winpoint = this.line_r4.enabled && highestBall1 == 1;
    }
    if (!GLOBALS.SINGLEPLAYER_MODE || !(bool) (Object) GLOBALS.topsingleplayer || GLOBALS.game_option != 3)
      return;
    this.score_red = (float) ((double) this.score_red - (double) this.score_blue + 63.0);
    this.score_blue = 0.0f;
  }

  public override int GetRedScore() => this.timerstate == Scorekeeper.TimerState.FINISHED ? this.score_redfinal : (int) this.score_red + this.score_redadj;

  public override int GetBlueScore() => this.timerstate == Scorekeeper.TimerState.FINISHED ? this.score_bluefinal : (int) this.score_blue + this.score_blueadj;

  private string LightToC(MeshRenderer light)
  {
    if (!(bool) (Object) this.cu_settings || !this.cu_settings.ENABLE_LINES && !this.cu_settings.ENABLE_LINES_SPEC || !light.enabled)
      return "0";
    return light.material.GetColor("_EmissionColor") == this.color_red.GetColor("_EmissionColor") ? (this.cu_settings.ENABLE_LINES ? "1" : "3") : (this.cu_settings.ENABLE_LINES ? "2" : "4");
  }

  private void CToLight(MeshRenderer light, char state)
  {
    if (state == '0')
    {
      light.enabled = false;
    }
    else
    {
      light.enabled = true;
      if (state == '1')
      {
        Object.Destroy((Object) light.material);
        light.material = this.color_red;
      }
      else if (state == '2')
      {
        Object.Destroy((Object) light.material);
        light.material = this.color_blue;
      }
      else if (state == '3' && this.client.isSpectator())
      {
        Object.Destroy((Object) light.material);
        light.material = this.color_red;
      }
      else if (state == '4' && this.client.isSpectator())
      {
        Object.Destroy((Object) light.material);
        light.material = this.color_blue;
      }
      else
        light.enabled = false;
    }
  }

  public override void SendServerData(Dictionary<string, string> serverFlags)
  {
    base.SendServerData(serverFlags);
    serverFlags["SK_LINES"] = this.LightToC(this.line_r1) + this.LightToC(this.line_r2) + this.LightToC(this.line_r3) + this.LightToC(this.line_r4) + this.LightToC(this.line_r5) + this.LightToC(this.line_r6) + this.LightToC(this.line_r7) + this.LightToC(this.line_r8);
    if ((bool) (Object) this.cu_settings && !this.cu_settings.ENABLE_LIGHTS)
    {
      serverFlags["SK_LIGHTS"] = "000000000";
    }
    else
    {
      Dictionary<string, string> dictionary = serverFlags;
      string[] strArray = new string[9]
      {
        this.scorer_tl.GetHighestBall().ToString(),
        this.scorer_tm.GetHighestBall().ToString(),
        this.scorer_tr.GetHighestBall().ToString(),
        this.scorer_l.GetHighestBall().ToString(),
        this.scorer_m.GetHighestBall().ToString(),
        null,
        null,
        null,
        null
      };
      int highestBall = this.scorer_r.GetHighestBall();
      strArray[5] = highestBall.ToString();
      highestBall = this.scorer_bl.GetHighestBall();
      strArray[6] = highestBall.ToString();
      highestBall = this.scorer_bm.GetHighestBall();
      strArray[7] = highestBall.ToString();
      highestBall = this.scorer_br.GetHighestBall();
      strArray[8] = highestBall.ToString();
      string str = string.Concat(strArray);
      dictionary["SK_LIGHTS"] = str;
    }
  }

  public override void ReceiveServerData(Dictionary<string, string> serverFlags)
  {
    base.ReceiveServerData(serverFlags);
    if (!serverFlags.ContainsKey("SK_LINES") || !serverFlags.ContainsKey("SK_LIGHTS"))
      return;
    string serverFlag1 = serverFlags["SK_LINES"];
    if (serverFlag1.Length < 8)
      return;
    this.CToLight(this.line_r1, serverFlag1[0]);
    this.CToLight(this.line_r2, serverFlag1[1]);
    this.CToLight(this.line_r3, serverFlag1[2]);
    this.CToLight(this.line_r4, serverFlag1[3]);
    this.CToLight(this.line_r5, serverFlag1[4]);
    this.CToLight(this.line_r6, serverFlag1[5]);
    this.CToLight(this.line_r7, serverFlag1[6]);
    this.CToLight(this.line_r8, serverFlag1[7]);
    string serverFlag2 = serverFlags["SK_LIGHTS"];
    if (serverFlag2.Length < 9)
      return;
    this.scorer_tl.SetHighestBall((int) serverFlag2[0] - 48);
    this.scorer_tm.SetHighestBall((int) serverFlag2[1] - 48);
    this.scorer_tr.SetHighestBall((int) serverFlag2[2] - 48);
    this.scorer_l.SetHighestBall((int) serverFlag2[3] - 48);
    this.scorer_m.SetHighestBall((int) serverFlag2[4] - 48);
    this.scorer_r.SetHighestBall((int) serverFlag2[5] - 48);
    this.scorer_bl.SetHighestBall((int) serverFlag2[6] - 48);
    this.scorer_bm.SetHighestBall((int) serverFlag2[7] - 48);
    this.scorer_br.SetHighestBall((int) serverFlag2[8] - 48);
  }
}
