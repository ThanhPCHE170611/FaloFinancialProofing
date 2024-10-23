namespace FALOFinancialProofing.Extensions
{
    public static class StringExtension
    {
        public static int ParseStringToInt(string str)
        {
            try
            {
                return Convert.ToInt32(str);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
