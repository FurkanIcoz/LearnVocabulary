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

namespace LearnVocabulary.Controllers
{
    public class WordController : Controller
    {
        private readonly LearnContext context;
        //private readonly IHttpClientFactory httpClientFactory;
        private readonly HttpClient httpClient;
        public WordController(LearnContext context, IHttpClientFactory httpClientFactory)
        {
            this.context = context;
            httpClient = httpClientFactory.CreateClient();
            //this.httpClientFactory = httpClientFactory.CreateClient();
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
            List<UnknownWord> words = await context.UnknownWords
                           .Include(w => w.WordsSentences)
                           .ToListAsync();
            return View(words);
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





        [HttpGet]
        public async Task<IActionResult> TranslateWord()
        {
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

            return View(word);
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


        //[HttpPost]
        //public async Task<IActionResult> Create(WordNameReq wordNameReq)
        //{
        //    Stopwatch stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    bool isWordAny = await context.Words.AnyAsync(w => w.WordText == wordNameReq.Text);
        //    if (wordNameReq.Text.IsNullOrEmpty())
        //    {
        //        return BadRequest("Kelime Bos Olamaz");
        //    }
        //    else if (isWordAny)
        //    {
        //        return BadRequest("Kelime Zaten Var");
        //    }
        //    else
        //    {
        //        //var httpClient = httpClientFactory.CreateClient();
        //        var apiUrl = $"https://api.dictionaryapi.dev/api/v2/entries/en/{wordNameReq.Text}";

        //        try
        //        {

        //            var responsebody = await httpClient.GetAsync(apiUrl);
        //            if (responsebody.IsSuccessStatusCode)
        //            {
        //                var response = await responsebody.Content.ReadAsStringAsync();
        //                var jsonArray = JArray.Parse(response);


        //                var firstElement = jsonArray.First;
        //                //response = response.Substring(1);
        //                //response = response.Substring(0, response.Length - 1);

        //                var apiUrlTrans = $"https://api.mymemory.translated.net/get?q={wordNameReq.Text}!&langpair=en|tr";
        //                var responseTrans = await httpClient.GetStringAsync(apiUrlTrans);

        //                var jsonArrayTrans = JObject.Parse(responseTrans);
        //                var translatedText = jsonArrayTrans["responseData"]?["translatedText"].ToString();
        //                if (translatedText != null && !string.IsNullOrWhiteSpace(translatedText.ToString()) && translatedText.ToString() != ".")
        //                {
        //                    translatedText = translatedText.ToString();
        //                }
        //                else
        //                {
        //                    var matches = jsonArrayTrans["matches"];
        //                    if (matches != null && matches.Any())
        //                    {
        //                        var firstMatch = matches.First;
        //                        var translationFromMatches = firstMatch["translation"]?.ToString();
        //                        if (string.IsNullOrEmpty(translationFromMatches) || translationFromMatches == ".")
        //                        {
        //                            firstMatch = matches.First.Next;
        //                            translationFromMatches = firstMatch["translation"]?.ToString();
        //                            if (!string.IsNullOrEmpty(translationFromMatches) || translationFromMatches != ".")
        //                            {
        //                                translatedText = translationFromMatches;
        //                            }
        //                            translatedText = translationFromMatches;
        //                        }
        //                        else
        //                        {
        //                            translatedText = "Not Found";

        //                        }
        //                    }
        //                    else
        //                    {
        //                        translatedText = "Not Found";
        //                    }
        //                }
        //                translatedText = new string(translatedText
        //                .Where(c => c != '!' || c != '-' || c != '.' || c != ',')
        //                .ToArray());
        //                var wordFromApi = JsonConvert.DeserializeObject<Word>(firstElement.ToString());

        //                UnknownWord unknownWord = new UnknownWord();
        //                unknownWord.EnglistText = wordNameReq.Text;
        //                unknownWord.Level = 1;
        //                unknownWord.WordDate = DateTime.Now;
        //                unknownWord.TurkishText = translatedText.ToLower();
        //                context.UnknownWords.Add(unknownWord);

        //                if (wordFromApi.Phonetic.IsNullOrEmpty())
        //                {
        //                    wordFromApi.Phonetic = " ";
        //                }

        //                wordFromApi.WordText = wordNameReq.Text;
        //                await context.LicanseInfos.AddAsync(wordFromApi.License);
        //                if (isWordAny)
        //                {
        //                    return BadRequest("Kelime Zaten Var");
        //                }
        //                await context.Words.AddAsync(wordFromApi);

        //                var meanings = wordFromApi.Meanings;

        //                foreach (var meaning in meanings)
        //                {
        //                    await context.Meanings.AddAsync(meaning);

        //                    foreach (var definition in meaning.Definitions)
        //                    {
        //                        await context.Definitions.AddAsync(definition);

        //                    }

        //                }


        //                foreach (var phonetic in wordFromApi.Phonetics)
        //                {
        //                    if (phonetic.Text.IsNullOrEmpty())
        //                    {
        //                        phonetic.Text = " ";
        //                        await context.Phonetics.AddAsync(phonetic);
        //                    }
        //                    else
        //                    {
        //                        await context.Phonetics.AddAsync(phonetic);
        //                    }
        //                }


        //                await context.SaveChangesAsync();
        //                stopwatch.Stop();
        //                Debug.Print(stopwatch.Elapsed.ToString());
        //                return View();
        //            }
        //            else if (responsebody.StatusCode == System.Net.HttpStatusCode.NotFound)
        //            {
        //                return BadRequest("Kelime Bulunamadi Hata: " + responsebody.StatusCode);
        //            }
        //            else
        //            {
        //                return BadRequest("Hata Olustu Hata: " + responsebody.StatusCode);
        //            }


        //        }
        //        catch (HttpRequestException ex)
        //        {

        //            return BadRequest("HTTP istegi sirasinda hata olustu: " + ex.Message);
        //        }


        //    }
        //}

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
