/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

namespace Assets.ACS.Scripts.Behaviours
{
    using UnityEngine;

    public class ACS_GameAudioManager : ACS_MonoBehaviour
    {
        #region Fields

        public AudioSource BlasterFiredSFX;

        public AudioSource EnemyBlasterFiredSFX;

        public AudioSource EnemyExplosionSFX;

        public AudioSource LargeAsteroidExplosionSFX;

        public AudioSource MediumAsteroidExplosionSFX;

        public AudioSource PowerUpPickupSFX;

        public AudioSource ShipExplosionSFX;

        public AudioSource SmallAsteroidExplosionSFX;

        public AudioSource MainTheme;

        private static ACS_GameAudioManager _Instance;

        #endregion

        #region Properties

        public static ACS_GameAudioManager Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = GameObject.FindObjectOfType<ACS_GameAudioManager>();
                return _Instance;
            }
        }

        #endregion

        #region Methods

        public void Start()
        {
            _Instance = GameObject.FindObjectOfType<ACS_GameAudioManager>();
            MainTheme.Play();
        }
        public void PlayBlasterFiredSFX(bool randomizePitchFlag = false, float randomPercent = 10f)
        {
            if (randomizePitchFlag)
                randomizePitch(BlasterFiredSFX, randomPercent);
            BlasterFiredSFX.Play();
        }

        public void PlayEnemyBlasterFiredSFX(bool randomizePitchFlag = false, float randomPercent = 10f)
        {
            if (randomizePitchFlag)
                randomizePitch(EnemyBlasterFiredSFX, randomPercent);
            EnemyBlasterFiredSFX.Play();
        }

        public void PlayEnemyExplosionSFX(bool randomizePitchFlag = false, float randomPercent = 10f)
        {
            if (randomizePitchFlag)
                randomizePitch(EnemyExplosionSFX, randomPercent);
            EnemyExplosionSFX.Play();
        }

        public void PlayLargeAsteroidExplosionSFX(bool randomizePitchFlag = false, float randomPercent = 10f)
        {
            if (randomizePitchFlag)
                randomizePitch(LargeAsteroidExplosionSFX, randomPercent);
            LargeAsteroidExplosionSFX.Play();
        }

        public void PlayMediumAsteroidExplosionSFX(bool randomizePitchFlag = false, float randomPercent = 10f)
        {
            if (randomizePitchFlag)
                randomizePitch(MediumAsteroidExplosionSFX, randomPercent);
            MediumAsteroidExplosionSFX.Play();
        }

        public void PlayPowerUpPickupSFX(bool randomizePitchFlag = false, float randomPercent = 10f)
        {
            if (randomizePitchFlag)
                randomizePitch(PowerUpPickupSFX, randomPercent);
            PowerUpPickupSFX.Play();
        }

        public void PlayShipExplosionSFX(bool randomizePitchFlag = false, float randomPercent = 10f)
        {
            if (randomizePitchFlag)
                randomizePitch(ShipExplosionSFX, randomPercent);
            ShipExplosionSFX.Play();
        }

        public void PlaySmallAsteroidExplosionSFX(bool randomizePitchFlag = false, float randomPercent = 10f)
        {
            if (randomizePitchFlag)
                randomizePitch(SmallAsteroidExplosionSFX, randomPercent);
            SmallAsteroidExplosionSFX.Play();
        }

        private void randomizePitch(AudioSource audioSource, float randomPercent)
        {
            audioSource.pitch *= 1 + Random.Range(-randomPercent / 100, randomPercent / 100);
        }

        #endregion
    }
}
