namespace NewLibrary
{
	public static class RandomStringGenerator
	{
		public static List<string> RandomGenerator(int wordCount)
		{
			Random random = new Random();
			char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
			List<string> words = new List<string>();
			for(int i = 0; i < wordCount; i++)
			{
				string word = "";

				for(int j = 0; j<8; j++)
				{
					int letter_num = random.Next(letters.Length-1);
					word += letters[letter_num];
				}
				words.Add(word);
			}
			return words;
		}

		public static List<string> RandomGenerator(int wordCount, int lowerCharacterLimit, int upperCharacterLimit)
		{
			Random random = new Random();
			char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
			List<string> words = new List<string>();
			for (int i = 0; i < wordCount; i++)
			{
				string word = "";
				for (int j = 0; j < upperCharacterLimit; j++)
				{
					if (j > lowerCharacterLimit && random.Next(0, 2) == 1)
						break;
					int letter_num = random.Next(letters.Length - 1);
					word += letters[letter_num];
				}
				words.Add(word);
			}
			return words;
		}

	}
}
