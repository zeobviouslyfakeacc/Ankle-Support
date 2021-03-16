using Harmony;
using UnityEngine;

namespace AnkleSupport
{
    [HarmonyPatch(typeof(GameManager), "Awake")]
    internal static class GameManager_Awake
    {
        private static void Postfix()
        {
            Implementation.Initialize();
        }
    }

    [HarmonyPatch(typeof(PlayerManager), "PutOnClothingItem")]
    internal static class PlayerManager_PutOnClothingItem
    {
        private static void Postfix(GearItem gi)
        {
            Implementation.OnClothingItemChange(gi);
        }
    }

    [HarmonyPatch(typeof(PlayerManager), "TakeOffClothingItem")]
    internal static class PlayerManager_TakeOffClothingItem
    {
        private static void Postfix(GearItem gi)
        {
            Implementation.OnClothingItemChange(gi);
        }
    }

    [HarmonyPatch(typeof(Sprains), "RollForSprainWhenMoving", new[] { typeof(float) })]
    internal static class Sprains_RollForSprainWhenMoving
    {
        private static void Prefix(Sprains __instance, ref float sprainChance)
        {
            if (Mathf.Approximately(sprainChance, 0f)) return;

            if (Implementation.ShouldRollForWristSprain())
            {
                Implementation.AdjustWristSprainMoveChance(ref sprainChance);
                __instance.m_ChanceOfWristSprainWhenMoving = 100.0f;
            }
            else
            {
                Implementation.AdjustAnkleSprainMoveChance(ref sprainChance);
                __instance.m_ChanceOfWristSprainWhenMoving = 0.0f;
            }
        }
    }
    [HarmonyPatch(typeof(Sprains), "MaybeSprainWhileMoving")]
    internal static class Sprains_MaybeSprainWhileMoving
    {
        private static void Prefix(Sprains __instance)
        {
            float overEncumbranceInKg = GameManager.GetEncumberComponent().m_GearWeightKG - GameManager.GetEncumberComponent().GetEffectiveCarryCapacityKG();
            __instance.m_ChanceIncreaseEncumbered = Mathf.Max(0.3f, AS_Settings.settings.OverEncumbranceValue * overEncumbranceInKg);
            __instance.m_ChanceIncreaseExhausted = AS_Settings.settings.ExhaustedValue;
        }
    }
}
