namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Models;

public sealed class LumenRuntimeContext
{
    public LumenRuntimeContext(string documentName, string documentPathPattern)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(documentName);
        ArgumentException.ThrowIfNullOrWhiteSpace(documentPathPattern);

        DocumentName = documentName;
        DocumentPathPattern = documentPathPattern;
    }

    public string DocumentName { get; }

    public string DocumentPathPattern { get; }

    public string ResolveDocumentUrl()
    {
        return DocumentPathPattern.Replace(
            "{documentName}",
            DocumentName,
            StringComparison.OrdinalIgnoreCase);
    }
}
