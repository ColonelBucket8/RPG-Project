using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon",
                     order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField]
        AnimatorOverrideController animatorOverride = null;
        [SerializeField]
        Weapon equippedPrefab = null;
        [SerializeField]
        float weaponDamage = 5f;
        [SerializeField]
        float percentageBonus = 0f;
        [SerializeField]
        float weaponRange = 2f;
        [SerializeField]
        bool isRightHanded = true;
        [SerializeField]
        Projectile projectile = null;

        const string weaponName = "Weapon";

        public Weapon Spawn(Transform rightHand, Transform leftHand,
                            Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            Weapon weapon = null;

            if (equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                weapon = Instantiate(equippedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }

            var overrideController =
                animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController =
                    overrideController.runtimeAnimatorController;
            }

            return weapon;
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }

            if (oldWeapon == null)
                return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        public bool HasProjectile() { return projectile != null; }

        public void LaunchProjectile(Transform rightHand, Transform leftHand,
                                     Health target, GameObject instigator,
                                     float calculateDamage)
        {
            Projectile projectileInstance =
                Instantiate(projectile, GetTransform(rightHand, leftHand).position,
                            Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, weaponDamage);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded)
            {
                handTransform = rightHand;
            }
            else
            {
                handTransform = leftHand;
            }

            return handTransform;
        }

        public float GetWeaponDamage() { return weaponDamage; }

        public float GetPercentageBonus() { return percentageBonus; }

        public float GetWeaponRange() { return weaponRange; }
    }
}
