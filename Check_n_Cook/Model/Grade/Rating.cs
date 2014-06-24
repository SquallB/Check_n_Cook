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
    public class Rating : Grade
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rating"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public Rating(int value = 1) : base(1, 5, value) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rating"/> class.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        public Rating(String jsonString) : base(jsonString) { }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Évaluation : " + GetGrade().ToString() + "/" + GetMax().ToString();
        }
    }
}
