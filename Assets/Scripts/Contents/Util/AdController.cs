using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Events;

public class AdController : MonoBehaviour
{
    public string interstitialAdUnitId = "ca-app-pub-3940256099942544/5224354917";

    private RewardedAd interstitialAd;
    public UnityEvent rewardEvent;

    public void Log(string log)
    {
        Debug.Log(log);
    }

    private void Start()
    {
        InitializeAdMob();
    }

    private void InitializeAdMob()
    {
        MobileAds.Initialize(initStatus => {
            Log("AdMob �ʱ�ȭ ����");

            // ���� �ʱ�ȭ �� ��� ���� �ε�
            RequestInterstitialAd();
        });
    }


    private void RequestInterstitialAd()
    {
        // ���� �̹� �ε�Ǿ� �ִ� ��� ����
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Log("���� ���� �ε� ����...");

        // ���� ��û ��ü ����
        AdRequest request = new AdRequest();

        // ���� ���� �ε� ��û
        RewardedAd.Load(interstitialAdUnitId, request, OnInterstitialAdLoaded);
    }

    // ���� �ε� �Ϸ� �ݹ�
    private void OnInterstitialAdLoaded(GoogleMobileAds.Api.RewardedAd ad, LoadAdError error)
    {
        // �ε� ���� ó��
        if (error != null)
        {
            Log($"���� ���� �ε� ����: {error.GetMessage()}");
            return;
        }

        // �ε� ���� �� ���� ��ü ����
        interstitialAd = ad;
        Log("���� ���� �ε� ����");

        // �̺�Ʈ �ڵ鷯 ���
        interstitialAd.OnAdFullScreenContentClosed += () => {
            Log("���� ���� �������ϴ�.");
            // ���� ���� �� �� ���� ��û
            RequestInterstitialAd();
        };

        interstitialAd.OnAdFullScreenContentFailed += (AdError adError) => {
            Log($"���� ���� ǥ�� ����: {adError.GetMessage()}");
            // ���� �� �� ���� ��û
            RequestInterstitialAd();
        };
    }

    [ContextMenu("TestAd")]
    public void ShowInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Show(RewardAd);
        }
        else
        {
            Log("���� ���� ���� �ε���� �ʾҽ��ϴ�.");
            RequestInterstitialAd();
        }
    }

    private void RewardAd(Reward reward)
    {
        rewardEvent?.Invoke();
    }
}
