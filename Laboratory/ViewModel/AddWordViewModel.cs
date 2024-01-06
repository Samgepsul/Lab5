using GalaSoft.MvvmLight.Command;
using Laboratory.Model;
using Laboratory.Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.ViewModel
{
    public class AddWordViewModel : BaseViewModel
    {
        private Word _word;

        public Word Word
        {
            get => _word;
            set
            {
                _word = value;
                OnProperty("Word");
            }
        }

        public AddWordViewModel()
        {
            Word = new Word();
        }

        private void Add()
        {
            if(Word.Root == "")
            {
                Message("Не введен корень слова");
            }

            Word.Full = Word.Prefix + "" + Word.Root + "" + Word.Suffix;

            Close();
        }

        public RelayCommand AddCommand => new RelayCommand(Add);
    }
}
