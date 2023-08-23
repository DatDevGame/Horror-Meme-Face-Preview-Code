
using Assets._GAME.Scripts.Skills;
using Assets._SDK.Weapon;
using System.Collections.Generic;

namespace _GAME.Scripts.Inventory
{
    public class WeaponInventory : AbstractWeaponInventory
    {
		private Weapon _weaponDefault => GameManager.Instance.Resources.WeaponDefaultSetting.weapon;

		public override void Load()
		{
            var equippedWeapon = GetWeapon();
			EquippedWeapon = equippedWeapon;

			if (equippedWeapon == null)
				PlayingWeapon = _weaponDefault;
			else
				PlayingWeapon = equippedWeapon;
		}
		private Weapon GetWeapon()
		{
			if (Weapon.ActivatedWeaponId != 0)
				return GameManager.Instance.Resources.AllWeaponsSettings[Weapon.ActivatedWeaponId].weapon;
			else
				return _weaponDefault;
		}
		public void SetOwnItemWeapon(AttackMeleeSkillSettings item)
		{
			var allWeaponSetting = GameManager.Instance.Resources.AllWeaponsSettings;
			if (allWeaponSetting == null) return;

			foreach (var weapon in allWeaponSetting)
			{
				if (weapon.Value.weapon.SkillSettings == item)
				{
					Weapon weaponItem = weapon.Value.weapon;
					weaponItem.Own();
					weaponItem.Activate();
					SetEquippedWeapon(weaponItem);
				}
			}
		}
		public void SetEquippedWeapon(Weapon weapon)
		{
			EquippedWeapon = weapon;
			EquippedWeapon.ActivatePlayed();

			SetPlayingWeapon((Weapon)EquippedWeapon);
		}
		public void SetPlayingWeapon(Weapon weapon)
		{
			PlayingWeapon = weapon;
		}

		public List<Weapon> GetWeaponsUnOwn()
		{
			List<Weapon> weapons = new List<Weapon>();

			var allWeaponSetting = GameManager.Instance.Resources.AllWeaponsSettings;
			if (allWeaponSetting == null) return null;

			foreach (var weapon in allWeaponSetting)
			{
				if (weapon.Value.weapon.IsOwned == false)
				{
					weapons.Add(weapon.Value.weapon);
				}
			}
			return weapons;
		}
        public List<Weapon> GetAllWeaponOwn()
        {
            List<Weapon> weapons = new List<Weapon>();

            var allWeaponSetting = GameManager.Instance.Resources.AllWeaponsSettings;
            if (allWeaponSetting == null) return null;

            foreach (var weapon in allWeaponSetting)
            {
                if (weapon.Value.weapon.IsOwned == true)
                {
                    weapons.Add(weapon.Value.weapon);
                }
            }
            return weapons;
        }
    }
}