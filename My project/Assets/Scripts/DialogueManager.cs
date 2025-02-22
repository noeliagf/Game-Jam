using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public float delay = 0.05f;  // Velocidad del efecto (segundos por carácter)
    public List<string> initialDialogues = new List<string>();  // Diálogos iniciales
    public TextMeshProUGUI textComponent;  // Componente de texto para mostrar el diálogo
    private int currentDialogueIndex = 0;  // Índice del diálogo actual
    private string fullText;  // Texto completo del diálogo actual
    private string currentText = "";  // Texto actual mostrado
    private bool isTyping = false;  // Indica si el efecto tipowriter está activo
    private bool isDialogueActive = false;  // Controlar si el diálogo está activo

    // Referencia al QuestionManager
    public QuestionManager questionManager;

    void Start()
    {
        if (textComponent == null)
        {
            textComponent = GetComponentInChildren<TextMeshProUGUI>();

            if (textComponent == null)
            {
                Debug.LogError("❌ ERROR: No se encontró un componente TextMeshProUGUI.");
                return;
            }
        }

        if (initialDialogues.Count > 0)
        {
            StartDialogue();  // Iniciar el primer diálogo
        }
        else
        {
            Debug.LogError("❌ ERROR: No hay diálogos iniciales en 'initialDialogues'.");
        }
    }

    void Update()
    {
        if (isDialogueActive)  // Solo avanzar el diálogo si está activo
        {
            // Avanzar el diálogo cuando se presiona "Espacio"
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isTyping)
                {
                    SkipTyping();
                }
                else
                {
                    NextDialogue();
                }
            }
        }
    }

    public void StartDialogue()
    {
        isDialogueActive = true;  // Activar el diálogo
        if (currentDialogueIndex < initialDialogues.Count)
        {
            fullText = initialDialogues[currentDialogueIndex];
            textComponent.text = "";
            StartCoroutine(ShowText());
        }
        else
        {
            Debug.LogError("❌ ERROR: Índice de diálogo actual fuera de rango.");
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
    }

    void SkipTyping()
    {
        StopAllCoroutines();
        textComponent.text = fullText;
        isTyping = false;
    }

    void NextDialogue()
    {
        if (currentDialogueIndex < initialDialogues.Count - 1)
        {
            currentDialogueIndex++;
            StartDialogue();
        }
        else
        {
            Debug.Log("🎉 ¡Diálogos terminados!");
            isDialogueActive = false;  // Desactivar diálogo

            // Llamamos a la función en QuestionManager para avanzar con las preguntas
            if (questionManager != null)
            {
                questionManager.StartQuestions();  // Avanzar a las preguntas
            }
            else
            {
                Debug.LogError("❌ ERROR: No se ha asignado el QuestionManager.");
            }
        }
    }
}