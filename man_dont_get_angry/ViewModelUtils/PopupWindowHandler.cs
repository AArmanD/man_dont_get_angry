using man_dont_get_angry.Models;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace man_dont_get_angry.ViewModelUtils
{
    /// <summary>
    /// Class which contains functions to Handle popup windows for opening/saving xml files
    /// </summary>
    public static class PopupWindowHandler
    {
        /// <summary>
        /// Creates an open file dialog, opens specified file and loads content into the game
        /// </summary>
        /// <param name="gameManager">Instance of gameManager for calling specific methods</param>
        public static void HandleOpenFile(GameManager gameManager) 
        {
            // stop auto player thread
            gameManager.TheAutoPlayerThreadManager.SetAutoThread(false);

            // create open file dialog for xml files
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse XML Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "xml",
                Filter = "XML files (*.xml)|*.xml",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            // show open dialog
            bool? result = openFileDialog1.ShowDialog();

            // check whether a xml file was chosen
            if (result ?? false)
            {
                var filePath = openFileDialog1.FileName;

                TextReader? reader = null;
                try
                {
                    XmlSerializer? deserializer = new XmlSerializer(typeof(GameManager));
                    reader = new StreamReader(filePath);
                    object? obj = deserializer.Deserialize(reader);
                    GameManager? xmlData = (GameManager?)obj;

                    if (xmlData != null)
                        gameManager.LoadGame(xmlData);
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    string title = "Error";
                    MessageBox.Show(message, title);

                    // start auto player thread
                    gameManager.TheAutoPlayerThreadManager.SetAutoThread(true);
                }
                finally
                { 
                    if (reader != null)
                        reader.Close();
                }
            }
            else
            {
                // start auto player thread
                gameManager.TheAutoPlayerThreadManager.SetAutoThread(true);
            }

        }

        /// <summary>
        /// Creates an save file dialog, saves game state into a xml file
        /// </summary>
        /// <param name="gameManager">Instance of gameManager for calling specific methods</param>
        public static void HandleSaveFile(GameManager gameManager)
        {

            // stop auto player thread
            gameManager.TheAutoPlayerThreadManager.SetAutoThread(false);

            // create save file dialog for xml files
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                InitialDirectory = @"C:\",
                Title = "Save Game State as XML",

                DefaultExt = "xml",
                Filter = "XML files (*.xml)|*.xml",

                FilterIndex = 2,
                RestoreDirectory = true,
            };

            // show open dialog
            bool? result = saveFileDialog.ShowDialog();

            // check whether a xml file was chosen
            if (result ?? false)
            {
                var filePath = saveFileDialog.FileName;
                TextWriter? writer = null;
                try
                {
                    XmlSerializer x = new XmlSerializer(gameManager.GetType());
                    writer = new StreamWriter(filePath, false);
                    x.Serialize(writer, gameManager);
                }
                finally
                {
                    if (writer != null)
                        writer.Close();
                }
            }

            // start auto player thread
            gameManager.TheAutoPlayerThreadManager.SetAutoThread(true);
        }
    }
}
