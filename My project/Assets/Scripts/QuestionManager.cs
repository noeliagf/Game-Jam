using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class QuestionManager : MonoBehaviour
{
    public List<Question> questions = new List<Question>();  // Lista de preguntas
    public Button[] optionButtons;  // Botones de opciones
    public string titleSceneName = "TitleScene";  // Nombre de la escena de título
    private List<Question> selectedQuestions = new List<Question>();  // Preguntas seleccionadas aleatoriamente
    private int currentQuestionIndex = 0;  // Índice de la pregunta actual

    void Start()
    {
        // Desactivar botones al inicio
        foreach (Button button in optionButtons)
        {
            button.gameObject.SetActive(false);
        }

        // Seleccionar preguntas aleatorias
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

        List<Question> availableQuestions = new List<Question>(questions);
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

        Question currentQuestion = selectedQuestions[currentQuestionIndex];
        Debug.Log("Pregunta actual: " + currentQuestion.questionText);

        // Mostrar las opciones en los botones
        optionButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.option1;
        optionButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.option2;
        optionButtons[2].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.option3;

        // Activar los botones de opciones
        foreach (Button button in optionButtons)
        {
            button.gameObject.SetActive(true);
        }

        // Asignar eventos a los botones
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index, currentQuestion.correctOption));
        }
    }

    void OnOptionSelected(int selectedIndex, int correctOption)
    {
        if (selectedIndex + 1 == correctOption)  // +1 porque los índices empiezan en 0
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
                // Aquí puedes añadir lógica para terminar el juego o mostrar un mensaje de victoria
            }
        }
        else
        {
            Debug.Log("❌ Incorrecto. Reiniciando...");
            SceneManager.LoadScene(titleSceneName);
        }
    }
}