using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Webtest
{
    public class Service : IService
    {
        //the text should be stored in the database
        //...
        static string _text = @"Китообразные, в частности киты, имеют самые большие размеры 
                        среди животных: синий кит (голубой кит) во взрослом состоянии 
                        достигает средней длины тела 25 м (самые крупные — до 33 м) и 
                        массы 90–120 т. Все китообразные, включая китов, дельфинов и 
                        морских свиней, являются потомками сухопутных млекопитающих 
                        отряда парнокопытных. Согласно молекулярно-генетическим данным, 
                        китообразные являются инфраотрядом парнокопытных[5]. Более того, 
                        по этим данным бегемоты являются одними из ближайших живых 
                        родственников китов; они произошли от общего предка, жившего 
                        примерно 54 миллиона лет назад[6][7]. Киты перешли к водному 
                        образу жизни приблизительно 50 миллионов лет назад[8]. 
                        Китообразные делятся на три парвотряда:

                            Усатые киты(Mysticeti), отличающиеся усами, фильтрообразной 
                            структурой, расположенной на верхней челюсти, состоящей в 
                            основном из кератина.Ус применяется для фильтрации планктона 
                            из воды, процеживания с помощью гребенчатой структуры рта 
                            большого количества воды.Усатые являются наиболее крупным 
                            подотрядом китов.

                            Зубатые киты (Odontoceti) обладают зубами и охотятся на 
                            крупных рыб и кальмаров. Это их основной источник пищи.
                            Замечательной способностью этой группы является возможность 
                            ощущать их окружающую среду при помощи эхолокации.К зубатым 
                            относятся дельфины и морские свиньи.

                            Древние киты (Archaeoceti) — в настоящее время полностью 
                            вымершая группа.";
        static ConcurrentDictionary<string, int> _dicText = new ConcurrentDictionary<string, int>();
        static Service()
        {
            CreateDictionary(_text);
        }
        /// <summary>
        /// Create a maniac letter
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> CreateLetterAsync(string request)
        {
            List<string> letter = new List<string>();
            var res = 0;
            Regex regex = new Regex(@"[а́-яА-яA-Za-z]+-*[а́-яА-яA-Za-z]+");
            foreach (Match word in regex.Matches(request))
            {
                if (_dicText.ContainsKey(word.Value) && _dicText[word.Value] > 0)
                {
                    letter.Add(word.Value);
                    _dicText[word.Value]--;
                    if (_dicText[word.Value] == 0)
                        _dicText.TryRemove(word.Value, out res);
                }
                else
                {
                    foreach (var item in letter)
                    {
                        if (_dicText.ContainsKey(item))
                            _dicText[item]++;
                        else
                            _dicText.TryAdd(item, 1);
                    }
                }
            }
            return string.Join(" ",letter);
        }

        private static void CreateDictionary(string text)
        {
            text = text.ToLowerInvariant();
            Regex regex = new Regex(@"[а́-яА-яA-Za-z]+-*[а́-яА-яA-Za-z]+");

            foreach (Match word in regex.Matches(text))
            {
                if (_dicText.ContainsKey(word.Value))
                    _dicText[word.Value]++;
                else
                    _dicText.TryAdd(word.Value, 1);
            }
        }
    }
}