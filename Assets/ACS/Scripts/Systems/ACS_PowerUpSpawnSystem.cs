/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

using Unity.Entities;
using Unity.Mathematics;
using Assets.ACS.Scripts.Utils;
using static Assets.ACS.Scripts.DataComponents.ACS_PowerUpData;
using System;
using Assets.ACS.Scripts.DataComponents;
using Unity.Collections;
using Unity.Transforms;
using Unity.Physics;
using UnityEngine;

namespace Assets.ACS.Scripts.Systems
{
    public partial class ACS_PowerUpSpawnSystem : SystemBase
    {

        private int2 MinimumSecondsBetweenPowerups = new int2 { x = 5, y = 10 };
        private Unity.Mathematics.Random random;
        private PowerUpType nextPowerUpType = PowerUpType.RepairKit;
        private float timeToNextPowerUp;
        private float timeSinceLastPowerUp;
        private Entity gameDataEntity;
        private ACS_GameData gameData;

        protected override void OnCreate()
        {
            random = new Unity.Mathematics.Random(56);
            updateNextPowerUpType();
        }

        protected override void OnUpdate()
        {
            if (!ACS_Globals.HasGameStarted) return;

            if (gameDataEntity == Entity.Null)
            {
                EntityQuery gameDataQuery = GetEntityQuery(typeof(ACS_GameData));
                gameDataEntity = gameDataQuery.ToEntityArray(Allocator.Temp)[0];
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

        private void updateNextPowerUpType()
        {
            Array powerUpTypes = typeof(PowerUpType).GetEnumValues();
            int randomInt = random.NextInt(0, powerUpTypes.Length);
            nextPowerUpType = (PowerUpType)powerUpTypes.GetValue(randomInt);
            timeSinceLastPowerUp = 0f;
            timeToNextPowerUp = random.NextInt(MinimumSecondsBetweenPowerups.x, MinimumSecondsBetweenPowerups.y);
        }

        private void spawnNextPowerUp()
        {
            Entity newPowerUp = Entity.Null;
            switch (nextPowerUpType)
            {
                case PowerUpType.Invulnerability: newPowerUp = EntityManager.Instantiate(gameData.InvulnerabilityPowerUp); break;
                case PowerUpType.RepairKit: newPowerUp = EntityManager.Instantiate(gameData.RepairKitPowerUp); break;
                case PowerUpType.MegaBomb: newPowerUp = EntityManager.Instantiate(gameData.MegaBombPowerUp); break;
            }

            float2 verticalEdges = ACS_GameManager.Instance.VerticalEdges;
            float2 horizontalEdges = ACS_GameManager.Instance.HorizontalEdges;

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

            Rotation newRotation;
            newRotation.Value = Quaternion.AngleAxis(90f, Vector3.right);
            EntityManager.SetComponentData(newPowerUp, newRotation);

            UnityEngine.Debug.Log($"Spawning the {nextPowerUpType.ToString()} power up, after waiting {timeToNextPowerUp} seconds");
        }

    }
}
