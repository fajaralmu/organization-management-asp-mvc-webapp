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
    [Entity]
    public partial class post
    {
        [FieldAttribute(FieldType = AttributeConstant.TYPE_ID_AI)]
        public int id { get; set; }
        public int user_id { get; set; }
        [FieldAttribute(FieldType = AttributeConstant.TYPE_TEXTBOX, Required = true)]
        public string title { get; set; }
        [FieldAttribute(FieldType = AttributeConstant.TYPE_RICHTEXT, Required = true, SkipInTable = true)]
        public string body { get; set; }
        [FieldAttribute(FieldType = AttributeConstant.TYPE_DATE, Required = true)]
        public System.DateTime date { get; set; }
        public int type { get; set; }
        public Nullable<int> post_id { get; set; }
        public DateTime created_date { get; set; }
        public virtual user user { get; set; }
    }
}
