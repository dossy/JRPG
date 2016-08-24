﻿using UnityEngine;
using System.Collections;
using System;

public class ShapeshifterTank : Battler {

    public override IEnumerator ChooseAction(Action Finish)
    {
        System.Random r = new System.Random();

        if (battleState.currentHealth > 0.75f * battleState.maximumHealth)
        {
            int n = r.Next(4);

            if (n == 3)
            {
                DoAction = StrongAttack;
            }
            else
            {
                DoAction = BasicAttack;
            }
        }
        else
        {
            int n = r.Next(5);

            if (n == 4)
            {
                DoAction = Heal;
            }
            else if (n == 3)
            {
                DoAction = StrongAttack;
            }
            else
            {
                DoAction = BasicAttack;
            }
        }
        
        GameObject player = GameObject.FindWithTag("Player");
        singleAttackTarget = player.transform.parent.gameObject.GetComponent<Battler>();

        Finish();

        yield break;
    }

    //deal 50% more damage than a basic attack
    IEnumerator StrongAttack(Action<bool, bool> Finish)
    {
        StartCoroutine(CombatUI.Instance.DisplayMessage("The enemy comes at you at full force!", 1f));
        
        float damage = 1.5f * CalculateStandardDamage(singleAttackTarget);

        // TODO: Animations for attack.
        // AnimateMethod(DoAction, ref bool)
        // yield return new WaitUntil(()=>bool)

        //in place of animations, there is a 2 second wait
        yield return new WaitForSeconds(2);

        StartCoroutine(DealDamage(damage, Finish));
    }

    //increases hp by 25% of max hp, but causes double damage to be taken next turn
    IEnumerator Heal(Action<bool, bool> Finish)
    {
        StartCoroutine(CombatUI.Instance.DisplayMessage(
            "The enemy momentarily lowers their guard to heal.", 1f));

        statusEffect se = new statusEffect("Shapeshifter Vulnerability", true, false, 1, true);
        battleState.statusEffects.Add(se);

        //in place of animations, there is a 2 second wait
        yield return new WaitForSeconds(2);

        battleState.currentHealth += (int)(0.25f * battleState.maximumHealth);

        StartCoroutine(CombatUI.Instance.UpdateHealthBar((double)battleState.currentHealth,
                (double)battleState.maximumHealth, false));

        Finish(false, false);
    }
}