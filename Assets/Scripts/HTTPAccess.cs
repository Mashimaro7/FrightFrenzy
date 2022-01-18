// Decompiled with JetBrains decompiler
// Type: HTTPAccess
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HTTPAccess : MonoBehaviour
{
  public GameObject gui_listofservers;
  public GameObject gui_serverlineprefab;
  public GameObject gui_ip_target_text;
  public GameObject error_msg;
  private List<ServerInfo> serverlist = new List<ServerInfo>();

  private void Start()
  {
  }

  public void ClearGUIServerList()
  {
    foreach (Component component in this.gui_listofservers.transform)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
  }

  public void RefreshGUIServerList()
  {
    if ((UnityEngine.Object) this.gui_listofservers == (UnityEngine.Object) null || (UnityEngine.Object) this.gui_serverlineprefab == (UnityEngine.Object) null)
      return;
    this.ClearGUIServerList();
    this.serverlist.Sort((Comparison<ServerInfo>) ((x, y) => x.PING.CompareTo(y.PING)));
    foreach (ServerInfo serverInfo in this.serverlist)
    {
      ServerInfo currserver = serverInfo;
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.gui_serverlineprefab, this.gui_listofservers.transform);
      currserver.gui_line = gameObject;
      gameObject.transform.Find("IP").GetComponent<UnityEngine.UI.Text>().text = currserver.IP;
      gameObject.transform.Find("GAME").GetComponent<UnityEngine.UI.Text>().text = currserver.GAME;
      gameObject.transform.Find("PING").GetComponent<UnityEngine.UI.Text>().text = (currserver.PING * 1000f).ToString("G0") + "ms";
      gameObject.transform.Find("PLAYERS").GetComponent<UnityEngine.UI.Text>().text = currserver.PLAYERS.ToString() + "/" + (object) currserver.MAXPLAYERS;
      gameObject.transform.Find("VERSION").GetComponent<UnityEngine.UI.Text>().text = currserver.VERSION;
      gameObject.transform.Find("COMMENT").GetComponent<UnityEngine.UI.Text>().text = currserver.COMMENT;
      if (currserver.PASSWORD == "1")
        gameObject.transform.Find("PASSWORD").gameObject.SetActive(true);
      else
        gameObject.transform.Find("PASSWORD").gameObject.SetActive(false);
      gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener((UnityAction) (() => this.OnServerClick(currserver.IP, currserver.PORT)));
    }
  }

  public void OnServerClick(string server_ip, int server_port)
  {
    this.gui_ip_target_text.transform.Find("IPInput").GetComponent<InputField>().text = server_ip;
    this.gui_ip_target_text.transform.Find("PORTInput").GetComponent<InputField>().text = server_port.ToString();
  }

  public void StartServerRefresh()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.RetrieveHTTPServerList());
  }

  private IEnumerator RetrieveHTTPServerList()
  {
    this.ClearGUIServerList();
    this.serverlist.Clear();
    string[] strArray;
    using (UnityWebRequest www = UnityWebRequest.Get("http://xrcsimulator.org/game/getserverlist.php"))
    {
      yield return (object) www.SendWebRequest();
      if (www.isNetworkError || www.isHttpError)
      {
        UnityEngine.Object.Instantiate<GameObject>(this.error_msg, this.gui_listofservers.transform).GetComponent<UnityEngine.UI.Text>().text = "Error! Unable to connect to master server!";
        yield break;
      }
      else
        strArray = www.downloadHandler.text.Split('\n');
    }
    bool flag = false;
    Regex regex1 = new Regex("^\\s*BEGIN\\s*\\n");
    Regex regex2 = new Regex("^\\s*END\\s*\\n");
    Regex regex3 = new Regex("^\\s*IP=(.+)\\|PORT=(\\d+)\\s*");
    for (int index = 0; index < strArray.Length && !regex2.Match(strArray[index]).Success; ++index)
    {
      if (!flag && regex1.Match(strArray[index]).Success)
      {
        flag = true;
      }
      else
      {
        Match match = regex3.Match(strArray[index]);
        if (match.Success)
        {
          ServerInfo serverInfo = new ServerInfo();
          serverInfo.IP = match.Groups[1].Value;
          if (!int.TryParse(match.Groups[2].Value, out serverInfo.PORT))
            Debug.Log((object) ("Failed to parse PORT # from " + strArray[index]));
          this.serverlist.Add(serverInfo);
        }
      }
    }
    if (this.serverlist.Count == 0)
    {
      UnityEngine.Object.Instantiate<GameObject>(this.error_msg, this.gui_listofservers.transform).GetComponent<UnityEngine.UI.Text>().text = "No servers found.";
    }
    else
    {
      this.RefreshGUIServerList();
      for (int index = 0; index < this.serverlist.Count; ++index)
        this.serverlist[index].pingobject = new Ping(this.serverlist[index].IP);
      yield return (object) 0;
      bool waiting_on_servers = true;
      bool servers_updated = false;
      long start_ping_time = MyUtils.GetTimeMillis();
      while (waiting_on_servers && MyUtils.GetTimeMillis() - start_ping_time < 1500L)
      {
        waiting_on_servers = false;
        if (this.serverlist.Count == 0)
        {
          yield break;
        }
        else
        {
          for (int index = 0; index < this.serverlist.Count; ++index)
          {
            if (this.serverlist[index].pingobject != null)
            {
              if (!this.serverlist[index].pingobject.isDone)
              {
                waiting_on_servers = true;
              }
              else
              {
                this.serverlist[index].PING = (float) this.serverlist[index].pingobject.time / 1000f;
                this.serverlist[index].pingobject.DestroyPing();
                this.serverlist[index].pingobject = (Ping) null;
                servers_updated = true;
              }
            }
          }
          if (servers_updated)
            this.RefreshGUIServerList();
          yield return (object) 0;
        }
      }
      for (int index = 0; index < this.serverlist.Count; ++index)
      {
        if (this.serverlist[index].pingobject != null)
        {
          this.serverlist[index].pingobject.DestroyPing();
          this.serverlist[index].pingobject = (Ping) null;
        }
      }
      this.RefreshGUIServerList();
      UdpClient m_udpClient = new UdpClient();
      for (int i = 0; i < this.serverlist.Count; ++i)
      {
        byte[] bytes = Encoding.UTF8.GetBytes("11115\u0011" + this.serverlist[i].IP + "\u0012" + (object) this.serverlist[i].PORT + "\u0011");
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(this.serverlist[i].IP), this.serverlist[i].PORT);
        try
        {
          m_udpClient.Send(bytes, bytes.Length, endPoint);
        }
        catch (Exception ex)
        {
        }
        yield return (object) 0;
      }
      start_ping_time = MyUtils.GetTimeMillis();
      waiting_on_servers = true;
      while (waiting_on_servers && MyUtils.GetTimeMillis() - start_ping_time < 1500L)
      {
        yield return (object) 0;
        if (m_udpClient == null)
          yield break;
        else if (m_udpClient.Available >= 10)
        {
          IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
          try
          {
            this.ProcessReceivedServerInfo(m_udpClient.Receive(ref remoteEP));
          }
          catch (Exception ex)
          {
          }
          this.RefreshGUIServerList();
        }
      }
      this.RefreshGUIServerList();
    }
  }

  private void ProcessReceivedServerInfo(byte[] receivedBytes)
  {
    List<byte[]> extracted_data = new List<byte[]>();
    MyUtils.ExtractMessageHeader(receivedBytes, extracted_data);
    if (!Encoding.UTF8.GetString(extracted_data[0]).Equals("11115") || extracted_data.Count < 3)
      return;
    string str1 = Encoding.UTF8.GetString(extracted_data[1]);
    string str2 = Encoding.UTF8.GetString(extracted_data[2]);
    char[] chArray = new char[1]{ '\u0012' };
    string[] strArray1 = str1.Split(chArray);
    string[] strArray2 = str2.Split('\u0012');
    if (strArray2.Length < 2)
      return;
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    for (int index = 0; index < strArray1.Length - 1; index += 2)
      dictionary[strArray1[index]] = strArray1[index + 1];
    string str3 = strArray2[0];
    int result = 0;
    if (!int.TryParse(strArray2[1], out result))
      return;
    for (int index = 0; index < this.serverlist.Count; ++index)
    {
      if (!(this.serverlist[index].IP != str3) && this.serverlist[index].PORT == result)
      {
        using (Dictionary<string, string>.KeyCollection.Enumerator enumerator = dictionary.Keys.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            string current = enumerator.Current;
            try
            {
              if (!(current == "GAME"))
              {
                if (!(current == "PLAYERS"))
                {
                  if (!(current == "MAXPLAYERS"))
                  {
                    if (!(current == "VERSION"))
                    {
                      if (!(current == "COMMENT"))
                      {
                        if (current == "PASSWORD")
                          this.serverlist[index].PASSWORD = dictionary["PASSWORD"];
                      }
                      else
                        this.serverlist[index].COMMENT = dictionary["COMMENT"];
                    }
                    else
                      this.serverlist[index].VERSION = dictionary["VERSION"];
                  }
                  else
                    this.serverlist[index].MAXPLAYERS = int.Parse(dictionary["MAXPLAYERS"]);
                }
                else
                  this.serverlist[index].PLAYERS = int.Parse(dictionary["PLAYERS"]);
              }
              else
                this.serverlist[index].GAME = dictionary["GAME"];
            }
            catch (Exception ex)
            {
            }
          }
          break;
        }
      }
    }
  }
}
