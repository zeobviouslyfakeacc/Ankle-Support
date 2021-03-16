using ModSettings;

namespace AnkleSupport
{
    internal class AnkleSupport_Settings : JsonModSettings
    {
        [Section("Ankle/ Wrist Support (A common sprain probability would be 30%)")]

        [Name("Boot Toughness Factor")]
        [Description("This value specifies how much each boot protection value reduces the ankle sprain probability percentage.\n(Vanilla = 0, Recommended = 2)")]
        [Slider(0f, 5f, 51)]
        public float BootToughnessFactor = 2f;

        [Name("Glove Toughness Factor")]
        [Description("This value specifies how much each glove protection value reduces the wrist sprain probability percentage.\n(Vanilla = 0, Recommended = 4)")]
        [Slider(0f, 5f, 51)]
        public float GloveToughnessFactor = 4f;

        [Section("Sprain Probability")]

        [Name("Over-Encumbrance")]
        [Description("How much every kg of additional weight gets added to the sprain probability when over-encumbered.\n(Vanilla = 0, Recommended = 3)")]
        [Slider(0f, 5f, 51)]
        public float OverEncumbranceValue = 3f;

        [Name("Exhausted")]
        [Description("How much gets added to the sprain probability when over-encumbered.\n(Vanilla = 0.3, Recommended = 30)")]
        [Slider(0f, 50f, 51)]
        public float ExhaustedValue = 30f;
    }

    internal static class AS_Settings
    {
        internal static AnkleSupport_Settings settings = new AnkleSupport_Settings();

        public static void OnLoad()
        {
            settings.AddToModSettings("Ankle Support");
        }
    }
}
