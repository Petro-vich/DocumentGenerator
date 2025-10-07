using DocumentGenerator.Models;
using System.Collections.Generic;

namespace DocumentGenerator.Services
{
    public interface IDocumentService
    {
        DocumentModel GenerateDocument(string template, List<TagData> tagValues);
        List<string> ExtractTags(string template);
        bool HasTags(string template);
        int GetTagsCount(string template);
    }
}
