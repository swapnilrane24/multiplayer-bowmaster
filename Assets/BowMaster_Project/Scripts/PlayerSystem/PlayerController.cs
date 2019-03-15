﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem;

namespace PlayerSystem
{
    public class PlayerController : IPlayerController
    {
        private string playerID;

        private List<PlayerCharacterController> playerCharacterControllerList;
        private PlayerService playerService;
        private IWeaponService weaponService;
        private Vector2 spawnCharacterPos;
        private string turnID;
        private Vector2 fixedPos;
        private GameObject playerHolder;

        public PlayerController(PlayerSpawnData playerSpawnData, PlayerService playerService
        , IWeaponService weaponSystem)
        {
            playerHolder = new GameObject();
            this.playerService = playerService;
            this.weaponService = weaponSystem;
            this.playerID = playerSpawnData.playerID;
            spawnCharacterPos = playerSpawnData.playerPosition;
            fixedPos = playerSpawnData.playerPosition;
            playerCharacterControllerList = new List<PlayerCharacterController>();
            playerHolder.transform.position = playerSpawnData.playerPosition;

            if(playerHolder.transform.position.x > 0)
            {
                playerHolder.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }

            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    PlayerCharacterController playerCharacterController = new CharacterAirController(
                            i, this, playerService.ReturnPlayerScriptableObj(PlayerCharacterType.Air)
                            , weaponService, spawnCharacterPos, playerHolder
                        );
                    playerCharacterController.SetHealthBarFirst(playerSpawnData.char1Health);
                    Debug.Log("[PlayerController] health:" + playerSpawnData.char1Health);
                    playerCharacterControllerList.Add(playerCharacterController);
                }
                else if (i == 1)
                {
                    PlayerCharacterController playerCharacterController = new CharacterWaterController(
                            i, this, playerService.ReturnPlayerScriptableObj(PlayerCharacterType.Water)
                            , weaponService, spawnCharacterPos, playerHolder
                        );
                    playerCharacterController.SetHealthBarFirst(playerSpawnData.char2Health);
                    Debug.Log("[PlayerController] health:" + playerSpawnData.char2Health);
                    playerCharacterControllerList.Add(playerCharacterController);
                }
                else if (i == 2)
                {
                    PlayerCharacterController playerCharacterController = new CharacterFireController(
                            i, this, playerService.ReturnPlayerScriptableObj(PlayerCharacterType.Fire)
                            , weaponService, spawnCharacterPos, playerHolder
                        );
                    playerCharacterController.SetHealthBarFirst(playerSpawnData.char3Health);
                    Debug.Log("[PlayerController] health:" + playerSpawnData.char3Health);
                    playerCharacterControllerList.Add(playerCharacterController);
                }
                spawnCharacterPos.x += 2;
            }
        }

        public void SetTurnId(string turnID)
        {
            this.turnID = turnID;
        }

        public string ReturnPlayerID()
        {
            return playerID;
        }

        public void SetShootInfo(float power, float angle, int characterID, bool gettingInput)
        {
            playerCharacterControllerList[characterID].SetShootInfo(power, angle, gettingInput);
        }

        public void SetHealth(HitInfo hitInfo)
        {
            if (hitInfo.destroy == false)
                playerCharacterControllerList[hitInfo.characterId].SetHealth(hitInfo.characterHealth);
            else
            {
                playerCharacterControllerList[hitInfo.characterId].DestroyCharacter();
                PlayerCharacterController characterController = playerCharacterControllerList[hitInfo.characterId];
                playerCharacterControllerList.RemoveAt(hitInfo.characterId);
                characterController = null;
            }

        }
        public void SendDamageInfoToServer(int charID,float damage)
        {
            playerService.SendPlayerDamageDataToServer(damage, charID, playerID);
        }

        public void DeactivateDisplayPanel()
        {
            for (int i = 0; i < playerCharacterControllerList.Count; i++)
            {
                playerCharacterControllerList[i].DeactivateInfoPanel();
            }
        }

        public Vector2 GetSpawnPos()
        {
            return fixedPos;
        }

        public PlayerService GetPlayerService()
        {
            return playerService;
        }

    }
}