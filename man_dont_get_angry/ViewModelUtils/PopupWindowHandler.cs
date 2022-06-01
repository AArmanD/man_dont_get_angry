using man_dont_get_angry.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace man_dont_get_angry.ViewModelUtils
{
    public static class PopupWindowHandler
    {
        public static void HandleOpenFile(GameManager gameManager) 
        {
            gameManager.SetAutoThread(false);
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

            bool? result = openFileDialog1.ShowDialog();

            if (result.Value)
            {
                var filePath = openFileDialog1.FileName;
                var deserializer = new XmlSerializer(typeof(GameManager));
                TextReader reader = new StreamReader(filePath);
                object obj = deserializer.Deserialize(reader);
                var xmlData = (GameManager)obj;
                reader.Close();

                gameManager.SetAutoThread(false);
                gameManager.LoadGame(xmlData);
            }
            gameManager.SetAutoThread(true);
        }

        public static void HandleSaveFile(GameManager gameManager)
        {
            gameManager.SetAutoThread(false);
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                InitialDirectory = @"C:\",
                Title = "Save Game State as XML",

                DefaultExt = "xml",
                Filter = "XML files (*.xml)|*.xml",

                FilterIndex = 2,
                RestoreDirectory = true,
            };

            bool? result = saveFileDialog.ShowDialog();

            if (result.Value)
            {
                var filePath = saveFileDialog.FileName;
                TextWriter writer = null;
                try
                {
                    XmlSerializer x = new System.Xml.Serialization.XmlSerializer(gameManager.GetType());
                    writer = new StreamWriter(filePath, false);
                    x.Serialize(writer, gameManager);
                }
                finally
                {
                    if (writer != null)
                        writer.Close();
                }
            }

            gameManager.SetAutoThread(true);
        }

        public static void HandleWinDialog(String playerName)
        {
            string message = "Player " + playerName + " has won the game";
            string title = "Game Won";

            MessageBox.Show(message, title);
        }
    }
}
