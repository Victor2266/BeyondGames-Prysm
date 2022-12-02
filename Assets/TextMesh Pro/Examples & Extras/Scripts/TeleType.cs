using UnityEngine;
using System.Collections;


namespace TMPro.Examples
{
    
    public class TeleType : MonoBehaviour
    {


        //[Range(0, 100)]
        //public int RevealSpeed = 50;

        public string DisplayText;

        private TMP_Text m_textMeshPro;
        public bool nonReactive;
        public float delay;
        public bool disableWrapping = false;
        void Awake()
        {
            // Get Reference to TextMeshPro Component
            m_textMeshPro = GetComponent<TMP_Text>();
            m_textMeshPro.text = DisplayText;
            if(!disableWrapping)
                m_textMeshPro.enableWordWrapping = true;
            m_textMeshPro.maxVisibleCharacters = 0;

            if (nonReactive)
            {
                StartCoroutine(StartText());
            }

            //if (GetComponentInParent(typeof(Canvas)) as Canvas == null)
            //{
            //    GameObject canvas = new GameObject("Canvas", typeof(Canvas));
            //    gameObject.transform.SetParent(canvas.transform);
            //    canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

            //    // Set RectTransform Size
            //    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 300);
            //    m_textMeshPro.fontSize = 48;
            //}


        }

        public void RefreshDisplayText()
        {
            m_textMeshPro.text = DisplayText;
            m_textMeshPro.enableWordWrapping = false;
            m_textMeshPro.maxVisibleCharacters = 0;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "Player" && !nonReactive)
            {
                m_textMeshPro.text = DisplayText;
                StartCoroutine(StartText());
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.name == "Player" && !nonReactive)
            {
                m_textMeshPro.text = "";
                StartCoroutine(StartText());
            }
        }
        public IEnumerator StartText()
        {

            // Force and update of the mesh to get valid information.
            m_textMeshPro.ForceMeshUpdate();


            int totalVisibleCharacters = m_textMeshPro.textInfo.characterCount; // Get # of Visible Character in text object
            int counter = 0;
            int visibleCount = 0;

            while (true)
            {
                visibleCount = counter % (totalVisibleCharacters + 1);

                m_textMeshPro.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

                /* Once the last character has been revealed, wait 1.0 second and start over.
                if (visibleCount >= totalVisibleCharacters)
                {
                    yield return new WaitForSeconds(1.0f);
                    m_textMeshPro.text = label02;
                    yield return new WaitForSeconds(1.0f);
                    m_textMeshPro.text = DisplayText;
                    yield return new WaitForSeconds(1.0f);
                }*/

                counter += 1;

                yield return new WaitForSeconds(delay);
                if (visibleCount >= totalVisibleCharacters)
                {
                    break;
                }
            }

            //Debug.Log("Done revealing the text.");
        }

    }
}