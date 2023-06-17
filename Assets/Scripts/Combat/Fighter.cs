using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField]
        float timeBetweenAttack = 1f;
        [SerializeField]
        Transform rightHandTransform = null;
        [SerializeField]
        Transform leftHandTransform = null;
        [SerializeField]
        Weapon defaultWeapon = null;

        Health target;
        Animator animator;
        BaseStats baseStats;

        // Enable player to attack immediately if within weapon range
        float timeSinceLastAttack = Mathf.Infinity;

        Mover mover;

        Weapon currentWeapon = null;

        private void Start()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
            baseStats = GetComponent<BaseStats>();

            if (currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null)
                return;

            if (target.IsDead())
                return;

            float distance =
                Vector3.Distance(transform.position, target.transform.position);

            if (distance < currentWeapon.GetWeaponRange())
            {
                mover.Cancel();
                AttackBehaviour();
            }
            else
            {
                mover.MoveTo(target.transform.position, 1f);
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget() { return target; }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttack)
            {
                // This will trigger the Hit() event
                TriggerAttack();
                timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            animator.ResetTrigger("stopAttack");
            animator.SetTrigger("attack");
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
                return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        /// <summary>
        /// Cancel and stop attacking
        /// </summary>
        public void Cancel()
        {
            StopAttack();
            target = null;
            mover.Cancel();
        }

        private void StopAttack()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
        }

        // Animation Event
        void Hit()
        {
            if (target == null)
                return;

            float damage = baseStats.GetStat(Stat.Damage);

            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform,
                                               leftHandTransform, target,
                                               gameObject, damage);
            }

            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }

        void Shoot() { Hit(); }

        public object CaptureState() { return currentWeapon.name; }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }

        public IEnumerable<float> GetAddictiveModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.GetWeaponDamage();
            }
        }
    }
}
