//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BlackGoldProperties_API.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EMPLOYEETYPE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EMPLOYEETYPE()
        {
            this.EMPLOYEEROLEs = new HashSet<EMPLOYEEROLE>();
        }
    
        public int EMPLOYEETYPEID { get; set; }
        public string EMPLOYEETYPEDESCRIPTION { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EMPLOYEEROLE> EMPLOYEEROLEs { get; set; }
    }
}
