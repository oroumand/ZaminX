namespace ZaminX.BuildingBlocks.CrossCutting.Translator.Parrot.Exceptions;


public sealed class ParrotConfigurationException(string message) : Exception(message)
{
}