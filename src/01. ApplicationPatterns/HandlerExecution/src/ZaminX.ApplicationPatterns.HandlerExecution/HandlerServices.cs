using Microsoft.Extensions.Logging;
using ZaminX.BuildingBlocks.CrossCutting.ObjectMapper.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Serializer.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Contracts;
using ZaminX.BuildingBlocks.IdentityAndUsers.Persona.Abstractions.Contracts;

namespace ZaminX.ApplicationPatterns.HandlerExecution;


public sealed class HandlerServices
{
    public HandlerServices(
        IMapperAdapter mapper,
        IJsonSerializer serializer,
        ITranslator translator,
        ILoggerFactory loggerFactory,
        ICurrentUser? currentUser = null)
    {
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(serializer);
        ArgumentNullException.ThrowIfNull(translator);
        ArgumentNullException.ThrowIfNull(loggerFactory);

        Mapper = mapper;
        Serializer = serializer;
        Translator = translator;
        LoggerFactory = loggerFactory;
        CurrentUser = currentUser;
    }

    public IMapperAdapter Mapper { get; }

    public IJsonSerializer Serializer { get; }

    public ITranslator Translator { get; }

    public ILoggerFactory LoggerFactory { get; }

    public ICurrentUser? CurrentUser { get; }
}