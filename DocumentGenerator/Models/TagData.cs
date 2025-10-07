using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentGenerator.Models
{
    public class TagData
    {
        public string TagName { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public TagData()
        {
            IsRequired = false;
        }

        public TagData(string tagName, string value, string description = null, bool isRequired = false)
        {
            TagName = tagName ?? throw new ArgumentNullException(nameof(tagName));
            Value = value;
            Description = description;
            IsRequired = isRequired;
        }

        public string GetFormattedTag => $"{{{TagName}}}";

        public override string ToString()
        {
            return base.ToString();
            //return $"{TagName} = {Value}";
        }
    }
}
