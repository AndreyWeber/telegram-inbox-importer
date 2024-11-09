using CredentialManagement;

namespace TelegramInboxImporter;

public static class CredentialManager
{
    public static string GetTelegramPassword()
    {
        var cm = new Credential
        {
            Target = "TelegramPassword",
            Type = CredentialType.Generic
        };

        if (cm.Load())
        {
            return cm.Password;
        }

        throw new InvalidOperationException("Failed to load TelegramPassword from Windows Credential Manager");
    }
}
