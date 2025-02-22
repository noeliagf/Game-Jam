using UnityEngine;
using TMPro;  // Asegúrate de importar TextMeshPro

public class TimerController : MonoBehaviour
{
    public float tiempoRestante = 180f;  // 3 minutos
    private float velocidadTiempo = 1f;  // Velocidad normal
    public TextMeshProUGUI tiempoText;  // Usamos TextMeshProUGUI en lugar de Text

    void Update()
    {
        // Reducir el tiempo en función de la velocidad
        tiempoRestante -= Time.deltaTime * velocidadTiempo;

        // Mostrar el tiempo en formato de minutos y segundos
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);
        tiempoText.text = string.Format("{0:00}:{1:00}", minutos, segundos);

        // Si el tiempo llega a 0, terminar la entrevista
        if (tiempoRestante <= 0f)
        {
            TerminarEntrevista();
        }
    }

    // Llamar a esta función para hacer que el tiempo aumente más lentamente
    public void ConsultarBitacora()
    {
        velocidadTiempo = 0.5f;  // Hacer que el tiempo avance a la mitad de velocidad
    }

    // Terminar la entrevista (puedes implementar lo que pase aquí)
    void TerminarEntrevista()
    {
        Debug.Log("¡Entrevista terminada!");
        // Aquí puedes mostrar un mensaje de derrota o pasar a otra escena
    }
}
