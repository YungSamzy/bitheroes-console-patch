using System.Collections.Generic;
using com.ultrabit.bitheroes.model.payment;
using com.ultrabit.bitheroes.model.utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

namespace com.ultrabit.bitheroes.transaction;

public class TransactionIAPMobile : MonoBehaviour, IStoreListener
{
	public class TransactionIAPMobileResult
	{
		public int code = -1;

		public string productID;

		public string receipt;

		public string failureReason;

		public PurchaseEventArgs args;
	}

	private static IStoreController m_StoreController;

	private static IExtensionProvider m_StoreExtensionProvider;

	public const int INITIALIZE_FAILED = 0;

	public const int PURCHASE_SUCCESS = 1;

	public const int PURCHASE_FAILED = 2;

	private UnityAction<TransactionIAPMobileResult> onPurchaseCompleted;

	private bool _onPurchaseProcess;

	private int _attempts;

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void InitializePurchasing(int attempts = 1)
	{
		D.Log("TransactionIAPMobileFlow - InitializePurchasing Attempt", forceLoggly: true);
		_attempts = attempts;
		_onPurchaseProcess = false;
		if (IsInitialized())
		{
			CheckForAdditionalProducts();
			return;
		}
		StandardPurchasingModule standardPurchasingModule = StandardPurchasingModule.Instance();
		D.Log($"TransactionIAPMobileFlow - StandardPurchasingModule:module: {standardPurchasingModule.appStore}", forceLoggly: true);
		ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(standardPurchasingModule);
		D.Log($"TransactionIAPMobileFlow - ConfigurationBuilder:builder: {standardPurchasingModule.appStore}", forceLoggly: true);
		List<PaymentData> paymentData = PaymentBook.GetPaymentData();
		D.Log($"TransactionIAPMobileFlow - Adding Items Count: {paymentData.Count}", forceLoggly: true);
		foreach (PaymentData item in paymentData)
		{
			if (item != null && item.paymentRef != null && item.paymentRef.paymentID != null)
			{
				configurationBuilder.AddProduct(item.paymentRef.paymentID, ProductType.Consumable);
			}
		}
		UnityPurchasing.Initialize(this, configurationBuilder);
	}

	private void CheckForAdditionalProducts()
	{
		HashSet<ProductDefinition> hashSet = new HashSet<ProductDefinition>();
		foreach (PaymentData paymentDatum in PaymentBook.GetPaymentData())
		{
			if (paymentDatum != null && paymentDatum.paymentRef != null && paymentDatum.paymentRef.paymentID != null && m_StoreController.products.WithID(paymentDatum.paymentRef.paymentID) == null)
			{
				hashSet.Add(new ProductDefinition(paymentDatum.paymentRef.paymentID, ProductType.Consumable));
			}
		}
		if (hashSet.Count > 0)
		{
			m_StoreController.FetchAdditionalProducts(hashSet, OnProductsFetchedSuccess, OnProductsFetchedFailed);
		}
	}

	private void OnProductsFetchedFailed(InitializationFailureReason obj)
	{
		D.LogError("TransactionIAPMobileFlow - [OnProductsFetched] Failed to add extra payments");
	}

	private void OnProductsFetchedSuccess()
	{
		D.Log("TransactionIAPMobileFlow - [OnProductsFetched] Payments Added Succesfully", forceLoggly: true);
	}

	public bool IsInitialized()
	{
		if (m_StoreController != null)
		{
			return m_StoreExtensionProvider != null;
		}
		return false;
	}

	public void InitPurchaseProcess(string productId, UnityAction<TransactionIAPMobileResult> onPurchaseCompleted)
	{
		this.onPurchaseCompleted = onPurchaseCompleted;
		_onPurchaseProcess = true;
		BuyProductID(productId);
	}

	private void BuyProductID(string productId)
	{
		if (IsInitialized())
		{
			Product product = m_StoreController.products.WithID(productId);
			if (product != null && product.availableToPurchase)
			{
				D.Log($"TransactionIAPMobileFlow - Purchasing product asychronously: '{0}'", product.definition.id, forceLoggly: true);
				m_StoreController.InitiatePurchase(product);
				return;
			}
			D.LogError("TransactionIAPMobileFlow - BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
			ExecutePurchaseCompleteCallback(new TransactionIAPMobileResult
			{
				code = 2,
				productID = productId,
				failureReason = "Product not available"
			});
		}
		else
		{
			D.LogError("TransactionIAPMobileFlow - Buy FAIL. Not initialized.");
			ExecutePurchaseCompleteCallback(new TransactionIAPMobileResult
			{
				code = 2,
				productID = productId,
				failureReason = "Initialization failed"
			});
		}
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		D.Log("TransactionIAPMobileFlow OnInitialized PASS", forceLoggly: true);
		m_StoreController = controller;
		m_StoreExtensionProvider = extensions;
		Product[] all = m_StoreController.products.all;
		foreach (Product product in all)
		{
			ConfirmPendingPurchase(product.definition.id);
		}
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		D.Log($"TransactionIAPMobileFlow - InitializationFailureReason: {error} => attempts: {_attempts}", forceLoggly: true);
		ExecutePurchaseCompleteCallback(new TransactionIAPMobileResult
		{
			code = 0
		});
		_attempts--;
		if (_attempts > 0)
		{
			InitializePurchasing(_attempts);
		}
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		D.Log("TransactionIAPMobileFlow - PurchaseProcessingResult " + args.purchasedProduct.definition.id);
		bool onPurchaseProcess = _onPurchaseProcess;
		ExecutePurchaseCompleteCallback(new TransactionIAPMobileResult
		{
			code = 1,
			productID = args.purchasedProduct.definition.id,
			receipt = args.purchasedProduct.receipt,
			args = args
		});
		if (onPurchaseProcess)
		{
			return PurchaseProcessingResult.Pending;
		}
		return PurchaseProcessingResult.Complete;
	}

	public void ConfirmPendingPurchase(string productID)
	{
		m_StoreController.ConfirmPendingPurchase(m_StoreController.products.WithID(productID));
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		D.LogError($"TransactionIAPMobileFlow - OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
		ExecutePurchaseCompleteCallback(new TransactionIAPMobileResult
		{
			code = 2,
			productID = product.definition.storeSpecificId,
			failureReason = failureReason.ToString()
		});
	}

	private void ExecutePurchaseCompleteCallback(TransactionIAPMobileResult result)
	{
		_onPurchaseProcess = false;
		if (onPurchaseCompleted != null)
		{
			onPurchaseCompleted(result);
		}
	}
}
