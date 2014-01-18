using UnityEngine;
using System.Collections;

/// <summary>
/// An announcement of exits that get attached to a template to push updates to the parent.
/// </summary>
public class TemplateAnnounce : MonoBehaviour
{

    /// <summary>
    /// The supported exit paths out of a template.
    /// </summary>
    public enum Exit
    {
        TOP_LEFT,
        TOP_MIDDLE,
        TOP_RIGHT,
        RIGHT_TOP,
        RIGHT_MIDDLE,
        RIGHT_BOTTOM,
        BOTTOM_LEFT,
        BOTTOM_MIDDLE,
        BOTTOM_RIGHT,
        LEFT_TOP,
        LEFT_MIDDLE,
        LEFT_BOTTOM
    }

    /// <summary>
    /// The exits supported by a particular template.
    /// </summary>
    public Exit[] SupportedExits;

	// Use this for initialization
	void Start () {
	
	}

}
