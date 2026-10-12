using UnityEngine;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;
    string bannerID = "ca-app-pub-3940256099942544/6300978111";
    string interID = "ca-app-pub-3940256099942544/1033173712";
    string rewardedID = "ca-app-pub-3940256099942544/5224354917";

    BannerView bannerView;
    InterstitialAd interstitialAd;
    RewardedAd rewardedAd;

    void Awake(){
        if(Instance==null){ Instance=this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }
        MobileAds.Initialize(initStatus=>{});
    }

    void Start(){ ShowBanner(); LoadInterstitial(); LoadRewarded(); }

    public void ShowBanner(){
        if(bannerView!=null) bannerView.Destroy();
        bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);
        bannerView.LoadAd(new AdRequest());
    }

    void LoadInterstitial(){
        InterstitialAd.Load(interID, new AdRequest(), (ad, error)=>{
            if(error==null) interstitialAd = ad;
        });
    }

    public void ShowInterstitial(){
        if(interstitialAd!=null && interstitialAd.CanShowAd()){
            interstitialAd.Show();
            LoadInterstitial();
        }
    }

    void LoadRewarded(){
        RewardedAd.Load(rewardedID, new AdRequest(), (ad, error)=>{
            if(error==null) rewardedAd = ad;
        });
    }

    public void ShowRewarded(System.Action onReward){
        if(rewardedAd!=null && rewardedAd.CanShowAd()){
            rewardedAd.Show((reward)=>{
                onReward?.Invoke();
                LoadRewarded();
            });
        }
    }
}
