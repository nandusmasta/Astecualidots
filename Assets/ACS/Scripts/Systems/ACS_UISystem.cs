/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.Systems
{
    using Assets.ACS.Scripts.Behaviours;
    using Assets.ACS.Scripts.DataComponents;
    using Assets.ACS.Scripts.Utils;
    using Unity.Entities;
    using Unity.Jobs;

    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial class ACS_UISystem : SystemBase
    {
        #region Fields

        private Entity gameEntity;

        private Entity shipEntity;

        #endregion

        #region Methods

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

        #endregion
    }
}
