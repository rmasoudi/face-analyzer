using System.Collections.Generic;

namespace OpenFace.constants
{
    public class FeatureConstants
    {
        private static string F1 = "مات";
        private static string F2 = "براق";
        private static string F3 = "ترکیبی";
        private static string F4 = "ماژیکی";
        private static string F5 = "مویی";
        private static string F6 = "ژل";
        private static string F7 = "فشرده";
        private static string F8 = "رنگی";
        private static string F9 = "مشکی";
        private static string F10 = "تک رنگ";
        private static string F11 = "مایع";
        private static string F12 = "موس";
        private static string F13 = "ماندگاری طولانی";
        private static string F14 = "حجم دهنده";
        private static string F15 = "بلند کننده";
        private static string F16 = "حالت دهنده";
        private static string F17 = "جدا کننده";
        private static string F18 = "پنکیک";
        private static string F19 = "پودر مات کننده";
        private static string F20 = "تثبیت کننده";

        private static Dictionary<string, int> featureMap = new Dictionary<string, int>();
        static FeatureConstants()
        {
            featureMap.Add(F1, 1);
            featureMap.Add(F2, 2);
            featureMap.Add(F3, 3);
            featureMap.Add(F4, 4);
            featureMap.Add(F5, 5);
            featureMap.Add(F6, 6);
            featureMap.Add(F7, 7);
            featureMap.Add(F8, 8);
            featureMap.Add(F9, 9);
            featureMap.Add(F10, 10);
            featureMap.Add(F11, 11);
            featureMap.Add(F12, 12);
            featureMap.Add(F13, 13);
            featureMap.Add(F14, 14);
            featureMap.Add(F15, 15);
            featureMap.Add(F16, 16);
            featureMap.Add(F17, 17);
            featureMap.Add(F18, 18);
            featureMap.Add(F19, 19);
            featureMap.Add(F20, 20);
        }

        public static int GetFeatureCode(string text)
        {
            int output = 0;
            featureMap.TryGetValue(text.Trim(), out output);
            return output;
        }

        public static int[] GetToolFeatures(string tool)
        {
            switch (tool)
            {
                case ToolConstants.TOOLS_BLUSH:
                    return new int[] { 1, 2, 3 }; ;
                case ToolConstants.TOOLS_EYE_LINER:
                    return new int[] { 4, 5, 6, 7 };
                case ToolConstants.TOOLS_EYE_PENCEIL:
                    return new int[] { 8, 9 };
                case ToolConstants.TOOLS_EYESHADOW:
                    return new int[] { 10 };
                case ToolConstants.TOOLS_FOUNDATION:
                    return new int[] { 11, 7, 12 };
                case ToolConstants.TOOLS_LIP_GLOSS:
                    return new int[] { 1, 2, 13, 14 };
                case ToolConstants.TOOLS_LIPSTICK:
                    return new int[] { 1, 2 };
                case ToolConstants.TOOLS_MASCARA:
                    return new int[] { 14, 15, 16, 17 };
                case ToolConstants.TOOLS_POWDER:
                    return new int[] { 18, 19, 20 };
            }
            return new int[] { };
        }
    }
}
