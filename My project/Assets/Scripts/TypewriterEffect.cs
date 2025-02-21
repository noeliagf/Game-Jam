using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TypewriterEffect : MonoBehaviour
{
    public float delay = 0.05f;  // Velocidad del efecto (segundos por carácter)
    public List<string> dialogues = new List<string>();  // Lista de diálogos
    private int currentDialogueIndex = 0;  // Índice del diálogo actual
    private string fullText;  // Texto completo del diálogo actual
    private string currentText = "";  // Texto actual mostrado
    private TextMeshProUGUI textComponent;
    private bool isTyping = false;  // Indica si el efecto tipowriter está activo

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        if (dialogues.Count > 0)
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
            else
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
    }

    void SkipTyping()
    {
        StopAllCoroutines();  // Detener el efecto tipowriter
        textComponent.text = fullText;  // Mostrar el texto completo
        isTyping = false;
    }

    void NextDialogue()
    {
        if (currentDialogueIndex < dialogues.Count - 1)
        {
            currentDialogueIndex++;  // Avanzar al siguiente diálogo
            StartDialogue();  // Iniciar el nuevo diálogo
        }
        else
        {
            Debug.Log("No hay más diálogos.");  // Fin de los diálogos
            // Aquí puedes añadir lógica para terminar la escena o mostrar un mensaje
        }
    }
}