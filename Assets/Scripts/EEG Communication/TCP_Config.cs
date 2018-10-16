using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System;
using System.Collections.Generic;

public class TCP_Config : MonoBehaviour
{

    public static float numSecondsBeforeAlignment = 10.0f;
    //#if UNITY_EDITOR
    //    public static string HostIPAddress { get { return GetLocalIPAddress(); } }
    //    public static int ConnectionPort = 8001; //8001 for Mac Pro Desktop communication
    //#else
    public static string HostIPAddress = "192.168.137.200"; //"169.254.50.2" for Mac Pro Desktop.
    public static int ConnectionPort = 8888; //8001 for Mac Pro Desktop communication
    //public static string HostIPAddress = "127.0.0.1"; //"169.254.50.2" for Mac Pro Desktop.
    //public static int ConnectionPort = 8001; //8001 for Mac Pro Desktop communication

    //public static string HostIPAddress { get { return GetLocalIPAddress(); } }
    //public static int ConnectionPort = 8001; //8001 for Mac Pro Desktop communication
    //#endif


    public static char MSG_START = '{';
	public static char MSG_END = '}';

	public static string ExpName { get { return GetExpName (); } }
	//public static string SubjectName = ExperimentSettings.currentSubject.name;

	public static float ClockAlignInterval = 60.0f; //this should happen about once a minute

	public enum EventType {
		SUBJECTID,
		EXPNAME,
		VERSION,
		INFO,
		CONTROL,
		DEFINE,
		SESSION,
		PRACTICE,
		TRIAL,
		PHASE,
		DISPLAYON,
		DISPLAYOFF,
		HEARTBEAT,
		ALIGNCLOCK,
		ABORT,
		SYNC,
		SYNCNP,
		SYNCED,
		STATE,
		EXIT
	}

	public enum SessionType{
		CLOSED_STIM,
		OPEN_STIM,
		NO_STIM
	}

	public static SessionType sessionType { get { return GetSessionType (); } }


	void Start(){

	}

	static string GetExpName(){
		return Config.BuildVersion.ToString ();
	}
	
	public static SessionType GetSessionType(){
		switch (Config.BuildVersion) {
		case Config.Version.MAZE:
			return SessionType.NO_STIM;
		}

		return SessionType.NO_STIM;
	}

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    //fill in how you see fit!
    public enum DefineStates
	{
		NAVIGATION,
		STIM_NAVIGATION,
		TREASURE_OPEN_SPECIAL,
		TREASURE_OPEN_EMPTY,
		TREASURE_1,
		TREASURE_2,
		TREASURE_3,
		TREASURE_4,
		RECALLCUE_1,
		RECALLCUE_2,
		RECALLCUE_3,
		RECALLCHOOSE_1,
		RECALLCHOOSE_2,
		RECALLCHOOSE_3,
		FEEDBACK,
		SCORESCREEN,
		BLOCKSCREEN,
		DISTRACTOR,
		PAUSED
	}

	public static List<string> GetDefineList(){
		List<string> defineList = new List<string> ();

		DefineStates[] values = (DefineStates[])DefineStates.GetValues(typeof(DefineStates));

		foreach (DefineStates defineState in values)
		{
			defineList.Add(defineState.ToString());
		}

		return defineList;
	}

}
