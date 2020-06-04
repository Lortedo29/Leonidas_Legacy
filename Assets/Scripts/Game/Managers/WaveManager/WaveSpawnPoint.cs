﻿using Lortedo.Utilities.Pattern;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.WaveSystem
{
    /// <summary>
    /// This script start wave sequence when order received from WaveManager.
    /// It spawn every mob from its position.
    /// </summary>
    public class WaveSpawnPoint : MonoBehaviour
    {
        [Required]
        [SerializeField] private WavesData _wavesData;        
        
        private Coroutine _currentCoroutine;

        public WavesData WavesData { get => _wavesData; set => _wavesData = value; }

        void OnEnable()
        {
            WaveManager.OnWaveStart += WaveManager_OnWaveStart;
        }

        void OnDisable()
        {
            WaveManager.OnWaveStart -= WaveManager_OnWaveStart;
        }

        void WaveManager_OnWaveStart(int waveCount)
        {            
            StartWave(waveCount);
        }

        private void StartWave(int waveCount)
        {
            Assert.IsNotNull(_wavesData, string.Format("Please assign a WavesData to {0}.", name));
            _currentCoroutine = StartCoroutine(_wavesData.WaveSequence(waveCount, transform.position));
        }
    }
}
