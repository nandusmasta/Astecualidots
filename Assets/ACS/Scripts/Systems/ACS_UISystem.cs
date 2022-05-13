/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

using Unity.Jobs;
using Unity.Entities;
using Assets.ACS.Scripts.DataComponents;
using Assets.ACS.Scripts.Utils;
using Assets.ACS.Scripts.Behaviours;

namespace Assets.ACS.Scripts.Systems
{

    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial class ACS_UISystem : SystemBase
    {

        private Entity shipEntity;
        private Entity gameEntity;
        private ACS_ShipData shipData;

        protected override void OnCreate()
        {
            base.OnCreate();
        }

        protected override void OnUpdate()
        {
            if (!ACS_Globals.HasGameStarted) return;

            if (shipEntity == Entity.Null)
            {
                shipEntity = GetSingletonEntity<ACS_ShipData>();
                return;
            }

            if (gameEntity == Entity.Null)
            {
                gameEntity = GetSingletonEntity<ACS_GameData>();
                return;
            }

            Entities.ForEach((ACS_UIData uiData) =>
            {
                if (!EntityManager.Exists(shipEntity))
                {
                    uiData.Health.text = 0.ToString();
                    ACS_UIManager.Instance.ShowGameEndScreen(uiData.Score.text);
                    return;
                }
                ACS_ShipData shipData = GetComponent<ACS_ShipData>(shipEntity);
                uiData.Score.text = shipData.Score.ToString();
                uiData.Health.text = shipData.Health.ToString();
            }).WithoutBurst().Run();
        }
    }
}