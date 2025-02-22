using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class TypewriterEffect : MonoBehaviour
{
    public float delay = 0.05f;
    public List<string> initialDialogues = new List<string>();
    public List<string> questions = new List<string>();
    public Button[] optionButtons;
    public string correctOptionText = "Correcto";
    public string incorrectOptionText = "Incorrecto";
    public string titleSceneName = "TitleScene";

    private List<string> selectedQuestions = new List<string>();
    private int currentQuestionIndex = 0;
    private int currentDialogueIndex = 0;
    private string fullText;
    private string currentText = "";
    public TextMeshProUGUI textComponent; // Hacerlo público para asignarlo manualmente en el Inspector
    private bool isTyping = false;
    private bool isShowingOptions = false;
    private int correctOptionIndex;

    void Start()
    {
        
        // Validación: Verificar si el TextMeshProUGUI está asignado
        if (textComponent == null)
        {
            textComponent = GetComponentInChildren<TextMeshProUGUI>();

            if (textComponent == null)
            {
                Debug.LogError("❌ ERROR: No se encontró un componente TextMeshProUGUI en este GameObject.");
                return; // Detener ejecución para evitar más errores
            }
        }

        // Validación: Verificar si hay preguntas suficientes
        if (questions.Count < 7)
        {
            Debug.LogError("❌ ERROR: No hay suficientes preguntas en la lista (mínimo 7).");
            return;
        }

        // Validación: Verificar si hay diálogos iniciales
        if (initialDialogues.Count == 0)
        {
            Debug.LogError("❌ ERROR: No hay diálogos iniciales en la lista.");
            return;
        }

        // Desactivar los botones al inicio
        foreach (Button button in optionButtons)
        {
            if (button == null)
            {
                Debug.LogError("❌ ERROR: Uno de los botones de opción no está asignado en el Inspector.");
                return;
            }
            button.gameObject.SetActive(false);
        }

        // Selecciona 7 preguntas aleatorias sin repetición
        SelectRandomQuestions();

        // Inicia el diálogo inicial
        StartDialogue();
    }

    void SelectRandomQuestions()
    {
        List<string> availableQuestions = new List<string>(questions);

        for (int i = 0; i < 7; i++)
        {
            if (availableQuestions.Count == 0)
            {
                Debug.LogError("❌ ERROR: No hay suficientes preguntas para seleccionar aleatoriamente.");
                return;
            }

            int randomIndex = Random.Range(0, availableQuestions.Count);
            selectedQuestions.Add(availableQuestions[randomIndex]);
            availableQuestions.RemoveAt(randomIndex);
        }
    }

    void StartDialogue()
    {
        if (currentDialogueIndex < initialDialogues.Count)
        {
            fullText = initialDialogues[currentDialogueIndex];
            textComponent.text = "";
            StartCoroutine(ShowText());
        }
        else
        {
            ShowQuestion();
        }
    }

    IEnumerator ShowText()
    {
        isTyping = true;
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            textComponent.text = currentText;
            yield return new WaitForSeconds(delay);
        }
        isTyping = false;

        if (currentDialogueIndex == initialDialogues.Count - 1)
        {
            ShowQuestion();
        }
    }

    void ShowQuestion()
    {
        isShowingOptions = true;

        if (currentQuestionIndex >= selectedQuestions.Count)
        {
            Debug.LogError("❌ ERROR: Se ha intentado acceder a una pregunta fuera del rango.");
            return;
        }

        textComponent.text = selectedQuestions[currentQuestionIndex];

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
            if (optionButtons[i] == null)
            {
                Debug.LogError("❌ ERROR: Falta asignar un botón en el Inspector.");
                return;
            }

            TextMeshProUGUI buttonText = optionButtons[i].GetComponentInChildren<TextMeshProUGUI>();

            if (buttonText == null)
            {
                Debug.LogError($"❌ ERROR: No se encontró TextMeshProUGUI en el botón {i}.");
                return;
            }

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
