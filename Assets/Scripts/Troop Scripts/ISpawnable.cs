using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnable
{
    //This interface is designed to make getting troop stats easier
    bool GetIsFlying()
    {
        return false;
    }
    int GetDamage()
    {
        return 0;
    }
    float GetSpeed()
    {
        return 0f;
    }
    float GetRange()
    {
        return 0f;
    }
    float GetFireRate()
    {
        return 0f;
    }
    int GetHealth()
    {
        return 0;
    }
    int GetMaxHealth()
    {
        return 0;
    }
    bool GetCanAttackGround()
    {
        return false;
    }
    bool GetCanAttackAir()
    {
        return false;
    }
    bool GetCanAttackBuilding()
    {
        return false;
    }

    GameObject GetProjectile()
    {
        return null;
    }

    Sprite GetIcon()
    {
        return null;
    }

    void SetIsEnemy();

    void AddHealth(int health);
}
