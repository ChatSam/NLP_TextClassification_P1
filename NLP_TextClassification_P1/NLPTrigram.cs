using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NLP_TextClassification_P1
{
    class NlpTrigram
    {
        private static Dictionary<string, int> _trigramFrequencies; 

        static void Main(string[] args)
        {
            string testCorpus = "Your support is vital to the work we so and we thank you for it.We also respect \n your " +
                                "privacy and would like to assure you that compassion in World farming fully complies with \n" +
                                "the Data Protection Act(1998).";

            string testCorpus1 = "The and and the";

            _trigramFrequencies = new Dictionary<string, int>();

            Console.WriteLine("\t \t ------ Trigram Frequency Calculator ------ \t \t");
            
            Console.WriteLine("Insert the path of a file to calculate the Trigram frequencies ...");

            string filePath = Console.ReadLine();
            
            string corpus = GetTextFromFile(filePath);

            Console.WriteLine("Calculating...");

            CalculateTrigramFrequency(ref corpus);

            string saveResponse = Console.ReadLine();

            if (saveResponse != null && saveResponse.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                WriteDataToFile(ref _trigramFrequencies, filePath);

                Console.WriteLine("Saved");
            }

        }

        private static string GetTextFromFile(string path)
        {
            string corpus = null;
            try
            {
                string fullPath = Path.GetFullPath(path);

                corpus = File.ReadAllText(fullPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return corpus;
        }

        private static void CalculateTrigramFrequency(ref string refCorpus)
        {
            string corpus = refCorpus.Replace('\n',' ');

            if (corpus != null)
            {
                int corpusLength = corpus.Length;

                int trigramLength = 3;

                //iterating through 3 characters in the corpus at a time
                for (int i = 0; i < corpusLength-2; i++)
                {
                    string trigram = corpus.Substring(i, trigramLength);

                    bool trigramExists = _trigramFrequencies.ContainsKey(trigram);

                    if (trigramExists)
                    {
                        int trigramCount;

                        _trigramFrequencies.TryGetValue(trigram, out trigramCount);

                        _trigramFrequencies[trigram] = trigramCount+1;
                    }
                    else
                    {
                        _trigramFrequencies.Add(trigram, 1);
                    }
                }

               
                //Priniting out the trigram frequencies
                foreach (var trigramEntry  in _trigramFrequencies)
                {
                    Console.WriteLine("Trigram: "+trigramEntry.Key+ " count: "+ trigramEntry.Value);
                }

                Console.WriteLine("--- Completed ---");

                Console.WriteLine("Do you want save the data (Y/N) ? press any other character to quit. ");
            }

        }

        private static void WriteDataToFile(ref Dictionary<string, int> trigramFrequencies, string filePath)
        {
            Dictionary<string, int> trigramFrequenciesCopy = trigramFrequencies;

                string headingText = "Trigram Frequencies" + Environment.NewLine;

                StringBuilder appendText = new StringBuilder();

                foreach (var trigramEntry in trigramFrequenciesCopy)
                {
                    appendText.Append( "Trigram : " + trigramEntry.Key + " count : " + trigramEntry.Value +
                                 Environment.NewLine);

                    
                }

            string text = appendText.ToString();
            //File.WriteAllText(filePath, headingText + appendText);

            string saveFilePath = filePath.Substring(0, filePath.Length - 4) + "_Trigram_Frequencies.txt";

            if (File.Exists(saveFilePath))
            { 
                try
                {
                    File.Delete(saveFilePath);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }

            // Create the file.
            using (var fs =File.Create(saveFilePath))
            {
                try
                {
                    //File.WriteAllText(saveFilePath, headingText + text);
                }
                catch (Exception e)
                {
                 Console.WriteLine(e.Message);
                }
                Byte[] info = new UTF8Encoding(true).GetBytes(headingText + text);
                
                // Add information to the file.
                fs.Write(info, 0, info.Length);

            }
        }
    }
}
