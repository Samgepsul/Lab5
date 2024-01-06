using Laboratory.Model.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Laboratory.Model
{
    public class WordDictionary
    {
        private ObservableCollection<Word> words = new ObservableCollection<Word>();
        private ChangeDB _change;

        public WordDictionary()
        {
            _change = new ChangeDB();
            words = _change.GetWords();
        }

        public void Add(Word word)
        {
            if (word != null)
            {            
                word.ID = words.Count == 0 ? 1 : (words.Last().ID + 1);
                words.Add(word);
                _change.Add(word);
            }
        }

        public IEnumerable<Word> FindByRoot(string root)
        {
            return words.Where(w => root.ToLower().Contains(w.Root.ToLower())).ToList();
        }

        private string GetNameDoc()
        {
            DateTime _date = DateTime.Now;
            return $"{_date.Day} {_date.Month} {_date.Year} {_date.Hour} {_date.Minute} {_date.Second}";
        }
 
    }
}
