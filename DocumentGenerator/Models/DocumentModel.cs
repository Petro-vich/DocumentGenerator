using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentGenerator.Models
{
    public class DocumentModel
    {
        public byte[] Content { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public DateTime GeneratedAt { get; set; }
        public long Size
        {
            get
            {
                return Content != null ? Content.Length : 0;
            }
        }
        public string Description { get; set;}

        public DocumentModel()
        {
            GeneratedAt = DateTime.Now;
            ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; // Default to DOCX
            FileName = $"Document_{DateTime.Now:yyyyMMdd_HHmm}.docx"; 
        }
        
        public DocumentModel(byte[] content, string filename = null) : this()
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
            
            if (!string.IsNullOrEmpty(filename))
            {
                FileName = filename;
            }
        }
        
    }
}
