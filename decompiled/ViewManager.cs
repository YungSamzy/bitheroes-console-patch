using System.Collections;
using UnityEngine;

public class ViewManager : SingletonMonoBehaviour<ViewManager>
{
	public IEnumerator Initialize()
	{
		Debug.Log("ViewManager Initializer started");
		yield return new WaitForEndOfFrame();
	}
}
