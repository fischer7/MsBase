namespace Fischer.Core.Application.Abstractions;
public interface IMediatorResult
{
    bool HasErrors();
    IEnumerable<string> ErrorList { get; }
    IMediatorResult AddError(string error);
    IMediatorResult AddErrors(IEnumerable<string> error);
}