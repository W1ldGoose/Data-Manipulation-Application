using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Task6.Model
{
    // Для возможности изменения объектов и автоматического обновления связанных элементов 
    // управления необходимо реализовать интерфейс INotifyPropertyChanged
    class MyBook : INotifyPropertyChanged
    {
        public MyBook()
        {
        }

        public MyBook(string name)
        {
            Name = name;
        }

        public MyBook(string name, string author)
        {
            Name = name;
            Author = author;
        }
       
        public MyBook(string name, string author, string year)
        {
            Name = name;
            Author = author;
            Year = year;
        }
        public MyBook(string name, string author, string year, string label)
        {
            Name = name;
            Author = author;
            Year = year;
            Label = label;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
       
        private string name = "Не определено";
        public string Name
        {
            get { return name; }
            set
            {
                if (!(String.IsNullOrEmpty(value)) && Regex.IsMatch(value, @"^[а-яА-ЯA-Za-z0-9\s\-_,.\?]+$"))
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
                else
                {
                    name = "Не определено";
                    OnPropertyChanged("Name");
                }
            }
        }
        private string author = "Не определено";
        public string Author
        {
            get { return author; }
            set
            {
                if (!(String.IsNullOrEmpty(value)) && Regex.IsMatch(value, @"^[a-zA-ZА-Яа-я\s\.]+$"))
                {
                    author = value;
                    OnPropertyChanged("Author");
                }
                else
                {
                    author = "Не определено";
                    OnPropertyChanged("Author");
                }
            }
        } 
        private string year;
        public string Year
        {
            get { return year; }
            set
            {
                int number;
                if (Int32.TryParse(value, out number))
                {
                    year = value;
                    OnPropertyChanged("Year");
                }
                else
                {
                    //year = ;
                    OnPropertyChanged("Year");
                }
            }
        }
        private string label = "Не определено";
        public string Label
        {
            get { return label; }
            set
            {
                if (!(String.IsNullOrEmpty(value)) && Regex.IsMatch(value, @"^[a-zA-ZА-Яа-я ]+$"))
                {
                    label = value;
                    OnPropertyChanged("Label");
                }
                else
                {
                    label = "Не определено";
                    OnPropertyChanged("Label");
                }
            }
        }
    }
}
