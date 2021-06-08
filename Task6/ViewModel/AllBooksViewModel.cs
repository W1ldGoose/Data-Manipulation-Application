using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Task6.Model;

namespace Task6.ViewModel
{

    class AllBooksViewModel : INotifyPropertyChanged
    {
        // Реализация интерфейса предполагает введение события и метода генерации события
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private MyBook selectedBook;
        public MyBook SelectedBook
        {
            get { return selectedBook; }
            set
            {
                selectedBook = value;
                OnPropertyChanged("SelectedBook");
            }
        }

        public ObservableCollection<MyBook> MyBooks { get; set; }
        public AllBooksViewModel()
        {
            MyBooks = new ObservableCollection<MyBook>
            {
                new MyBook { Name="Над пропастью во ржи", Author="Дж.Д. Сэлинджер", Year="1951", Label="Эксмо" },
                new MyBook { Name="Убить пересмешника", Author="Харпер Ли", Year="1960", Label="АСТ" },
                new MyBook { Name="Мечтают ли андроиды об электроовцах?", Author="Ф.К. Дик", Year="1968", Label="Центрполиграф" },
                new MyBook { Name="Вино из одуванчиков", Author="Рэй Брэдбери", Year="1957", Label="Эксмо" }
            };
        }


        private BaseCommand addCommand;
        public BaseCommand AddCommand
        {
            get
            {
                if (addCommand != null)
                    return addCommand;
                else return addCommand = new BaseCommand(obj =>
                {
                    MyBook bk = new MyBook("Название книги", "Автор", "Год", "Издательство");
                    MyBooks.Insert(0, bk);
                    SelectedBook = bk;
                });
            }
        }

        private BaseCommand delCommand;
        public BaseCommand DelCommand
        {
            get
            {
                if (delCommand != null)
                    return delCommand;
                else
                {
                    Action<object> Execute = o =>
                    {
                        MyBook bk = (MyBook)o;
                        MyBooks.Remove(bk);
                    };
                    Func<object, bool> CanExecute = o => MyBooks.Count > 0;
                    return delCommand = new BaseCommand(Execute, CanExecute);
                }
            }
        }

        private BaseCommand saveCommand;
        public BaseCommand SaveCommand
        {
            get
            {
                if (saveCommand != null)
                    return saveCommand;
                else
                {
                    Action<object> Execute = o =>
                    {
                        SaveFileDialog opf = new SaveFileDialog();
                        opf.InitialDirectory = System.Environment.CurrentDirectory;
                        opf.Filter = "Файлы (*.xml, *.json ) | *.xml;*.json;";
                        opf.FileName = "data.xml";

                        if (opf.ShowDialog() == true)
                        {
                            // Сериализация
                            if (Path.GetExtension(opf.FileName) == ".xml")
                            {
                                // Преобразование коллекции объектов в строку в XML формате
                                XElement x = new XElement("Books",
                                    from bk in MyBooks
                                    select new XElement("Book",
                                    new XElement("Name", bk.Name),
                                    new XElement("Author", bk.Author),
                                    new XElement("Year", bk.Year),
                                    new XElement("Label", bk.Label)));
                                string s = x.ToString();
                                File.WriteAllText(opf.FileName, s);
                            }
                            else if (Path.GetExtension(opf.FileName) == ".json")
                            {
                                string s = Newtonsoft.Json.JsonConvert.SerializeObject(MyBooks, Formatting.Indented);
                                File.WriteAllText(opf.FileName, s);
                            }
                        }
                    };
                    return saveCommand = new BaseCommand(Execute);
                }
            }
        }

        private BaseCommand openCommand;
        private string TryGetElementValue(XElement parentEl, string elementName, string defaultValue = null)
        {
            var foundEl = parentEl.Element(elementName);
            if (foundEl != null)
            {
                return foundEl.Value;
            }
            return defaultValue;
        }

        public BaseCommand OpenCommand
        {
            get
            {
                if (openCommand != null)
                    return openCommand;
                else
                {
                    Action<object> Execute = o =>
                    {
                        OpenFileDialog opf = new OpenFileDialog();
                        opf.InitialDirectory = System.Environment.CurrentDirectory;
                        opf.Filter = "Файлы (*.xml, *.json ) | *.xml;*.json;";

                        if (opf.ShowDialog() == true)
                        {
                            if (Path.GetExtension(opf.FileName) == ".xml")
                            {
                                var x = XElement.Parse(File.ReadAllText(opf.FileName));

                                var books = from e in x.Elements()
                                            select new MyBook(TryGetElementValue(e, "Name"), TryGetElementValue(e, "Author"),
                                            TryGetElementValue(e, "Year"), TryGetElementValue(e, "Label"));
                                MyBooks.Clear();
                                foreach (var bk in books)
                                    MyBooks.Add(bk);
                            }
                            else if (Path.GetExtension(opf.FileName) == ".json")
                            {
                                var books = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<MyBook>>(File.ReadAllText(opf.FileName));
                                MyBooks.Clear();
                                foreach (var bk in books)
                                    MyBooks.Add(bk);
                            }
                        }
                    };
                    return openCommand = new BaseCommand(Execute);
                }
            }
        }
    }
}
