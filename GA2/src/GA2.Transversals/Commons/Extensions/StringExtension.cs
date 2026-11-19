using System.Globalization;
using System.Linq;
using System.Text;

namespace GA2.Transversals.Commons.Extensions
{
    public static class StringExtension
    {
        public static string RemoveDiacritics(this string str)
        {
            var normalizeString = str.Normalize(NormalizationForm.FormD);
            var withoutDiacritics = normalizeString.Where(x => CharUnicodeInfo.GetUnicodeCategory(x) != UnicodeCategory.NonSpacingMark);
            var result = string.Concat(withoutDiacritics).Normalize(NormalizationForm.FormC);
            return result;
        }
    }
}
