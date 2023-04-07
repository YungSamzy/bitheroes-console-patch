using System.Collections;
using UnityEngine;

public class SessionManager : SingletonMonoBehaviour<SessionManager>
{
	public IEnumerator Initialize()
	{
		Debug.Log("SessionManager Initializer started");
		yield return new WaitForEndOfFrame();
	}
}
