using System;
using System.Text;

namespace TtsGenerator
{
    public class Generator
    {
        public string GenerateUrl(string text, string lang)
        {
            string token = makeToken(text);
            return $"https://translate.google.com/translate_tts?ie=UTF-8&q={text}&tl={lang}&total={text}&idx=0&textlen={text.Length}&client=tw-ob&tk={token}&prev=input";
        }
        private string makeToken(string text)
        {
            int time = ((int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds) / 3600;
            char[] chars = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(text)).ToCharArray();
            long stamp = time;

            foreach (int ch in chars)
            {
                stamp = makeRl((stamp + ch), "+-a^+6");
            }

            stamp = makeRl(stamp, "+-3^+b+-f");

            if (stamp < 0)
            {
                stamp = (stamp & 2147483647) + 2147483648;
            }

            stamp = stamp % long.Parse((Math.Pow(10.00, 6.00)).ToString());

            return stamp + "." + (stamp ^ time);
        }

        private long makeRl(long num, string str)
        {
            for (int i = 0; i < str.Length - 2; i += 3)
            {
                string d = str.Substring(i + 2, 1);

                if (Encoding.ASCII.GetBytes(d.ToString())[0] >= Encoding.ASCII.GetBytes("a")[0])
                {
                    d = (Encoding.ASCII.GetBytes(d.ToString())[0] - 87).ToString();
                }
                else
                {
                    d = long.Parse(d).ToString();
                }

                if (str.Substring(i + 1, 1) == "+")
                {
                    d = (num >> int.Parse(d)).ToString();
                }
                else
                {
                    d = (num << int.Parse(d)).ToString();
                }

                if (str.Substring(i, 1) == "+")
                {
                    num = num + long.Parse(d) & 4294967295;
                }
                else
                {
                    num = num ^ long.Parse(d);
                }
            }
            return num;
        }
    }
}