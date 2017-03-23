using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Negative_Infinity_Note;

namespace Negative_Infinity_Note_Test_Project
{
    [TestClass]
    public class UnitTest1
    {
        Note note;

        public UnitTest1()
        {
            note = new Note();
        }

        [TestMethod]
        public void CanReadNotesFromDatabase()
        {
            Assert.IsNotNull(NoteDBManager.Read());
        }

        [TestMethod]
        public void CanCreateNotesInDatabase()
        {
            int before = NoteDBManager.Read().Count;
            NoteDBManager.Create(note);
            int after = NoteDBManager.Read().Count;
            Assert.AreEqual(before + 1, after);
        }

        [TestMethod]
        public void CanDeleteNotesFromDatabase()
        {
            int before = NoteDBManager.Read().Count;
            NoteDBManager.Delete(note);
            int after = NoteDBManager.Read().Count;
            Assert.AreEqual(before - 1, after);
        }

        [TestMethod]
        public void CanUpdateExistingNotesInDatabase()
        {
            NoteDBManager.Update(note);
        }

        [TestMethod]
        public void CanSearchForNotesInDatabase()
        {
            NoteDBManager.Read("1");
        }
    }
}
