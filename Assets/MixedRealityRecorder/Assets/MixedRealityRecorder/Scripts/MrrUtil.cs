using MRR.Model;

namespace MRR.Utility
{
    public class Util
    {

        public static float ReturnValidFloatFromString(string input)
        {
            if (input == "" || input == "-")
                return 0.0f;
            else
                return float.Parse(input);
        }

        public static int ReturnValidIntFromString(string input)
        {
            if (input == "" || input[0] == '-')
                return 0;
            else
                return int.Parse(input);
        }

        public static OutputFormat GetOutputFormat(string input)
        {
            switch (input)
            {
                case "TGA Image Sequence":
                    return OutputFormat.TgaImageSequence;
                case "BMP Image Sequence":
                    return OutputFormat.BmpImageSequence;
                default:
                    return OutputFormat.ManualScreencapture;
            }
        }

        public static string GetOutputFormatName(OutputFormat outputFormat)
        {
            switch (outputFormat)
            {
                case OutputFormat.ManualScreencapture:
                    return "Manual Screencapture";
                case OutputFormat.TgaImageSequence:
                    return "TGA Image Sequence";
                case OutputFormat.BmpImageSequence:
                    return "BMP Image Sequence";
                default:
                    return "None";
            }
        }
    }
}
