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
    
    public partial class INSPECTION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public INSPECTION()
        {
            this.PROPERTYDEFECTs = new HashSet<PROPERTYDEFECT>();
        }
    
        public int INSPECTIONID { get; set; }
        public Nullable<int> INSPECTIONTYPEID { get; set; }
        public int PROPERTYID { get; set; }
        public Nullable<int> IVSTATUSID { get; set; }
        public Nullable<int> USERID { get; set; }
        public string INSPECTIONDOCUMENT { get; set; }
        public string INSPECTIONCOMMENT { get; set; }
        public Nullable<System.DateTime> INSPECTIONDATE { get; set; }
    
        public virtual EMPLOYEE EMPLOYEE { get; set; }
        public virtual INSPECTIONTYPE INSPECTIONTYPE { get; set; }
        public virtual IVSTATU IVSTATU { get; set; }
        public virtual PROPERTY PROPERTY { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROPERTYDEFECT> PROPERTYDEFECTs { get; set; }
    }
}
