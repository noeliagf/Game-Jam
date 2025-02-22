using UnityEngine;

[System.Serializable]
public class Question
{
    public string questionText;  // Texto de la pregunta
    public string option1;       // Opción 1
    public string option2;       // Opción 2
    public string option3;       // Opción 3
    public int correctOption;    // Índice de la opción correcta (1, 2 o 3)
}