//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CapstoneProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class WS_User_Roles
    {
        public int ID { get; set; }
        public int WorkSpace_ID { get; set; }
        public string User_ID { get; set; }
        public Nullable<int> Role_ID { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual WorkSpace WorkSpace { get; set; }
    }
}