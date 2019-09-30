// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        13.09.2019 10:15
// Developer:      Manuel Fasching
// Project         CADMeshConverter
//
// Released under GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Models
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class FilterScript
    {
        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        private object[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("filter", typeof(FilterScriptFilter))]
        [System.Xml.Serialization.XmlElementAttribute("xmlfilter", typeof(FilterScriptXmlfilter))]
        [System.Xml.Serialization.XmlElementAttribute("hiddenFilter", typeof(bool))]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FilterScriptFilter
    {

        private FilterScriptFilterParam[] paramField;

        private string nameField;

        private bool hiddenFilterField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Param")]
        public FilterScriptFilterParam[] Param
        {
            get
            {
                return this.paramField;
            }
            set
            {
                this.paramField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool hiddenFilter
        {
            get
            {
                return this.hiddenFilterField;
            }
            set
            {
                this.hiddenFilterField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FilterScriptFilterParam
    {

        private string tooltipField;

        private byte isxmlparamField;

        private decimal maxField;

        private bool maxFieldSpecified;

        private string typeField;

        private string descriptionField;

        private string nameField;

        private string valueField;

        private byte minField;

        private bool minFieldSpecified;

        private string enum_val0Field;

        private string enum_val1Field;

        private byte enum_cardinalityField;

        private bool enum_cardinalityFieldSpecified;

        private byte xField;

        private bool xFieldSpecified;

        private byte zField;

        private bool zFieldSpecified;

        private byte yField;

        private bool yFieldSpecified;

        private string enum_val2Field;

        private string enum_val3Field;

        private bool hiddenParamField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tooltip
        {
            get
            {
                return this.tooltipField;
            }
            set
            {
                this.tooltipField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte isxmlparam
        {
            get
            {
                return this.isxmlparamField;
            }
            set
            {
                this.isxmlparamField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal max
        {
            get
            {
                return this.maxField;
            }
            set
            {
                this.maxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool maxSpecified
        {
            get
            {
                return this.maxFieldSpecified;
            }
            set
            {
                this.maxFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte min
        {
            get
            {
                return this.minField;
            }
            set
            {
                this.minField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool minSpecified
        {
            get
            {
                return this.minFieldSpecified;
            }
            set
            {
                this.minFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string enum_val0
        {
            get
            {
                return this.enum_val0Field;
            }
            set
            {
                this.enum_val0Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string enum_val1
        {
            get
            {
                return this.enum_val1Field;
            }
            set
            {
                this.enum_val1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte enum_cardinality
        {
            get
            {
                return this.enum_cardinalityField;
            }
            set
            {
                this.enum_cardinalityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool enum_cardinalitySpecified
        {
            get
            {
                return this.enum_cardinalityFieldSpecified;
            }
            set
            {
                this.enum_cardinalityFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte x
        {
            get
            {
                return this.xField;
            }
            set
            {
                this.xField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool xSpecified
        {
            get
            {
                return this.xFieldSpecified;
            }
            set
            {
                this.xFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte z
        {
            get
            {
                return this.zField;
            }
            set
            {
                this.zField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool zSpecified
        {
            get
            {
                return this.zFieldSpecified;
            }
            set
            {
                this.zFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte y
        {
            get
            {
                return this.yField;
            }
            set
            {
                this.yField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ySpecified
        {
            get
            {
                return this.yFieldSpecified;
            }
            set
            {
                this.yFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string enum_val2
        {
            get
            {
                return this.enum_val2Field;
            }
            set
            {
                this.enum_val2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string enum_val3
        {
            get
            {
                return this.enum_val3Field;
            }
            set
            {
                this.enum_val3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool hiddenParam
        {
            get
            {
                return this.hiddenParamField;
            }
            set
            {
                this.hiddenParamField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FilterScriptXmlfilter
    {

        private FilterScriptXmlfilterXmlparam[] xmlparamField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("xmlparam")]
        public FilterScriptXmlfilterXmlparam[] xmlparam
        {
            get
            {
                return this.xmlparamField;
            }
            set
            {
                this.xmlparamField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FilterScriptXmlfilterXmlparam
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }



}
