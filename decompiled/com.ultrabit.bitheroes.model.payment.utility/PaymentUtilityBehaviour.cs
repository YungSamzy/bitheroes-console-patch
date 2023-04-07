using System;
using System.Collections;
using System.Text;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.transaction;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace com.ultrabit.bitheroes.model.payment.utility;

public class PaymentUtilityBehaviour : MonoBehaviour
{
	public class KongregatePurchaseResponseEvent : UnityEvent<bool>
	{
	}

	private class GooglePurchaseData
	{
		[Serializable]
		private struct GooglePurchaseReceipt
		{
			public string Payload;
		}

		[Serializable]
		private struct GooglePurchasePayload
		{
			public string json;

			public string signature;
		}

		public string inAppPurchaseData;

		public string inAppDataSignature;

		public GooglePurchaseData(string receipt)
		{
			try
			{
				GooglePurchasePayload googlePurchasePayload = JsonUtility.FromJson<GooglePurchasePayload>(JsonUtility.FromJson<GooglePurchaseReceipt>(receipt).Payload);
				inAppPurchaseData = googlePurchasePayload.json;
				inAppDataSignature = googlePurchasePayload.signature;
			}
			catch
			{
				D.LogError("all", "IAPFlow - Receipt Validation Failed - Unable to Validate Receipt String");
				inAppPurchaseData = "";
				inAppDataSignature = "";
			}
		}

		public bool isValid()
		{
			if (inAppDataSignature != null && !inAppPurchaseData.Equals("") && inAppDataSignature != null)
			{
				return !inAppDataSignature.Equals("");
			}
			return false;
		}
	}

	[Serializable]
	public class ValidateAndroidReceiptObject
	{
		public int playerID;

		public string userID;

		public int charID;

		public int serverID;

		public string signature;

		public string jsonData;

		public string orderID;

		public string paymentID;
	}

	public class ValidateAndroidReceiptResponse
	{
		public enum CODE
		{
			UNKNOWN,
			SUCCESS,
			TRANSACTION_ALREADY_EXISTS,
			VALIDATION_ERROR,
			NETWORK_ERROR
		}

		public CODE code;

		public string message;

		public ValidateAndroidReceiptObject receiptObject;
	}

	public class AndroidValidationPurchaseComplete : UnityEvent<ValidateAndroidReceiptResponse>
	{
	}

	[Serializable]
	public class ValidateiOSReceiptObject
	{
		public int playerID;

		public string userID;

		public int charID;

		public int serverID;

		public string receipt;

		public string transactionID;
	}

	public class ValidateiOSReceiptResponse
	{
		public enum CODE
		{
			UNKNOWN,
			SUCCESS,
			TRANSACTION_ALREADY_EXISTS,
			VALIDATION_ERROR,
			NETWORK_ERROR
		}

		public CODE code;

		public string message;

		public ValidateiOSReceiptObject receiptObject;
	}

	[Serializable]
	public class ServerResponse
	{
		public string error;

		public int credits;
	}

	public class iOSValidationPurchaseComplete : UnityEvent<ValidateiOSReceiptResponse>
	{
	}

	public KongregatePurchaseResponseEvent onKongregatePaymentComplete = new KongregatePurchaseResponseEvent();

	public AndroidValidationPurchaseComplete onAndroidValidationPurchaseComplete = new AndroidValidationPurchaseComplete();

	private ValidateAndroidReceiptObject validateAndroidReceiptObject;

	public iOSValidationPurchaseComplete oniOSValidationPurchaseComplete = new iOSValidationPurchaseComplete();

	private ValidateiOSReceiptObject validateiOSReceiptObject;

	public void Start()
	{
		D.Log("Creating PaymentUtilityKongregatePurchase on GameObject " + base.gameObject.name);
	}

	public void OnPurchaseProductResult(string status)
	{
		bool arg = (status.Equals("SUCCESS") ? true : false);
		onKongregatePaymentComplete.Invoke(arg);
	}

	public void DoKongregatePurchase(string PaymentID)
	{
	}

	private void ExecuteCallback(ValidateAndroidReceiptResponse response)
	{
		if (onAndroidValidationPurchaseComplete != null)
		{
			onAndroidValidationPurchaseComplete.Invoke(response);
		}
		else
		{
			D.LogError("all", "IAPFlow - Receipt Validation Failed  - ExecuteCallback does not have a valid target");
		}
	}

	private void ExecuteCallback(ValidateiOSReceiptResponse response)
	{
		if (oniOSValidationPurchaseComplete != null)
		{
			oniOSValidationPurchaseComplete.Invoke(response);
		}
		else
		{
			D.LogError("all", "IAPFlow - Receipt Validation Failed  - ExecuteCallback does not have a valid target");
		}
	}

	public void ValidateAndroidReceipt(TransactionIAPMobile.TransactionIAPMobileResult transactionResult)
	{
		validateAndroidReceiptObject = new ValidateAndroidReceiptObject();
		GooglePurchaseData googlePurchaseData = new GooglePurchaseData(transactionResult.args.purchasedProduct.receipt);
		if (!googlePurchaseData.isValid())
		{
			ExecuteCallback(new ValidateAndroidReceiptResponse
			{
				code = ValidateAndroidReceiptResponse.CODE.VALIDATION_ERROR,
				message = "Unable to validate Receipt"
			});
			D.LogError("all", "IAPFlow - Receipt Validation Failed  - Unable to Validate Receipt");
			return;
		}
		validateAndroidReceiptObject.playerID = AppInfo.playerID;
		validateAndroidReceiptObject.userID = AppInfo.userID;
		validateAndroidReceiptObject.charID = GameData.instance.PROJECT.character.id;
		validateAndroidReceiptObject.serverID = GameData.instance.SAVE_STATE.serverID;
		validateAndroidReceiptObject.signature = googlePurchaseData.inAppDataSignature;
		validateAndroidReceiptObject.jsonData = googlePurchaseData.inAppPurchaseData;
		validateAndroidReceiptObject.orderID = transactionResult.args.purchasedProduct.transactionID;
		validateAndroidReceiptObject.paymentID = transactionResult.args.purchasedProduct.definition.id;
		string s = JsonUtility.ToJson(validateAndroidReceiptObject);
		string encoder = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
		StartCoroutine(ValidateAndroidWWW(encoder));
	}

	private IEnumerator ValidateAndroidWWW(string encoder)
	{
		WWWForm form = new WWWForm();
		form.AddField("data", encoder);
		string serverResponse = "";
		string networkError = "";
		for (int i = 0; i < 3; i++)
		{
			using (UnityWebRequest www = UnityWebRequest.Post(AppInfo.getAndroidValidateURL(), form))
			{
				yield return www.SendWebRequest();
				while (!www.isDone)
				{
					yield return null;
				}
				if (string.IsNullOrEmpty(www.error))
				{
					string text = www.downloadHandler.text;
					try
					{
						ServerResponse serverResponse2 = JsonUtility.FromJson<ServerResponse>(text);
						if (serverResponse2 != null && serverResponse2.error.Equals(""))
						{
							ExecuteCallback(new ValidateAndroidReceiptResponse
							{
								code = ValidateAndroidReceiptResponse.CODE.SUCCESS,
								message = null,
								receiptObject = validateAndroidReceiptObject
							});
							yield break;
						}
						serverResponse = serverResponse2.error;
						D.LogError("all", $"IAPFlow - Receipt Validation error -  Step {i} - {serverResponse}");
					}
					catch (Exception ex)
					{
						networkError = ex.Message + " - " + text;
						D.LogError("all", $"IAPFlow - Receipt Validation error -  Step {i} - {networkError}");
					}
				}
				else
				{
					D.LogError("all", $"IAPFlow - Receipt Validation error -  Step {i} - {www.error}");
					networkError = www.error;
				}
			}
			yield return new WaitForSeconds(2f);
		}
		ValidateAndroidReceiptResponse response = ((string.IsNullOrEmpty(serverResponse) && !string.IsNullOrEmpty(networkError)) ? new ValidateAndroidReceiptResponse
		{
			code = ValidateAndroidReceiptResponse.CODE.NETWORK_ERROR,
			message = networkError
		} : ((serverResponse == null || !(serverResponse == "Transaction already exists")) ? new ValidateAndroidReceiptResponse
		{
			code = ValidateAndroidReceiptResponse.CODE.VALIDATION_ERROR,
			message = serverResponse
		} : new ValidateAndroidReceiptResponse
		{
			code = ValidateAndroidReceiptResponse.CODE.TRANSACTION_ALREADY_EXISTS,
			message = serverResponse
		}));
		ExecuteCallback(response);
	}

	public void ValidateiOSReceipt(TransactionIAPMobile.TransactionIAPMobileResult transactionResult)
	{
		validateiOSReceiptObject = new ValidateiOSReceiptObject();
		if (JSON.Parse(transactionResult.args.purchasedProduct.receipt) == null)
		{
			oniOSValidationPurchaseComplete?.Invoke(new ValidateiOSReceiptResponse
			{
				code = ValidateiOSReceiptResponse.CODE.VALIDATION_ERROR,
				message = "Unable to validate Receipt"
			});
			return;
		}
		validateiOSReceiptObject.playerID = AppInfo.playerID;
		validateiOSReceiptObject.userID = AppInfo.userID;
		validateiOSReceiptObject.charID = GameData.instance.PROJECT.character.id;
		validateiOSReceiptObject.serverID = GameData.instance.SAVE_STATE.serverID;
		validateiOSReceiptObject.receipt = transactionResult.receipt;
		validateiOSReceiptObject.transactionID = transactionResult.args.purchasedProduct.transactionID;
		string s = JsonUtility.ToJson(validateiOSReceiptObject);
		string encoder = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
		StartCoroutine(ValidateiOSWWW(encoder));
	}

	private IEnumerator ValidateiOSWWW(string encoder)
	{
		WWWForm form = new WWWForm();
		form.AddField("data", encoder);
		string serverResponse = "";
		string networkError = "";
		for (int i = 0; i < 3; i++)
		{
			using (UnityWebRequest www = UnityWebRequest.Post(AppInfo.getiOSValidateURL(), form))
			{
				yield return www.SendWebRequest();
				while (!www.isDone)
				{
					yield return null;
				}
				if (string.IsNullOrEmpty(www.error))
				{
					string text = www.downloadHandler.text;
					try
					{
						ServerResponse serverResponse2 = JsonUtility.FromJson<ServerResponse>(text);
						if (serverResponse2 != null && !serverResponse2.error.Equals("") && int.Parse(serverResponse2.error) == 0)
						{
							ExecuteCallback(new ValidateiOSReceiptResponse
							{
								code = ValidateiOSReceiptResponse.CODE.SUCCESS,
								message = null,
								receiptObject = validateiOSReceiptObject
							});
							yield break;
						}
						serverResponse = serverResponse2.error;
						D.LogError("all", $"IAPFlow - Receipt Validation error -  Step {i} - {serverResponse}");
					}
					catch (Exception ex)
					{
						networkError = ex.Message + " - " + text;
						D.LogError("all", $"IAPFlow - Receipt Validation error -  Step {i} - {networkError}");
					}
				}
				else
				{
					D.LogError("all", $"IAPFlow - Receipt Validation error -  Step {i} - {www.error}");
					networkError = www.error;
				}
			}
			yield return new WaitForSeconds(2f);
		}
		ValidateiOSReceiptResponse response = ((string.IsNullOrEmpty(serverResponse) && !string.IsNullOrEmpty(networkError)) ? new ValidateiOSReceiptResponse
		{
			code = ValidateiOSReceiptResponse.CODE.NETWORK_ERROR,
			message = networkError
		} : ((int.Parse(serverResponse) != -2) ? new ValidateiOSReceiptResponse
		{
			code = ValidateiOSReceiptResponse.CODE.VALIDATION_ERROR,
			message = Language.GetString("error_payment_validate")
		} : new ValidateiOSReceiptResponse
		{
			code = ValidateiOSReceiptResponse.CODE.VALIDATION_ERROR,
			message = Language.GetString("error_payment_redeemed")
		}));
		ExecuteCallback(response);
	}
}
