using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class TypewriterEffect : MonoBehaviour
{
    public float delay = 0.05f;  // Velocidad del efecto (segundos por carácter)
    public List<string> dialogues = new List<string>();  // Lista de diálogos
    public Button[] optionButtons;  // Los tres botones de opciones
    public string correctOptionText = "Correcto";  // Texto de la opción correcta
    public string incorrectOptionText = "Incorrecto";  // Texto de las opciones incorrectas
    public string titleSceneName = "TitleScene";  // Nombre de la escena de título

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

        if (dialogues.Count > 0)
        {
            StartDialogue();  // Iniciar el primer diálogo
        }
    }

    void Update()
    {
        // Saltar el efecto tipowriter o avanzar al siguiente diálogo
        if (Input.GetKeyDown(KeyCode.Space)
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

    void StartDialogue()
    {
        fullText = dialogues[currentDialogueIndex];  // Obtener el diálogo actual
        textComponent.text = "";  // Borrar el texto inicial
        StartCoroutine(ShowText());  // Iniciar el efecto tipowriter
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

        // Si es el último diálogo, mostrar opciones
        if (currentDialogueIndex == dialogues.Count - 1)
        {
            ShowOptions();
        }
    }

    void SkipTyping()
    {
        StopAllCoroutines();  // Detener el efecto tipowriter
        textComponent.text = fullText;  // Mostrar el texto completo
        isTyping = false;

        // Si es el último diálogo, mostrar opciones
        if (currentDialogueIndex == dialogues.Count - 1)
        {
            ShowOptions();
        }
    }

    void NextDialogue()
    {
        if (currentDialogueIndex < dialogues.Count - 1)
        {
            currentDialogueIndex++;  // Avanzar al siguiente diálogo
            StartDialogue();  // Iniciar el nuevo diálogo
        }
    }

    void ShowOptions()
    {
        isShowingOptions = true;

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
            // Aquí puedes continuar con la siguiente parte del juego
        }
        else
        {
            Debug.Log("Incorrecto. Reiniciando...");
            // Carga la escena de título
            SceneManager.LoadScene(titleSceneName);
        }
    }
}