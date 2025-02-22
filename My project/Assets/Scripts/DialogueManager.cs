using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public float delay = 0.05f;
    public List<string> initialDialogues = new List<string>();
    public TextMeshProUGUI textComponent;
    private int currentDialogueIndex = 0;
    private string fullText;
    private string currentText = "";
    private bool isTyping = false;

    public delegate void DialogueFinished();
    public event DialogueFinished OnDialogueFinished;  // Evento para notificar cuando el diálogo termine

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
            StartDialogue();
        }
        else
        {
            Debug.LogError("❌ ERROR: No hay diálogos iniciales en 'initialDialogues'.");
        }
    }

    void Update()
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

    public void StartDialogue()
    {
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
            Debug.Log("🎉 ¡Diálogos terminados! Ahora puedes continuar con las preguntas.");
            OnDialogueFinished?.Invoke();  // Notificar que el diálogo ha terminado
        }
    }
}
