using System.Collections;
using UnityEngine.SceneManagement; 
using UnityEngine;
using UnityEngine.UI;

public class StartupController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject loginButton;
    [SerializeField] private GameObject registerButton;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject registerPanel;
    [SerializeField] private GameObject logoutButton;
    [SerializeField] private GameObject logoutPanel;
    [SerializeField] private GameObject forgotEmailPanel;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject textEULA;
    [SerializeField] private GameObject notLoggedInBanner;
    [SerializeField] private GameObject leafParticles;
    [SerializeField] private PlayFabLoginController pflc;
    [SerializeField] private GameObject notification;
#pragma warning restore 0649
    private bool loginSuccess;
    private bool returnRequest;
    private string userEmail;
    private string userPassword;
    private string userPasswordConf;
    private string userName;

    // TODO: Put up a loading wheel animation once they clicked a button. We don't want multiple calls.
    // TODO: Create a GetStats() function whenever logged in and populate the stats to GGM.

    private void Start()
    {
        StartCoroutine(LoginHandler());
    }
    
    public void SetUserEmail(string emailIn)
    {
        userEmail = emailIn;
    }
    public void SetUserPassword(string passwordIn)
    {
        userPassword = passwordIn;
    }
    public void SetUserPasswordConfirm(string passwordIn)
    {
        userPasswordConf = passwordIn;
    }
    public void SetUserName(string userNameIn)
    {
        userName = userNameIn;
    }

    #region Play
    public void OnPlay()
    {
        // TODO: Make a loading screen w/ tips, change to that
        SceneManager.LoadScene("OverWorld");
    }
    #endregion Play


    #region Login
    public void OnLogin()
    {
        loginPanel.SetActive(true);
        loginButton.SetActive(false);
        registerButton.SetActive(false);
    }
    public void OnLoginSubmit()
    {
        bool isValid = false;
        isValid = CheckLoginFields();
        if (isValid)
        {
            StartCoroutine(LoginSubmit());
        }
    }
    public void OnLoginExit()
    {
        loginPanel.SetActive(false);
        loginButton.SetActive(true);
        registerButton.SetActive(true);
    }
    private IEnumerator LoginSubmit()
    {
        pflc.SetUserEmail(userEmail);
        pflc.SetUserPassword(userPassword);
        pflc.SetUserName(userName);
        pflc.OnClickLogin();
        yield return StartCoroutine(WaitingForPlayfab(8));
        if (loginSuccess)
        {
            loginPanel.SetActive(false);

            logoutButton.SetActive(true);
            notLoggedInBanner.SetActive(false);
            leafParticles.SetActive(true);
        }
    }
    private IEnumerator LoginHandler()
    {
        pflc.CheckLogin();
        yield return StartCoroutine(WaitingForPlayfab(9));
        if (loginSuccess)
        {
            // Set up Loading Screen & Tips
            // Animate
            logoutButton.SetActive(true);
        }
        else
        {
            // TODO: Log in locally
            loginButton.SetActive(true);
            registerButton.SetActive(true);
            notLoggedInBanner.SetActive(true);
            leafParticles.SetActive(false);
        }
        playButton.SetActive(true);
        textEULA.SetActive(true);
    }
    #endregion Login


    #region Register
    public void OnRegister()
    {
        registerPanel.SetActive(true);
        loginButton.SetActive(false);
        registerButton.SetActive(false);
    }
    public void OnRegisterSubmit()
    {
        bool isValid = false;
        isValid = CheckLoginFields();
        if (isValid)
        {
            isValid = CheckRegisterFields();
            if (isValid)
            {
                StartCoroutine(RegisterSubmit());
            }
        }
    }
    public void OnRegisterExit()
    {
        registerPanel.SetActive(false);
        loginButton.SetActive(true);
        registerButton.SetActive(true);
    }
    private IEnumerator RegisterSubmit()
    {
        // Checking to see if the username is taken;
        pflc.SetUserName(userName);
        pflc.UsernameTaken();
        yield return StartCoroutine(WaitingForPlayfab(6));
        // TODO: make another test and write notification accordingly if it takes too long.
        if (!loginSuccess)
        {
            yield break;
        }
        // After registerring, we log them in
        pflc.SetUserEmail(userEmail);
        pflc.SetUserPassword(userPassword);
        pflc.OnClickRegister();
        yield return StartCoroutine(WaitingForPlayfab(6));
        if (loginSuccess)
        {
            registerPanel.SetActive(false);
            StartCoroutine(LoginSubmit());
        }
    }
    #endregion Register


    #region Logout
    public void LogoutButton()
    {
        logoutPanel.SetActive(true);
    }

    public void LogoutConfirm(bool confirm)
    {
        if (confirm)
        {
            pflc.OnClickLogout();
            loginButton.SetActive(true);
            registerButton.SetActive(true);
            logoutPanel.SetActive(false);
            logoutButton.SetActive(false);
            notLoggedInBanner.SetActive(true);
            leafParticles.SetActive(false);
        }
        else
        {
            logoutPanel.SetActive(false);
        }
    }
    #endregion Logout


    #region ForgotEmail
    public void OnForgotEmail()
    {
        loginPanel.SetActive(false);
        forgotEmailPanel.SetActive(true);
    }

    public void OnForgotEmailSubmit()
    {
        if (string.IsNullOrEmpty(userEmail))
        {
            notification.GetComponent<Notification>()
                .UseNotification("Enter a valid email address", 1.2f);
            return;
        }
        StartCoroutine(LoginSubmit());
    }
    public void OnForgotExit()
    {
        loginPanel.SetActive(true);
        forgotEmailPanel.SetActive(false);
    }
    private IEnumerator EmailSubmit()
    {
        pflc.OnClickRecoverAccount(userEmail);
        yield return StartCoroutine(WaitingForPlayfab(6));
        if (loginSuccess) 
        {
            forgotEmailPanel.SetActive(false);
        }
    }
    #endregion ForgotEmail


    #region InputChecks
    private bool CheckLoginFields()
    {
        if (string.IsNullOrEmpty(userName))
        {
            notification.GetComponent<Notification>()
                .UseNotification("Invalid Username", 1.2f);
            return false;
        }
        if (userName.Length > 20 || userName.Length < 3)
        {
            notification.GetComponent<Notification>()
                .UseNotification("Usernames must be between 3 and 20 characters in length", 1.2f);
            return false;
        }
        else if (string.IsNullOrEmpty(userPassword))
        {
            notification.GetComponent<Notification>()
                .UseNotification("Invalid Password", 1.2f);
            return false;
        }
        else if (userPassword.Length < 8)
        {
            notification.GetComponent<Notification>()
                .UseNotification("Password must be at least 8 characters", 1.2f);
            return false;
        }
        else if (string.IsNullOrEmpty(userEmail))
        {
            notification.GetComponent<Notification>()
                .UseNotification("Invalid Email", 1.2f);
            return false;
        }
        else
        {
            return true;
        }
    }
    private bool CheckRegisterFields()
    {
        if (string.IsNullOrEmpty(userPasswordConf))
        {
            notification.GetComponent<Notification>()
                .UseNotification("Passwords do not match", 1.2f);
            return false;
        }
        if (userPassword != userPasswordConf)
        {
            notification.GetComponent<Notification>()
                .UseNotification("Passwords do not match", 1.2f);
            return false;
        }
        else
        {
        return true;
        }
    }
    #endregion InputChecks


    private IEnumerator WaitingForPlayfab(float time)
    {
        returnRequest = false;
        loginSuccess = false;
        float secondCounter = 0.0f;
        while (!returnRequest)
        {
            returnRequest = pflc.GetRequestReturned();
            yield return new WaitForSeconds(0.1f);
            secondCounter += 0.5f;
            if (secondCounter > 6.0f)
            {
                break;
            }
        }
        loginSuccess = pflc.GetLoginSuccess();
    }
}
