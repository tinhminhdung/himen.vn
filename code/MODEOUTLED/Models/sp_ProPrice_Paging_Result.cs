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
    
    public partial class sp_ProPrice_Paging_Result
    {
        public Nullable<long> RowNum { get; set; }
        public int Id { get; set; }
        public int IdPro { get; set; }
        public string PriceImport { get; set; }
        public string PriceExport_S { get; set; }
        public string PriceExport_L { get; set; }
        public string PricePromotion { get; set; }
        public Nullable<System.DateTime> SDate { get; set; }
        public Nullable<System.DateTime> EDate { get; set; }
        public Nullable<int> Ord { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    }
}