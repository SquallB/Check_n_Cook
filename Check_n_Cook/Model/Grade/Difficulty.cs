﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Grade
{
    public class Difficulty : Grade
    {
        public Difficulty(int value = 1) : base(1, 5, value) { }

        public Difficulty(String jsonString) : base(jsonString) { }

        public override string ToString()
        {
            return "Difficulté : " + GetGrade().ToString() + "/" + GetMax().ToString();
        }
    }
}
