using DevExpress.XtraRichEdit;
//using DevExpress.XtraRichEdit.Model;
using System.Collections.Generic;
using DocumentGenerator.Models;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using DevExpress.XtraRichEdit.API.Native;
using System.IO;

namespace DocumentGenerator.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly RichEditDocumentServer _richEditServer;

        public DocumentService()
        {
            _richEditServer = new RichEditDocumentServer();
        }

        public DocumentModel GenerateDocument(string template, List<TagData> tagValues)
        {
            if (string.IsNullOrWhiteSpace(template))
            {
                throw new ArgumentException("Шаблон не может быть пустым", nameof(template));
            }



            try
            {
                LoadTemplate(template);

                ReplaceTags(tagValues);

                byte[] documentBytes = SaveDocumentToByteArray();

                return new DocumentModel(documentBytes);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка генерации документа: {ex.Message}", ex);
            }
        }

        public List<string> ExtractTags(string template)
        {
            if (string.IsNullOrWhiteSpace(template))
            {
                return new List<string>();
            }

            var tags = new List<string>();
            var matches = Regex.Matches(template, @"\{([^{}]+)\}");

            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                if (match.Success && match.Groups.Count > 1)
                {
                    string tagName = match.Groups[1].Value.Trim();
                    if (!string.IsNullOrEmpty(tagName) && !tags.Contains(tagName))
                    {
                        tags.Add(tagName);
                    }
                }
            }

            return tags.OrderBy(t => t).ToList();
        }

        public bool HasTags(string template)
        {
            return !string.IsNullOrWhiteSpace(template) &&
                   Regex.IsMatch(template, @"\{([^{}]+)\}");
        }

        public int GetTagsCount(string template)
        {
            int count = ExtractTags(template).Count;
            return count;
        }
        private void LoadTemplate(string template)
        {
            _richEditServer.Document.Text = ""; // How use

            _richEditServer.HtmlText = template;
        }

        private void ReplaceTags(List<TagData> tagValues)
        {
            if (tagValues == null || !tagValues.Any()) { return; }

            var document = _richEditServer.Document;

            foreach (var tagData in tagValues)
            {
                if (string.IsNullOrWhiteSpace(tagData.TagName)){
                    continue;
                }

                string searchText = "{" + tagData.TagName + "}";
                string replaceText = tagData.Value ?? string.Empty; //How use

                var foundRanges = document.FindAll(searchText, SearchOptions.None); // How use

                foreach (var range in foundRanges.Reverse())
                {
                    document.Delete(range);
                    document.InsertText(range.Start, replaceText);
                }
            }
        }

        private byte[] SaveDocumentToByteArray()
        {
            using (var stram = new MemoryStream())
            {
                _richEditServer.SaveDocument(stram, DocumentFormat.OpenXml);
                return stram.ToArray();
            }
        }

        public void Dispose()
        {
            _richEditServer?.Dispose();
        }
    }
}