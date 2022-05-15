/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.Systems
{
    using Assets.ACS.Scripts.DataComponents;
    using Assets.ACS.Scripts.Utils;
    using Unity.Entities;
    using Unity.Mathematics;
    using Unity.Physics;
    using Unity.Transforms;

    public partial class ACS_AsteroidSpawnerSystem : ComponentSystem
    {
        #region Fields

        private Random random;

        #endregion

        #region Methods

        protected override void OnCreate()
        {
            random = new Random(56);
        }

        protected override void OnUpdate()
        {
            if (!ACS_Globals.IsPlayerPlaying) return;

            int maxAsteroidsOnScreen = ACS_Globals.MaxAsteroidsOnScreen;
            float2 verticalEdges = ACS_Globals.VerticalEdges;
            float2 horizontalEdges = ACS_Globals.HorizontalEdges;
            int spawnedLargeAsterois = ACS_Globals.SpawnedLargeAsteroids;

            Entities.ForEach((ref ACS_GameData gameData) =>
            {
                if (spawnedLargeAsterois < maxAsteroidsOnScreen)
                {
                    Entity newAsteroid = EntityManager.Instantiate(gameData.LargeAsteroidPrefab);
                    // UnityEngine.Debug.Log($"Spawning new large asteroid so we keep them at {maxAsteroidsOnScreen}");

                    // Get asteroid data
                    ComponentDataFromEntity<ACS_AsteroidData> asteroidDataFromEntity = GetComponentDataFromEntity<ACS_AsteroidData>(true);
                    ACS_AsteroidData asteroidData = asteroidDataFromEntity[newAsteroid];

                    // Set position
                    float randomAsteroidPositionX = random.NextFloat(0f, horizontalEdges.x);
                    float3 randomAsteroidPosition = new float3(randomAsteroidPositionX, 0f, verticalEdges.y);
                    Translation newAsteroidTranslation = new Translation { Value = randomAsteroidPosition };
                    EntityManager.SetComponentData(newAsteroid, newAsteroidTranslation);

                    // Set velocity
                    float randomLinearInitialVelocity = random.NextFloat(asteroidData.MinMaxVelocityOnCreation.x, asteroidData.MinMaxVelocityOnCreation.y);
                    float3 randomAngularInitialVelocity = new float3(0f, 1f, 0f) * math.radians(random.NextFloat(0, 360));
                    float3 randomXZDirection = new float3(ACS_Utils.GetRandomFloat(-5f, 5f), 0f, random.NextFloat(-5f, 5f));
                    PhysicsVelocity physicsVelocity = new PhysicsVelocity();
                    physicsVelocity.Angular = randomAngularInitialVelocity;
                    physicsVelocity.Linear = randomXZDirection * randomLinearInitialVelocity;
                    EntityManager.SetComponentData(newAsteroid, physicsVelocity);

                    spawnedLargeAsterois++;
                }
            });

            ACS_Globals.SpawnedLargeAsteroids = spawnedLargeAsterois;
        }

        #endregion
    }
}
