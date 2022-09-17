using System.Collections;
using CodeBase.Enemy.Components;
using CodeBase.Hero.Components;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Factory;
using CodeBase.StaticData;
using UnityEngine;
namespace CodeBase.Logic.EnemySpawn
{
    public class Spawner : MonoBehaviour, ICoroutineRunner
    {
        private EnemyDeath _enemyDeath;

        private IGameFactory _factory;
        private HeroDeath _heroDeath;
        private float _maxX;
        private float _maxZ;

        private float _minX;
        private float _minZ;
        private float _monsterCap;

        private int _monsterCount;
        private MonsterTypeId _monsterTypeId;
        private float _secondsToSpawn;

        public void Construct(IGameFactory factory, EnemySpawnerData spawnerData, BattleFieldData battleFieldData,
            HeroDeath heroDeath)
        {
            _factory = factory;

            _monsterTypeId = spawnerData.monsterTypeId;
            _secondsToSpawn = spawnerData.secondsToSpawn;
            _monsterCap = spawnerData.monsterCap;

            _minX = battleFieldData.minX;
            _maxX = battleFieldData.maxX;
            _minZ = battleFieldData.minZ;
            _maxZ = battleFieldData.maxZ;

            _heroDeath = heroDeath;
        }

        public void StartSpawning()
        {
            StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            while (!_heroDeath.isDead)
            {
                Spawn();
                yield return new WaitForSeconds(_secondsToSpawn);
                yield return new WaitUntil(() => _monsterCount < _monsterCap);
            }
        }

        private async void Spawn()
        {
            GameObject monster = await _factory.CreateMonster(_monsterTypeId, RandomPosition());
            _monsterCount++;
            _enemyDeath = monster.GetComponent<EnemyDeath>();
            _enemyDeath.Happened += Slay;
        }

        private Vector3 RandomPosition()
        {
            return new Vector3(
                Random.Range(_minX, _maxX),
                0,
                Random.Range(_minZ, _maxZ));
        }

        private void Slay()
        {
            if (_enemyDeath == null)
                return;

            _enemyDeath.Happened -= Slay;
            _monsterCount--;
        }
    }
}
