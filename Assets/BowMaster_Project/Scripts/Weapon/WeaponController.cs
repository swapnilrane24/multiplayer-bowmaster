﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    public class WeaponController
    {
        protected WeaponService weaponService;
        protected WeaponView weaponView;
        protected WeaponType weaponType;

        protected virtual void SetWeaponType()
        {
            weaponType = WeaponType.Air;
        }

        protected WeaponView GetWeaponView()
        {
            for (int i = 0; i < weaponService.ReturnWeaponScriptable().weaponList.Count; i++)
            {
                if (weaponType == weaponService.ReturnWeaponScriptable().weaponList[i].weaponType)
                    return weaponService.ReturnWeaponScriptable().weaponList[i].weaponView;
            }

            return null;
        }
    }
}