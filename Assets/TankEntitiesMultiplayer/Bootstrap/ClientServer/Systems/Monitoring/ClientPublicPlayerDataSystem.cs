using CommunityToolkit.Mvvm.Messaging;
using MVVMToolkit.Messaging;
using TankEntitiesMultiplayer.Data;
using Unity.Collections;
using Unity.Entities;

namespace TankEntitiesMultiplayer.Bootstrap
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class ClientPublicPlayerDataSystem : SystemBase
    {
        private EntityQuery _playerDataQuery;

        protected override void OnCreate()
        {
            _playerDataQuery = SystemAPI.QueryBuilder().WithAll<PublicPlayerData>().Build();
            _playerDataQuery.AddChangedVersionFilter(ComponentType.ReadOnly<PublicPlayerData>());
            RequireForUpdate(_playerDataQuery);
        }

        protected override void OnUpdate()
        {
            // No IgnoreFilter because of changed version filter
            if (!_playerDataQuery.IsEmpty)
            {
                var playerData = SystemAPI.GetSingletonBuffer<PublicPlayerData>(true).AsNativeArray().AsReadOnly();
                Game.Messenger.Send(PublicPlayerDataMessage.Message(playerData));
            }
        }
    }

    public class PublicPlayerDataMessage :
        ValueMessage<PublicPlayerDataMessage, NativeArray<PublicPlayerData>.ReadOnly> { }
}