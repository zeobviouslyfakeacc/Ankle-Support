using MelonLoader;
using UnityEngine;

namespace AnkleSupport
{
    public class Implementation : MelonMod
    {

        private static float chanceOfWristSprainWhenMoving;
        private static float ankleBaseFallChance;
        private static float wristBaseFallChance;

        private static float ankleMoveChanceReduction;
        private static float wristMoveChanceReduction;

        public override void OnApplicationStart()
        {
            Debug.Log($"[{Info.Name}] version {Info.Version} loaded!");
            AS_Settings.OnLoad();
        }

        internal static void Initialize()
        {
            SprainedAnkle sprainedAnkle = GameManager.GetSprainedAnkleComponent();
            SprainedWrist sprainedWrist = GameManager.GetSprainedWristComponent();
            Sprains sprains = GameManager.GetSprainsComponent();

            chanceOfWristSprainWhenMoving = sprains.m_ChanceOfWristSprainWhenMoving;
            ankleBaseFallChance = sprainedAnkle.m_ChanceSprainAfterFall;
            wristBaseFallChance = sprainedWrist.m_ChanceSprainAfterFall;

            ankleMoveChanceReduction = 0;
            wristMoveChanceReduction = 0;
        }

        internal static bool ShouldRollForWristSprain()
        {
            return Utils.RollChance(chanceOfWristSprainWhenMoving);
        }

        internal static void AdjustAnkleSprainMoveChance(ref float sprainChance)
        {
            sprainChance -= ankleMoveChanceReduction;
        }

        internal static void AdjustWristSprainMoveChance(ref float sprainChance)
        {
            sprainChance -= wristMoveChanceReduction;
        }

        internal static void OnClothingItemChange(GearItem gi)
        {
            if (gi?.m_ClothingItem == null) return;

            if (gi.m_ClothingItem.m_Region == ClothingRegion.Feet)
            {
                UpdateAnkleSupport();
            }
            else if (gi.m_ClothingItem.m_Region == ClothingRegion.Hands)
            {
                UpdateWristSupport();
            }
        }

        private static void UpdateAnkleSupport()
        {
            SprainedAnkle sprainedAnkle = GameManager.GetSprainedAnkleComponent();
            float toughness = GetShoesToughness();
            float chanceReduction = toughness * AS_Settings.settings.BootToughnessFactor;

            ankleMoveChanceReduction = chanceReduction;
            sprainedAnkle.m_ChanceSprainAfterFall = ankleBaseFallChance - chanceReduction;
        }

        private static void UpdateWristSupport()
        {
            SprainedWrist sprainedWrist = GameManager.GetSprainedWristComponent();
            float toughness = GetGlovesToughness();
            float chanceReduction = toughness * AS_Settings.settings.GloveToughnessFactor;

            wristMoveChanceReduction = chanceReduction;
            sprainedWrist.m_ChanceSprainAfterFall = wristBaseFallChance - chanceReduction;
        }

        private static float GetShoesToughness()
        {
            PlayerManager playerManager = GameManager.GetPlayerManagerComponent();
            GearItem gearItem = playerManager.GetClothingInSlot(ClothingRegion.Feet, ClothingLayer.Top);
            return gearItem?.m_ClothingItem?.m_Toughness ?? 0f;
        }

        private static float GetGlovesToughness()
        {
            PlayerManager playerManager = GameManager.GetPlayerManagerComponent();
            GearItem gearItem = playerManager.GetClothingInSlot(ClothingRegion.Hands, ClothingLayer.Base);
            return gearItem?.m_ClothingItem?.m_Toughness ?? 0f;
        }
    }
}
