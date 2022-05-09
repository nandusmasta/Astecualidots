using Unity.Jobs;
using Unity.Entities;
using Assets.ACS.Scripts.Utils;
using Assets.ACS.Scripts.DataComponents;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Burst;

namespace Assets.ACS.Scripts.Systems
{
    public partial class ACS_AsteroidSpawnerSystem : ComponentSystem
    {
        private Random random;

        protected override void OnCreate()
        {
            random = new Random(56);
        }

        protected override void OnUpdate()
        {
            int maxAsteroidsOnScreen = ACS_GameManager.Instance.maxAsteroidsOnScreen;
            float2 verticalEdges = ACS_GameManager.Instance.verticalEdges;
            float2 horizontalEdges = ACS_GameManager.Instance.horizontalEdges;

            Entities.ForEach((ref ACS_GameData gameData) =>
            {
                if (ACS_Globals.SpawnedLargeAsteroids < maxAsteroidsOnScreen)
                {
                    //int randomLargeAsteroidIndex = ACS_Utils.GetRandomInt(0, gameData.LargeAsteroidPrefabs.Length);
                    Entity newAsteroid = EntityManager.Instantiate(gameData.LargeAsteroidPrefab);

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

                    ACS_Globals.SpawnedLargeAsteroids++;
                }
            });
        }
    }
}
