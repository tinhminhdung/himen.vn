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
    
    public partial class sp_Member_GetById_Result
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public System.DateTime Birthday { get; set; }
        public Nullable<int> Sex { get; set; }
        public string Image { get; set; }
        public Nullable<System.DateTime> SDate { get; set; }
        public Nullable<System.DateTime> MDate { get; set; }
        public Nullable<System.DateTime> EDate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int IdGroupMb { get; set; }
        public bool Active { get; set; }
    }
}
