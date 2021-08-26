using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    public static AdManager instance;
    private string appID;

    private BannerView bannerView;
    private string bannerId= "ca-app-pub-1228319715524391/6288099220";

    private InterstitialAd interstitialAdView;
    private string fullScreenAdId = "ca-app-pub-1228319715524391/1007852389";

    bool enableAds = false;

    private void Awake()
    {

        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadBanner()
    {
        if (!enableAds)
            return;
        //bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Bottom);
        //AdRequest adRequest = new AdRequest.Builder().Build();
        //bannerView.LoadAd(adRequest);
        
    }
    public void showBanner()
    {
        if (!enableAds)
            return;
        //bannerView.Show();
    }
    public void hideBanner()
    {
        if (!enableAds)
            return;
        //bannerView.Hide();
    }
    public void loadFullScreenAd()
    {
        if (!enableAds)
            return;
        interstitialAdView = new InterstitialAd(fullScreenAdId);
        AdRequest request = new AdRequest.Builder().Build();
        interstitialAdView.LoadAd(request);


    }


    public void showFullScreenAd()
    {
        if (!enableAds)
            return;
        if (interstitialAdView.IsLoaded())
            interstitialAdView.Show();
        else
            Debug.Log("full screen ad not loaded");
    }
}
