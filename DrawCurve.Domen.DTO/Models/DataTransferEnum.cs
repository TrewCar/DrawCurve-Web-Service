namespace DrawCurve.Domen.DTO.Models
{
    public static class DataTransferEnum
    {
        public static TTargetEnum DataTransfer<TSourceEnum, TTargetEnum>(this TSourceEnum source)
    where TSourceEnum : Enum
    where TTargetEnum : Enum
        {
            string sourceName = source.ToString();

            if (Enum.TryParse(typeof(TTargetEnum), sourceName, out object result))
            {
                return (TTargetEnum)result;
            }

            throw new ArgumentException($"Значение '{sourceName}' из {typeof(TSourceEnum)} не может быть преобразовано в {typeof(TTargetEnum)}.");
        }
    }
}
