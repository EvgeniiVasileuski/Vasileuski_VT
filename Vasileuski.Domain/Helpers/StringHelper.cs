using System.Text;
using System.Text.RegularExpressions;

namespace Vasileuski.Domain.Helpers
{
    /// <summary>
    /// Помощник для работы со строками
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Преобразует строку в kebab-case для URL
        /// Пример: "Премьер Лига" → "premier-liga"
        /// </summary>
        public static string ToKebabCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // Транслитерация кириллицы в латиницу
            var transliterated = Transliterate(input);

            // Удаляем все не-буквенно-цифровые символы, заменяем пробелы на дефисы
            var normalized = Regex.Replace(transliterated, @"[^a-zA-Z0-9\s-]", "");
            normalized = Regex.Replace(normalized, @"\s+", "-");
            normalized = Regex.Replace(normalized, @"-+", "-");

            return normalized.ToLower();
        }

        /// <summary>
        /// Простая транслитерация кириллицы в латиницу
        /// </summary>
        private static string Transliterate(string text)
        {
            var transliterationMap = new Dictionary<char, string>
            {
                {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "g"}, {'д', "d"},
                {'е', "e"}, {'ё', "yo"}, {'ж', "zh"}, {'з', "z"}, {'и', "i"},
                {'й', "y"}, {'к', "k"}, {'л', "l"}, {'м', "m"}, {'н', "n"},
                {'о', "o"}, {'п', "p"}, {'р', "r"}, {'с', "s"}, {'т', "t"},
                {'у', "u"}, {'ф', "f"}, {'х', "kh"}, {'ц', "ts"}, {'ч', "ch"},
                {'ш', "sh"}, {'щ', "sch"}, {'ъ', ""}, {'ы', "y"}, {'ь', ""},
                {'э', "e"}, {'ю', "yu"}, {'я', "ya"},
                {'А', "A"}, {'Б', "B"}, {'В', "V"}, {'Г', "G"}, {'Д', "D"},
                {'Е', "E"}, {'Ё', "Yo"}, {'Ж', "Zh"}, {'З', "Z"}, {'И', "I"},
                {'Й', "Y"}, {'К', "K"}, {'Л', "L"}, {'М', "M"}, {'Н', "N"},
                {'О', "O"}, {'П', "P"}, {'Р', "R"}, {'С', "S"}, {'Т', "T"},
                {'У', "U"}, {'Ф', "F"}, {'Х', "Kh"}, {'Ц', "Ts"}, {'Ч', "Ch"},
                {'Ш', "Sh"}, {'Щ', "Sch"}, {'Ъ', ""}, {'Ы', "Y"}, {'Ь', ""},
                {'Э', "E"}, {'Ю', "Yu"}, {'Я', "Ya"}
            };

            var result = new StringBuilder();
            foreach (var c in text)
            {
                if (transliterationMap.TryGetValue(c, out var translit))
                    result.Append(translit);
                else
                    result.Append(c);
            }

            return result.ToString();
        }
    }
}