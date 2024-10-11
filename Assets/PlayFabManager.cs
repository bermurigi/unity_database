using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using PlayFab.ProfilesModels;
using UnityEngine.UI;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager instance;
    public Text coinsValueText,LogText;
    public GameObject loginPanel, gamePanel;
    public InputField EmailInput, PasswordInput, UsernameInput;
    public PlayerData playerData;


    public Text errorText;
    private void Awake()
    {
        instance = this;
    }

    public void LoginBtn()
    {
        var request = new LoginWithEmailAddressRequest { Email=EmailInput.text, Password = PasswordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginError);
        
    }
    public void RegisterBtn()
    {
        var request = new RegisterPlayFabUserRequest { Email=EmailInput.text, Password = PasswordInput.text,Username = UsernameInput.text,DisplayName = UsernameInput.text,};
        PlayFabClientAPI.RegisterPlayFabUser(request, (result)=>Debug.Log("회원가입 성공"), OnRegisterFaliure);
    }
    private void OnRegisterSuccess(LoginResult result)
    {
        
        Debug.Log("로그인 성공!");
        
    }
    private void OnRegisterFaliure(PlayFabError error)
    {
        Debug.Log("회원가입 실패");
        Debug.Log(error);
        errorText.text = error.ToString();
        

    }
    private void OnLoginError(PlayFabError error)
    {
        Debug.Log("로그인실패");
        Debug.Log(error);
        errorText.text = error.ToString();
        

    }

    private void OnLoginSuccess(LoginResult result)
    {
        
        Debug.Log("로그인 성공!");
        loginPanel.SetActive(false);
        gamePanel.SetActive(true);
        GetVirtualCurrencies();
        GetAppearance();
        GetReaderboard();
        GetStat();
        Debug.Log(GetServerTime());
        playerData.TextUpdate();
        GetDisplayName();
    }
    // private void OnLoginFailure(PlayFabError error) => Debug.Log("로그인 실패");
    // private void OnRegisterSuccess(RegisterPlayFabUserResult result) => Debug.Log("로그인 성공!");
    // private void OnRegisterFailure(PlayFabError error) => Debug.Log("로그인 실패");
    public string customId = "";
    public string nickName = "";

    public Text nickNameText;

    public void GetDisplayName()
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
            {
                PlayFabId = customId,
                ProfileConstraints = new PlayerProfileViewConstraints()
                {
                    ShowDisplayName = true
                }
            },
            (result) =>
            {
                nickName = result.PlayerProfile.DisplayName;
                nickNameText.text = "닉네임: "+nickName;
            },
            DisplayPlayfabError);
    }
    

    
    //서버시간 가져오는코드
    private void DisplayPlayfabError(PlayFabError error) => Debug.LogError("error : " + error.GenerateErrorReport());
    
        public System.DateTime GetServerTime()
        {
            System.DateTime _time = System.DateTime.Now;
    
            PlayFabClientAPI.GetTime(new GetTimeRequest(),
                result =>
                {
                    _time = result.Time;
                }, DisplayPlayfabError);
            return _time;
        }
        //순위
        public void SetStat()
        {
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                    { new StatisticUpdate { StatisticName = "HighScore", Value = playerData.score } }
            };
            PlayFabClientAPI.UpdatePlayerStatistics(request,(result)=>print("값 저장됨"),OnError);
        }

        public void GetStat()
        {
            PlayFabClientAPI.GetPlayerStatistics(
                new GetPlayerStatisticsRequest(),
                (result) =>
                {

                    playerData.score = result.Statistics[0].Value;
                },
                OnError);
            playerData.TextUpdate();
        }

        public void GetReaderboard()
        {
            var request = new GetLeaderboardRequest
            {
                StartPosition = 0, StatisticName = "HighScore", MaxResultsCount = 20,
                ProfileConstraints = new PlayerProfileViewConstraints() { ShowLocations = true, ShowDisplayName = true }
            };
            PlayFabClientAPI.GetLeaderboard(request, (result) =>
            {
                for (int i = 0; i < result.Leaderboard.Count; i++)
                {
                    var curBoard = result.Leaderboard[i];
                    LogText.text += (i+1)+ "/" + curBoard.Profile.Locations[0].CountryCode + "/" + curBoard.DisplayName + "/" +
                                    curBoard.StatValue + "" +
                                    "\n";
                }
            },(error) => print("리더보드 불러오기 실패"));
        }
        //Currency
        public void GetVirtualCurrencies()
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess,OnError);
        }

        void OnGetUserInventorySuccess(GetUserInventoryResult result)
        {
            int coins = result.VirtualCurrency["CN"];
            coinsValueText.text = coins.ToString();
            playerData.money = coins;

        }
        

        public void GrantVirtualCurrency()
        {
            var request = new AddUserVirtualCurrencyRequest
            {
                VirtualCurrency = "CN",
                Amount = playerData.lv
            };
            PlayFabClientAPI.AddUserVirtualCurrency(request, OnGrantVirtualCurrencySuccess,OnError);
        }

        void OnGrantVirtualCurrencySuccess(ModifyUserVirtualCurrencyResult result)
        {
            Debug.Log("Currnecy granted");
        }
        
        //Player data
        public void GetAppearance()
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(),OnDataRecieved,OnError);
            
           
        }

        void OnDataRecieved(GetUserDataResult result)
        {
            Debug.Log("Recieved user data!");
            if (result.Data != null && result.Data.ContainsKey("Lv")
                && result.Data.ContainsKey("Exp"))
            {
                 playerData.SetApperance( 
                     result.Data["Lv"].Value, result.Data["Exp"].Value );
                 playerData.TextUpdate();
            }
            else
            {
                Debug.Log("Player data not compelete");
            }

        }

        public void SaveAppearance()
        {
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    { "Lv", playerData.lv.ToString() },
                    { "Exp", playerData.exp.ToString() },
                }
            };
            PlayFabClientAPI.UpdateUserData(request,OnDataSend,OnError);
            SetStat();
            LogText.text = "";
            GetReaderboard();
        }

        void OnDataSend(UpdateUserDataResult result)
        {
            Debug.Log("Successful user data send!");
        }

        void OnError(PlayFabError error)
        {
            Debug.Log("Error: "+error.ErrorMessage);
        }
       
        
    
}
