using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void EventHandler(params object [] arg);
public struct EventArg{
	public EventHandler eH;
	public object [] arg;
}
public class EventDispatcher : MonoBehaviour
{
	public Dictionary <string,List<EventArg>> eventDict = new Dictionary<string,List<EventArg>> ();

	public void getEventHandler(string msg,EventHandler eH, params object [] arg){
		EventArg eA;
		eA.eH = eH;
		eA.arg = arg;
		if (!eventDict.ContainsKey (msg)) {
			List<EventArg> eAL = new List<EventArg> ();
			eventDict.Add (msg, eAL);
		}
		eventDict [msg].Add (eA);	

	}
	public void sendMassage(string msg,params object [] arg){
		if (eventDict.ContainsKey (msg)) {
			for(int i = 0 ; i<eventDict[msg].Count;i++){
				//eventDict[msg][i].eH.Invoke(eventDict[msg][i].arg); //the rigister parameter
				eventDict[msg][i].eH.Invoke(arg);
			}
		}
	}
	public void sendMassageDelay(string msg,float delayTime, params object [] arg){
		//In Start() or wherever
		StartCoroutine(sendMassage(msg, delayTime, arg));
	}

	IEnumerator sendMassage(string msg,float delayTime,params object [] arg)
	{
		yield return new WaitForSeconds (delayTime);
		sendMassage(msg,arg);
	}

}