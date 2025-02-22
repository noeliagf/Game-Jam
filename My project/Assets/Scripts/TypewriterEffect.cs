using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class TypewriterEffect : MonoBehaviour
{
    public float delay = 0.05f;  // Velocidad del efecto (segundos por carácter)
    public List<string> initialDialogues = new List<string>();  // Diálogos iniciales
    public List<string> questions = new List<string>();  // Lista de 30 preguntas
    public Button[] optionButtons;  // Los tres botones de opciones
    public string correctOptionText = "Correcto";  // Texto de la opción correcta
    public string incorrectOptionText = "Incorrecto";  // Texto de las opciones incorrectas
    public string titleSceneName = "TitleScene";  // Nombre de la escena de título

    private List<string> selectedQuestions = new List<string>();  // Preguntas seleccionadas aleatoriamente
    private int currentQuestionIndex = 0;  // Índice de la pregunta actual
    private int currentDialogueIndex = 0;  // Índice del diálogo actual
    private string fullText;  // Texto completo del diálogo actual
    private string currentText = "";  // Texto actual mostrado
    private TextMeshProUGUI textComponent;
    private bool isTyping = false;  // Indica si el efecto tipowriter está activo
    private bool isShowingOptions = false;  // Indica si se están mostrando opciones
    private int correctOptionIndex;  // Índice de la opción correcta

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();

        // Desactiva los botones al inicio
        foreach (Button button in optionButtons)
        {
            button.gameObject.SetActive(false);
        }

        // Selecciona 7 preguntas aleatorias sin repetición
        SelectRandomQuestions();

        // Inicia el diálogo inicial
        if (initialDialogues.Count > 0)
        {
            StartDialogue();  // Iniciar el primer diálogo
        }
    }

    void Update()
    {
        // Saltar el efecto tipowriter o avanzar al siguiente diálogo
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                SkipTyping();  // Saltar el efecto tipowriter
            }
            else if (!isShowingOptions)
            {
                NextDialogue();  // Avanzar al siguiente diálogo
            }
        }
    }

    void SelectRandomQuestions()
    {
        // Copia la lista de preguntas para no modificar la original
        List<string> availableQuestions = new List<string>(questions);

        // Selecciona 7 preguntas aleatorias sin repetición
        for (int i = 0; i < 7; i++)
        {
            int randomIndex = Random.Range(0, availableQuestions.Count);
            selectedQuestions.Add(availableQuestions[randomIndex]);
            availableQuestions.RemoveAt(randomIndex);  // Elimina la pregunta seleccionada para evitar repeticiones
        }
    }

    void StartDialogue()
    {
        if (currentDialogueIndex < initialDialogues.Count)
        {
            // Muestra el diálogo inicial
            fullText = initialDialogues[currentDialogueIndex];
            textComponent.text = "";
            StartCoroutine(ShowText());
        }
        else
        {
            // Muestra la primera pregunta
            ShowQuestion();
        }
    }

    IEnumerator ShowText()
    {
        isTyping = true;
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);  // Obtener una parte del texto
            textComponent.text = currentText;  // Actualizar el texto mostrado
            yield return new WaitForSeconds(delay);  // Esperar antes de mostrar el siguiente carácter
        }
        isTyping = false;

        // Si es el último diálogo inicial, muestra la primera pregunta
        if (currentDialogueIndex == initialDialogues.Count - 1)
        {
            ShowQuestion();
        }
    }

    void SkipTyping()
    {
        StopAllCoroutines();  // Detener el efecto tipowriter
        textComponent.text = fullText;  // Mostrar el texto completo
        isTyping = false;

        // Si es el último diálogo inicial, muestra la primera pregunta
        if (currentDialogueIndex == initialDialogues.Count - 1)
        {
            ShowQuestion();
        }
    }

    void NextDialogue()
    {
        if (currentDialogueIndex < initialDialogues.Count - 1)
        {
            currentDialogueIndex++;  // Avanzar al siguiente diálogo
            StartDialogue();  // Iniciar el nuevo diálogo
        }
    }

    void ShowQuestion()
    {
        isShowingOptions = true;

        // Muestra la pregunta actual
        textComponent.text = selectedQuestions[currentQuestionIndex];

        // Activa los botones de opciones
        foreach (Button button in optionButtons)
        {
            button.gameObject.SetActive(true);
        }

        // Asigna la opción correcta de manera aleatoria
        AssignCorrectOptionRandomly();
    }

    void AssignCorrectOptionRandomly()
    {
        // Elige un índice aleatorio para la opción correcta
        correctOptionIndex = Random.Range(0, optionButtons.Length);

        // Asigna los textos a los botones
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i == correctOptionIndex)
            {
                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = correctOptionText;
            }
            else
            {
                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = incorrectOptionText;
            }

            // Asigna el evento OnClick a cada botón
            int index = i;  // Captura el índice para el evento
            optionButtons[i].onClick.RemoveAllListeners();  // Limpia eventos anteriores
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }
    }

    void OnOptionSelected(int selectedIndex)
    {
        // Verifica si la opción seleccionada es la correcta
        if (selectedIndex == correctOptionIndex)
        {
            Debug.Log("¡Correcto! Avanzando...");
            currentQuestionIndex++;  // Pasa a la siguiente pregunta

            if (currentQuestionIndex < selectedQuestions.Count)
            {
                ShowQuestion();  // Muestra la siguiente pregunta
            }
            else
            {
                Debug.Log("¡Has completado todas las preguntas!");
                // Aquí puedes añadir lógica para terminar el juego o mostrar un mensaje de victoria
            }
        }
        else
        {
            Debug.Log("Incorrecto. Reiniciando...");
            // Carga la escena de título
            SceneManager.LoadScene(titleSceneName);
        }
    }
}