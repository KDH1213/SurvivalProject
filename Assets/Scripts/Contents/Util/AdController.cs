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
            Log("AdMob 초기화 성공");

            // 광고 초기화 후 배너 광고 로드
            RequestInterstitialAd();
        });
    }


    private void RequestInterstitialAd()
    {
        // 광고가 이미 로드되어 있는 경우 제거
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Log("전면 광고 로드 시작...");

        // 광고 요청 객체 생성
        AdRequest request = new AdRequest();

        // 전면 광고 로드 요청
        RewardedAd.Load(interstitialAdUnitId, request, OnInterstitialAdLoaded);
    }

    // 광고 로드 완료 콜백
    private void OnInterstitialAdLoaded(GoogleMobileAds.Api.RewardedAd ad, LoadAdError error)
    {
        // 로드 실패 처리
        if (error != null)
        {
            Log($"전면 광고 로드 실패: {error.GetMessage()}");
            return;
        }

        // 로드 성공 시 광고 객체 저장
        interstitialAd = ad;
        Log("전면 광고 로드 성공");

        // 이벤트 핸들러 등록
        interstitialAd.OnAdFullScreenContentClosed += () => {
            Log("전면 광고가 닫혔습니다.");
            // 광고가 닫힐 때 새 광고 요청
            RequestInterstitialAd();
        };

        interstitialAd.OnAdFullScreenContentFailed += (AdError adError) => {
            Log($"전면 광고 표시 실패: {adError.GetMessage()}");
            // 실패 시 새 광고 요청
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
            Log("전면 광고가 아직 로드되지 않았습니다.");
            RequestInterstitialAd();
        }
    }

    private void RewardAd(Reward reward)
    {
        rewardEvent?.Invoke();
    }
}
