﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem;

namespace PlayerSystem
{
    public class CharacterFireController : PlayerCharacterController
    {
        public CharacterFireController(int characterID, PlayerController playerController,
        ScriptableObjCharacter scriptableObjPlayer, IWeaponService weaponService,
            Vector2 spawnPos, GameObject parentObj, string localPlayerID)
        {
            this.localPlayerID = localPlayerID;
            this.scriptableObjPlayer = scriptableObjPlayer;
            this.characterID = characterID;
            this.weaponService = weaponService;
            this.playerController = playerController;
            this.playerCharacterType = scriptableObjPlayer.playerType;
            GameObject playerObj = GameObject.Instantiate<GameObject>(
                        scriptableObjPlayer.playerView.gameObject
                );
            playerObj.transform.SetParent(parentObj.transform);
            playerObj.transform.position = spawnPos;
            playerCharacterView = playerObj.GetComponent<CharacterFireView>();
            playerCharacterView.SetCharacterController(this);
        }

        public override void SetShootInfo(float power, float angle, bool gettingInput)
        {
            playerCharacterView.SetShootInfo(power, angle, gettingInput);

            if (gettingInput == false)
                weaponService.SpawnWeapon(power, angle
                , playerCharacterView.ShootPos
                    , scriptableObjPlayer.weaponType
                , playerController.IsLocalPlayer());
        }


    }
}