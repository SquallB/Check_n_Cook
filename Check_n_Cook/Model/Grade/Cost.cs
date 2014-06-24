using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Grade
{
    /// <summary>
    /// 
    /// </summary>
    public class Cost : Grade
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cost"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public Cost(int value = 1) : base(1, 3, value) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cost"/> class.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        public Cost(String jsonString) : base(jsonString) { }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Coût : " + GetGrade().ToString() + "/" + GetMax().ToString();
        }
    }
}
