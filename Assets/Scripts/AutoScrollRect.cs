using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Add the script to your Dropdown Menu Template Object via (Your Dropdown Button > Template)

[RequireComponent(typeof(ScrollRect))]
public class AutoScrollRect : MonoBehaviour
{
    // Sets the speed to move the scrollbar
    //public float scrollSpeed = 50f;

    //Make sure rect anchors are the same as dropdown template and displacement is based on size of item
    public Vector3 displacementVal;

    // Set as Template Object via (Your Dropdown Button > Template)
    public ScrollRect m_templateScrollRect;

    // Set as Template Viewport Object via (Your Dropdown Button > Template > Viewport)
    public RectTransform m_templateViewportTransform;

    // Set as Template Content Object via (Your Dropdown Button > Template > Viewport > Content)
    public RectTransform m_ContentRectTransform;

    private RectTransform m_SelectedRectTransform;

    //Dropdown items are not children
    [SerializeField] bool buttonIsChild = false;

    void Update()
    {
        UpdateScrollToSelected(m_templateScrollRect, m_ContentRectTransform, m_templateViewportTransform);
    }

    void UpdateScrollToSelected(ScrollRect scrollRect, RectTransform contentRectTransform, RectTransform viewportRectTransform)
    {
        // Get the current selected option from the eventsystem.
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected == null)
        {
            return;
        }
        if (!buttonIsChild)
        {
            if (selected.transform.parent != contentRectTransform.transform)
            {
                return;
            }

            m_SelectedRectTransform = selected.GetComponent<RectTransform>();
        }
        else
        {

            if (selected.transform.parent.parent != contentRectTransform.transform)
            {
                return;
            }

            m_SelectedRectTransform = selected.transform.parent.GetComponent<RectTransform>();
        }


        // Math stuff
        Vector3 selectedDifference = viewportRectTransform.localPosition - m_SelectedRectTransform.localPosition - displacementVal;
        float contentHeightDifference = (contentRectTransform.rect.height - viewportRectTransform.rect.height);

        float selectedPosition = (contentRectTransform.rect.height - selectedDifference.y);
        float currentScrollRectPosition = scrollRect.normalizedPosition.y * contentHeightDifference;
        float above = currentScrollRectPosition - (m_SelectedRectTransform.rect.height / 2) + viewportRectTransform.rect.height;
        float below = currentScrollRectPosition + (m_SelectedRectTransform.rect.height / 2);

        //Debug.Log(m_SelectedRectTransform);
        //Debug.Log("above position" + above);
        //Debug.Log("selectedPosition" + selectedPosition);
        //Debug.Log("below position" + below);
        //Debug.Log(selectedDifference);

        // Check if selected option is out of bounds.
        if (selectedPosition > above)
        {
            float step = selectedPosition - above;
            float newY = currentScrollRectPosition + step;
            float newNormalizedY = newY / contentHeightDifference;
            scrollRect.normalizedPosition = new Vector2(0, newNormalizedY);
        }
        else if (selectedPosition < below)
        {
            float step = selectedPosition - below;
            float newY = currentScrollRectPosition + step;
            float newNormalizedY = newY / contentHeightDifference;
            //scrollRect.normalizedPosition = Vector2.Lerp(scrollRect.normalizedPosition, new Vector2(0, newNormalizedY), scrollSpeed * Time.deltaTime);
            scrollRect.normalizedPosition = new Vector2(0, newNormalizedY);
        }
    }
}