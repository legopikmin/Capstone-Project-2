using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentTestScript : MonoBehaviour
{
    // This is an example of using a [Header] to describe what is underneath it in the Inspector
    [Header("Health Settings Using A Header Example")]
    [SerializeField]
    private int health = 0;
    [SerializeField]
    private int maxHealth = 100;

    // DO NOT make variable names that make no sense for our project or what they do
    // Names that are easily identifiable makes all programming and design easier to understand
    [Header("This is a bad way of naming a variable")]
    public string dontNameAVariableCheeseAndSausagePizzaForOurProject;

    /// <summary>
    /// This is how a /// summary example would be used
    /// You can use multiple lines of writing to describe what anything is being created for
    /// But these should specifically used above Methods so whomever looks at it they can
    /// easily understand what the Method was created for
    /// </summary>
    public void ProperMethodNameExample()
    {
     /*
     * If wanting to use a block of comments then you can use this as an example
     * It essentially works like summary does however should be used within Methods.
     * This makes reading the code much easier and more clearly
     */
        string properExampleOfCamelCase;
    }

    
}
