namespace EmptySoul.AdultTwitch.Utils
{
    public static class DigitHandler
    {
        private static readonly string[] Prefixes = { "", "K", "M", "B", "T", "q", "Q", "s", "S", "O", "N", "d", "U", "D" };

        public static string FormatValue(double digit, bool clipped = false)
        {
            int n = 0;
            while (n + 1 < Prefixes.Length && digit >= 1000)
            {
                digit /= 1000;
                n++;
            }
            return clipped ? digit.ToString("#").Replace(',', ' ') + Prefixes[n] : digit.ToString("#.#").Replace(',', '.') + Prefixes[n];
        }
    }
}