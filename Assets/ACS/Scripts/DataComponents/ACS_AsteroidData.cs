﻿using Unity.Entities;
using Unity.Mathematics;

namespace Assets.ACS.Scripts.DataComponents
{
    [GenerateAuthoringComponent]
    public struct ACS_AsteroidData : IComponentData
    {

        public bool isStatic;
        public float Health;
        public enum AsteroidType { Large, Medium, Small };
        public AsteroidType type;
        public Entity pieceToSpawnOnDestroy;
        public int piecesToSpawnOnDestroy;
        public float2 MinMaxVelocityOnCreation;

        public bool SpawnsPieceOnDestroy
        {
            get
            {
                return pieceToSpawnOnDestroy != null && piecesToSpawnOnDestroy > 0;
            }
        }
        public bool IsDestroyed
        {
            get
            {
                return Health <= 0;
            }
        }

    }
}
