using Game.Data;

namespace Game.Base
{
    interface IEnemyBaseAction
    {
        EnemyUnitData EnemyUnitData { get; }
        EnemyState EnemyState { get; }
        EnemyAttackState EnemyAttackState { get; }

        void Born(EnemyUnitData data);
        void Dead();
        void Attack(int targetId, float damage);
        void Hit(int sourceId, float damage);
        void Move();
    }
}
