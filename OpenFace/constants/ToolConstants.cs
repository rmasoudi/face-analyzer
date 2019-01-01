namespace OpenFace.constants
{
    public class ToolConstants
    {
        public const string TOOLS_BLUSH = "blush";
        public const string TOOLS_CONCEALER = "concealer";
        public const string TOOLS_EYEBROW_PENCIL = "eyebrow-pencil";
        public const string TOOLS_EYE_LINER = "eye-liner";
        public const string TOOLS_EYE_PENCEIL = "eye-pencil";
        public const string TOOLS_EYESHADOW = "eyeshadow";
        public const string TOOLS_FOUNDATION = "foundation";
        public const string TOOLS_LIP_GLOSS = "lip-gloss";
        public const string TOOLS_LIP_LINER = "lip-liner";
        public const string TOOLS_LIPSTICK = "lipstick";
        public const string TOOLS_LIPSTICK_PEN = "lipstick-pen";
        public const string TOOLS_MASCARA = "mascara";
        public const string TOOLS_POWDER = "powder";

        public static string[] TOOLS_ARRAY;
        static ToolConstants()
        {
            TOOLS_ARRAY = new string[13];
            TOOLS_ARRAY[0] = TOOLS_BLUSH;
            TOOLS_ARRAY[1] = TOOLS_CONCEALER;
            TOOLS_ARRAY[2] = TOOLS_EYEBROW_PENCIL;
            TOOLS_ARRAY[3] = TOOLS_EYE_LINER;
            TOOLS_ARRAY[4] = TOOLS_EYE_PENCEIL;
            TOOLS_ARRAY[5] = TOOLS_EYESHADOW;
            TOOLS_ARRAY[6] = TOOLS_FOUNDATION;
            TOOLS_ARRAY[7] = TOOLS_LIP_GLOSS;
            TOOLS_ARRAY[8] = TOOLS_LIP_LINER;
            TOOLS_ARRAY[9] = TOOLS_LIPSTICK;
            TOOLS_ARRAY[10] = TOOLS_LIPSTICK_PEN;
            TOOLS_ARRAY[11] = TOOLS_MASCARA;
            TOOLS_ARRAY[12] = TOOLS_POWDER;

        }
    }
}
