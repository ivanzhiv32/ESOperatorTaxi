using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOperatorTaxi
{
    /// <summary>
    /// Степень соответствия водителя для заказа
    /// </summary>
    enum DegreeCompliance
    {
        /// <summary>
        /// Первая степень (хорошо)
        /// </summary>
        One,
        /// <summary>
        /// Вторая степень (средне)
        /// </summary>
        Two,
        /// <summary>
        /// Третья степень (плохо)
        /// </summary>
        Three,
        /// <summary>
        /// Четвертая степень (недопустимо)
        /// </summary>
        Four
    }
}
