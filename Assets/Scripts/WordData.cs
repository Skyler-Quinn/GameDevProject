[System.Serializable]
public class WordData
{
    public string word;
    public string option1;
    public string option2;
    public string option3;
    public string option4;
    public string correctAnswer;
}

[System.Serializable]
public class WordDataList
{
    public WordData[] items;
}
