using TMPro;

namespace QLE
{
    public class UILog : MonoBehaviourSingletonPersistent<UILog>
    {
        static TextMeshProUGUI text;
        public override void Awake() {
            base.Awake();
            text = transform.Find("Log").GetComponent<TextMeshProUGUI>();
        }
        public static void Log(string message) => text.text = message;
    }
}
