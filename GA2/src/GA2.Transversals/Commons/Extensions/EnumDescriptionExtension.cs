using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Transversals.Commons.Extensions
{
    public static class EnumDescriptionExtension
    {
        /// <summary>
        /// Obtiene la descripcion de la enumeración
        /// </summary>
        /// <param name="enumValue"></param>
        /// <author>JC</author>
        /// <returns></returns>
        public static string GetDescription(this Enum enumValue)
        {
            return HelpersEnums.GetEnumDescription(enumValue);
        }
    }
}
