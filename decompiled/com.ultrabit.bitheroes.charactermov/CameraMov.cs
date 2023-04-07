using com.ultrabit.bitheroes.core;
using UnityEngine;
using UnityEngine.AI;

namespace com.ultrabit.bitheroes.charactermov;

public class CameraMov : MonoBehaviour
{
	private Vector2 velocity;

	public float smoothTimeX;

	public float smoothTimeY;

	public GameObject player;

	public NavMeshAgent playerAgent;

	private Vector2 minCameraPos;

	private Vector2 maxCameraPos;

	private Vector2 camSize;

	public GameObject target;

	private void Start()
	{
		base.transform.position = new Vector3(playerAgent.transform.position.x, base.transform.position.y, playerAgent.transform.position.z);
		camSize = new Vector2((float)GetComponent<Camera>().pixelWidth * 0.01f, (float)GetComponent<Camera>().pixelHeight * 0.01f);
		CameraBounds();
	}

	private void Update()
	{
	}

	private void LateUpdate()
	{
		if (player.GetComponent<CharacterMov>().click)
		{
			float num = Mathf.SmoothDamp(base.transform.position.x, playerAgent.transform.position.x, ref velocity.x, smoothTimeX);
			float num2 = Mathf.SmoothDamp(base.transform.position.z, playerAgent.transform.position.z, ref velocity.y, smoothTimeX);
			Vector3 position = new Vector3((float)(int)(num * 1000f) * 0.001f, base.transform.position.y, (float)(int)(num2 * 1000f) * 0.001f);
			base.transform.position = position;
			base.transform.position = new Vector3(Mathf.Clamp(base.transform.position.x, minCameraPos.x, maxCameraPos.x), base.transform.position.y, Mathf.Clamp(base.transform.position.z, minCameraPos.y, maxCameraPos.y));
		}
	}

	private void FixedUpdate()
	{
		if (!player.GetComponent<CharacterMov>().click)
		{
			float num = Mathf.SmoothDamp(base.transform.position.x, playerAgent.transform.position.x, ref velocity.x, smoothTimeX);
			float num2 = Mathf.SmoothDamp(base.transform.position.z, playerAgent.transform.position.z, ref velocity.y, smoothTimeX);
			Vector3 position = new Vector3((float)(int)(num * 1000f) * 0.001f, base.transform.position.y, (float)(int)(num2 * 1000f) * 0.001f);
			base.transform.position = position;
			base.transform.position = new Vector3(Mathf.Clamp(base.transform.position.x, minCameraPos.x, maxCameraPos.x), base.transform.position.y, Mathf.Clamp(base.transform.position.z, minCameraPos.y, maxCameraPos.y));
		}
	}

	private void CameraBounds()
	{
		float x = camSize.x;
		float y = camSize.y;
		float num = y * 0.5f / 2.3999999f;
		float num2 = x * 0.5f / (2.3999999f * (x / y));
		float x2 = GameData.instance.layerAdjustment.map.transform.position.x;
		float z = GameData.instance.layerAdjustment.map.transform.position.z;
		Vector2 vector = new Vector2(GameData.instance.layerAdjustment.mapSizeX * 3f, GameData.instance.layerAdjustment.mapSizeY * 3f);
		minCameraPos.x = x2 - vector.x * 0.5f + x * 0.5f / num2;
		minCameraPos.y = z - vector.y * 0.5f + y * 0.5f / num;
		maxCameraPos.x = x2 + vector.x * 0.5f - x * 0.5f / num2;
		maxCameraPos.y = z + vector.y * 0.5f - y * 0.5f / num;
	}
}
