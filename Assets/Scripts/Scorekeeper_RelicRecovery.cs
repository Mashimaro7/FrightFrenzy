// Decompiled with JetBrains decompiler
// Type: Scorekeeper_RelicRecovery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Scorekeeper_RelicRecovery : Scorekeeper
{
  private CubeScoring[,] RedBox1 = new CubeScoring[4, 3];
  private CubeScoring[,] RedBox2 = new CubeScoring[4, 3];
  private CubeScoring[,] BlueBox1 = new CubeScoring[4, 3];
  private CubeScoring[,] BlueBox2 = new CubeScoring[4, 3];

  private void Awake()
  {
    GLOBALS.PlayerCount = 4;
    GLOBALS.TIMER_TOTAL = 120;
    GLOBALS.TIMER_AUTO = 0;
    GLOBALS.TIMER_ENDGAME = 30;
  }

  public override void OnTimerStart()
  {
    base.OnTimerStart();
    this.ScorerReset();
  }

  public override void ScorerReset() => base.ScorerReset();

  public override void ScorerInit()
  {
    this.FindScoringElements("r1", this.RedBox1);
    this.FindScoringElements("r2", this.RedBox2);
    this.FindScoringElements("b1", this.BlueBox1);
    this.FindScoringElements("b2", this.BlueBox2);
  }

  private void FindScoringElements(string prefix, CubeScoring[,] Box)
  {
    for (int index1 = 0; index1 < 4; ++index1)
    {
      for (int index2 = 0; index2 < 3; ++index2)
      {
        GameObject gameObject = GameObject.Find(prefix + "_r" + (object) (index1 + 1) + "_" + (object) (index2 + 1));
        if ((Object) gameObject == (Object) null)
          Debug.LogError((object) ("Unable to find box scorring element for " + prefix + "_r" + (object) (index1 + 1) + "_" + (object) (index2 + 1)));
        else
          Box[index1, index2] = gameObject.GetComponent<CubeScoring>();
      }
    }
  }

  private int FindBoxScore(CubeScoring[,] Box)
  {
    int boxScore = 0;
    for (int index1 = 0; index1 < 4; ++index1)
    {
      int num = 0;
      gameElement previous_cube = (gameElement) null;
      for (int index2 = 0; index2 < 3; ++index2)
      {
        if ((bool) (Object) Box[index1, index2].IsCube(previous_cube))
        {
          boxScore += 2;
          ++num;
        }
      }
      if (num >= 3)
        boxScore += 10;
    }
    int num1 = 0;
    string str = "";
    for (int index3 = 0; index3 < 3; ++index3)
    {
      int num2 = 0;
      gameElement previous_cube = (gameElement) null;
      for (int index4 = 0; index4 < 4; ++index4)
      {
        if ((bool) (Object) (previous_cube = Box[index4, index3].IsCube(previous_cube)))
        {
          ++num2;
          str = previous_cube.type != ElementType.Cube ? str + "B" : str + "W";
        }
      }
      if (num2 >= 4)
      {
        boxScore += 20;
        ++num1;
      }
    }
    if (num1 >= 3 && (str == "WBWBBWBWWBWB" || str == "BWBWWBWBBWBW" || str == "WBBWBWWBWBBW" || str == "BWWBWBBWBWWB" || str == "BBWWBWWBWWBB" || str == "WWBBWBBWBBWW"))
      boxScore += 30;
    return boxScore;
  }

  public override int GetRedScore()
  {
    int num = 0;
    return this.timerstate == Scorekeeper.TimerState.FINISHED ? this.score_redfinal : num + this.FindBoxScore(this.RedBox1) + this.FindBoxScore(this.RedBox2) + this.score_redadj;
  }

  public override int GetBlueScore()
  {
    int num = 0;
    return this.timerstate == Scorekeeper.TimerState.FINISHED ? this.score_bluefinal : num + this.FindBoxScore(this.BlueBox1) + this.FindBoxScore(this.BlueBox2) + this.score_blueadj;
  }
}
