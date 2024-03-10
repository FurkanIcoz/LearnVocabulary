using Microsoft.Scripting.Hosting;
using LearnVocabulary.Models;
using LearnVocabulary.Models.RequestFolder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http;
using static System.Net.WebRequestMethods;
using IronPython.Hosting;
using System.Net;
using System.Security.Cryptography.Xml;
using LearnVocabulary.ViewModels;
using OfficeOpenXml;
using NPOI.XSSF.UserModel;

namespace LearnVocabulary.Controllers
{
    public class WordController : Controller
    {
        private readonly LearnContext context;
        private readonly HttpClient httpClient;
        public WordController(LearnContext context, IHttpClientFactory httpClientFactory)
        {
            this.context = context;
            httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {

            List<Word> words = await context.Words
                           .Include(w => w.Phonetics)
                           .Include(w => w.Meanings)
                               .ThenInclude(m => m.Definitions)
                           .ToListAsync();
            return View(words);
        }

        public async Task<IActionResult> GetWordsDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var word = await context.Words
                .Include(w => w.Phonetics)
                .Include(w => w.Meanings)
                    .ThenInclude(m => m.Definitions)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (word == null)
            {
                return NotFound();
            }

            return View(word);
        }

        public async Task<IActionResult> GetWordWithTurkish()
        {
            List<UnknownWord> originalList = await context.UnknownWords
                           .Include(w => w.WordsSentences)
                           .ToListAsync();

            List<UnknownWord> sortedByLevelAscending = originalList.OrderBy(w => w.Level).ToList();
            List<UnknownWord> sortedByLevelDescending = originalList.OrderByDescending(w => w.Level).ToList();
            List<UnknownWord> sortedByDateAscending = originalList.OrderBy(w => w.WordDate).ToList();
            List<UnknownWord> sortedByDateDescending = originalList.OrderByDescending(w => w.WordDate).ToList();
            List<UnknownWord> sortedByViewsAscending = originalList.OrderBy(w => w.NumberOfViews).ToList();
            List<UnknownWord> sortedByViewsDescending = originalList.OrderByDescending(w => w.NumberOfViews).ToList();

            WordWithTurkishSorted viewModel = new WordWithTurkishSorted
            {
                OriginalList = originalList,
                SortedByLevelAscending = sortedByLevelAscending,
                SortedByLevelDescending = sortedByLevelDescending,
                SortedByDateAscending = sortedByDateAscending,
                SortedByDateDescending = sortedByDateDescending,
                SortedByViewsAscending = sortedByViewsAscending,
                SortedByViewsDescending = sortedByViewsDescending,
            };

            return View(viewModel);
        }

        public async Task<IActionResult> GetWordSentences(int? id)
        {
            var word = await context.UnknownWords
                .Include(w => w.WordsSentences)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (word == null)
            {
                return NotFound();
            }

            return View(word.WordsSentences);

        }
        
        public async Task<IActionResult> GetWordsToLearn()
        {
            var unknownWords = await context.UnknownWords.Where(w => w.HasLearned == false && w.Level == 1)
               .OrderBy(word => Guid.NewGuid())
               .Take(30)
               .ToListAsync();


            foreach (var word in unknownWords)
            {
                word.HasLearned = true;
                context.UnknownWords.Update(word);
            }
            await context.SaveChangesAsync();

            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("Sheet1");

            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("English");
            headerRow.CreateCell(1).SetCellValue("Turkish");
            headerRow.CreateCell(2).SetCellValue("Level");
            headerRow.CreateCell(3).SetCellValue("Views");
            headerRow.CreateCell(4).SetCellValue("Had Learn");

            for (int i = 0; i < unknownWords.Count; i++)
            {
                var row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(unknownWords[i].EnglistText);
                row.CreateCell(1).SetCellValue(unknownWords[i].TurkishText);
                row.CreateCell(2).SetCellValue(unknownWords[i].Level);
                row.CreateCell(3).SetCellValue(unknownWords[i].NumberOfViews);
                row.CreateCell(4).SetCellValue(unknownWords[i].HasLearned);

                sheet.AutoSizeColumn(0);
                sheet.AutoSizeColumn(1);

                
            }

            using(var memoryStream = new MemoryStream())
            {
                workbook.Write(memoryStream);
                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LearnWords.xlsx");
            }

            
        }

        [HttpGet]
        public async Task<IActionResult> UpdateWord(int? id)
        {
            var word = await context.UnknownWords.FindAsync(id);
            if (word == null)
            {
                return NotFound();
            }
            return View(word);

        }
        [HttpPost]
        public async Task<IActionResult> UpdateWord(UnknownWord word)
        {
            var newWord = await context.UnknownWords.FindAsync(word.Id);

            newWord.TurkishText = word.TurkishText;

            context.UnknownWords.Update(newWord);
            await context.SaveChangesAsync();
            return RedirectToAction("GetWordWithTurkish");

        }



        [HttpGet]
        public async Task<IActionResult> TranslateWord()
        {

            //var translateWords = await context.UnknownWords.Where(w => w.Level > 1 || w.HasLearned == true).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
            var translateWords = await context.UnknownWords.Where(w => w.HasLearned ==true).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();


            var word = await context.UnknownWords.OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();

            if (word == null)
            {
                UnknownWord nullWord = new UnknownWord()
                {
                    EnglistText = "Please Add Word First",
                    Level = 1,
                    TurkishText = "safs",
                };
                return View(nullWord);
            }
            translateWords.NumberOfViews += 1;
            context.UnknownWords.Update(translateWords);
            await context.SaveChangesAsync();
            return View(translateWords);
        }

        [HttpPost]
        public async Task<IActionResult> TranslateWord(UnknownWord unknownWord)
        {
            var word = await context.UnknownWords.FindAsync(unknownWord.Id);

            if (unknownWord.TurkishText.IsNullOrEmpty())
            {
                return BadRequest("Bos Birakmayiniz");
            }
            else if (unknownWord.TurkishText == "yyy" || !(word.TurkishText == unknownWord.TurkishText))

            {
                if (word.Level > 1)
                {
                    word.Level -= 1;
                }
                context.UnknownWords.Update(word);
                await context.SaveChangesAsync();
                word = await context.UnknownWords.OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();

                return RedirectToAction("TranslateWord");
            }
            else
            {
                if (word.Level < 100)
                {
                    
                    word.Level += 1;
                }
                if (word.Level == 100)
                {

                    var wordSentences = context.WordsSentences.Where(ws => ws.UnknownWordId == unknownWord.Id).ToList();
                    context.WordsSentences.RemoveRange(wordSentences);
                    context.UnknownWords.Remove(word);
                    await context.SaveChangesAsync();
                }
                context.UnknownWords.Update(word);
                await context.SaveChangesAsync();

                return RedirectToAction("AddSentence", word);


            }

        }

        [HttpGet]
        public async Task<IActionResult> AddSentence(UnknownWord word)
        {
            if (word != null)
            {
                var model = new SentenceViewModel
                {
                    UnknownWordId = word.Id,
                    UnknownWordforSentence = word.EnglistText
                };
                return View(model);

            }
            return NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> AddSentence(SentenceViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Sentence.IsNullOrEmpty())
                {
                    var wordd = await context.UnknownWords.FindAsync(model.UnknownWordId);

                    return View(wordd);
                }
                var sentence = new WordsSentence
                {
                    Sentence = model.Sentence,
                    UnknownWordId = model.UnknownWordId
                };
                context.WordsSentences.Add(sentence);
                await context.SaveChangesAsync();

                return RedirectToAction("TranslateWord");
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(WordNameReq wordNameReq)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool isWordAny = await context.Words.AnyAsync(w => w.WordText == wordNameReq.Text);
            if (wordNameReq.Text.IsNullOrEmpty())
            {
                return BadRequest("Kelime Bos Olamaz");
            }
            else if (isWordAny)
            {
                return BadRequest("Kelime Zaten Var");
            }
            else
            {
                try
                {
                    var apiTask = FetchApiWordAsync(wordNameReq.Text);
                    var translationTask = TranslateAsync(wordNameReq.Text);

                    await Task.WhenAll(apiTask, translationTask);

                    var apiResult = await apiTask;
                    var translationResult = await translationTask;

                    var jsonArray = JArray.Parse(apiResult);

                    var firstElement = jsonArray.First;

                    var jsonArrayTrans = JObject.Parse(translationResult);
                    var translatedText = jsonArrayTrans["responseData"]?["translatedText"].ToString();
                    if (translatedText != null && !string.IsNullOrWhiteSpace(translatedText.ToString()) && translatedText.ToString() != ".")
                    {
                        translatedText = translatedText.ToString();
                    }
                    else
                    {
                        var matches = jsonArrayTrans["matches"];
                        if (matches != null && matches.Any())
                        {
                            var firstMatch = matches.First;
                            var translationFromMatches = firstMatch["translation"]?.ToString();
                            if (string.IsNullOrEmpty(translationFromMatches) || translationFromMatches == ".")
                            {
                                firstMatch = matches.First.Next;
                                translationFromMatches = firstMatch["translation"]?.ToString();
                                if (!string.IsNullOrEmpty(translationFromMatches) || translationFromMatches != ".")
                                {
                                    translatedText = translationFromMatches;
                                }
                                translatedText = translationFromMatches;
                            }
                            else
                            {
                                translatedText = "Not Found";

                            }
                        }
                        else
                        {
                            translatedText = "Not Found";
                        }
                    }
                    translatedText = new string(translatedText
                    .Where(c => c != '!' || c != '-' || c != '.' || c != ',')
                    .ToArray());
                    var wordFromApi = JsonConvert.DeserializeObject<Word>(firstElement.ToString());

                    UnknownWord unknownWord = new UnknownWord();
                    unknownWord.EnglistText = wordNameReq.Text;
                    unknownWord.Level = 1;
                    unknownWord.WordDate = DateTime.Now;
                    unknownWord.TurkishText = translatedText.ToLower();
                    context.UnknownWords.Add(unknownWord);

                    if (wordFromApi.Phonetic.IsNullOrEmpty())
                    {
                        wordFromApi.Phonetic = " ";
                    }

                    wordFromApi.WordText = wordNameReq.Text;
                    await context.LicanseInfos.AddAsync(wordFromApi.License);
                    if (isWordAny)
                    {
                        return BadRequest("Kelime Zaten Var");
                    }
                    await context.Words.AddAsync(wordFromApi);

                    var meanings = wordFromApi.Meanings;

                    foreach (var meaning in meanings)
                    {
                        await context.Meanings.AddAsync(meaning);

                        foreach (var definition in meaning.Definitions)
                        {

                            await context.Definitions.AddAsync(definition);


                        }

                    }


                    foreach (var phonetic in wordFromApi.Phonetics)
                    {
                        if (phonetic.Text.IsNullOrEmpty())
                        {
                            phonetic.Text = " ";
                            await context.Phonetics.AddAsync(phonetic);
                        }
                        else
                        {
                            await context.Phonetics.AddAsync(phonetic);
                        }
                    }

                    await context.SaveChangesAsync();
                    stopwatch.Stop();
                    Debug.Print(stopwatch.Elapsed.ToString());
                    return RedirectToAction("Create");

                }
                catch (HttpRequestException ex)
                {

                    return BadRequest("HTTP istegi sirasinda hata olustu: " + ex.Message);
                }


            }
        }


        private async Task<string> FetchApiWordAsync(string word)
        {
            var apiUrl = $"https://api.dictionaryapi.dev/api/v2/entries/en/{word}";
            var responsebody = await httpClient.GetAsync(apiUrl);

            if (responsebody.IsSuccessStatusCode)
            {
                var response = await responsebody.Content.ReadAsStringAsync();
                return response;

            }
            else
            {
                var statusCode = (int)responsebody.StatusCode;
                throw new HttpRequestException($"Status code: {statusCode}");
            }
        }
        private async Task<string> TranslateAsync(string word)
        {
            var apiUrl = $"https://api.mymemory.translated.net/get?q={word}&langpair=en|tr";
            var responseBody = await httpClient.GetAsync(apiUrl);
            if (responseBody.IsSuccessStatusCode)
            {
                var response = await responseBody.Content.ReadAsStringAsync();
                return response;

            }
            else
            {
                var statusCode = (int)responseBody.StatusCode;
                throw new HttpRequestException($"Status code: {statusCode}");
            }
        }
    }
}
