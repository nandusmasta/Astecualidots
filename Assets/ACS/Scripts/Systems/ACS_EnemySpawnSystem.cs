/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.Systems
{
    using Assets.ACS.Scripts.DataComponents;
    using Assets.ACS.Scripts.Utils;
    using Unity.Collections;
    using Unity.Entities;
    using Unity.Mathematics;
    using Unity.Physics;
    using Unity.Transforms;
    using UnityEngine;

    public partial class ACS_EnemySpawnSystem : SystemBase
    {
        #region Fields

        private ACS_GameData gameData;

        private Entity gameDataEntity;

        private Unity.Mathematics.Random random;

        private float timeSinceLastEnemy;

        private float timeToNextEnemy;

        #endregion

        #region Methods

        protected override void OnCreate()
        {
            random = new Unity.Mathematics.Random(56);
            prepareForNextEnemy();
        }

        protected override void OnUpdate()
        {
            if (!ACS_Globals.HasGameStarted) return;

            if (gameDataEntity == Entity.Null)
            {
                EntityQuery gameDataQuery = GetEntityQuery(typeof(ACS_GameData));
                NativeArray<Entity> entityArray = gameDataQuery.ToEntityArray(Allocator.Temp);
                if (entityArray.Length > 0)
                    gameDataEntity = entityArray[0];
                if (gameDataEntity != Entity.Null)
                    gameData = EntityManager.GetComponentData<ACS_GameData>(gameDataEntity);
            }

            if (timeSinceLastEnemy >= timeToNextEnemy)
            {
                spawnRandomEnemy();
                prepareForNextEnemy();
            }

            timeSinceLastEnemy += Time.DeltaTime;
        }

        private void prepareForNextEnemy()
        {
            timeSinceLastEnemy = 0f;
            timeToNextEnemy = random.NextInt(ACS_Globals.MinimumSecondsBetweenEnemies.x, ACS_Globals.MinimumSecondsBetweenEnemies.y);
        }

        private void spawnRandomEnemy()
        {
            int randomEnemyIndex = random.NextInt(1, 4);
            Entity newEnemy = Entity.Null;
            switch (randomEnemyIndex)
            {
                case 1: newEnemy = EntityManager.Instantiate(gameData.Enemy1); break;
                case 2: newEnemy = EntityManager.Instantiate(gameData.Enemy2); break;
                case 3: newEnemy = EntityManager.Instantiate(gameData.Enemy3); break;
            }

            float2 verticalEdges = ACS_Globals.VerticalEdges;
            float2 horizontalEdges = ACS_Globals.HorizontalEdges;

            // Set position
            float newEnemyPositionX = random.NextFloat(0f, horizontalEdges.x);
            float3 newEnemyPosition = new float3(newEnemyPositionX, 0f, verticalEdges.y);
            Translation newEnemyTranslation = new Translation { Value = newEnemyPosition };
            EntityManager.SetComponentData(newEnemy, newEnemyTranslation);

            // Set velocity
            float randomLinearInitialVelocity = random.NextFloat(ACS_Globals.MinMaxEnemyVelocity.x, ACS_Globals.MinMaxEnemyVelocity.y);
            float3 randomXZDirection = new float3(ACS_Utils.GetRandomFloat(-5f, 5f), 0f, random.NextFloat(-5f, 5f));
            PhysicsVelocity physicsVelocity = new PhysicsVelocity();
            physicsVelocity.Linear = randomXZDirection * randomLinearInitialVelocity;
            EntityManager.SetComponentData(newEnemy, physicsVelocity);

            // Set rotation
            Rotation newRotation = new Rotation { Value = Quaternion.FromToRotation(Vector3.forward, randomXZDirection) };
            EntityManager.SetComponentData(newEnemy, newRotation);

            UnityEngine.Debug.Log($"Spawning the Enemy{randomEnemyIndex}, after waiting {timeToNextEnemy} seconds");
        }

        #endregion
    }
}
