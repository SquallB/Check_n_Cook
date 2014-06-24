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
    public class Difficulty : Grade
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Difficulty"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public Difficulty(int value = 1) : base(1, 4, value) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Difficulty"/> class.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        public Difficulty(String jsonString) : base(jsonString) { }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Difficulté : " + GetGrade().ToString() + "/" + GetMax().ToString();
        }
    }
}
