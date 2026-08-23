using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using UnityEngine;

public static class FileConverter
{

    public static string ExtractFromPdf(string filePath)
    {
        var sb = new StringBuilder();

        using (PdfDocument document = PdfDocument.Open(filePath))
        {
            foreach (var page in document.GetPages())
            {
                // ContentOrderTextExtractor tends to handle complex layouts
                // and encodings better than the raw page.Text property
                string pageText = ContentOrderTextExtractor.GetText(page);
                sb.AppendLine(pageText);
            }
        }

        return sb.ToString();
    }
    public static string ExtractFromDocx(string filePath)
    {
        var sb = new StringBuilder();

        using (var archive = ZipFile.OpenRead(filePath))
        {
            var documentEntry = archive.GetEntry("word/document.xml");
            if (documentEntry == null)
                throw new System.Exception("Invalid .docx file: document.xml not found.");

            using (var stream = documentEntry.Open())
            {
                XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
                XDocument doc = XDocument.Load(stream);

                // Extract all text runs (<w:t> elements), preserving paragraph breaks
                foreach (var paragraph in doc.Descendants(w + "p"))
                {
                    foreach (var textNode in paragraph.Descendants(w + "t"))
                        sb.Append(textNode.Value);
                    sb.AppendLine();
                }
            }
        }

        return sb.ToString();
    }

    public static bool LooksLikeGarbage(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return true;

        string[] words = text.Split(
            new[] { ' ', '\n', '\r', '\t' },
            System.StringSplitOptions.RemoveEmptyEntries);

        if (words.Length < 5)
            return true; // too short to judge reliably, treat as suspicious

        int plausibleWords = words.Count(IsPlausibleWord);
        double plausibleRatio = (double)plausibleWords / words.Length;

        return plausibleRatio < 0.5;
    }

    static bool IsPlausibleWord(string word)
    {
        // Strip trailing punctuation for the check
        string trimmed = word.Trim('.', ',', ';', ':', '!', '?', '"', '\'', '(', ')');
        if (trimmed.Length == 0) return true; // pure punctuation, don't penalize

        if (trimmed.Length > 25) return false; // absurdly long "word" = likely garbage

        // A word made only of letters should contain at least one vowel
        // (works reasonably for Latin-alphabet languages like Italian/English)
        bool isAllLetters = trimmed.All(char.IsLetter);
        if (isAllLetters && trimmed.Length > 2)
        {
            bool hasVowel = trimmed.ToLowerInvariant().Any(c => "aeiouàèéìòù".Contains(c));
            return hasVowel;
        }

        return true; // numbers, mixed alphanumeric, non-Latin scripts: don't penalize here
    }
}
