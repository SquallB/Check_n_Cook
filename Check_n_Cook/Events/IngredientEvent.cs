﻿using Check_n_Cook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check_n_Cook.Events
{
    public class IngredientEvent : Event
    {
        public Ingredient Ingredient { get; set; }

        public String GroupName { get; set; }

        public IngredientEvent() : this(null, null, "") { }

        public IngredientEvent(AbstractModel model, Ingredient ingredient, String groupName) : base(model)
        {
            this.Ingredient = ingredient;
            this.GroupName = groupName;
        }
    }
}
