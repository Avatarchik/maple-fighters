﻿using CharacterService.Application.Components.Interfaces;
using CommonTools.Log;
using ComponentModel.Common;
using Database.Common.Components.Interfaces;
using Database.Common.TablesDefinition;
using Game.Common;
using ServiceStack.OrmLite;

namespace CharacterService.Application.Components
{
    internal class DatabaseCharacterCreator : Component, IDatabaseCharacterCreator
    {
        private IDatabaseConnectionProvider databaseConnectionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            databaseConnectionProvider = Components.GetComponent<IDatabaseConnectionProvider>().AssertNotNull();
        }

        public void Create(int userId, string name, CharacterClasses characterClass, CharacterIndex characterIndex)
        {
            using (var db = databaseConnectionProvider.GetDbConnection())
            {
                var user = new CharactersTableDefinition
                {
                    UserId = userId,
                    Name = name,
                    CharacterType = characterClass.ToString(),
                    CharacterIndex = (byte)characterIndex
                };
                db.Insert(user);
            }
        }
    }
}