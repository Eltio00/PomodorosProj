public static class TextNormalizer
{
    // Collapses line-wrap newlines into spaces, so SentenceSplitter
    // (which treats \n as a sentence delimiter) doesn't break sentences
    // mid-way just because the PDF wrapped a line at that point.
    public static string NormalizeForChunking(string text)
    {
        // Normalize all line-ending styles to a single \n first,
        // so stray \r characters don't survive and still trigger
        // SentenceSplitter's \r delimiter.
        string unified = text.Replace("\r\n", "\n").Replace("\r", "\n");

        // Now collapse single newlines (mid-sentence line wraps) into a space,
        // while preserving double newlines (paragraph breaks)
        string normalized = System.Text.RegularExpressions.Regex.Replace(unified, @"(?<!\n)\n(?!\n)", " ");

        // Collapse multiple spaces into one
        normalized = System.Text.RegularExpressions.Regex.Replace( normalized, @"(?<=(?:€|Euro|EUR)\s?)(\d{1,3}(?:\.\d{3})+)\b", m => m.Value.Replace(".", ""));

        return normalized.Trim();
    }
}