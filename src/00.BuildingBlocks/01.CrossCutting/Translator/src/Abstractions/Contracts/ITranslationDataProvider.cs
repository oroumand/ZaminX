using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Models;

namespace ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Contracts;

public interface ITranslationDataProvider
{
    Task<IReadOnlyCollection<TranslationEntry>> LoadAsync(CancellationToken cancellationToken = default);
}