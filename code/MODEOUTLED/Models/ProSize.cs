//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace onsoft.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProSize
    {
        public int Id { get; set; }
        public int IdPro { get; set; }
        public int IdSize { get; set; }
        public Nullable<int> SpTon { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual Size Size { get; set; }
    }
}
