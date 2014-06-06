using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Model.Grade
{
    public class Grade
    {
        private int grade;
        private int min;
        private int max;

        public int Value
        {
            get{return grade;}
            set
            {
                if(min<=value && value<=max){
                    grade = value;
                }
            }
        }
        public Grade(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }
}
