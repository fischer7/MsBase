namespace Fischer.Core.Infraestructure.Queues.NewEmail.EmailInputs;
public sealed record UserSuccessfullyRegisteredEmailInput : EmailInput
{
    public UserSuccessfullyRegisteredEmailInput(
        string to,
        string userName)
        : base(to, EmailKind.UserSuccessfullyRegistered)
    {
        UserName = userName;
    }

    public string UserName { get; set; }
}