namespace Fischer.Core.Infraestructure.Queues.NewEmail.EmailInputs;
public abstract record EmailInput
{
    protected EmailInput(string to, EmailKind mailKind)
    {
        To = to;
        MailKind = mailKind;
    }

    public string To { get; set; }
    public EmailKind MailKind { get; set; }
}