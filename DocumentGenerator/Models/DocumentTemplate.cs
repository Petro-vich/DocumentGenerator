using DevExpress.XtraRichEdit.Import.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentGenerator.Models
{
    public class DocumentTemplate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public List<string> Tags { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description {  get; set; }

        public DocumentTemplate()
        {
            Id = Guid.NewGuid().ToString();
            Tags = new List<string>();
            CreatedAt = DateTime.Now;
        }

        public DocumentTemplate(string name, string content) : this()
        {
            Name = Name;
            Content = content;
        }
    }
}
