using CommunityToolkit.Mvvm.ComponentModel;

namespace NotesMaui.Models;

public class Note
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdateDate { get; set; }

}