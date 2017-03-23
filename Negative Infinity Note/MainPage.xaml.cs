using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls; 

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Negative_Infinity_Note
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string Filter;
        private bool _isNew = false;
        Note CurrentNote;
        ObservableCollection<Note> Notes;

        public MainPage()
        {
            Filter = "";
            InitializeComponent();
            Notes = NoteDBManager.Read(Filter);
            NoteList.ItemsSource = Notes;
            NoteList.SelectionChanged += NoteList_SelectionChanged;
            EditButton.IsEnabled = false;
            SaveButton.IsEnabled = false;
            DeleteButton.IsEnabled = false;
            editor.IsEnabled = false;
            BoldButton.IsEnabled = false;
            ItalicButton.IsEnabled = false;
            UnderlineButton.IsEnabled = false;
        }

        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = editor.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Bold = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = editor.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Italic = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = editor.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                if (charFormatting.Underline == Windows.UI.Text.UnderlineType.None)
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.Single;
                }
                else
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.None;
                }
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NoteList.SelectedItem = null;
            editor.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, "");
            editor.IsEnabled = false;
            CurrentNote = new Note();
            EditButton.IsEnabled = true;
            _isNew = true;
            BoldButton.IsEnabled = false;
            ItalicButton.IsEnabled = false;
            UnderlineButton.IsEnabled = false;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            editor.IsEnabled = true;
            EditButton.IsEnabled = false;
            SaveButton.IsEnabled = true;
            BoldButton.IsEnabled = true;
            ItalicButton.IsEnabled = true;
            UnderlineButton.IsEnabled = true;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var saveTextBox = new TextBox { Width = 200, Height = 10 };
            SaveButton.IsEnabled = true;

            ContentDialog saveFileDialog = new ContentDialog()
            {
                Title = "Save As...",
                Content = saveTextBox,
                PrimaryButtonText = "Save",
                SecondaryButtonText = "Cancel"
            };
            if (CurrentNote != null && CurrentNote.Title != null)
            {
                saveTextBox.Text = CurrentNote.Title;
            }

            ContentDialogResult result = await saveFileDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                CurrentNote.Title = saveTextBox.Text;
                SaveCurrentNote();
                EditButton.IsEnabled = true;
                SaveButton.IsEnabled = false;
                editor.IsEnabled = false;
                BoldButton.IsEnabled = false;
                ItalicButton.IsEnabled = false;
                UnderlineButton.IsEnabled = false;
                Notes = NoteDBManager.Read(Filter);
                NoteList.ItemsSource = Notes;
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog deleteFileDialog = new ContentDialog()
            {
                Title = "Delete note "+CurrentNote.Title,
                Content = "Are you sure you want to delete "+CurrentNote.Title+"?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();

            if (result == ContentDialogResult.Primary) // TODO: replace once confirm dialog is in place
            {
                NoteDBManager.Delete(CurrentNote);
                EditButton.IsEnabled = false;
                editor.IsEnabled = false;
                DeleteButton.IsEnabled = false;
                BoldButton.IsEnabled = false;
                ItalicButton.IsEnabled = false;
                UnderlineButton.IsEnabled = false;
                
                ContentDialog deletedFileDialog = new ContentDialog()
                {
                    Title = "Deleted note " + CurrentNote.Title,
                    Content = "Deleted note " + CurrentNote.Title,
                    PrimaryButtonText = "Ok"
                };

                await deletedFileDialog.ShowAsync();

                Notes = NoteDBManager.Read(Filter);
                NoteList.ItemsSource = Notes;
                NoteList.SelectedItem = null;
                CurrentNote = null;
            }
            
        }

        private async void NoteList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentNote = NoteList.SelectedItem as Note;
            _isNew = false;
            EditButton.IsEnabled = true;
            editor.IsEnabled = false;
            SaveButton.IsEnabled = false;
            BoldButton.IsEnabled = false;
            ItalicButton.IsEnabled = false;
            UnderlineButton.IsEnabled = false;

            if (CurrentNote == null)
            {
                editor.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, "");
                DeleteButton.IsEnabled = false;
                return;
            }

            using (var memory = new InMemoryRandomAccessStream())
            {
                var dataWriter = new DataWriter(memory);
                dataWriter.WriteBytes(CurrentNote.Data);
                await dataWriter.StoreAsync();
                editor.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.FormatRtf, memory);
                DeleteButton.IsEnabled = true;
            }
        }

        private async void SaveCurrentNote()
        {
            if (CurrentNote == null) return; // TODO: alert the user that something is wrong
            
            byte[] bytes;
            using (var memory = new InMemoryRandomAccessStream())
            {
                editor.Document.SaveToStream(Windows.UI.Text.TextGetOptions.FormatRtf, memory);
                var streamToSave = memory.AsStream();
                var dataReader = new DataReader(streamToSave.AsInputStream());
                bytes = new byte[streamToSave.Length];
                await dataReader.LoadAsync((uint)streamToSave.Length);
                dataReader.ReadBytes(bytes);
                CurrentNote.Data = bytes;

                if (_isNew)
                {
                    NoteDBManager.Create(CurrentNote);
                }
                else
                {
                    NoteDBManager.Update(CurrentNote);
                }
                Notes = NoteDBManager.Read(Filter);
            }
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutPage));
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter = SearchBox.Text;
            Notes = NoteDBManager.Read(Filter);
            NoteList.ItemsSource = Notes;
        }
    }
}
