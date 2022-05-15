/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.Systems
{
    using Assets.ACS.Scripts.DataComponents;
    using Assets.ACS.Scripts.Utils;
    using System;
    using Unity.Collections;
    using Unity.Entities;
    using Unity.Mathematics;
    using Unity.Physics;
    using Unity.Transforms;
    using UnityEngine;
    using static Assets.ACS.Scripts.DataComponents.ACS_PowerUpData;

    public partial class ACS_PowerUpSpawnSystem : SystemBase
    {
        #region Fields

        private ACS_GameData gameData;

        private Entity gameDataEntity;

        private PowerUpType nextPowerUpType = PowerUpType.RepairKit;

        private Unity.Mathematics.Random random;

        private float timeSinceLastPowerUp;

        private float timeToNextPowerUp;

        #endregion

        #region Methods

        protected override void OnCreate()
        {
            random = new Unity.Mathematics.Random(56);
            updateNextPowerUpType();
        }

        protected override void OnUpdate()
        {
            if (!ACS_Globals.IsPlayerPlaying) return;

            if (gameDataEntity == Entity.Null)
            {
                EntityQuery gameDataQuery = GetEntityQuery(typeof(ACS_GameData));
                NativeArray<Entity> entityArray = gameDataQuery.ToEntityArray(Allocator.Temp);
                if (entityArray.Length > 0)
                    gameDataEntity = entityArray[0];
                if (gameDataEntity != Entity.Null)
                    gameData = EntityManager.GetComponentData<ACS_GameData>(gameDataEntity);
            }

            if (timeSinceLastPowerUp >= timeToNextPowerUp)
            {
                spawnNextPowerUp();
                updateNextPowerUpType();
            }

            timeSinceLastPowerUp += Time.DeltaTime;
        }

        private void spawnNextPowerUp()
        {
            Entity newPowerUp = Entity.Null;
            switch (nextPowerUpType)
            {
                case PowerUpType.Invulnerability: newPowerUp = EntityManager.Instantiate(gameData.InvulnerabilityPowerUp); break;
                case PowerUpType.RepairKit: newPowerUp = EntityManager.Instantiate(gameData.RepairKitPowerUp); break;
                case PowerUpType.MegaBomb: newPowerUp = EntityManager.Instantiate(gameData.MegaBombPowerUp); break;
                case PowerUpType.MultiShoot: newPowerUp = EntityManager.Instantiate(gameData.TripleShootPowerUp); break;
            }

            float2 verticalEdges = ACS_Globals.VerticalEdges;
            float2 horizontalEdges = ACS_Globals.HorizontalEdges;

            // Set position
            float newPowerUpPositionX = random.NextFloat(0f, horizontalEdges.x);
            float3 newPowerUpPosition = new float3(newPowerUpPositionX, 0f, verticalEdges.y);
            Translation newPowerUpTranslation = new Translation { Value = newPowerUpPosition };
            EntityManager.SetComponentData(newPowerUp, newPowerUpTranslation);

            // Set velocity
            float randomLinearInitialVelocity = random.NextFloat(10f, 15f);
            float3 randomXZDirection = new float3(ACS_Utils.GetRandomFloat(-5f, 5f), 0f, random.NextFloat(-5f, 5f));
            PhysicsVelocity physicsVelocity = new PhysicsVelocity();
            physicsVelocity.Linear = randomXZDirection * randomLinearInitialVelocity;
            EntityManager.SetComponentData(newPowerUp, physicsVelocity);

            // Set rotation
            Rotation newRotation = new Rotation { Value = Quaternion.AngleAxis(90f, Vector3.right) };
            EntityManager.SetComponentData(newPowerUp, newRotation);

            //UnityEngine.Debug.Log($"Spawning the {nextPowerUpType.ToString()} power up, after waiting {timeToNextPowerUp} seconds");
        }

        private void updateNextPowerUpType()
        {
            Array powerUpTypes = typeof(PowerUpType).GetEnumValues();
            int randomInt = random.NextInt(0, powerUpTypes.Length);
            nextPowerUpType = (PowerUpType)powerUpTypes.GetValue(randomInt);
            timeSinceLastPowerUp = 0f;
            timeToNextPowerUp = random.NextInt(ACS_Globals.MinimumSecondsBetweenPowerups.x, ACS_Globals.MinimumSecondsBetweenPowerups.y);
        }

        #endregion
    }
}
