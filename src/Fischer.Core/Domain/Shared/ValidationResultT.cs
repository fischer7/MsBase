namespace Fischer.Core.Domain.Shared;

public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    private ValidationResult(List<Error> errors)
        : base(default, false, IValidationResult.ValidationError) =>
        Errors = errors;

    public List<Error> Errors { get; }

    public static ValidationResult<TValue> WithErrors(List<Error> errors) => new(errors);
}
