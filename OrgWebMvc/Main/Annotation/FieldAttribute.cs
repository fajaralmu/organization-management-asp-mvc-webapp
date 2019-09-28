﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstApp.Annotation
{
    public class FieldAttribute:Attribute
    {
        public FieldAttribute()
        {
            SkipInTable = false;
            FieldType = AttributeConstant.TYPE_TEXTBOX;
        }

        public string FieldName { get; set; }
        public AttributeConstant FieldType { get; set; }
        public object[] Values { get; set; }
        public object[] ItemNames { get; set; }
        public string ClassReference { get; set; }
        public string ClassRefPropName { get; set; }
        public string[] AttrToDisplay { get; set; }
        public bool SkipInTable { get; set; }
        public bool Required { get; set; }
        public bool AutoGenerated { get; set; }
        public string ClassAttributeConverter { get; set; }
        public int FixSize { get; set; }
    }
}