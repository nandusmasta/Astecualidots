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

    public partial class ACS_AsteroidSystem : SystemBase
    {
        #region Fields

        private BeginInitializationEntityCommandBufferSystem ecbSystem;

        private Entity gameDataEntity;

        private Entity shipEntity;

        #endregion

        #region Methods

        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            ecbSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();

            EntityQuery gameDataQuery = GetEntityQuery(typeof(ACS_GameData));
            gameDataEntity = gameDataQuery.ToEntityArray(Allocator.Temp)[0];

            CompleteDependency();
        }

        protected override void OnUpdate()
        {
            if (!ACS_Globals.HasGameStarted) return;

            if (shipEntity == Entity.Null)
            {
                shipEntity = GetSingletonEntity<ACS_ShipData>();
                return;
            }

            float2 verticalEdges = ACS_Globals.VerticalEdges;
            float2 horizontalEdges = ACS_Globals.HorizontalEdges;
            EntityCommandBuffer entityCommandBuffer = ecbSystem.CreateCommandBuffer();
            Unity.Mathematics.Random random = new Unity.Mathematics.Random(56);

            Entities.WithoutBurst().ForEach((ref Translation translation, ref PhysicsVelocity physicsVelocity, ref ACS_AsteroidData asteroidData, in Rotation rotation, in Entity entity) =>
            {

                // Keep the asteroid  within the screen boundaries
                float offset = 0.5f;
                if (translation.Value.z > verticalEdges.y)
                {
                    translation.Value.z = verticalEdges.x + offset;
                }
                else
                {
                    if (translation.Value.z < verticalEdges.x)
                    {
                        translation.Value.z = verticalEdges.y - offset;
                    }
                }
                if (translation.Value.x > horizontalEdges.y)
                {
                    translation.Value.x = horizontalEdges.x + offset;
                }
                else
                {
                    if (translation.Value.x < horizontalEdges.x)
                    {
                        translation.Value.x = horizontalEdges.y - offset;
                    }
                }

                // Makre sure no funny physics mess with the z plane
                if (translation.Value.y != 0)
                    translation.Value.y = 0;

                // Fire the megabomb!
                if (ACS_Globals.HasFiredMegaBomb)
                {
                    asteroidData.Health = 0;
                }

                // Destroy the asteroid if it's supposed to
                if (asteroidData.IsDestroyed)
                {
                    // Spawn the explosion and destroy the entity
                    if (asteroidData.HasExplosion)
                    {
                        Entity explosion = entityCommandBuffer.Instantiate(asteroidData.explosion);
                        entityCommandBuffer.SetComponent<Translation>(explosion, EntityManager.GetComponentData<Translation>(entity));
                    }
                    entityCommandBuffer.DestroyEntity(entity);

                    // Ship update
                    if (EntityManager.Exists(shipEntity))
                    {
                        ACS_ShipData shipData = GetComponent<ACS_ShipData>(shipEntity);
                        shipData.Score += asteroidData.ScoreWorth;
                        entityCommandBuffer.SetComponent(shipEntity, shipData);
                    }

                    // Control the number of asteroids on screen
                    if (asteroidData.IsLarge)
                    {
                        ACS_Globals.SpawnedLargeAsteroids--;
                        //Debug.Log($"There are {ACS_Globals.SpawnedLargeAsteroids} spawned large asteroids now");
                    }

                    // Spawn little asteroids if required
                    if (asteroidData.SpawnsPieceOnDestroy)
                    {
                        for (int i = 0; i < asteroidData.piecesToSpawnOnDestroy; i++)
                        {
                            Entity newAsteroidPiece = entityCommandBuffer.Instantiate(asteroidData.pieceToSpawnOnDestroy);

                            // Random position
                            float3 randomOffset = new float3(random.NextFloat(-8f, 8f), 0f, random.NextFloat(-8, 8f));
                            Translation randomOffsetTranslation = new Translation { Value = translation.Value + randomOffset };
                            entityCommandBuffer.SetComponent<Translation>(newAsteroidPiece, randomOffsetTranslation);

                            // Random velocity
                            float randomLinearInitialVelocity = random.NextFloat(asteroidData.MinMaxVelocityOnCreation.x, asteroidData.MinMaxVelocityOnCreation.y);
                            float3 randomAngularInitialVelocity = new float3(0f, 1f, 0f) * math.radians(random.NextFloat(0, 360));
                            float3 randomXZDirection = new float3(random.NextFloat(-5f, 5f), 0f, random.NextFloat(-5f, 5f));
                            PhysicsVelocity newAsteroidPiecePhysicsVelocity = new PhysicsVelocity();
                            newAsteroidPiecePhysicsVelocity.Angular = randomAngularInitialVelocity;
                            newAsteroidPiecePhysicsVelocity.Linear = randomXZDirection * randomLinearInitialVelocity;
                            entityCommandBuffer.SetComponent(newAsteroidPiece, newAsteroidPiecePhysicsVelocity);

                            //Debug.Log($"Created asteroid with {newAsteroidPiecePhysicsVelocity.Linear} linear and {newAsteroidPiecePhysicsVelocity.Angular} angular velocity");
                        }
                    }
                }

            }).Run();

            ACS_Globals.HasFiredMegaBomb = false;

            CompleteDependency();
        }

        #endregion
    }
}
