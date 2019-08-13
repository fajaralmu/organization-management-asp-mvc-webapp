//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OrgWebMvc.Models
{
    using InstApp.Annotation;
    using System;
    using System.Collections.Generic;

    public partial class section
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public section()
        {
            this.positions = new HashSet<position>();
            this.programs = new HashSet<program>();
            this.section1 = new HashSet<section>();
        }

        [FieldAttribute(FieldType = AttributeConstant.TYPE_ID_AI)]
        public int id { get; set; }
        [FieldAttribute(FieldType = AttributeConstant.TYPE_TEXTBOX, Required = true)]
        public string name { get; set; }
        [FieldAttribute(Required = true, FieldType = AttributeConstant.TYPE_DROPDOWN, FieldName = "Division", ClassReference = "division", ClassAttributeConverter = "name")]
        public int division_id { get; set; }
        [FieldAttribute(Required = false, FieldType = AttributeConstant.TYPE_DROPDOWN, FieldName = "Parent_Section",ClassRefPropName ="section2", ClassReference = "section", ClassAttributeConverter = "name")]
        public Nullable<int> parent_section_id { get; set; }
        [FieldAttribute(FieldType = AttributeConstant.TYPE_TEXTAREA, Required = true)]
        public string description { get; set; }

        public section sectionParent
        {
            get
            {
                if (section1.Count > 0)
                {
                    return section1.GetEnumerator().Current;
                }
                return null;
            }
        }

        //public int id { get; set; }
        //public string name { get; set; }
        //public int division_id { get; set; }
        //public Nullable<int> parent_section_id { get; set; }
        //public string description { get; set; }
    
        public virtual division division { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<position> positions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<program> programs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<section> section1 { get; set; }
        public virtual section section2 { get; set; }
    }
}