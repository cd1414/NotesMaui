namespace NotesMaui.Models
{
    public class Memento
    {
        string _state;

        public Memento(string state)
        {
            _state = state;
        }

        public string State => _state;
    }
}

