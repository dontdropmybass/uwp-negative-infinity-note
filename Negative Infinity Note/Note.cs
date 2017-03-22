namespace Negative_Infinity_Note
{
    public class Note
    {
        public int NoteID { get; set; }
        public string Title { get; set; }
        public byte[] Data { get; set; }

        public Note() { }

        public Note(string Title, byte[] Data)
        {
            this.Title = Title;
            this.Data = Data;
        }

        public Note(int NoteID, string Title, byte[] Data)
        {
            this.NoteID = NoteID;
            this.Title = Title;
            this.Data = Data;
        }
    }
}
