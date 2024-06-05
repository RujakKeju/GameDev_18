using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEvents
{
    // characters damage and damage value
    public static UnityAction<GameObject, int> characterDamaged;

    // character healed adn amount value
    public static UnityAction<GameObject, int> characterHealed;
}
