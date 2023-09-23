using Fischer.Core.Domain.Shared;

namespace Fischer.Core.Domain.Shared;

public sealed class ExceptionResult<TValue> : Result<TValue>, IExceptionResult
{
    private static Error ExceptionError = new Error("Exception", "An exception has occured");
    private ExceptionResult(List<Error> errors)
        : base(default, false, ExceptionError) =>
        Errors = errors;

    public List<Error> Errors { get; }

    public static ExceptionResult<TValue> WithErrors(List<Error> errors) => new(errors);
}

public interface IExceptionResult
{
    public List<Error> Errors { get; }

}