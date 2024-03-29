using FluentValidation.Results;

namespace Fischer.Core.Infraestructure.Queues.Integration;
public class ResponseMessage : Message
{
    public ValidationResult ValidationResult { get; set; }

    public ResponseMessage(ValidationResult validationResult)
    {
        ValidationResult = validationResult;
    }
}