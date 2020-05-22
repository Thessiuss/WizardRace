using UnityEngine;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.DataModels;
using PlayFab.ProfilesModels;

public class PlayFabLoginController : MonoBehaviour
{
    // TODO: Check if result.newlyCreated for the user and set all stats to 0.
    public static PlayFabLoginController pflc;

#pragma warning disable 0649
    [SerializeField] private GameObject notification;
#pragma warning restore 0649

    private string userEmail;
    private string userPassword;
    private string username;
    private string userID;
    private bool requestReturned;
    private bool loginSuccess;

    #region AccessorVariables
    public void SetUserEmail(string emailIn)
    {
        userEmail = emailIn;
    }
    public void SetUserPassword(string passwordIn)
    {
        userPassword = passwordIn;
    }
    public void SetUserName(string usernameIn)
    {
        username = usernameIn;
    }
    public bool GetRequestReturned()
    {
        return requestReturned;
    }
    public bool GetLoginSuccess()
    {
        return loginSuccess;
    }
    #endregion AccessorVariables

    #region Login/Logout
    public void CheckLogin()
    {
        //Note: Setting title Id here can be skipped if you have set the value in Editor Extensions already.
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "85D11";
        }
        //PlayerPrefs.DeleteAll();
        if (PlayerPrefs.HasKey("EMAIL"))
        {
            userEmail = PlayerPrefs.GetString("EMAIL");
            userPassword = PlayerPrefs.GetString("PASSWORD");
            var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
            // Used to tell when a return request has arrived.
            requestReturned = false;
            loginSuccess = false;
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, ErrorHandler);
        }
    }

    public void LocalLogin()
    {
    #if UNITY_ANDROID
            var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDevice = ReturnMobileID(), CreateAccount = true };
            requestReturned = false;
            loginSuccess = false;
            PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginMobileSuccess, ErrorHandler);
    #endif
    #if UNITY_IOS
            var requestIOS = new LoginWithIOSDeviceIDRequest { DeviceId = ReturnMobileID(), CreateAccount = true };
            requestReturned = false;
            loginSuccess = false;
            PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, OnLoginMobileSuccess, ErrorHandler);
    #endif
    #if UNITY_EDITOR_WIN
            var requestEditor = new LoginWithCustomIDRequest { CustomId = "TestPlayFab", CreateAccount = true };
            requestReturned = false;
            loginSuccess = false;
            PlayFabClientAPI.LoginWithCustomID(requestEditor, OnLoginSuccess, ErrorHandler);
    #endif
    #if UNITY_STANDALONE
            var requestStandalone = new LoginWithCustomIDRequest { CustomId = "TestPlayFab", CreateAccount = true };
            requestReturned = false;
            loginSuccess = false;
            PlayFabClientAPI.LoginWithCustomID(requestStandalone, OnLoginSuccess, ErrorHandler);
    #endif
    }
    //First test to see if they have login. If not, then OnLoginFailure to register.
    public void TestUsername(string usernameIn)
    {
        var request = new GetAccountInfoRequest { Username = usernameIn };
    }

    public void OnClickLogin()
    {
        var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
        requestReturned = false;
        loginSuccess = false;
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, ErrorHandler);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        userID = result.PlayFabId;
        loginSuccess = true;
        notification.SetActive(true);
        notification.GetComponent<Notification>().UseNotification("Successfully logged in!", 1.2f);
        requestReturned = true;
    }

    public void OnClickLogout()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        PlayerPrefs.DeleteAll();
        notification.SetActive(true);
        notification.GetComponent<Notification>().UseNotification("Abracadabra!\nYou have logged out!", 1.2f);
    }
    #endregion Login/Logout

    #region Register
    public void OnClickRegister()
    {
        var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userPassword, Username = username };
        loginSuccess = false;
        requestReturned = false;
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, ErrorHandler);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        notification.SetActive(true);
        notification.GetComponent<Notification>().UseNotification("Successfully registered!", 1.2f);
        // TODO: Call a login now and make sure they are logged in.
        // TODO: Show them Username & Email used.
        // R&D: See if registering automatically logs them in.
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        loginSuccess = true;
        requestReturned = true;
    }

    public void OnClickAddLogin()
    {
        var addLoginRequest = new AddUsernamePasswordRequest { Email = userEmail, Password = userPassword, Username = username };
        requestReturned = false;
        loginSuccess = false;
        PlayFabClientAPI.AddUsernamePassword(addLoginRequest, OnAddLoginSuccess, ErrorHandler);
    }

    private void OnAddLoginSuccess(AddUsernamePasswordResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        loginSuccess = true;
        requestReturned = true;
    }

    public void UsernameTaken()
    {
        var requestStandalone = new LoginWithCustomIDRequest { CustomId = "TestPlayFabWR2019", CreateAccount = true };
        requestReturned = false;
        loginSuccess = false;
        PlayFabClientAPI.LoginWithCustomID(requestStandalone, CIDRequest, CIDError);
        
    }
    private void CIDRequest(LoginResult result)
    {
        var accountRequest = new GetAccountInfoRequest { Username = username };
        PlayFabClientAPI.GetAccountInfo(accountRequest, AccountTaken, AccountAvailable);
    }
    private void CIDError(PlayFabError error)
    {
        loginSuccess = false;
        requestReturned = true;
    }
    private void AccountTaken(GetAccountInfoResult result)
    {
        notification.SetActive(true);
        notification.GetComponent<Notification>().UseNotification(
                    username + " is taken.\n Please choose another.", 4.0f);
        loginSuccess = false;
        requestReturned = true;
    }
    private void AccountAvailable(PlayFabError error)
    {
        
        loginSuccess = true;
        requestReturned = true;
    }
    #endregion Register

    #region Mobile
    private void OnLoginMobileSuccess(LoginResult result)
    {
        notification.SetActive(true);
        notification.GetComponent<Notification>().UseNotification("Successfully logged in!", 1.2f);
        userID = result.PlayFabId;
        loginSuccess = true;
        requestReturned = true;
    }

    public static string ReturnMobileID()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        return deviceID;
    }
    #endregion Mobile

    #region EmailRecovery
    public void OnClickRecoverAccount(string emailIn)
    {
        var request = new SendAccountRecoveryEmailRequest { Email = emailIn, TitleId = "85D11" };
        requestReturned = false;
        loginSuccess = false;
        PlayFabClientAPI.SendAccountRecoveryEmail(request, RecoverdEmailSuccess, ErrorHandler);
    }
    private void RecoverdEmailSuccess(SendAccountRecoveryEmailResult result)
    {
        notification.SetActive(true);
        notification.GetComponent<Notification>().UseNotification(
            "Check your email!\nallow a few minutes for arrival", 2.0f);
        loginSuccess = true;
        requestReturned = true;
    }
    #endregion EmailRecovery

    private void ErrorHandler(PlayFabError error)
    {
        notification.SetActive(true);
        switch (error.Error)
        {
            case PlayFabErrorCode.UnableToConnectToDatabase:
                notification.GetComponent<Notification>().UseNotification(
                    "Connection error (your end)\nWe tested both ends", 3.0f);
                break;
            case PlayFabErrorCode.DownstreamServiceUnavailable:
                notification.GetComponent<Notification>().UseNotification(
                    "Connection error (our end)\nDatabase is down (needs a hug)", 3.0f);
                break;
            case PlayFabErrorCode.AccountNotFound:
                notification.GetComponent<Notification>().UseNotification(
                    "Account doesn't exist.\nDouble check spelling\n" + userEmail + "\nOr register an account!", 4.0f);
                break;
            case PlayFabErrorCode.InvalidParams:
                notification.GetComponent<Notification>().UseNotification(
                    "Double check your inputs./n We checked, and something's wrong.", 3.0f);
                break;
            case PlayFabErrorCode.InvalidEmailOrPassword:
                notification.GetComponent<Notification>().UseNotification(
                    "Invalid email or password.\n" + userEmail, 3.0f);
                break;
            case PlayFabErrorCode.AccountBanned:
                notification.GetComponent<Notification>().UseNotification(
                    "Your account is banned.\nYou did something against our EULA. " +
                    "Contact us at SCOJAY.L2P@gmail.com", 3.0f);
                break;
            case PlayFabErrorCode.AccountDeleted:
                notification.GetComponent<Notification>().UseNotification(
                   "Your account has been deleted. Contact us at SCOJAY.L2P@gmail.com", 4.0f);
                break;
            case PlayFabErrorCode.NotAuthenticated:
                notification.GetComponent<Notification>().UseNotification(
                   "You will need to login.", 4.0f);
                // Player needs to login in order to have it work.
                break;
            case PlayFabErrorCode.NotAuthorized:
                notification.GetComponent<Notification>().UseNotification(
                   "Something went wrong.\nDouble check your credentials.\n" + userEmail, 4.0f);
                // Bad inputs related to logging in
                break;
            case PlayFabErrorCode.ProfileDoesNotExist:
                notification.GetComponent<Notification>().UseNotification(
                   "Profile doesn't exist.\nDouble check your credentials.\n" + userEmail, 4.0f);
                // Likely a typo, or bad input somewhere.
                break;
            default:
                notification.GetComponent<Notification>().UseNotification(
                   "Something went wrong. Email us at SCOJAY.L2P@gmail.com", 4.0f);
                break;
        }
        loginSuccess = false;
        requestReturned = true;
    } 
}