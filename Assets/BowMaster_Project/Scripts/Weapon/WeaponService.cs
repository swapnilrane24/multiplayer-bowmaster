﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace WeaponSystem
{
    public class WeaponService : IWeaponService
    {
        readonly SignalBus signalBus;
        private ScriptableObjWeapon scriptableObjWeapon;
        private WeaponController weaponController;

        public WeaponService(SignalBus signalBus, ScriptableObjWeapon scriptableObjWeapon)
        {
            this.signalBus = signalBus;
            this.scriptableObjWeapon = scriptableObjWeapon;
            signalBus.Subscribe<SignalDestroyWeapon>(DestroyWeapon);
        }

        public void SpawnWeapon(float power, float angle, Vector2 spawnPos, WeaponType weaponType
        , bool localPlayer)
        {
            Vector2 direction = new Vector2((float)Mathf.Cos((angle) * Mathf.Deg2Rad),
                                            (float)Mathf.Sin((angle) * Mathf.Deg2Rad));

            if (weaponType == WeaponType.Air)
                weaponController = new WeaponAirController(this, power, direction, spawnPos
                , signalBus, localPlayer);
            else if (weaponType == WeaponType.Water)
                weaponController = new WeaponWaterController(this, power, direction, spawnPos
                , signalBus, localPlayer);
            else if (weaponType == WeaponType.Fire)
                weaponController = new WeaponFireController(this, power, direction, spawnPos
                , signalBus, localPlayer);
        }

        public ScriptableObjWeapon ReturnWeaponScriptable()
        {
            return scriptableObjWeapon;
        }

        public SignalBus GetSignalBus()
        {
            return signalBus; 
        }

        public void DestroyWeapon(SignalDestroyWeapon weaponDestroy)
        {
            weaponDestroy.weaponInfo.weaponController = null;
        }
    }
}