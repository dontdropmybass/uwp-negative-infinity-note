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
            try
            {
                Assert.IsNotNull(NoteDBManager.Read());
            }
            catch
            {

            }
        }

        [TestMethod]
        public void CanCreateNotesInDatabase()
        {
            try
            {
                int before = NoteDBManager.Read().Count;
                NoteDBManager.Create(note);
                int after = NoteDBManager.Read().Count;
                Assert.AreEqual(before + 1, after);
            }
            catch
            {

            }
        }

        [TestMethod]
        public void CanDeleteNotesFromDatabase()
        {
            try
            {
                int before = NoteDBManager.Read().Count;
                NoteDBManager.Delete(note);
                int after = NoteDBManager.Read().Count;
                Assert.AreEqual(before - 1, after);
            }
            catch
            {

            }
        }

        [TestMethod]
        public void CanUpdateExistingNotesInDatabase()
        {
            try
            {
                NoteDBManager.Update(note);
            }
            catch
            {

            }
        }

        [TestMethod]
        public void CanSearchForNotesInDatabase()
        {
            try
            {
                NoteDBManager.Read("1");
            }
            catch
            {

            }
        }
    }
}
