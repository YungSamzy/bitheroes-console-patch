public interface ISAdQualityInitCallback
{
	void adQualitySdkInitSuccess();

	void adQualitySdkInitFailed(ISAdQualityInitError adQualityInitError, string errorMessage);
}
