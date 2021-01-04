using ScriptableObjects.Configurations;
using Scripts.Services.GameApi;

namespace Scripts.Services
{
    public static class ApiProvider
    {
        #region GameApi
        public static IGameApi ProvideGameApi()
        {
            if (gameApi == null)
            {
                var networkConfiguration = NetworkConfiguration.GetInstance();
                if (networkConfiguration.IsProduction())
                {
                    gameApi = WebSocketGameApi.GetInstance();
                }
                else
                {
                    // TODO: Set dummy game api
                }
            }

            return gameApi;
        }

        private static IGameApi gameApi;
        #endregion
    }
}