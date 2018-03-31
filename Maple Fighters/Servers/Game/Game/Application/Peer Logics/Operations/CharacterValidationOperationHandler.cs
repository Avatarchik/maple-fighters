﻿using System;
using System.Threading.Tasks;
using Character.Server.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class CharacterValidationOperationHandler : IAsyncOperationRequestHandler<ValidateCharacterRequestParameters, ValidateCharacterResponseParameters>
    {
        private readonly int userId;
        private readonly Action<CharacterParameters?> onCharacterSelected;
        private readonly ICharacterServiceAPI characterServiceAPI;

        public CharacterValidationOperationHandler(int userId, Action<CharacterParameters?> onCharacterSelected)
        {
            this.userId = userId;
            this.onCharacterSelected = onCharacterSelected;

            characterServiceAPI = Server.Components.GetComponent<ICharacterServiceAPI>().AssertNotNull();
        }

        public Task<ValidateCharacterResponseParameters?> Handle(IYield yield, MessageData<ValidateCharacterRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var parameters = new GetCharacterRequestParametersEx(userId, messageData.Parameters.CharacterIndex);
            return GetCharacter(yield, parameters);
        }

        private async Task<ValidateCharacterResponseParameters?> GetCharacter(IYield yield, GetCharacterRequestParametersEx parameters)
        {
            var responseParameters = await characterServiceAPI.GetCharacter(yield, parameters);
            onCharacterSelected?.Invoke(responseParameters.Character);

            var status = responseParameters.Character.HasCharacter ? CharacterValidationStatus.Ok : CharacterValidationStatus.Wrong;
            return new ValidateCharacterResponseParameters(status);
        }
    }
}