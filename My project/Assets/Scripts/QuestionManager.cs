using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class QuestionManager : MonoBehaviour
{
    public List<string> questions = new List<string>();
    public Button[] optionButtons;
    public string correctOptionText = "Correcto";
    public string incorrectOptionText = "Incorrecto";
    public string titleSceneName = "TitleScene";
    private List<string> selectedQuestions = new List<string>();
    private int currentQuestionIndex = 0;
    private int correctOptionIndex;

    void Start()
    {
        foreach (Button button in optionButtons)
        {
            button.gameObject.SetActive(false);
        }

        SelectRandomQuestions();
    }

    public void StartQuestions()
    {
        Debug.Log("Iniciando preguntas...");
        ShowQuestion();
    }

    void SelectRandomQuestions()
    {
        if (questions.Count == 0)
        {
            Debug.LogError("❌ ERROR: No hay preguntas disponibles.");
            return;
        }

        List<string> availableQuestions = new List<string>(questions);
        int questionsToSelect = Mathf.Min(7, availableQuestions.Count);

        for (int i = 0; i < questionsToSelect; i++)
        {
            int randomIndex = Random.Range(0, availableQuestions.Count);
            selectedQuestions.Add(availableQuestions[randomIndex]);
            availableQuestions.RemoveAt(randomIndex);
        }

        if (selectedQuestions.Count == 0)
        {
            Debug.LogError("❌ ERROR: No se seleccionaron preguntas.");
        }
    }

    void ShowQuestion()
    {
        if (currentQuestionIndex >= selectedQuestions.Count)
        {
            Debug.Log("🎉 ¡Has completado todas las preguntas!");
            return;
        }

        string currentQuestion = selectedQuestions[currentQuestionIndex];
        Debug.Log("Pregunta actual: " + currentQuestion);

        foreach (Button button in optionButtons)
        {
            button.gameObject.SetActive(true);
        }

        AssignCorrectOptionRandomly();
    }

    void AssignCorrectOptionRandomly()
    {
        correctOptionIndex = Random.Range(0, optionButtons.Length);

        for (int i = 0; i < optionButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = optionButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = (i == correctOptionIndex) ? correctOptionText : incorrectOptionText;

            int index = i;
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }
    }

    void OnOptionSelected(int selectedIndex)
    {
        if (selectedIndex == correctOptionIndex)
        {
            Debug.Log("✅ ¡Correcto! Avanzando...");
            currentQuestionIndex++;
            if (currentQuestionIndex < selectedQuestions.Count)
            {
                ShowQuestion();
            }
            else
            {
                Debug.Log("🎉 ¡Has completado todas las preguntas!");
            }
        }
        else
        {
            Debug.Log("❌ Incorrecto. Reiniciando...");
            SceneManager.LoadScene(titleSceneName);
        }
    }
}