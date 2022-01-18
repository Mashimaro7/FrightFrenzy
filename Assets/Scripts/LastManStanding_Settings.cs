// Decompiled with JetBrains decompiler
// Type: LastManStanding_Settings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LastManStanding_Settings : GameSettings
{
  public GameObject menu;
  public Transform HitCount;
  public int HIT_COUNT = 2;
  public Transform PlayerCount;
  public int PLAYER_COUNT = 12;
  public Transform FreeForAll;
  public bool FREE_FOR_ALL = true;
  public Transform RowCount;
  public int ROW_COUNT = 3;
  public Transform MapSize;
  public float ROW_SPACING = 2.5f;
  public Transform SpawnWalls;
  public bool SPAWN_WALLS = true;
  private bool init_done;

  private void Start()
  {
    this.init_done = false;
    this.HitCount.GetComponent<InputField>().text = this.HIT_COUNT.ToString();
    this.PlayerCount.GetComponent<InputField>().text = this.PLAYER_COUNT.ToString();
    this.FreeForAll.GetComponent<Toggle>().isOn = this.FREE_FOR_ALL;
    this.RowCount.GetComponent<TMP_Dropdown>().value = this.RowValueToIndex(this.ROW_COUNT);
    this.MapSize.GetComponent<TMP_Dropdown>().value = this.RowSpacingToMapSize(this.ROW_SPACING);
    GLOBALS.PlayerCount = this.PLAYER_COUNT;
    this.SpawnWalls.GetComponent<Toggle>().isOn = this.SPAWN_WALLS;
    this.init_done = true;
    this.MenuChanged();
    this.init_done = true;
  }

  private int RowValueToIndex(int value) => value == 3 || value != 4 ? 0 : 1;

  private int RowSpacingToMapSize(float value)
  {
    if ((double) value < 2.90000009536743)
      return 0;
    return (double) value > 3.09999990463257 ? 2 : 1;
  }

  private float MapSizeToRowSpacing(string size)
  {
    if (size == "Small")
      return 2.5f;
    return size == "Large" ? 3.5f : 3f;
  }

  public void MenuChanged()
  {
    if ((Object) this.menu == (Object) null || !this.init_done)
      return;
    this.FREE_FOR_ALL = this.FreeForAll.GetComponent<Toggle>().isOn;
    if (int.TryParse(this.HitCount.GetComponent<InputField>().text, out this.HIT_COUNT) && this.HIT_COUNT < 1)
      this.HIT_COUNT = 1;
    this.HitCount.GetComponent<InputField>().text = this.HIT_COUNT.ToString();
    if (int.TryParse(this.PlayerCount.GetComponent<InputField>().text, out this.PLAYER_COUNT))
    {
      if (this.PLAYER_COUNT < 2)
        this.PLAYER_COUNT = 2;
      if (this.PLAYER_COUNT > 12)
        this.PLAYER_COUNT = 12;
    }
    this.PlayerCount.GetComponent<InputField>().text = this.PLAYER_COUNT.ToString();
    GLOBALS.PlayerCount = this.PLAYER_COUNT;
    this.ROW_COUNT = int.Parse(this.RowCount.GetComponent<TMP_Dropdown>().options[this.RowCount.GetComponent<TMP_Dropdown>().value].text);
    this.ROW_SPACING = this.MapSizeToRowSpacing(this.MapSize.GetComponent<TMP_Dropdown>().options[this.MapSize.GetComponent<TMP_Dropdown>().value].text);
    this.SPAWN_WALLS = this.SpawnWalls.GetComponent<Toggle>().isOn;
    if (this.ROW_COUNT == 3 && this.MapSize.GetComponent<TMP_Dropdown>().value == 0)
    {
      this.SPAWN_WALLS = false;
      this.SpawnWalls.GetComponent<Toggle>().isOn = false;
      this.SpawnWalls.GetComponent<Toggle>().interactable = false;
    }
    else
      this.SpawnWalls.GetComponent<Toggle>().interactable = true;
    this.UpdateServer();
  }

  public void OnClose()
  {
    if ((Object) this.menu == (Object) null)
      return;
    this.menu.SetActive(false);
  }

  public override string GetString() => (this.FREE_FOR_ALL ? "1" : "0") + ":" + this.HIT_COUNT.ToString() + ":" + this.PLAYER_COUNT.ToString() + ":" + this.ROW_COUNT.ToString() + ":" + this.ROW_SPACING.ToString() + ":" + (this.SPAWN_WALLS ? "1" : "0");

  public override void SetString(string data)
  {
    if (this.menu.activeSelf)
      return;
    string[] strArray = data.Split(':');
    if (strArray.Length < 6)
    {
      Debug.Log((object) ("Free-For-All settings string did not have >=6 entries. It had " + (object) strArray.Length));
    }
    else
    {
      this.FREE_FOR_ALL = strArray[0] == "1";
      this.HIT_COUNT = int.Parse(strArray[1]);
      this.PLAYER_COUNT = int.Parse(strArray[2]);
      this.ROW_COUNT = int.Parse(strArray[3]);
      this.ROW_SPACING = float.Parse(strArray[4]);
      this.SPAWN_WALLS = strArray[5] == "1";
      this.Start();
      this.UpdateServer();
    }
  }
}
