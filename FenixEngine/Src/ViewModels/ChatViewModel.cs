using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FenixEngine.Src.ViewModels;

public partial class ChatViewModel : BaseViewModel
{
    [ObservableProperty]
    private string? currentMessage;

    [ObservableProperty]
    private Contact selectedContact;

    public ObservableCollection<Message> Messages { get; } = new();
    public ObservableCollection<Contact> Contacts { get; } = new();

    private readonly Random _random = new();

    public ChatViewModel()
    {
        // Simulación de contactos
        Contacts.Add(new Contact { Name = "Alice" });
        Contacts.Add(new Contact { Name = "Bob" });
        Contacts.Add(new Contact { Name = "Charlie" });

        SelectedContact = Contacts.FirstOrDefault();
    }

    [RelayCommand]
    private async Task SendMessageAsync()
    {
        if (string.IsNullOrWhiteSpace(CurrentMessage)) return;

        Messages.Add(new Message { Text = CurrentMessage, IsUser = true });

        var userText = CurrentMessage;
        CurrentMessage = string.Empty;

        Messages.Add(new Message { Text = "🤖 Pensando...", IsUser = false });
        await Task.Delay(1000);

        string[] respuestas =
        {
            $"Hola {SelectedContact?.Name}, recibí tu mensaje.",
            "Interesante lo que dices.",
            $"¿Quieres que te explique '{userText}' en detalle?",
            "👌 Recibido, procesando tu consulta."
        };

        Messages[^1] = new Message
        {
            Text = respuestas[_random.Next(respuestas.Length)],
            IsUser = false
        };
    }
}

public class Message
{
    public string? Text { get; set; }
    public bool IsUser { get; set; }
}

public class Contact
{
    public string? Name { get; set; }
}