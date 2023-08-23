using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   [SerializeField] private Player _player;
   [SerializeField]
   private Enemy _enemy;

    void Update()
   {
      if (_player.HP<0)
      {
         Debug.Log("Enenmy win");
      }

      if (_enemy.HP <= 0)
      {
         Debug.Log("Player win");
      }
   }
}
