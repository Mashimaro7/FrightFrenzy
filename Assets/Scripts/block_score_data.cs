// Decompiled with JetBrains decompiler
// Type: block_score_data
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

public class block_score_data : gameElement
{
  public PositionState currposition = PositionState.Loading;
  public int pieces_in_crossing;
  public bool has_scored;

  public void StartTransition(float center_x)
  {
    if (this.pieces_in_crossing == 0)
      this.currposition = (double) this.transform.position.x >= (double) center_x ? PositionState.ToLoading : PositionState.ToBuilding;
    ++this.pieces_in_crossing;
  }

  public int EndTransition(float center_x)
  {
    --this.pieces_in_crossing;
    if (this.pieces_in_crossing > 0)
      return 0;
    if (this.currposition == PositionState.ToLoading)
    {
      if ((double) this.transform.position.x < (double) center_x)
      {
        this.has_scored = false;
        this.currposition = PositionState.Loading;
        return -1;
      }
      this.currposition = PositionState.Building;
    }
    if (this.currposition == PositionState.ToBuilding)
    {
      if ((double) this.transform.position.x > (double) center_x)
      {
        this.currposition = PositionState.Building;
        if (this.has_scored)
          return 0;
        this.has_scored = true;
        return 1;
      }
      this.currposition = PositionState.Loading;
    }
    return 0;
  }

  public bool OutOfRegion() => this.pieces_in_crossing == 0;

  public override void ResetPosition(int option = 1)
  {
    this.currposition = PositionState.Loading;
    this.pieces_in_crossing = 0;
    this.has_scored = false;
    base.ResetPosition(option);
  }
}
