﻿using Scripts.Services;
using Scripts.Services.AuthenticatorApi;
using UnityEngine;

namespace Scripts.UI.Authenticator
{
    [RequireComponent(typeof(IOnLoginFinishedListener))]
    [RequireComponent(typeof(IOnRegistrationFinishedListener))]
    public class AuthenticatorInteractor : MonoBehaviour
    {
        private IAuthenticatorApi authenticatorApi;
        private IOnLoginFinishedListener onLoginFinishedListener;
        private IOnRegistrationFinishedListener onRegistrationFinishedListener;

        private void Awake()
        {
            authenticatorApi =
                ApiProvider.ProvideAuthenticatorApi();
            onLoginFinishedListener =
                GetComponent<IOnLoginFinishedListener>();
            onRegistrationFinishedListener =
                GetComponent<IOnRegistrationFinishedListener>();
        }

        public void Login(UIAuthenticationDetails uiAuthenticationDetails)
        {
            var email = uiAuthenticationDetails.Email;
            var password = uiAuthenticationDetails.Password;

            authenticatorApi?.Authenticate(email, password);

            // TODO: onException: (e) => onLoginFinishedListener.OnLoginFailed());
            // TODO: Handle other statuses
            onLoginFinishedListener.OnLoginSucceed();
        }

        public void Register(UIRegistrationDetails uiRegistrationDetails)
        {
            var email = uiRegistrationDetails.Email;
            var password = uiRegistrationDetails.Password;
            var firstName = uiRegistrationDetails.FirstName;
            var lastName = uiRegistrationDetails.LastName;

            authenticatorApi?.Register(email, password, firstName, lastName);

            // TODO: onException: (e) => onRegistrationFinishedListener.OnRegistrationFailed()
            // TODO: Handle other statuses
            onRegistrationFinishedListener.OnRegistrationSucceed();
        }
    }
}