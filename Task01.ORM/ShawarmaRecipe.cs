//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Task01.ORM
{
    using System;
    using System.Collections.Generic;
    
    public partial class ShawarmaRecipe
    {
        public int ShawarmaRecipeId { get; set; }
        public int ShawarmaId { get; set; }
        public int IngradientId { get; set; }
        public int Weight { get; set; }
    
        public virtual Ingradient Ingradient { get; set; }
        public virtual Shawarma Shawarma { get; set; }
    }
}
