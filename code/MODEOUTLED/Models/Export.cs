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
    
    public partial class Export
    {
        public Export()
        {
            this.ExportDetails = new HashSet<ExportDetail>();
        }
    
        public int Id { get; set; }
        public string CusName { get; set; }
        public System.DateTime SDate { get; set; }
        public double Amount { get; set; }
        public int IdMember { get; set; }
        public int IdWareHouse { get; set; }
        public string Description { get; set; }
        public Nullable<int> Ord { get; set; }
    
        public virtual ICollection<ExportDetail> ExportDetails { get; set; }
        public virtual WareHouse WareHouse { get; set; }
        public virtual Member Member { get; set; }
    }
}
