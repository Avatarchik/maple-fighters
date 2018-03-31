﻿using Character.Server.Common;
using CharacterService.Application.Components;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Common;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;

namespace CharacterService.Application.PeerLogics.Operations
{
    internal class RemoveCharacterOperationHandler : IOperationRequestHandler<RemoveCharacterRequestParametersEx, RemoveCharacterResponseParameters>
    {
        private readonly IDatabaseCharacterRemover databaseCharacterRemover;
        private readonly IDatabaseCharacterExistence databaseCharacterExistence;

        public RemoveCharacterOperationHandler()
        {
            databaseCharacterRemover = Server.Components.GetComponent<IDatabaseCharacterRemover>().AssertNotNull();
            databaseCharacterExistence = Server.Components.GetComponent<IDatabaseCharacterExistence>().AssertNotNull();
        }

        public RemoveCharacterResponseParameters? Handle(MessageData<RemoveCharacterRequestParametersEx> messageData, ref MessageSendOptions sendOptions)
        {
            var userId = messageData.Parameters.UserId;
            var characterIndex = messageData.Parameters.CharacterIndex;
            databaseCharacterRemover.Remove(userId, characterIndex);

            var removed = !databaseCharacterExistence.Exists(userId, (CharacterIndex)characterIndex);
            return new RemoveCharacterResponseParameters(removed ? RemoveCharacterStatus.Succeed : RemoveCharacterStatus.Failed);
        }
    }
}