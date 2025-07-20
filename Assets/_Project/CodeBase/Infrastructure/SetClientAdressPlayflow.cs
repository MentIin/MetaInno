using UnityEngine;
using FishNet.Managing;
using FishNet.Transporting.Bayou; // Изменяем namespace на Bayou


public class SetClientAdressPlayflow : MonoBehaviour
{
    
    // Назначьте эти поля в Inspector'e
    [SerializeField]
    private NetworkManager _networkManager;
    [SerializeField]
    private Bayou _bayouTransport; // Меняем тип транспорта на BayouTransport

    // Используем константы для ясности на основе ваших данных Playflow Cloud
    private const string PLAYFLOW_CLOUD_HOSTNAME = "connect.computeflow.cloud";
    [SerializeField]private ushort WSS_PORT = 8691; // Используем Port1 как пример. Можно также использовать 6161 для 'wewrwer'

    void Update()
    {
        // При нажатии клавиши 'S' запускаем клиент
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log($"Starting FishNet Client (Bayou Transport), attempting to connect to {PLAYFLOW_CLOUD_HOSTNAME}:{WSS_PORT}...");

            if (_networkManager == null)
            {
                Debug.LogError("NetworkManager reference is missing. Please assign it in the Inspector.");
                return;
            }
            if (_bayouTransport == null) // Проверяем, что BayouTransport назначен
            {
                Debug.LogError("BayouTransport reference is missing. Please assign it in the Inspector.");
                return;
            }

            // --- Конфигурируем BayouTransport с вашими данными Playflow Cloud ---

            // Устанавливаем hostname. BayouTransport, как и SimpleWebTransport,
            // обработает разрешение DNS через браузер в WebGL.
            _bayouTransport.SetClientAddress(PLAYFLOW_CLOUD_HOSTNAME);
            // Устанавливаем внешний порт.
            _bayouTransport.SetPort(WSS_PORT);

            // Критично для безопасных WebSocket (WSS) соединений через TLS.
            // Playflow Cloud использует TLS для своих портов 'tcpTLS Enabled'.


            // Запускаем клиент FishNet
            _networkManager.ClientManager.StartConnection();
        }
    }
}


