using GalaSoft.MvvmLight.Command;
using Laboratory.Model;
using Laboratory.Model.DB;
using Laboratory.View;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Laboratory.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private ChangeDB _changeDB;
        private ObservableCollection<Word> _allWord;
        private ObservableCollection<Word> _words;
        //private ObservableCollection<string> _root;
        private Word _selecWord;
        //private string _selectRoot;
        private string _findRoot;

        public ObservableCollection<Word> AllWord
        {
            get => _allWord;
            set
            {
                _allWord = value;
                OnProperty("AllWord");
            }
        }
        public ObservableCollection<Word> Words
        {
            get => _words;
            set
            {
                _words = value;
                OnProperty("Words");
            }
        }
        //public ObservableCollection<string> Root
        //{
        //    get => _root;
        //    set
        //    {
        //        _root = value;
        //        OnProperty("Root");
        //    }
        //}

        public string FindRoot
        {
            get => _findRoot;
            set
            {
                _findRoot = value;
                Words = new ObservableCollection<Word>(AllWord.Where(x => x.Root.ToLower().Contains(value.ToLower())).ToList());
                OnProperty("FindRoot");
            }
        }

        public Word SelectcWord
        {
            get => _selecWord;
            set
            {
                _selecWord = value;
                OnProperty("SelectcWord");
            }
        }
        //public string SelectRoot
        //{
        //    get => _selectRoot;
        //    set
        //    {
        //        _selectRoot = value;
        //        Words = new ObservableCollection<Word>(_allWord.Where(x => x.Root == value));
        //        OnProperty("SelectRoot");
        //    }
        //}

        public MainViewModel()
        {
            _changeDB = new ChangeDB();
            AllWord = _changeDB.GetWords();
            Words = AllWord;
        }

        public void RemoveWord()
        {
            Word temp = SelectcWord;
            AllWord.Remove(temp);
            _changeDB.DeleteWords(temp);      

            if (FindRoot == null)
                Words = AllWord;
            else Words = new ObservableCollection<Word>(AllWord.Where(x => x.Root.ToLower().Contains(FindRoot.ToLower())).ToList());
        }

        private void Add()
        {
            AddWord add = new AddWord();
            add.ShowDialog();

            Word temp = add.Model.Word;

            if(temp != null)
            {
                if(temp.Root != "")
                {
                    temp.ID = AllWord.Count() == 0 ? 1 : AllWord.Last().ID + 1;
                    _changeDB.Add(temp);

                    AllWord.Add(temp);

                    if (FindRoot == null)
                        Words = AllWord;
                    else Words = new ObservableCollection<Word>(AllWord.Where(x => x.Root.ToLower().Contains(FindRoot.ToLower())).ToList());
                }
            }
        }

        private void ImportJSON()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "JSON (*.json)|*.json";


            if(open.ShowDialog() == true)
            {
                string path = open.FileName;
                string value = File.ReadAllText(path);
                List<Word> temp = JsonConvert.DeserializeObject<List<Word>>(value);
                int count = 0;

                temp.ForEach(x =>
                {
                    if (ContainsWord(x.Full) == false)
                    {
                        x.ID = AllWord.Count() == 0 ? 1 : (AllWord.Last().ID + 1);
                        AllWord.Add(x);
                        _changeDB.Add(x);
                        count++;
                    }
                });


                Message($"Добавленно {count} записей");
            }
        }
        private void ImportXML()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "XML (*.xml)|*.xml";

            if(open.ShowDialog() == true)
            {
                string path = open.FileName;

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Word>));

                List<Word> temp = new List<Word>();

                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    temp = xmlSerializer.Deserialize(fs) as List<Word>;
                }

                int count = 0;

                if (temp != null)
                    temp.ForEach(x =>
                    {
                        if (ContainsWord(x.Full) == false)
                        {
                            x.ID = AllWord.Count() == 0 ? 1 : (AllWord.Last().ID + 1);
                            AllWord.Add(x);
                            _changeDB.Add(x);
                            count++;
                        }
                    });


                Message($"Добавленно {count} записей");
            }
        }
        private void ExportJson()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "JSON (*.json)|*.json";

            string json = JsonConvert.SerializeObject(AllWord);

            if (save.ShowDialog() == true)
            {
                File.WriteAllText(save.FileName, json);
            }    
        }
        private void ExportXML()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Word>));

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "XML (*.xml)|*.xml";

            if (save.ShowDialog() == true)
            {
                using (var writer = new StreamWriter(save.FileName))
                {
                    xmlSerializer.Serialize(writer, AllWord.ToList());
                }
            }        
        }

        private bool ContainsWord(string word)
        {
            List<Word> temp = AllWord.ToList().Where(x => x.Full.ToLower() == word).ToList();

            if (temp.Count() == 0)
                return false;
            else return true;
        }

        public RelayCommand AddCommand => new RelayCommand(Add);
        public RelayCommand ExportJSONCommand => new RelayCommand(ExportJson);
        public RelayCommand ExportXMLCommand => new RelayCommand(ExportXML);
        public RelayCommand ImportJSONCommand => new RelayCommand(ImportJSON);
        public RelayCommand ImportXMLCommand => new RelayCommand(ImportXML);
    }
}
